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
using Microsoft.Data.Sqlite;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.View
{
    /// <summary>
    /// Lógica de interacción para TrainerFileSelector.xaml
    /// </summary>
    public partial class TrainerFileSelector : Window, ICloneable
    {
        private const string _defaultExt = ".db";
        private const string _filter = "All supported files (*.db, *.sqlite, *.sqlite3, *.tsv)|*.db; *.sqlite; *.sqlite3; *.tsv|Sqlite3 Database (*.db, *.sqlite, *.sqlite3)|*.db; *.sqlite; *.sqlite3|Tab separated value (*.tsv)|*.tsv";
        public TrainerFileSelector()
        {
            InitializeComponent();

            LAHVbrowseButton.Click += LAHVbrowseButton_Click;
            LALVbrowseButton.Click += LALVbrowseButton_Click;
            HAHVbrowseButton.Click += HAHVbrowseButton_Click;
            HALVbrowseButton.Click += HALVbrowseButton_Click;
            Accept.Click += Accept_Click;
            Cancel.Click += Cancel_Click;
            UseInternalDataChbx.Checked += UseInternalDataChbx_Checked;
            UseInternalDataChbx.Unchecked += UseInternalDataChbx_Unchecked;
        }

        private void UseInternalDataChbx_Unchecked(object sender, RoutedEventArgs e)
        {
            HALVfileNameTextBox.IsEnabled = true;
            HAHVfileNameTextBox.IsEnabled = true;
            LALVfileNameTextBox.IsEnabled = true;
            LAHVfileNameTextBox.IsEnabled = true;

            LAHVbrowseButton.IsEnabled = true;
            LALVbrowseButton.IsEnabled = true;
            HAHVbrowseButton.IsEnabled = true;
            HALVbrowseButton.IsEnabled = true;
        }

        private void UseInternalDataChbx_Checked(object sender, RoutedEventArgs e)
        {
            HALVfileNameTextBox.IsEnabled = false;
            HAHVfileNameTextBox.IsEnabled = false;
            LALVfileNameTextBox.IsEnabled = false;
            LAHVfileNameTextBox.IsEnabled = false;

            LAHVbrowseButton.IsEnabled = false;
            LALVbrowseButton.IsEnabled = false;
            HAHVbrowseButton.IsEnabled = false;
            HALVbrowseButton.IsEnabled = false;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if ((UseInternalDataChbx.IsChecked.HasValue && UseInternalDataChbx.IsChecked.Value) || 
                (!String.IsNullOrEmpty(HALVfileNameTextBox.Text) &&
                !String.IsNullOrEmpty(HAHVfileNameTextBox.Text) &&
                !String.IsNullOrEmpty(LALVfileNameTextBox.Text) &&
                !String.IsNullOrEmpty(LAHVfileNameTextBox.Text)))
            {
                try
                {
                    Console.WriteLine("empezando");
                    var dict = new Dictionary<TagType, List<List<double[]>>>();

                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("A procesar HALV, faltan 4");
                    if (UseInternalDataChbx.IsChecked.HasValue && UseInternalDataChbx.IsChecked.Value)
                    {
                        //dict.Add(TagType.HALV, ReadFromData(Data.HALV.data));
                    }
                    else if (System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".tsv"))
                    {
                        var halvFile = new StreamReader(HALVfileNameTextBox.Text);
                        var halvString = halvFile.ReadToEnd();
                        dict.Add(TagType.HALV, ReadTsv(halvString));
                    }
                    else if (System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".db") ||
                        System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".sqlite") ||
                        System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".sqlite3"))
                    {
                        dict.Add(TagType.HALV, ReadSqlite(HALVfileNameTextBox.Text));
                    }


                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("A procesar HAHV, faltan 3");
                    if (UseInternalDataChbx.IsChecked.HasValue && UseInternalDataChbx.IsChecked.Value)
                    {
                        //dict.Add(TagType.HAHV, ReadFromData(Data.HAHV.data));
                    }
                    else if (System.IO.Path.GetExtension(HAHVfileNameTextBox.Text).Equals(".tsv"))
                    {
                        var hahvFile = new StreamReader(HAHVfileNameTextBox.Text);
                        var hahvString = hahvFile.ReadToEnd();
                        dict.Add(TagType.HAHV, ReadTsv(hahvString));
                    }
                    else if (System.IO.Path.GetExtension(HAHVfileNameTextBox.Text).Equals(".db") ||
                        System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".sqlite") ||
                        System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".sqlite3"))
                    {
                        dict.Add(TagType.HAHV, ReadSqlite(HAHVfileNameTextBox.Text));
                    }

                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("A procesar LALV, faltan 2");
                    if (UseInternalDataChbx.IsChecked.HasValue && UseInternalDataChbx.IsChecked.Value)
                    {
                        //dict.Add(TagType.LALV, ReadFromData(Data.LALV.data));
                    }
                    else if (System.IO.Path.GetExtension(LALVfileNameTextBox.Text).Equals(".tsv"))
                    {
                        var lalvFile = new StreamReader(LALVfileNameTextBox.Text);
                        var lalvString = lalvFile.ReadToEnd();
                        dict.Add(TagType.LALV, ReadTsv(lalvString));
                    }
                    else if (System.IO.Path.GetExtension(LALVfileNameTextBox.Text).Equals(".db") ||
                        System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".sqlite") ||
                        System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".sqlite3"))
                    {
                        dict.Add(TagType.LALV, ReadSqlite(LALVfileNameTextBox.Text));
                    }

                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("A procesar LAHV, falta 1");
                    if (UseInternalDataChbx.IsChecked.HasValue && UseInternalDataChbx.IsChecked.Value)
                    {
                        //dict.Add(TagType.LAHV, ReadFromData(Data.LAHV.data));
                    }
                    else if (System.IO.Path.GetExtension(LAHVfileNameTextBox.Text).Equals(".tsv"))
                    {
                        var lahvFile = new StreamReader(LAHVfileNameTextBox.Text);
                        var lahvString = lahvFile.ReadToEnd();
                        dict.Add(TagType.LAHV, ReadTsv(lahvString));
                    }
                    else if (System.IO.Path.GetExtension(LAHVfileNameTextBox.Text).Equals(".db") ||
                        System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".sqlite") ||
                        System.IO.Path.GetExtension(HALVfileNameTextBox.Text).Equals(".sqlite3"))
                    {
                        dict.Add(TagType.LAHV, ReadSqlite(LAHVfileNameTextBox.Text));
                    }


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
                DefaultExt = _defaultExt,
                Filter = _filter
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
                DefaultExt = _defaultExt,
                Filter = _filter
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
                DefaultExt = _defaultExt,
                Filter = _filter
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
                DefaultExt = _defaultExt,
                Filter = _filter
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                LAHVfileNameTextBox.Text = dlg.FileName;
            }
        }

        private List<List<double[]>> ReadSqlite(string fileName)
        {
            string cs = "Filename=" + fileName;
            var result = new List<List<double[]>>();
            Console.WriteLine("procesando: "+fileName);
            using (SqliteConnection con = new SqliteConnection(cs))
            {
                Console.WriteLine("conecta2");
                con.Open();
                Console.WriteLine("abierto");

                string stm = "SELECT * FROM data ORDER BY id";

                using (SqliteCommand cmd = new SqliteCommand(stm, con))
                {
                    Console.WriteLine("comman2");
                    using (SqliteDataReader rdr = cmd.ExecuteReader())
                    {
                        Console.WriteLine("ejecuta3");

                        var frame = new List<double[]>();
                        int secStart = 0;
                        int i = 0;
                        int lastTime = 0;
                        double lastF3 = 0;
                        double lastC4 = 0;
                        int time = 0;

                        while (rdr.Read())
                        {
                            if (secStart == 0)
                            {
                                secStart = rdr.GetInt32(1);
                                lastTime = secStart;
                                time = secStart;
                            }
                            if (rdr.GetInt32(1) < lastTime 
                                || (Math.Abs(rdr.GetInt32(1) - time) != 0 
                                && Math.Abs(rdr.GetInt32(1) - time) != 1))
                            {
                                frame = new List<double[]>();
                                secStart = rdr.GetInt32(1);
                                Console.WriteLine($"nuevo archivo con segundo: {secStart} y el anterior: {lastTime}");
                                lastTime = secStart;
                            }
                            double[] values = new double[2];
                            bool added = false;
                            switch (EEGEmoProc2ChSettings.Instance.SamplingHz.Value)
                            {
                                case 128:
                                    if (rdr.GetInt32(2) % 2 != 0)
                                    {
                                        values[0] = (lastF3 + rdr.GetDouble(3)) / 2;
                                        values[1] = (lastC4 + rdr.GetDouble(4)) / 2;
                                        added = true;
                                    }
                                    break;
                                case 256:
                                    values[0] = rdr.GetDouble(3);
                                    values[1] = rdr.GetDouble(4);
                                    added = true;
                                    break;
                            }
                            if (rdr.GetInt32(1) < secStart + EEGEmoProc2ChSettings.Instance.secs)
                            {
                                if (added)
                                    frame.Add(values);
                            }
                            else
                            {
                                result.Add(frame);
                                frame = new List<double[]>();
                                secStart = rdr.GetInt32(1);
                                Console.WriteLine($"frame añadido en el segundo {secStart}");
                            }
                            i++;
                            time = rdr.GetInt32(1);
                            lastTime = secStart;
                            lastF3 = rdr.GetDouble(3);
                            lastC4 = rdr.GetDouble(4);


                        }
                    }
                }

                con.Close();
                return result;
            }
        }

        private List<List<double[]>> ReadTsv(string tsvString)
        {
            var result = new List<List<double[]>>();
            var frame = new List<double[]>();
            var lines = tsvString.Split('\n');
            Console.WriteLine("lineas:"+ lines.Length);
            bool first = true;
            int secStart = 0;
            int i = 0;
            int lastTime = 0;
            double lastF3 = 0;
            double lastC4 = 0;
            foreach (var line in lines)
            {
                if (i > 30000) break;
                if (first)
                {
                    first = false;
                    continue;
                }
                var fields = line.Split('\t');
                if (fields.Length < 4)
                    break;
                if (secStart == 0)
                {
                    secStart = Int32.Parse(fields[0]);
                    lastTime = secStart;
                } 
                if(Int32.Parse(fields[0]) < lastTime)
                {
                    frame = new List<double[]>();
                    secStart = Int32.Parse(fields[0]);
                    Console.WriteLine($"nuevo archivo con segundo: {secStart} y el anterior: {lastTime}");
                }
                double[] values = new double[2];
                bool added = false;
                switch (EEGEmoProc2ChSettings.Instance.SamplingHz.Value)
                {
                    case 128:
                        if (int.Parse(fields[1]) % 2 != 0)
                        {
                            values[0] = (lastF3 + double.Parse(fields[2])) /2;
                            values[1] = (lastC4 + double.Parse(fields[3])) /2;
                            added = true;
                        }
                        break;
                    case 256:
                        values[0] = double.Parse(fields[2]);
                        values[1] = double.Parse(fields[3]);
                        added = true;
                        break;
                }
                if (Int32.Parse(fields[0]) < secStart + EEGEmoProc2ChSettings.Instance.secs)
                {
                    if(added)
                        frame.Add(values);
                }
                else
                {
                    result.Add(frame);
                    frame = new List<double[]>();
                    secStart = Int32.Parse(fields[0]);
                    Console.WriteLine($"frame añadido en el segundo {secStart}");
                    Console.WriteLine("Lleva "+i+" de: "+lines.Length);
                }
                i++;
                lastTime = secStart;
                lastF3 = double.Parse(fields[2]);
                lastC4 = double.Parse(fields[3]);
            }
            return result;
        }

        public List<List<double[]>> ReadFromData(double[][] data)
        {
            var result = new List<List<double[]>>();
            var frame = new List<double[]>();
            int secStart = 0;
            int i = 0;
            int lastTime = 0;
            double lastF3 = 0;
            double lastC4 = 0;
            foreach (var fields in data)
            {
                if (secStart == 0)
                {
                    secStart = (int)fields[0];
                    lastTime = secStart;
                }
                if (fields[0] < lastTime)
                {
                    frame = new List<double[]>();
                    secStart = (int)fields[0];
                    Console.WriteLine($"nuevo archivo con segundo: {secStart} y el anterior: {lastTime}");
                }
                double[] values = new double[2];
                bool added = false;
                switch (EEGEmoProc2ChSettings.Instance.SamplingHz.Value)
                {
                    case 128:
                        if ((int)fields[1] % 2 != 0)
                        {
                            values[0] = (lastF3 + fields[2]) / 2;
                            values[1] = (lastC4 + fields[3]) / 2;
                            added = true;
                        }
                        break;
                    case 256:
                        values[0] = fields[2];
                        values[1] = fields[3];
                        added = true;
                        break;
                }
                if ((int)(fields[0]) < secStart + EEGEmoProc2ChSettings.Instance.secs)
                {
                    if (added)
                        frame.Add(values);
                }
                else
                {
                    result.Add(frame);
                    frame = new List<double[]>();
                    secStart = (int)(fields[0]);
                    Console.WriteLine($"frame añadido en el segundo {secStart}");
                    Console.WriteLine("Lleva " + i + " de: " + data.Length);
                }
                i++;
                lastTime = secStart;
                lastF3 = fields[2];
                lastC4 = fields[3];
            }
            return result;
        }

        public object Clone()
        {
            return new TrainerFileSelector();
        }
    }
}
