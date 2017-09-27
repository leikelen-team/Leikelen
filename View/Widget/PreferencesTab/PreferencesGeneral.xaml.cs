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
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.View.Widget.PreferencesTab
{
    /// <summary>
    /// Lógica de interacción para PreferencesGeneral.xaml
    /// </summary>
    public partial class PreferencesGeneral : TabItem, IPreference
    {
        public PreferencesGeneral()
        {
            InitializeComponent();

            TmpDirectoryTxt.Text = GeneralSettings.Instance.TmpDirectory.Value;
            DataDirectoryTxt.Text = GeneralSettings.Instance.DataDirectory.Value;
            TmpSceneDirectoryTxt.Text = GeneralSettings.Instance.TmpSceneDirectory.Value;
            DefaultMillisecondsTxt.Text = GeneralSettings.Instance.DefaultMillisecondsThreshold.Value.ToString();
            
        }

        public void Apply()
        {
            GeneralSettings.Instance.TmpDirectory.Write(TmpDirectoryTxt.Text);
            GeneralSettings.Instance.DataDirectory.Write(DataDirectoryTxt.Text);
            GeneralSettings.Instance.TmpSceneDirectory.Write(TmpSceneDirectoryTxt.Text);
            if (int.TryParse(DefaultMillisecondsTxt.Text, out int millSec))
            {
                GeneralSettings.Instance.DefaultMillisecondsThreshold.Write(millSec);
            }
        }
    }
}
