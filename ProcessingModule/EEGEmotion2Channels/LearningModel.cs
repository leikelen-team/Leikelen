﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels.PreProcessing;
using Accord.MachineLearning.Performance;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math.Optimization.Losses;
using Accord.IO;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels
{
    public class LearningModel
    {
        public static TagType Classify(List<double[]> signalsList)
        {
            var featureVector = PreProcess(signalsList);
            //TODO: esto es temporal, hay que clasificar
            return 0;
        }

        public static void Train(Dictionary<TagType, List<List<double[]>>> allsignalsList)
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
            var gridsearch = new GridSearch<SupportVectorMachine<Gaussian>, double[], int>()
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
                Learner = (p) => new SequentialMinimalOptimization<Gaussian>
                {
                    Complexity = p["constant"],
                    Kernel = new Gaussian(p["sigma"])
                },

                // Define how the performance of the models should be measured
                Loss = (actual, expected, m) => new ZeroOneLoss(expected).Loss(actual)
            };
            gridsearch.ParallelOptions.MaxDegreeOfParallelism = 4;

            Console.WriteLine("y nos ponemos a aprender");
            // Search for the best model parameters
            var result = gridsearch.Learn(inputsList.ToArray(), outputsList.ToArray());

            Console.WriteLine("aprendido");
            // Get the best SVM found during the parameter search
            SupportVectorMachine<Gaussian> svm = result.BestModel;
            Console.WriteLine("svm obtenido!");
            Serializer.Save<SupportVectorMachine<Gaussian>>(obj: svm, path: "mimodelo.svm");
            Console.WriteLine("guardado :3");
            // Get an estimate for its error:
            double bestError = result.BestModelError;

            // Get the best values found for the model parameters:
            double bestSigma = result.BestParameters["sigma"].Value;
            double bestConstant = result.BestParameters["constant"].Value;

            Console.WriteLine($"error: {bestError}, Sigma: {bestSigma}, C: {bestConstant}");

        }

        private static List<double> PreProcess(List<double[]> signalsList)
        {
            double[] f3 = new double[signalsList.Count];
            for (int i = 0; i < f3.Length;i++)
            {
                f3[i] = signalsList[i][0];
            }

            double[] c4 = new double[signalsList.Count];
            for (int i = 0; i < c4.Length; i++)
            {
                c4[i] = signalsList[i][1];
            }

            var emdF3 = new Emd();
            var imfsF3 = emdF3.GetImfs(f3, 4, 1, 0);
            Console.WriteLine("obtenido imfs F3");

            var emdC4 = new Emd();
            var imfsC4 = emdC4.GetImfs(c4, 4, 1, 0);
            Console.WriteLine("obtenido imfs C4");

            List<double> features = new List<double>();
            foreach (var imfF3 in imfsF3)
            {
                features.Add(SampleEntropy.CalcSampleEntropy(imfF3, EEGEmoProc2ChSettings.Instance.N.Value,
                    EEGEmoProc2ChSettings.Instance.m.Value, EEGEmoProc2ChSettings.Instance.r.Value,
                    EEGEmoProc2ChSettings.Instance.shift.Value));
            }

            foreach (var imfC4 in imfsC4)
            {
                features.Add(SampleEntropy.CalcSampleEntropy(imfC4, EEGEmoProc2ChSettings.Instance.N.Value,
                    EEGEmoProc2ChSettings.Instance.m.Value, EEGEmoProc2ChSettings.Instance.r.Value,
                    EEGEmoProc2ChSettings.Instance.shift.Value));
            }
            Console.WriteLine(features);
            return features;
        }
    }
}
