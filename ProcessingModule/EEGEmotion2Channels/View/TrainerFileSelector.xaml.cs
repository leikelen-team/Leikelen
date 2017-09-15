using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels.View
{
    /// <summary>
    /// Lógica de interacción para TrainerFileSelector.xaml
    /// </summary>
    public partial class TrainerFileSelector : Window, ICloneable
    {
        public TrainerFileSelector()
        {
            InitializeComponent();

            LAHVbrowseButton.Click += LAHVbrowseButton_Click;
            LALVbrowseButton.Click += LALVbrowseButton_Click;
            HAHVbrowseButton.Click += HAHVbrowseButton_Click;
            HALVbrowseButton.Click += HALVbrowseButton_Click;
            Accept.Click += Accept_Click;
            Cancel.Click += Cancel_Click;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(HALVfileNameTextBox.Text) &&
                !String.IsNullOrEmpty(HAHVfileNameTextBox.Text) &&
                !String.IsNullOrEmpty(LALVfileNameTextBox.Text) &&
                !String.IsNullOrEmpty(LAHVfileNameTextBox.Text))
            {
                try
                {
                    Console.WriteLine("empezando");
                    var dict = new Dictionary<TagType, List<List<double[]>>>();

                    var halvFile = new StreamReader(HALVfileNameTextBox.Text);
                    var halvString = halvFile.ReadToEnd();
                    dict.Add(TagType.HighArousalLowValence, ReadTsv(halvString));

                    var hahvFile = new StreamReader(HAHVfileNameTextBox.Text);
                    var hahvString = hahvFile.ReadToEnd();
                    dict.Add(TagType.HighArousalHighValence, ReadTsv(hahvString));

                    var lalvFile = new StreamReader(LALVfileNameTextBox.Text);
                    var lalvString = lalvFile.ReadToEnd();
                    dict.Add(TagType.LowArousalLowValence, ReadTsv(lalvString));

                    var lahvFile = new StreamReader(LAHVfileNameTextBox.Text);
                    var lahvString = lahvFile.ReadToEnd();
                    dict.Add(TagType.LowArousalHighValence, ReadTsv(lahvString));

                    Console.WriteLine("procesados, ahora a entrenar");
                    LearningModel.Train(dict);

                    //TODO: terminó bien
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error: {ex}");
                }
                
            }
        }

        private void HALVbrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = ".tsv",
                Filter = "Tab separated value (*.tsv)|*tsv"
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                HALVfileNameTextBox.Text = dlg.FileName;
            }
        }

        private void HAHVbrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = ".tsv",
                Filter = "Tab separated value (*.tsv)|*tsv"
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                HAHVfileNameTextBox.Text = dlg.FileName;
            }
        }

        private void LALVbrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = ".tsv",
                Filter = "Tab separated value (*.tsv)|*tsv"
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                LALVfileNameTextBox.Text = dlg.FileName;
            }
        }

        private void LAHVbrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = ".tsv",
                Filter = "Tab separated value (*.tsv)|*tsv"
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                LAHVfileNameTextBox.Text = dlg.FileName;
            }
        }

        private List<List<double[]>> ReadTsv(string tsvString)
        {
            var result = new List<List<double[]>>();
            var frame = new List<double[]>();
            var lines = tsvString.Split('\n');
            Console.WriteLine($"lineas: {lines.Length}");
            bool first = true;
            int secStart = 0;
            int i = 0;
            foreach (var line in lines)
            {
                if (secStart > 20) break;
                if (first)
                {
                    first = false;
                    continue;
                }
                var fields = line.Split('\t');
                if (fields.Length < 4)
                    break;
                if(secStart == 0)
                    secStart = i/256;
                double[] values = new double[2];
                bool added = false;
                switch (EEGEmoProc2ChSettings.Instance.SamplingHz.Value)
                {
                    case 128:
                        if (int.Parse(fields[1]) % 2 == 0)
                        {
                            values[0] = double.Parse(fields[2]);
                            values[1] = double.Parse(fields[3]);
                            added = true;
                        }
                        break;
                    case 256:
                        values[0] = double.Parse(fields[2]);
                        values[1] = double.Parse(fields[3]);
                        added = true;
                        break;
                }
                if (added && i/256 < secStart + EEGEmoProc2ChSettings.Instance.secs)
                {
                    frame.Add(values);
                }
                else
                {
                    result.Add(frame);
                    frame = new List<double[]>();
                    secStart = i / 256;
                    Console.WriteLine($"frame añadido en el segundo {secStart}");
                }
                i++;
            }
            return result;
        }

        public object Clone()
        {
            return new TrainerFileSelector();
        }
    }
}
