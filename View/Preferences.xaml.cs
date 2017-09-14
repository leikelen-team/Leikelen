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

namespace cl.uv.leikelen.View
{
    /// <summary>
    /// Lógica de interacción para Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {
        public Preferences()
        {
            InitializeComponent();
            var generalTab = new Widget.PreferencesGeneral();
            var DbTab = new Widget.PreferencesBD();
            var colorsTab = new Widget.PreferencesColor();
            var personColorsTab = new Widget.PreferencesPersonColors();
            Tabs.AddToSource(generalTab);
            Tabs.AddToSource(DbTab);
            Tabs.AddToSource(colorsTab);
            Tabs.AddToSource(personColorsTab);

            DbTab.CancelBtn.Click += CancelBtn_Click;
            generalTab.CancelBtn.Click += CancelBtn_Click;
            personColorsTab.CancelBtn.Click += CancelBtn_Click;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ShowBd()
        {
            Tabs.SelectedIndex = 1;
            Show();
        }
    }
}
