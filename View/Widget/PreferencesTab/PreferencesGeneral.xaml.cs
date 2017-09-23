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

namespace cl.uv.leikelen.View.Widget.PreferencesTab
{
    /// <summary>
    /// Lógica de interacción para PreferencesGeneral.xaml
    /// </summary>
    public partial class PreferencesGeneral : TabItem
    {
        public PreferencesGeneral()
        {
            InitializeComponent();

            TmpDirectoryTxt.Text = GeneralSettings.Instance.TmpDirectory.Value;
            DataDirectoryTxt.Text = GeneralSettings.Instance.DataDirectory.Value;
            CurrentSceneTxt.Text = GeneralSettings.Instance.SceneInUseDirectory.Value;
            DefaultMillisecondsTxt.Text = GeneralSettings.Instance.DefaultMillisecondsThreshold.Value.ToString();

            AcceptBtn.Click += AcceptBtn_Click;
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            GeneralSettings.Instance.TmpDirectory.Write(TmpDirectoryTxt.Text);
            GeneralSettings.Instance.DataDirectory.Write(DataDirectoryTxt.Text);
            GeneralSettings.Instance.SceneInUseDirectory.Write(CurrentSceneTxt.Text);
            GeneralSettings.Instance.DefaultMillisecondsThreshold.Write(int.Parse(DefaultMillisecondsTxt.Text));
        }
    }
}
