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

            ScenesDataGrid.ItemsSource = _dataAccessFacade.GetSceneAccess().GetAll();

            TagCmbx.SelectionChanged += TagCmbx_SelectionChanged;
            AddScenesToTag.Click += AddScenesToTag_Click;
            Accept.Click += AcceptBtnOnClick;
            Cancel.Click += Cancel_Click;
        }

        private void TagCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tag = (TagType)TagCmbx.SelectedIndex;
            if (TagCmbx.SelectedIndex >= 0)
            {
                ScenesAddedDataGrid.ItemsSource = TrainerEntryPoint.ScenesAndTags[tag];
            }
        }

        private void AddScenesToTag_Click(object sender, RoutedEventArgs e)
        {
            var scenes = ScenesAddedDataGrid.SelectedItems as List<Scene>;
            if(TagCmbx.SelectedIndex >= 0)
            {
                var tag = (TagType)TagCmbx.SelectedIndex;
                if (scenes != null)
                {
                    foreach (var scene in scenes)
                    {
                        TrainerEntryPoint.ScenesAndTags[tag].Add(scene);
                    }
                }
            }
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
                if (TagCmbx.SelectedIndex != -1 && Int32.TryParse(mTextBox.Text, out m) && Double.TryParse(rTextBox.Text, out r) &&
                    Int32.TryParse(SecsTextBox.Text, out secs))
                {
                    EEGEmoProc2ChSettings.Instance.SamplingHz.Write((int) SamplingCmbx.SelectedItem);
                    EEGEmoProc2ChSettings.Instance.m.Write(m);
                    EEGEmoProc2ChSettings.Instance.r.Write(r);
                    EEGEmoProc2ChSettings.Instance.secs.Write(secs);
                    EEGEmoProc2ChSettings.Instance.TagToTrain.Write(TagCmbx.SelectedIndex);
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
