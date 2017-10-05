using cl.uv.leikelen.Util;
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

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.View
{
    /// <summary>
    /// Lógica de interacción para ClassifierWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window, ICloneable
    {

        public ConfigurationWindow(string title)
        {
            InitializeComponent();
            Title = title;
            

            SamplingCmbx.ItemsSource = new int[] { 256, 128 };
            SamplingCmbx.SelectedIndex = 0;
            mTextBox.Text = EEGEmoProc2ChSettings.Instance.m.Value.ToString();
            rTextBox.Text = EEGEmoProc2ChSettings.Instance.r.Value.ToString();
            SecsTextBox.Text = EEGEmoProc2ChSettings.Instance.secs.Value.ToString();
            NTextBox.Text = EEGEmoProc2ChSettings.Instance.N.Value.ToString();
            shiftTextBox.Text = EEGEmoProc2ChSettings.Instance.shift.Value.ToString();
            

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
                if (Int32.TryParse(mTextBox.Text, out int m) && Double.TryParse(rTextBox.Text, out double r) &&
                    Int32.TryParse(SecsTextBox.Text, out int secs) && Int32.TryParse(NTextBox.Text, out int n) &&
                    Int32.TryParse(shiftTextBox.Text, out int shift))
                {
                    EEGEmoProc2ChSettings.Instance.SamplingHz.Write((int)SamplingCmbx.SelectedItem);
                    EEGEmoProc2ChSettings.Instance.m.Write(m);
                    EEGEmoProc2ChSettings.Instance.r.Write(r);
                    EEGEmoProc2ChSettings.Instance.secs.Write(secs);
                    EEGEmoProc2ChSettings.Instance.N.Write(n);
                    EEGEmoProc2ChSettings.Instance.shift.Write(shift);
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

        private void Int_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            IntegerInput.PastingHandler(sender, e);
        }

        private void Int_PreviewText(object sender, TextCompositionEventArgs e)
        {
            IntegerInput.PreviewTextInputHandler(sender, e);
        }

        public object Clone()
        {
            return new ConfigurationWindow(Title);
        }
    }
}
