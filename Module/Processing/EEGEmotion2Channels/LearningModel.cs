using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.PreProcessing;
using Accord.MachineLearning.Performance;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math.Optimization.Losses;
using Accord.IO;
using System.IO;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels
{
    public class LearningModel
    {
        private static IDataAccessFacade _dataAccessFacade = new DataAccessFacade();
        private static FilterButterworth _lowFilter;
        private static FilterButterworth _highFilter;
        private static MulticlassSupportVectorMachine<Gaussian> _svm;

        public static TagType Classify(List<double[]> signalsList)
        {
            try
            {
                if (ReferenceEquals(null, _svm))
                {
                    string internalPath = $"{_dataAccessFacade.GetGeneralSettings().GetDataDirectory()}" +
                        $"modal/Emotion/emotionmodel.svm";
                    _svm = Serializer.Load<MulticlassSupportVectorMachine<Gaussian>>(path: internalPath);
                }

                var featureVector = PreProcess(signalsList);
                Console.WriteLine("preprocesado" + featureVector.ToJsonString());
                return (TagType)_svm.Decide(featureVector.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                throw ex;
            }
        }

        public static MulticlassSupportVectorMachine<Gaussian> Train(Dictionary<TagType, List<List<double[]>>> allsignalsList)
        {
            Console.WriteLine("a entrenar se ha dicho");
            List<double[]> inputsList = new List<double[]>();
            List<int> outputsList = new List<int>();
            foreach (var tag in allsignalsList.Keys)
            {
                Console.WriteLine($"empezando pro el tag {tag}");
                int i = 0;
                foreach (var signalList in allsignalsList[tag])
                {
                    double[] featureVector = PreProcess(signalList).ToArray();
                    inputsList.Add(featureVector);
                    outputsList.Add(tag.GetHashCode());
                    i = i + 1;
                    Console.WriteLine($"ya van {i} listas del tag {tag}");
                }
            }
            Console.WriteLine("procesado todo, ahora a buscar");
            // Instantiate a new Grid Search algorithm for Kernel Support Vector Machines
            
            var gridsearch = new GridSearch<MulticlassSupportVectorMachine<Gaussian>, double[], int>()
            {
                // Here we can specify the range of the parameters to be included in the search
                ParameterRanges = new GridSearchRangeCollection()
                {
                    new GridSearchRange("sigma", new double[] { Math.Pow(2,-10), Math.Pow(2, -8),
                        Math.Pow(2, -6), Math.Pow(2,-4), Math.Pow(2,-2), Math.Pow(2,0), Math.Pow(2,2),
                        Math.Pow(2,4), Math.Pow(2,6), Math.Pow(2,8), Math.Pow(2,10)} ),
                    new GridSearchRange("constant",   new double[] { Math.Pow(2,-10), Math.Pow(2, -8),
                        Math.Pow(2, -6), Math.Pow(2,-4), Math.Pow(2,-2), Math.Pow(2,0), Math.Pow(2,2),
                        Math.Pow(2,4), Math.Pow(2,6), Math.Pow(2,8), Math.Pow(2,10) } )
                },

                // Indicate how learning algorithms for the models should be created
                Learner = (p) => new MulticlassSupportVectorLearning<Gaussian>()
                {
                    // Configure the learning algorithm to use SMO to train the
                    //  underlying SVMs in each of the binary class subproblems.
                    Learner = (param) => new SequentialMinimalOptimization<Gaussian>()
                    {
                        // Estimate a suitable guess for the Gaussian kernel's parameters.
                        // This estimate can serve as a starting point for a grid search.
                        Complexity = p["constant"],
                        Kernel = new Gaussian(p["sigma"])
                    }
                },

                // Define how the performance of the models should be measured
                Loss = (actual, expected, m) => new HammingLoss(expected).Loss(actual)
            };
            gridsearch.ParallelOptions.MaxDegreeOfParallelism = 1;

            Console.WriteLine("y nos ponemos a aprender");
            // Search for the best model parameters
            var result = gridsearch.Learn(inputsList.ToArray(), outputsList.ToArray());
            
            Console.WriteLine("aprendido");
            // Get the best SVM found during the parameter search
            MulticlassSupportVectorMachine<Gaussian> svm = result.BestModel;
            _svm = svm;
            Console.WriteLine("svm obtenido!");
            string internalPath = $"{_dataAccessFacade.GetGeneralSettings().GetDataDirectory()}" +
                    $"modal/Emotion/emotionmodel.svm";
            Serializer.Save<MulticlassSupportVectorMachine<Gaussian>>(obj: svm, path: internalPath);
            Console.WriteLine("guardado :3");
            // Get an estimate for its error:
            double bestError = result.BestModelError;

            // Get the best values found for the model parameters:
            double bestSigma = result.BestParameters["sigma"].Value;
            double bestConstant = result.BestParameters["constant"].Value;

            Console.WriteLine($"error: {bestError}, Sigma: {bestSigma}, C: {bestConstant}");
            string outInternalPath = $"{_dataAccessFacade.GetGeneralSettings().GetDataDirectory()}" +
                    $"modal/Emotion/result.txt";
            var file_emotrain = File.CreateText(outInternalPath);
            file_emotrain.WriteLine($"error: {result.BestModelError}\t" +
                $"Sigma: {bestSigma}\t" +
                $"C: {bestConstant}\t" +
                $"Count Models: {result.Count}\t" +
                $"Index: {result.BestModelIndex}\t" +
                $"Inputs: {result.NumberOfInputs}\t" +
                $"Outputs: {result.NumberOfOutputs}");
            file_emotrain.Flush();
            file_emotrain.Close();
            return svm;
            
        }

        private static List<double> PreProcess(List<double[]> signalsList)
        {
            double[] f3 = new double[signalsList.Count];
            bool first = true;

            
            for (int i = 0; i < f3.Length;i++)
            {
                if (first)
                {
                    f3[i] = BetaBandpass(signalsList[i][0], true);
                    //f3[i] = signalsList[i][0];
                    first = false;
                }
                else
                {
                    f3[i] = BetaBandpass(signalsList[i][0], false);
                    //f3[i] = signalsList[i][0];
                }
            }
            f3 = FilterRLC.LCHP(f3, EEGEmoProc2ChSettings.Instance.SamplingHz.Value, 12.5, 0.7);
            f3 = FilterRLC.LCLP(f3, EEGEmoProc2ChSettings.Instance.SamplingHz.Value, 30, 0.7);


            double[] c4 = new double[signalsList.Count];
            first = true;
            for (int i = 0; i < c4.Length; i++)
            {
                if (first)
                {
                    c4[i] = BetaBandpass(signalsList[i][1], true);
                    //c4[i] = signalsList[i][1];
                    first = false;
                }
                else
                {
                    c4[i] = BetaBandpass(signalsList[i][1], false);
                    //c4[i] = signalsList[i][1];
                }
            }
            c4 = FilterRLC.LCHP(c4, EEGEmoProc2ChSettings.Instance.SamplingHz.Value, 12.5, 0.7);
            c4 = FilterRLC.LCLP(c4, EEGEmoProc2ChSettings.Instance.SamplingHz.Value, 30, 0.7);

            var emdF3 = new Emd();
            var imfsF3 = emdF3.GetImfs(f3, 4, 1, 0);
            Console.WriteLine("obtenido imfs F3");

            var emdC4 = new Emd();
            var imfsC4 = emdC4.GetImfs(c4, 4, 1, 0);
            Console.WriteLine("obtenido imfs C4");

            List<double> features = new List<double>();
            foreach (var imfF3 in imfsF3)
            {
                features.Add(SampleEntropy.CalcSampleEntropy(imfF3, (int)(imfF3.Length*0.5) /*EEGEmoProc2ChSettings.Instance.N.Value*/,
                    EEGEmoProc2ChSettings.Instance.m.Value, EEGEmoProc2ChSettings.Instance.r.Value,
                    EEGEmoProc2ChSettings.Instance.shift.Value));
            }

            foreach (var imfC4 in imfsC4)
            {
                features.Add(SampleEntropy.CalcSampleEntropy(imfC4, (int)(imfC4.Length * 0.5)/*EEGEmoProc2ChSettings.Instance.N.Value*/,
                    EEGEmoProc2ChSettings.Instance.m.Value, EEGEmoProc2ChSettings.Instance.r.Value,
                    EEGEmoProc2ChSettings.Instance.shift.Value));
            }
            Console.WriteLine(features);
            return features;
        }

        private static double BetaBandpass(double signal, bool newFilters)
        {
            if (newFilters || ReferenceEquals(null, _lowFilter) || ReferenceEquals(null, _highFilter))
            {
                _lowFilter = new FilterButterworth(30,
                    EEGEmoProc2ChSettings.Instance.SamplingHz,
                    FilterButterworth.PassType.Lowpass, 0.1f);

                _highFilter = new FilterButterworth(12.5f,
                    EEGEmoProc2ChSettings.Instance.SamplingHz,
                    FilterButterworth.PassType.Highpass, 0.1f);
            }
            return _highFilter.Update(_lowFilter.Update(signal));
        }
    }
}
