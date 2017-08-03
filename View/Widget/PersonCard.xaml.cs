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
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.View.Widget
{
    /// <summary>
    /// Lógica de interacción para PersonCard.xaml
    /// </summary>
    public partial class PersonCard : UserControl
    {
        public PersonCard()
        {
            InitializeComponent();
        }

        public Control GetPerson(Person person)
        {
            Name.Content = person.Name;
            string photoUri = GeneralSettings.Instance.TmpDirectory + GeneralSettings.Instance.CurrentSceneDirectory + person.Photo;
            Photo.Source = new BitmapImage(new Uri(photoUri, UriKind.Relative));
            return Card;
        }
    }
}
