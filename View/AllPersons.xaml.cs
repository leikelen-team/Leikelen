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
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.View
{
    /// <summary>
    /// Lógica de interacción para AllPersons.xaml
    /// </summary>
    public partial class AllPersons : Window
    {
        public AllPersons()
        {
            InitializeComponent();
            
            PersonDataGrid.ItemsSource = DataAccessFacade.Instance.GetPersonAccess().GetAll();

            NewBtn.Click += NewBtn_Click;
            EditBtn.Click += EditBtn_Click;
            AddBtn.Click += AddBtn_Click;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var person = PersonDataGrid.SelectedItem as Person;
            if (person != null)
            {
                DataAccessFacade.Instance.GetPersonAccess().AddToScene(person, 
                    DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());
                Close();
            }
            else
            {
                MessageBox.Show(Properties.GUI.AllPersonsNotExists, 
                    Properties.GUI.AllPersonsNotExistsTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var person = PersonDataGrid.SelectedItem as Person;
            if(person != null)
            {
                var configurePersonWin = new ConfigurePerson(person);
                configurePersonWin.Show();
                configurePersonWin.Closed += (confSender, confE) =>
                {
                    PersonDataGrid.ItemsSource = DataAccessFacade.Instance.GetPersonAccess().GetAll();
                };
            }
            else
            {
                MessageBox.Show(Properties.GUI.AllPersonsNotExists,
                    Properties.GUI.AllPersonsNotExistsTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            var configurePersonWin = new ConfigurePerson();
            configurePersonWin.Show();
            configurePersonWin.Closed += (confSender, confE) =>
            {
                PersonDataGrid.ItemsSource = DataAccessFacade.Instance.GetPersonAccess().GetAll();
            };
        }
    }
}
