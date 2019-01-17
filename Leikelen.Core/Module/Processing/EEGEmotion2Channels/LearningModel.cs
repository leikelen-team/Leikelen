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
        private static API.DataAccess.IDataAccessFacade _dataAccessFacade = new DataAccessFacade();
        private static Module.Processing.EEGEmotion2Channels.PreProcessing.FilterButterworth _lowFilter;
        private static Module.Processing.EEGEmotion2Channels.PreProcessing.FilterButterworth _highFilter;
        private static MulticlassSupportVectorMachine<Gaussian> _svm;

        private static Random _xrand;// = new Random(DateTime.Now.Second);

        public static Module.Processing.EEGEmotion2Channels.TagType Classify(List<double[]> signalsList)
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

        public struct Star2
        {
            public MulticlassSupportVectorMachine<Gaussian> svm;
            public double error;
            public double Complexity;
            public double Gamma;
            public List<double[]> inputsList;
        }

        public static MulticlassSupportVectorMachine<Gaussian> Train(
            Dictionary<TagType, List<List<double[]>>> allsignalsList,
            bool useMH=false)
        {
            int seed = DateTime.Now.Second;
            _xrand = new Random(seed);
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
            if (!useMH)
            {
                var trainRes = Training(inputsList, outputsList);
                WriteFiles(trainRes.Item2, trainRes.Item3, trainRes.Item4, inputsList, outputsList, trainRes.Item1);
                return trainRes.Item1;
            }
            // Instantiate a new Grid Search algorithm for Kernel Support Vector Machines
            int iterationsMax = 7;
            double minError = 0.1;
            int maxPopulation = 3;
            MulticlassSupportVectorMachine<Gaussian> svm = null;// Training(inputsList, outputsList).BestModel;
            Tuple<MulticlassSupportVectorMachine<Gaussian>, double, double, double> result = null;// Training(inputsList, outputsList);
            //svm = result.BestModel;
            string internalDirectory = _dataAccessFacade.GetGeneralSettings().GetModalDirectory("Emotion");
            var file_all_models = File.AppendText(Path.Combine(internalDirectory, "all_models.txt"));
            var file_all_stars = File.AppendText(Path.Combine(internalDirectory, "all_stars_all_its.txt"));
            result = Training(inputsList, outputsList);
            Star2[] stars = new Star2[maxPopulation];
            Star2 best = new Star2()
            {
                svm = result.Item1.DeepClone(),
                error = result.Item2,
                Complexity = result.Item3,
                Gamma = result.Item4,
                inputsList = inputsList

            };
            //inicialization
            for (int iStar = 0; iStar < maxPopulation; iStar++)
            {
                stars[iStar] = new Star2()
                {
                    svm = result.Item1.DeepClone(),
                    error = result.Item2,
                    Complexity = result.Item3,
                    Gamma = result.Item4,
                    inputsList = inputsList

                };
                try
                {
                    for (int iInput = 0; iInput < stars[iStar].inputsList.Count; iInput++)
                    {
                        for (int jinput = 0; jinput < stars[iStar].inputsList[iInput].Length; jinput++)
                        {
                            stars[iStar].inputsList[iInput][jinput] = stars[iStar].inputsList[iInput][jinput]
                                + ((_xrand.NextDouble() * -1) * result.Item2 * stars[iStar].inputsList[iInput][jinput]);
                        }

                    }
                    var res = Training(stars[iStar].inputsList, outputsList);
                    stars[iStar].svm = res.Item1;
                    stars[iStar].error = res.Item2;
                    stars[iStar].Complexity = res.Item3;
                    stars[iStar].Gamma = res.Item4;
                }catch(Exception ex)
                {
                    Console.WriteLine("Error al inicializar estrella "+iStar+": "+ex.Message);
                }
                
            }
            foreach (var star in stars)
            {
                if (star.error < best.error)
                {
                    best = star;
                }
            }
            //cycle
            for (int i = 0; i < iterationsMax; i++)
            {
                
                file_all_models.WriteLine($"Model: {i}, Seed: {seed}, Error: {best.error}, Gamma: {best.Gamma}, C: {best.Complexity}\n inputs: {best.inputsList.ToJsonString(true)}");
                file_all_models.Flush();
                
                if (best.error < minError)
                {
                    svm = best.svm;
                    WriteFiles(best.error, best.Gamma, best.Complexity, best.inputsList, outputsList, best.svm);
                    return svm;
                }
                //each star
                for(int iStar = 0; iStar < stars.Length; iStar++)
                {
                    Star2 prevStar = stars[iStar];
                    try
                    {
                        for (int iInput = 0; iInput < stars[iStar].inputsList.Count; iInput++)
                        {
                            for (int jinput = 0; jinput < stars[iStar].inputsList[iInput].Length; jinput++)
                            {
                                stars[iStar].inputsList[iInput][jinput] = stars[iStar].inputsList[iInput][jinput]
                                    + _xrand.NextDouble()
                                    * (best.inputsList[iInput][jinput] - stars[iStar].inputsList[iInput][jinput]);
                            }
                        }
                        var res = Training(stars[iStar].inputsList, outputsList);
                        stars[iStar].svm = res.Item1;
                        stars[iStar].error = res.Item2;
                        stars[iStar].Complexity = res.Item3;
                        stars[iStar].Gamma = res.Item4;
                        file_all_stars.WriteLine($"Model: {i}, Seed: {seed}, Error: {stars[iStar].error}, Gamma: {stars[iStar].Gamma}, C: {stars[iStar].Complexity}\n inputs: {stars[iStar].inputsList.ToJsonString(true)}");
                        file_all_stars.Flush();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error en it "+i+" al cambiar estrella " + iStar + ": " + ex.Message);
                        stars[iStar] = prevStar;
                    }
                    

                }
                foreach (var star in stars)
                {
                    if (star.error < best.error)
                    {
                        best = star;
                    }
                }
                svm = best.svm;
            }
            file_all_models.Close();
            file_all_stars.Close();
            WriteFiles(result.Item2, result.Item3, result.Item4, inputsList, outputsList, result.Item1);
            return svm;
        }

        private static Tuple<MulticlassSupportVectorMachine<Gaussian>, double, double, double> Training(List<double[]> inputsList, List<int> outputsList)
        {
            var gridsearch = GridSearch<double[], int>.CrossValidate(
                // Here we can specify the range of the parameters to be included in the search
                ranges: new
                {
                    Complexity = GridSearch.Values( Math.Pow(2, -10), Math.Pow(2, -8),
                        Math.Pow(2, -6), Math.Pow(2, -4), Math.Pow(2, -2), Math.Pow(2, 0), Math.Pow(2, 2),
                        Math.Pow(2, 4), Math.Pow(2, 6), Math.Pow(2, 8), Math.Pow(2, 10)),
                    Gamma = GridSearch.Values(Math.Pow(2, -10), Math.Pow(2, -8),
                        Math.Pow(2, -6), Math.Pow(2, -4), Math.Pow(2, -2), Math.Pow(2, 0), Math.Pow(2, 2),
                        Math.Pow(2, 4), Math.Pow(2, 6), Math.Pow(2, 8), Math.Pow(2, 10))
                },

                // Indicate how learning algorithms for the models should be created
                learner: (p, ss) => new MulticlassSupportVectorLearning<Gaussian>()
                {
                    // Configure the learning algorithm to use SMO to train the
                    //  underlying SVMs in each of the binary class subproblems.
                    Learner = (param) => new SequentialMinimalOptimization<Gaussian>()
                    {
                        // Estimate a suitable guess for the Gaussian kernel's parameters.
                        // This estimate can serve as a starting point for a grid search.
                        //UseComplexityHeuristic = true,
                        //UseKernelEstimation = true
                        Complexity = p.Complexity,
                        Kernel = new Gaussian(p.Gamma)
                    }
                },
                // Define how the model should be learned, if needed
                fit: (teacher, x, y, w) => teacher.Learn(x, y, w),

                // Define how the performance of the models should be measured
                loss: (actual, expected, m) => new HammingLoss(expected).Loss(actual),
                folds: 10
            );

            gridsearch.ParallelOptions.MaxDegreeOfParallelism = 1;

            Console.WriteLine("to learn");
            // Search for the best model parameters
            var result = gridsearch.Learn(inputsList.ToArray(), outputsList.ToArray());

            
            return new Tuple<MulticlassSupportVectorMachine<Gaussian>, double, double, double>(CreateModel( inputsList, outputsList, result.BestParameters.Complexity, result.BestParameters.Gamma), result.BestModelError, result.BestParameters.Gamma, result.BestParameters.Complexity);
        }

        private static MulticlassSupportVectorMachine<Gaussian> CreateModel(List<double[]> inputsList,
            List<int> outputsList, double complexity, double gamma)
        {
            var teacher =  new MulticlassSupportVectorLearning<Gaussian>()
            {
                // Configure the learning algorithm to use SMO to train the
                //  underlying SVMs in each of the binary class subproblems.
                Learner = (param) => new SequentialMinimalOptimization<Gaussian>()
                {
                    // Estimate a suitable guess for the Gaussian kernel's parameters.
                    // This estimate can serve as a starting point for a grid search.

                    Complexity = complexity,
                    Kernel = new Gaussian(gamma)
                }
            };
            return teacher.Learn(inputsList.ToArray(), outputsList.ToArray());
        }

        private static void WriteFiles(double error, double gamma, double complexity,
            List<double[]> inputsList, List<int> outputsList, MulticlassSupportVectorMachine<Gaussian> svm)
        {
            Console.WriteLine("learned");
            // Get the best SVM found during the parameter search
            _svm = svm;
            Console.WriteLine("SVM obtained!");
            string internalDirectory = _dataAccessFacade.GetGeneralSettings().GetModalDirectory("Emotion");
            

            Console.WriteLine($"error: {error}, Gamma: {gamma}, C: {complexity}");
            string outInternalPath = Path.Combine(internalDirectory, "result.txt");
            var file_emotrain = File.CreateText(outInternalPath);
            file_emotrain.Flush();
            file_emotrain.Close();

            string internalPath = Path.Combine(internalDirectory, "emotionmodel.svm");
            Serializer.Save<MulticlassSupportVectorMachine<Gaussian>>(obj: svm, path: internalPath);
            Console.WriteLine("saved :3");

            var file_features = File.CreateText(Path.Combine(internalDirectory, "features.json"));
            file_features.WriteLine(inputsList.ToJsonString());
            file_features.Flush();
            file_features.Close();

            var file_outputs = File.CreateText(Path.Combine(internalDirectory, "outputs.json"));
            file_outputs.WriteLine(outputsList.ToJsonString());
            file_outputs.Flush();
            file_outputs.Close();
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
