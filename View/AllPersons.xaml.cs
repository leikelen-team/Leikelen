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
            AssignBtn.Click += AssignBtn_Click;
        }

        private void AssignBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            var configurePersonWin = new ConfigurePerson();
            configurePersonWin.Show();
        }
    }
}
