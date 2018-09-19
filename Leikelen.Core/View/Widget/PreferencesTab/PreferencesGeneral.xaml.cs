﻿using System;
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
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.Util;
using WinForms = System.Windows.Forms;

namespace cl.uv.leikelen.View.Widget.PreferencesTab
{
    /// <summary>
    /// Interaction logic for PreferencesGeneral.xaml
    /// </summary>
    public partial class PreferencesGeneral : TabItem, IPreference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreferencesGeneral"/> class.
        /// </summary>
        public PreferencesGeneral()
        {
            InitializeComponent();

            TmpDirectoryTxt.Text = GeneralSettings.Instance.TmpDirectory.Value;
            DataDirectoryTxt.Text = GeneralSettings.Instance.DataDirectory.Value;
            TmpSceneDirectoryTxt.Text = GeneralSettings.Instance.TmpSceneDirectory.Value;
            DefaultMillisecondsTxt.Text = GeneralSettings.Instance.DefaultMillisecondsThreshold.Value.ToString();
            IntervalMinHeightTxt.Text = GeneralSettings.Instance.IntervalsGraphMinHeight.Value.ToString();
            EventMinHeightTxt.Text = GeneralSettings.Instance.EventsGraphMinHeight.Value.ToString();
            SecondshresholdPdfTxt.Text = GeneralSettings.Instance.SecsWaitPdfReport.Value.ToString();
            MaxNumberin2DTxt.Text = GeneralSettings.Instance.MaxNumberPointsInEventGraph.Value.ToString();
        }

        void API.Helper.IPreference.Apply()
        {
            GeneralSettings.Instance.TmpDirectory.Write(TmpDirectoryTxt.Text);
            GeneralSettings.Instance.DataDirectory.Write(DataDirectoryTxt.Text);
            GeneralSettings.Instance.TmpSceneDirectory.Write(TmpSceneDirectoryTxt.Text);
            if (int.TryParse(DefaultMillisecondsTxt.Text, out int millSec) &&
                int.TryParse(IntervalMinHeightTxt.Text, out int intervalMinHeight) &&
                int.TryParse(EventMinHeightTxt.Text, out int eventsMinHeight) &&
                int.TryParse(SecondshresholdPdfTxt.Text, out int secondsWaitPdf) &&
                int.TryParse(MaxNumberin2DTxt.Text, out int maxNumberIn2d))
            {
                GeneralSettings.Instance.DefaultMillisecondsThreshold.Write(millSec);
                GeneralSettings.Instance.IntervalsGraphMinHeight.Write(intervalMinHeight);
                GeneralSettings.Instance.EventsGraphMinHeight.Write(eventsMinHeight);
                GeneralSettings.Instance.SecsWaitPdfReport.Write(secondsWaitPdf);
                GeneralSettings.Instance.MaxNumberPointsInEventGraph.Write(maxNumberIn2d);
            }
        }

        private void Int_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IntegerInput.PreviewTextInputHandler(sender, e);
        }

        private void Int_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            IntegerInput.PastingHandler(sender, e);
        }

        private void DataDirectoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new WinForms.FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = Properties.GUI.DataDirectory
            };
            WinForms.DialogResult result = folderDialog.ShowDialog();
            if (result.Equals(WinForms.DialogResult.OK))
            {
                DataDirectoryTxt.Text = folderDialog.SelectedPath;
            }
        }

        private void TmpDirectoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new WinForms.FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = Properties.GUI.TmpDirectory
            };
            WinForms.DialogResult result = folderDialog.ShowDialog();
            if (result.Equals(WinForms.DialogResult.OK))
            {
                TmpDirectoryTxt.Text = folderDialog.SelectedPath;
            }
        }

        private void TmpSceneDirectoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new WinForms.FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = Properties.GUI.CurrentScene
            };
            WinForms.DialogResult result = folderDialog.ShowDialog();
            if (result.Equals(WinForms.DialogResult.OK))
            {
                TmpSceneDirectoryTxt.Text = folderDialog.SelectedPath;
            }
        }
    }
}
