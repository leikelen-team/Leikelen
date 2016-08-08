using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.views
{
    public class PersonView
    {
        public int bodyIndex { get; private set; }
        public Grid postureGroupsGrid { get; private set; } = null;
        public StackPanel combosStackPanel { get; private set; }
        public List<Tuple<PostureType, ComboBox, RowDefinition>> 
                    visiblePostures { get; private set; } = null;
        private List<PostureIntervalGroup> postureIntervalGroups;
        private System.Windows.Media.Brush color;

        //private PostureType[] visiblePostureTypes = null;

        public PersonView(
            int bodyIndex, 
            StackPanel combosStackPanel, 
            List<PostureIntervalGroup> postureIntervalGroups,
            System.Windows.Media.Brush color
        ){
            this.bodyIndex = bodyIndex;
            this.combosStackPanel = combosStackPanel;
            this.postureIntervalGroups = postureIntervalGroups;
            this.color = color;
            int maxVisiblePostureTypes = Convert.ToInt32(Properties.Resources.MaxPostureIntervalGroupViewPerUser);

            visiblePostures = new List<Tuple<PostureType, ComboBox, RowDefinition>>();

            postureGroupsGrid = new Grid();
            foreach (TimeSpan time in MainWindow.Instance().kstudio.frameTimes)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(5, GridUnitType.Pixel);
                postureGroupsGrid.ColumnDefinitions.Add(colDef);
            }
            Grid.SetRow(postureGroupsGrid, this.bodyIndex);
            Grid.SetColumn(postureGroupsGrid, 0);
            Grid.SetColumnSpan(postureGroupsGrid, MainWindow.Instance().kstudio.frameTimes.Count);
            

            //visiblePostureTypes = new PostureType[maxVisiblePostureTypes];
            PostureType postureType;
            for (int i = 0; i < maxVisiblePostureTypes; i++)
            {
                postureType = i + 1 < PostureType.availablesPostureTypes.Count ?
                                PostureType.availablesPostureTypes[i + 1] :
                                PostureType.none;

                ComboBox combo = new ComboBox();
                combo.HorizontalAlignment = HorizontalAlignment.Right;
                combo.VerticalAlignment = VerticalAlignment.Top;
                combo.SelectedIndex = 0;
                foreach (PostureType pt in PostureType.availablesPostureTypes)
                {
                    combo.Items.Add(pt);
                }
                this.combosStackPanel.Children.Add(combo);

                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);
                postureGroupsGrid.RowDefinitions.Add(rowDef);

                combo.SelectedItem = postureType;
                combo.SelectionChanged += Combo_SelectionChanged;
                visiblePostures.Add(
                    new Tuple<PostureType, ComboBox, RowDefinition>
                        (postureType, combo, rowDef)
                        );
            }
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            PostureType newPostureType = (PostureType)combo.SelectedItem;

            var visiblePosture = visiblePostures.Find(v => v.Item2 == combo);
            PostureType oldPostureType = visiblePosture.Item1;
            RowDefinition rowDef = visiblePosture.Item3;

            // CLEAR ROW OF INTERVAL GROUP
            int rowDefIndex = postureGroupsGrid.RowDefinitions.IndexOf(rowDef);
            //foreach (UIElement path in postureGroupsGrid.Children)
            for (int i = postureGroupsGrid.Children.Count-1; i >= 0; i--)
            {
                if (Grid.GetRow(postureGroupsGrid.Children[i]) == rowDefIndex)
                {
                    postureGroupsGrid.Children.RemoveAt(i);
                }
            }

            int visiblePostureIndex = visiblePostures.IndexOf(visiblePosture);
            visiblePostures.Remove(visiblePosture);
            visiblePostures.Insert(visiblePostureIndex,
                new Tuple<PostureType, ComboBox, RowDefinition>
                    (newPostureType, combo, rowDef)
                );
            PostureIntervalGroup postureIntervalGroup = postureIntervalGroups
                                    .FirstOrDefault
                                    (g => g.postureType == newPostureType);
            if (postureIntervalGroup != null)
            {
                //StackPanel pathStackPanel = new StackPanel();
                foreach (var interval in postureIntervalGroup.Intervals)
                {
                    Console.WriteLine("\t[" + interval.Item1.sceneLocationTime.ToString(@"mm\:ss") + ", "
                        + interval.Item2.sceneLocationTime.ToString(@"mm\:ss") + "]");

                    int intervalIniCol = Convert.ToInt32(interval.Item1.sceneLocationTime.TotalSeconds);
                    int intervalFinCol = Convert.ToInt32(interval.Item2.sceneLocationTime.TotalSeconds);

                    System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
                    path.Stroke = this.color;//System.Windows.Media.Brushes.Red; //person.Color; // postureIntervalGroup.postureType.color; //
                    path.StrokeThickness = 10;
                    path.Stretch = System.Windows.Media.Stretch.Fill;
                    Grid.SetRow(path, rowDefIndex);
                    Grid.SetColumn(path, intervalIniCol - 2);
                    Grid.SetColumnSpan(path, intervalFinCol - intervalIniCol + 2);
                    System.Windows.Media.LineGeometry line = new System.Windows.Media.LineGeometry();
                    line.StartPoint = new System.Windows.Point(0d, 0d);
                    line.EndPoint = new System.Windows.Point(1d, 0d);
                    path.Data = line;
                    //postureGroupsGrid.
                    postureGroupsGrid.Children.Add(path);
                }
            }

        }


    }
}
