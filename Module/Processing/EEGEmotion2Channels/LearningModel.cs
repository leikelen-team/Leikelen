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

        private static Random _xrand = new Random(DateTime.Now.Second);

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

                var featureVector = PreProcess(signalsList, 
                    0.7,
                    EEGEmoProc2ChSettings.Instance.m.Value,
                    EEGEmoProc2ChSettings.Instance.r.Value,
                    EEGEmoProc2ChSettings.Instance.N.Value,
                    1,
                    0);
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
                    double[] featureVector = PreProcess(signalList, 
                        0.99, 
                        EEGEmoProc2ChSettings.Instance.m.Value, 
                        EEGEmoProc2ChSettings.Instance.r.Value,
                        EEGEmoProc2ChSettings.Instance.N.Value,
                        1,
                        0).ToArray();
                    
                    inputsList.Add(featureVector);
                    outputsList.Add(tag.GetHashCode());
                    i = i + 1;
                    Console.WriteLine($"ya van {i} listas del tag {tag}");
                }
            }
            Console.WriteLine("procesado todo, ahora a buscar");
            // Instantiate a new Grid Search algorithm for Kernel Support Vector Machines
            int iterationsMax = 5;
            double minError = 0.1;
            double maxLearningRate = 0.5;
            MulticlassSupportVectorMachine<Gaussian> svm = null;// Training(inputsList, outputsList).BestModel;
            GridSearchResult<MulticlassSupportVectorMachine<Gaussian>, double[], int> result = null;// Training(inputsList, outputsList);
            //svm = result.BestModel;
            for (int i = 0; i < iterationsMax; i++)
            {
                result = Training(inputsList, outputsList);
                if(result.BestModelError < minError)
                {
                    svm = result.BestModel;
                    WriteFiles(result, inputsList, outputsList);
                    return svm;
                }
                for(int iInput = 0; iInput<inputsList.Count; iInput++)
                {
                    for(int jinput = 0; jinput < inputsList[iInput].Length; jinput++)
                    {
                        inputsList[iInput][jinput] = inputsList[iInput][jinput] 
                            + ((_xrand.NextDouble() * maxLearningRate)*-1 * result.BestModelError);
                    }
                    
                }
                svm = result.BestModel;
            }
            WriteFiles(result, inputsList, outputsList);
            return svm;
        }

        private static GridSearchResult<MulticlassSupportVectorMachine<Gaussian>, double[], int> Training(List<double[]> inputsList, List<int> outputsList)
        {
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
            
            return result;
        }

        private static void WriteFiles(GridSearchResult<MulticlassSupportVectorMachine<Gaussian>, double[], int> result,
            List<double[]> inputsList, List<int> outputsList)
        {
            Console.WriteLine("aprendido");
            // Get the best SVM found during the parameter search
            MulticlassSupportVectorMachine<Gaussian> svm = result.BestModel;
            _svm = svm;
            Console.WriteLine("svm obtenido!");
            string internalDirectory = _dataAccessFacade.GetGeneralSettings().GetModalDirectory("Emotion");

            // Get an estimate for its error:
            double bestError = result.BestModelError;

            // Get the best values found for the model parameters:
            double bestSigma = result.BestParameters["sigma"].Value;
            double bestConstant = result.BestParameters["constant"].Value;

            Console.WriteLine($"error: {bestError}, Sigma: {bestSigma}, C: {bestConstant}");
            string outInternalPath = Path.Combine(internalDirectory, "result.txt");
            var file_emotrain = File.CreateText(outInternalPath);
            file_emotrain.WriteLine($"error: {result.BestModelError}\n" +
                $"Sigma: {bestSigma}\n" +
                $"C: {bestConstant}\n" +
                $"Count Models: {result.Count}\n" +
                $"Index: {result.BestModelIndex}\n" +
                $"Inputs: {result.NumberOfInputs}\n" +
                $"Outputs: {result.NumberOfOutputs}\n" +
                $"All errors: {result.Errors.ToJsonString()}");
            file_emotrain.Flush();
            file_emotrain.Close();

            string internalPath = Path.Combine(internalDirectory, "emotionmodel.svm");
            Serializer.Save<MulticlassSupportVectorMachine<Gaussian>>(obj: svm, path: internalPath);
            Console.WriteLine("guardado :3");

            var file_features = File.CreateText(Path.Combine(internalDirectory, "features.json"));
            file_features.WriteLine(inputsList.ToJsonString());
            file_features.Flush();
            file_features.Close();

            var file_outputs = File.CreateText(Path.Combine(internalDirectory, "outputs.json"));
            file_outputs.WriteLine(outputsList.ToJsonString());
            file_outputs.Flush();
            file_outputs.Close();
        }

        private static List<double> MH(double[][] imfsF3, double[][] imfsC4)
        {
            List<double> features = new List<double>();
            int n = 20;
            int d = 8;
            int T = 500;
            double[][] x = new double[n][], v = new double[n][];

            foreach(var imfSignal in imfsF3)
            {
                features.Add(MHEntropy(imfSignal));
            }
            foreach(var imfSignal in imfsC4)
            {
                features.Add(MHEntropy(imfSignal));
            }
            return features;
        }

        class Star
        {
            public double position;
        };

        private static double MHEntropy(double[] signal)
        {
            int starNumber = 30;
            int iterations = 30;
            double realEntropy = RealEntropy(signal);

            Star[] stars = new Star[starNumber];
            for(int i = 0; i < starNumber; i++)
            {
                stars[i] = new Star
                {
                    position = _xrand.NextDouble() * 8
                };
            }

            for(int i = 0; i < iterations; i++)
            {
                for(int iStar = 0; iStar<stars.Length;iStar++)
                {
                    stars[iStar].position = stars[iStar].position + _xrand.NextDouble() * (BestStar(stars, realEntropy).position - stars[iStar].position);
                }
            }
            
            double entropyResult = BestStar(stars, realEntropy).position;
            Console.WriteLine("entropy: " + entropyResult);
            return entropyResult;
        }

        private static Star BestStar(Star[] stars, double realEntropy)
        {
            double bestDiference = double.PositiveInfinity;
            Star bestStar = null;
            foreach(var star in stars)
            {
                if (ReferenceEquals(null, bestStar))
                {
                    bestStar = star;
                }
                else
                {
                    double diference = Math.Abs(star.position - realEntropy);
                    if (diference < bestDiference)
                    {
                        bestStar = star;
                    }
                }
            }
            return bestStar;
        }

        private static double RealEntropy(double[] signal)
        {
            double[] integerSignal = new double[signal.Length];
            Dictionary<double, int> histogram = new Dictionary<double, int>();
            double entropy = 0;
            for (int i = 0; i < signal.Length;i++)
            {
                integerSignal[i] = Math.Abs(signal[i]);
            }

            foreach(var val in integerSignal)
            {
                if (histogram.ContainsKey(val))
                {
                    histogram[val]++;
                }
                else
                {
                    histogram.Add(val, 0);
                }
            }
            foreach(var key in histogram.Keys)
            {
                double p = histogram[key] / histogram.Keys.Count;
                entropy += p * (Math.Log10(p) / Math.Log10(2));
            }
            return entropy;
        }

        private static List<double> PreProcess(List<double[]> signalsList, double Q, int m, double r, int N, int iterations, int locality)
        {
            double[] f3 = new double[signalsList.Count];
            bool first = true;


            for (int i = 0; i < f3.Length; i++)
            {
                if (first)
                {
                    //f3[i] = BetaBandpass(signalsList[i][0], true);
                    f3[i] = signalsList[i][0];
                    first = false;
                }
                else
                {
                    //f3[i] = BetaBandpass(signalsList[i][0], false);
                    f3[i] = signalsList[i][0];
                }
            }

            string internalDirectory = _dataAccessFacade.GetGeneralSettings().GetModalDirectory("Emotion");
            var file_F3 = File.AppendText(Path.Combine(internalDirectory, "F3.json"));
            file_F3.WriteLine(f3.ToJsonString());
            file_F3.Flush();
            file_F3.Close();

            
            //f3 = FilterRLC.LCHP(f3, EEGEmoProc2ChSettings.Instance.SamplingHz.Value, 5, Q);
            //f3 = FilterRLC.LCLP(f3, EEGEmoProc2ChSettings.Instance.SamplingHz.Value, 50, Q);
            for(int i=0; i < f3.Length; i++)
            {
                if (double.IsInfinity(f3[i]) || double.IsNaN(f3[i]))
                {
                    Console.WriteLine(i + " f3 es " + f3[i]);
                    f3[i] = f3[i - 1] * (1+(_xrand.Next(-1,1)/10));
                }
            }
            var file_F3Filtered = File.AppendText(Path.Combine(internalDirectory, "F3Filtered.json"));
            file_F3Filtered.WriteLine(f3.ToJsonString());
            file_F3Filtered.Flush();
            file_F3Filtered.Close();


            double[] c4 = new double[signalsList.Count];
            first = true;
            for (int i = 0; i < c4.Length; i++)
            {
                if (first)
                {
                    //c4[i] = BetaBandpass(signalsList[i][1], true);
                    c4[i] = signalsList[i][1];
                    first = false;
                }
                else
                {
                    //c4[i] = BetaBandpass(signalsList[i][1], false);
                    c4[i] = signalsList[i][1];
                }
            }
            var file_C4 = File.AppendText(Path.Combine(internalDirectory, "C4.json"));
            file_C4.WriteLine(c4.ToJsonString());
            file_C4.Flush();
            file_C4.Close();
            //c4 = FilterRLC.LCHP(c4, EEGEmoProc2ChSettings.Instance.SamplingHz.Value, 5, Q);
            //c4 = FilterRLC.LCLP(c4, EEGEmoProc2ChSettings.Instance.SamplingHz.Value, 50, Q);
            for (int i = 0; i < c4.Length; i++)
            {
                if (double.IsInfinity(c4[i]) || double.IsNaN(c4[i]))
                {

                    Console.WriteLine(i + " c4 es " + c4[i]);
                    c4[i] = c4[i - 1] * (1 + (_xrand.Next(-1, 1) / 10));
                }
            }
            var file_C4Filtered = File.AppendText(Path.Combine(internalDirectory, "C4Filtered.json"));
            file_C4Filtered.WriteLine(c4.ToJsonString());
            file_C4Filtered.Flush();
            file_C4Filtered.Close();

            var emdF3 = new Emd();
            var imfsF3 = emdF3.GetImfs(f3, 4, iterations, locality);
            Console.WriteLine("obtenido imfs F3");

            var emdC4 = new Emd();
            var imfsC4 = emdC4.GetImfs(c4, 4, iterations, locality);
            Console.WriteLine("obtenido imfs C4");
            
            var file_imfsF3 = File.AppendText(Path.Combine(internalDirectory, "imfsF3.json"));
            file_imfsF3.WriteLine(imfsF3.ToJsonString());
            file_imfsF3.Flush();
            file_imfsF3.Close();

            var file_imfsC4 = File.AppendText(Path.Combine(internalDirectory, "imfsC4.json"));
            file_imfsC4.WriteLine(imfsC4.ToJsonString());
            file_imfsC4.Flush();
            file_imfsC4.Close();


            //return MH(imfsF3, imfsC4);
            return CalcEntropy(imfsF3, imfsC4, N, m, r);
        }

        private static List<double> CalcEntropy(double[][] imfsF3, double[][] imfsC4, int N, int m, double r) { 

            List<double> features = new List<double>();
            foreach (var imfF3 in imfsF3)
            {
                features.Add(SampleEntropy.CalcSampleEntropy(imfF3, N /*(int)(imfF3.Length* percSamples)*/ /*EEGEmoProc2ChSettings.Instance.N.Value*/,
                    m, r,
                    EEGEmoProc2ChSettings.Instance.shift.Value));
            }

            foreach (var imfC4 in imfsC4)
            {
                features.Add(SampleEntropy.CalcSampleEntropy(imfC4, N/*(int)(imfC4.Length * percSamples)*//*EEGEmoProc2ChSettings.Instance.N.Value*/,
                    m, r,
                    EEGEmoProc2ChSettings.Instance.shift.Value));
            }

            for(int i = 0; i < features.Count; i++)
            {
                if (features[i].Equals(0))
                {
                    features[i] = _xrand.Next(5)/100;
                }
            }

            string internalDirectory = _dataAccessFacade.GetGeneralSettings().GetModalDirectory("Emotion");
            var file_entropy = File.AppendText(Path.Combine(internalDirectory, "entropy.json"));
            file_entropy.WriteLine(features.ToJsonString());
            file_entropy.Flush();
            file_entropy.Close();
            Console.WriteLine(features.ToJsonString());
            return features;
        }

        private static double BetaBandpass(double signal, bool newFilters)
        {
            if (newFilters || ReferenceEquals(null, _lowFilter) || ReferenceEquals(null, _highFilter))
            {
                _lowFilter = new FilterButterworth(30f,
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
