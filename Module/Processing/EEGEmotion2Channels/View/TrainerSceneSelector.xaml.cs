using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;
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

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.View
{
    /// <summary>
    /// Lógica de interacción para TrainerSceneSelector.xaml
    /// </summary>
    public partial class TrainerSceneSelector : Window, ICloneable
    {
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public TrainerSceneSelector()
        {
            InitializeComponent();

            ScenesDataGrid.ItemsSource = _dataAccessFacade.GetSceneAccess().GetAll();

            TagCmbx.SelectionChanged += TagCmbx_SelectionChanged;
            AddScenesToTag.Click += AddScenesToTag_Click;
        }

        private void TagCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tag = (TagType)TagCmbx.SelectedIndex;
            if (TagCmbx.SelectedIndex >= 0)
            {
                ScenesAddedDataGrid.ItemsSource = TrainerEntryPoint.ScenesAndTags[tag];
            }
        }

        private void AddScenesToTag_Click(object sender, RoutedEventArgs e)
        {
            var scenes = ScenesAddedDataGrid.SelectedItems as List<Scene>;
            if (TagCmbx.SelectedIndex >= 0)
            {
                var tag = (TagType)TagCmbx.SelectedIndex;
                if (scenes != null)
                {
                    foreach (var scene in scenes)
                    {
                        TrainerEntryPoint.ScenesAndTags[tag].Add(scene);
                    }
                }
            }
        }

        public object Clone()
        {
            return new TrainerSceneSelector();
        }
    }
}
