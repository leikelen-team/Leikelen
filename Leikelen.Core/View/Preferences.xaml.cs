using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using cl.uv.leikelen.View.Widget.PreferencesTab;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.View
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Preferences"/> class.
        /// </summary>
        public Preferences()
        {
            InitializeComponent();
            var generalTab = new PreferencesGeneral();
            var DbTab = new PreferencesBD();
            var colorsTab = new PreferencesColor();
            var personColorsTab = new PreferencesPersonColors();
            Tabs.AddToSource(generalTab);
            Tabs.AddToSource(DbTab);
            //Tabs.AddToSource(colorsTab);
            //Tabs.AddToSource(personColorsTab);

            CancelBtn.Click += CancelBtn_Click;
            AcceptBtn.Click += AcceptBtn_Click;
            ApplytBtn.Click += ApplytBtn_Click;
        }

        private void ApplytBtn_Click(object sender, RoutedEventArgs e)
        {
            var selTab = Tabs.SelectedItem as IPreference;
            selTab?.Apply();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach(var tab in Tabs.Items)
            {
                var prefTab = Tabs.SelectedItem as IPreference;
                prefTab?.Apply();
            }
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Shows the window in the database tab.
        /// </summary>
        public void ShowBd()
        {
            Tabs.SelectedIndex = 1;
            Show();
        }
    }
}
