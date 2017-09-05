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
    /// Lógica de interacción para AllScenes.xaml
    /// </summary>
    public partial class AllScenes : Window
    {
        public AllScenes()
        {
            InitializeComponent();
            
            PersonDataGrid.ItemsSource = DataAccessFacade.Instance.GetSceneAccess().GetAll();

            OpenBtn.Click += OpenBtn_Click;
            DeleteBtn.Click += DeleteBtn_Click;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
