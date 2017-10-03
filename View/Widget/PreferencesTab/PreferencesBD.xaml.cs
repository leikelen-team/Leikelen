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
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.View.Widget.PreferencesTab
{
    /// <summary>
    /// Lógica de interacción para PreferencesBD.xaml
    /// </summary>
    public partial class PreferencesBD : TabItem, IPreference
    {
        public PreferencesBD()
        {
            InitializeComponent();

            EngineCmbx.ItemsSource = DbFacade.Instance.DbEngineList.Keys;
            EngineCmbx.SelectedItem = GeneralSettings.Instance.Database.Value;
            HostTxt.Text = GeneralSettings.Instance.DbHost.Value;
            PortTxt.Text = GeneralSettings.Instance.DbPort.Value == -1 
                ? DbFacade.Instance.DbEngineList[(string)EngineCmbx.SelectedItem].DefaultPort.ToString() 
                : GeneralSettings.Instance.DbPort.Value.ToString();
            NameTxt.Text = GeneralSettings.Instance.DbName.Value;
            UserTxt.Text = GeneralSettings.Instance.DbUser.Value;
            PasswordTxt.Text = GeneralSettings.Instance.DbPassword.Value;

            EngineCmbx.SelectionChanged += EngineCmbx_SelectionChanged;
        }

        private void EngineCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PortTxt.Text = GeneralSettings.Instance.DbPort.Value == -1
                ? DbFacade.Instance.DbEngineList[(string)EngineCmbx.SelectedItem].DefaultPort.ToString()
                : GeneralSettings.Instance.DbPort.Value.ToString();
        }

        public void Apply()
        {
            string error = "";
            int port = 0;
            if (!int.TryParse(PortTxt.Text, out port))
            {
                error += "\n"+Properties.GUI.PrefBDErrorPortEmpty;
            }
            if(ReferenceEquals(null, EngineCmbx))
            {
                error += "\n"+Properties.GUI.PrefBDErrorEngineEmpty;
            }
            if (!DbFacade.Instance.DbEngineList.ContainsKey((string)EngineCmbx?.SelectedItem))
            {
                error += "\n" + Properties.GUI.PrefBDErrorEngineDoesntExists;
            }
            if (String.IsNullOrEmpty(HostTxt.Text))
            {
                error += "\n"+Properties.GUI.PrefBDErrorHostEmpty;
            }
            if (String.IsNullOrEmpty(NameTxt.Text))
            {
                error += "\n"+Properties.GUI.PrefBDErrorDbNameEmpty;
            }
            if (String.IsNullOrEmpty(UserTxt.Text))
            {
                error += "\n"+Properties.GUI.PrefBDErrorUserEmpty;
            }
            if (String.IsNullOrEmpty(PasswordTxt.Text))
            {
                error += "\n"+Properties.GUI.PrefBDErrorPassEmpty;
            }
            if (String.IsNullOrEmpty(error))
            {
                if (GeneralSettings.Instance.Database.Value.Equals((string)EngineCmbx.SelectedItem)
                    && GeneralSettings.Instance.DbHost.Value.Equals(HostTxt.Text)
                    && GeneralSettings.Instance.DbName.Value.Equals(NameTxt.Text)
                    && GeneralSettings.Instance.DbUser.Value.Equals(UserTxt.Text)
                    && GeneralSettings.Instance.DbPassword.Value.Equals(PasswordTxt.Text)
                    && (GeneralSettings.Instance.DbPort.Value.Equals(port)
                        || (port == -1 
                            && GeneralSettings.Instance.DbPort.Value
                            .Equals(DbFacade.Instance.DbEngineList[NameTxt.Text].DefaultPort))))
                {
                    //the fields are the same than actually configured
                    MessageBox.Show(Properties.GUI.PrefBDErrorSame,
                    Properties.GUI.PrefBDErrorSameTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string testError = DbFacade.Instance.TestError((string)EngineCmbx.SelectedItem,
                        HostTxt.Text, port, NameTxt.Text, UserTxt.Text, PasswordTxt.Text);
                    if (String.IsNullOrEmpty(testError))
                    {
                        //successfully connection
                        //it will be reset the modules
                        MessageBoxResult result = MessageBox.Show(Properties.GUI.PrefBDWillNew,
                            Properties.GUI.PrefBDWillNewTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        if(result == MessageBoxResult.OK)
                        {
                            try
                            {
                                GeneralSettings.Instance.Database.Write((string)EngineCmbx.SelectedItem);
                                GeneralSettings.Instance.DbHost.Write(HostTxt.Text);
                                GeneralSettings.Instance.DbName.Write(NameTxt.Text);
                                GeneralSettings.Instance.DbUser.Write(UserTxt.Text);
                                GeneralSettings.Instance.DbPassword.Write(PasswordTxt.Text);
                                GeneralSettings.Instance.DbPort.Write(port);

                                DbFacade.Reset();

                                Module.GeneralLoader.Reset();
                                Module.ProcessingLoader.Reset();

                                MessageBox.Show(Properties.GUI.PrefBDConnectedOk,
                                Properties.GUI.PrefBDConnectedOkTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(Properties.GUI.PrefBDError + "\n" + ex.Message,
                                Properties.GUI.PrefBDErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        
                    }
                    else
                    {
                        //error at connect
                        MessageBox.Show(Properties.GUI.PrefBDErrorConnect+":\n"+testError,
                        Properties.GUI.PrefBDErrorConnectTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                //fields with errors
                MessageBox.Show(Properties.GUI.PrefBDError+error,
                    Properties.GUI.PrefBDErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
