using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;
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

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels.View
{
    /// <summary>
    /// Lógica de interacción para ClassifierWindow.xaml
    /// </summary>
    public partial class ClassifierWindow : Window, ICloneable
    {
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public ClassifierWindow()
        {
            InitializeComponent();
            SamplingCmbx.ItemsSource = new int[] { 256, 128 };
            SamplingCmbx.SelectedIndex = 0;
            mTextBox.Text = EEGEmoProc2ChSettings.Instance.m.Value.ToString();
            rTextBox.Text = EEGEmoProc2ChSettings.Instance.r.Value.ToString();
            SecsTextBox.Text = EEGEmoProc2ChSettings.Instance.secs.Value.ToString();

            
            Accept.Click += AcceptBtnOnClick;
            Cancel.Click += Cancel_Click;
        }

        

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptBtnOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult sureResult = MessageBox.Show(Properties.EEGEmotion2Channels.AreSure, Properties.EEGEmotion2Channels.Confirmation, MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (sureResult == MessageBoxResult.Yes)
            {
                int m = 0;
                int secs = 0;
                double r = 0;
                if (Int32.TryParse(mTextBox.Text, out m) && Double.TryParse(rTextBox.Text, out r) &&
                    Int32.TryParse(SecsTextBox.Text, out secs))
                {
                    EEGEmoProc2ChSettings.Instance.SamplingHz.Write((int) SamplingCmbx.SelectedItem);
                    EEGEmoProc2ChSettings.Instance.m.Write(m);
                    EEGEmoProc2ChSettings.Instance.r.Write(r);
                    EEGEmoProc2ChSettings.Instance.secs.Write(secs);
                    Close();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(Properties.EEGEmotion2Channels.ErrorWindowValues,
                        Properties.EEGEmotion2Channels.ErrorWindowValuesTitle, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }

        }

        public object Clone()
        {
            return new ClassifierWindow();
        }
    }
}
