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
using cl.uv.leikelen.Data.Access.Internal;

namespace cl.uv.leikelen.View
{
    /// <summary>
    /// Interaction logic for AllScenes.xaml
    /// </summary>
    public partial class AllScenes : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllScenes"/> class.
        /// </summary>
        public AllScenes()
        {
            InitializeComponent();

            ScenesDataGrid.ItemsSource = DataAccessFacade.Instance.GetSceneAccess().GetAll();

            OpenBtn.Click += OpenBtn_Click;
            DeleteBtn.Click += DeleteBtn_Click;
            checkBxAllScenes.Checked += CheckBxAllScenes_Checked;
            checkBxAllScenes.Unchecked += CheckBxAllScenes_Unchecked;
        }

        private void CheckBxAllScenes_Unchecked(object sender, RoutedEventArgs e)
        {
            ScenesDataGrid.Items.Refresh();
        }

        private void CheckBxAllScenes_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ScenesDataGrid.Items.Count; i++)
            {
                if (ScenesDataGrid.Items[i] is Scene scene)
                {
                    if (checkBxAllScenes?.IsChecked == true)
                    {
                        scene.Selected = true;
                    }
                }
            }
            ScenesDataGrid.Items.Refresh();

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ScenesDataGrid.Items.Count; i++)
            {
                if (ScenesDataGrid.Items[i] is Scene scene && scene.Selected)
                {
                    DataAccessFacade.Instance.GetSceneAccess().Delete(scene);
                }
            }
            ScenesDataGrid.ItemsSource = DataAccessFacade.Instance.GetSceneAccess().GetAll();
            ScenesDataGrid.Items.Refresh();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedNumber = 0;
            Scene scSelected = null;

            for (int i = 0; i < ScenesDataGrid.Items.Count; i++)
            {
                if (ScenesDataGrid.Items[i] is Scene scene && scene.Selected)
                {
                    selectedNumber++;
                    scSelected = scene;
                }
            }

            if(selectedNumber == 1 && scSelected != null)
            {
                try
                {
                    var scene = DataAccessFacade.Instance.GetSceneAccess().Get(scSelected.SceneId);
                    SceneInUse.Instance.Set(scene);
                    MessageBox.Show(Properties.GUI.SceneImportedSuccesfully,
                                    Properties.GUI.SceneImportedSuccesfullyTitle,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Properties.Error.ErrorOcurred+"\n"+ex.Message,
                                Properties.Error.ErrorOcurredTitle,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show(Properties.Error.SelectOneScene,
                                Properties.Error.ErrorOcurredTitle,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void checkBxAllScenes_Click(object sender, RoutedEventArgs e)
        {
            for(int i=0;i< ScenesDataGrid.Items.Count; i++)
            {
                if(ScenesDataGrid.Items[i] is Scene scene)
                {
                    if (checkBxAllScenes?.IsChecked == true)
                    {
                        scene.Selected = true;
                    }
                    else
                    {
                        scene.Selected = false;
                    }
                }
            }
        }

        private void DataGridCheckBoxColumn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.RemovedItems.Count > 0)
            {
                checkBxAllScenes.IsChecked = false;
            }
            else if(e.AddedItems.Count > 0)
            {
                bool allChecked = true;
                for (int i = 0; i < ScenesDataGrid.Items.Count; i++)
                {
                    if (ScenesDataGrid.Items[i] is Scene scene && !scene.Selected)
                    {
                        allChecked = false;
                    }
                }
                if (allChecked)
                {
                    checkBxAllScenes.IsChecked = true;
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            bool allChecked = true;
            for (int i = 0; i < ScenesDataGrid.Items.Count; i++)
            {
                if (ScenesDataGrid.Items[i] is Scene scene && !scene.Selected)
                {
                    allChecked = false;
                }
            }
            if (allChecked)
            {
                checkBxAllScenes.IsChecked = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            checkBxAllScenes.IsChecked = false;
        }
    }
}
