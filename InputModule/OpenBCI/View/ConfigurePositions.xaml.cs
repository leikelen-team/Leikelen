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
using System.Collections.ObjectModel;
using cl.uv.leikelen.InputModule.OpenBCI.Properties;

namespace cl.uv.leikelen.InputModule.OpenBCI.View
{
    /// <summary>
    /// Lógica de interacción para ConfigurePositions.xaml
    /// </summary>
    public partial class ConfigurePositions : Window
    {
        private Window _backWindow;

        private ReadOnlyCollection<string> _positions1020 = new ReadOnlyCollection<string>( new string[]{"Fpz", "Fp1", "Fp2", "Fz", "F3", "F4", "F7", "F8", "Cz", "C3", "C4", "T3", "T4", "T5", "T6", "Pz", "P3", "P4", "Oz", "O1", "O2"});
        private ReadOnlyCollection<string> _positions1010 = new ReadOnlyCollection<string>(new string[] { "Fpz", "Fp1", "Fp2", "AFz","AF3", "AF4", "AF7", "AF8", "Fz", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "FCz", "FC1", "FC2", "FC3", "FC4", "FC5", "FC6", "FT7", "FT8", "FT9", "FT10", "Cz", "C1", "C2", "C3", "C4", "C5", "C6", "T3", "T4", "T5", "T6", "T9", "T10", "CPz", "CP1", "CP2", "CP3", "CP4", "CP5", "CP6", "TP7", "TP8", "TP9", "TP10", "Pz", "P1", "P2", "P3", "P4", "P5", "P6", "P9", "P10", "POz", "PO3", "PO4", "PO7", "PO8", "Oz", "O1", "O2" });

        public ConfigurePositions(Window backWindow)
        {
            InitializeComponent();

            _backWindow = backWindow;
            posSystemCmbx.SelectionChanged += PosSystemCmbx_SelectionChanged;
            posSystemCmbx.SelectedIndex = 0;
            CancelBtn.Click += CancelBtn_Click;
            AcceptBtn.Click += AcceptBtn_Click;
            BackBtn.Click += BackBtn_Click;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _backWindow.Show();
            Close();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(OpenBCI.Properties.OpenBCI.AreSure, OpenBCI.Properties.OpenBCI.Confirmation, MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                OpenBCISettings.Instance.PositionChannel1.Write((string) ChannelCmbx1.SelectedItem);
                OpenBCISettings.Instance.PositionChannel1.Write((string) ChannelCmbx1.SelectedItem);
                OpenBCISettings.Instance.PositionChannel1.Write((string) ChannelCmbx1.SelectedItem);
                OpenBCISettings.Instance.PositionChannel1.Write((string) ChannelCmbx1.SelectedItem);
                OpenBCISettings.Instance.PositionChannel1.Write((string) ChannelCmbx1.SelectedItem);
                OpenBCISettings.Instance.PositionChannel1.Write((string) ChannelCmbx1.SelectedItem);
                OpenBCISettings.Instance.PositionChannel1.Write((string) ChannelCmbx1.SelectedItem);
                OpenBCISettings.Instance.PositionChannel1.Write((string) ChannelCmbx1.SelectedItem);
                _backWindow.Close();
                Close();
                
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _backWindow.Close();
            Close();
            
        }

        private void PosSystemCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            
            if (comboBox != null)
            {
                var posSystem = comboBox.SelectedIndex;
                switch (posSystem)
                {
                    case 0:
                        Console.WriteLine("es 10/10");
                        PositionPhotoImage.Source =
                            new BitmapImage(new Uri("../Assets/eegPosSystem_10_10.png", UriKind.Relative));
                        ChannelCmbx1.ItemsSource = _positions1010;
                        ChannelCmbx2.ItemsSource = _positions1010;
                        ChannelCmbx3.ItemsSource = _positions1010;
                        ChannelCmbx4.ItemsSource = _positions1010;
                        ChannelCmbx5.ItemsSource = _positions1010;
                        ChannelCmbx6.ItemsSource = _positions1010;
                        ChannelCmbx7.ItemsSource = _positions1010;
                        ChannelCmbx8.ItemsSource = _positions1010;
                        break;
                    case 1:
                        Console.WriteLine("es 10/20");
                        PositionPhotoImage.Source =
                            new BitmapImage(new Uri("../Assets/eegPosSystem_10_20.png", UriKind.Relative));
                        ChannelCmbx1.ItemsSource = _positions1020;
                        ChannelCmbx2.ItemsSource = _positions1020;
                        ChannelCmbx3.ItemsSource = _positions1020;
                        ChannelCmbx4.ItemsSource = _positions1020;
                        ChannelCmbx5.ItemsSource = _positions1020;
                        ChannelCmbx6.ItemsSource = _positions1020;
                        ChannelCmbx7.ItemsSource = _positions1020;
                        ChannelCmbx8.ItemsSource = _positions1020;
                        break;
                }
            }
        }
    }
}
