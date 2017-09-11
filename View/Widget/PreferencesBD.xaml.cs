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
using cl.uv.leikelen.Data.Persistence;

namespace cl.uv.leikelen.View.Widget
{
    /// <summary>
    /// Lógica de interacción para PreferencesBD.xaml
    /// </summary>
    public partial class PreferencesBD : TabItem
    {
        public PreferencesBD()
        {
            InitializeComponent();

            EngineCmbx.ItemsSource = DbFacade.Instance.DbEngineList.Keys;
            EngineCmbx.SelectedItem = GeneralSettings.Instance.Database.Value;
            HostTxt.Text = GeneralSettings.Instance.DbHost.Value;
            PortTxt.Text = GeneralSettings.Instance.DbPort.Value == -1 
                ? DbFacade.Instance.DbEngineList[GeneralSettings.Instance.Database.Value].DefaultPort.ToString() 
                : GeneralSettings.Instance.DbPort.Value.ToString();
            NameTxt.Text = GeneralSettings.Instance.DbName.Value;
            UserTxt.Text = GeneralSettings.Instance.DbUser.Value;
            PasswordTxt.Text = GeneralSettings.Instance.DbPassword.Value;

            AcceptBtn.Click += AcceptBtn_Click;
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.GUI.BdWillNew, Properties.GUI.BdWillNewTitle, MessageBoxButton.YesNo,
                   MessageBoxImage.Exclamation);
            if(result == MessageBoxResult.Yes)
            {
                int port = 0;
                if (int.TryParse(PortTxt.Text, out port) && EngineCmbx != null && !String.IsNullOrEmpty(HostTxt.Text)
                    && !String.IsNullOrEmpty(HostTxt.Text) && !String.IsNullOrEmpty(NameTxt.Text)
                    && !String.IsNullOrEmpty(UserTxt.Text) && !String.IsNullOrEmpty(PasswordTxt.Text))
                {
                    GeneralSettings.Instance.Database.Write((string)EngineCmbx.SelectedItem);
                    GeneralSettings.Instance.DbHost.Write(HostTxt.Text);
                    GeneralSettings.Instance.DbName.Write(NameTxt.Text);
                    GeneralSettings.Instance.DbUser.Write(UserTxt.Text);
                    GeneralSettings.Instance.DbPassword.Write(PasswordTxt.Text);
                    GeneralSettings.Instance.DbPort.Write(port);
                }
                DbFacade.Reset();
            }
        }
    }
}
