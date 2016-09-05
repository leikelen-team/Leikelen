using cl.uv.multimodalvisualizer.models;
//using cl.uv.multimodalvisualizer.pojos;

using cl.uv.multimodalvisualizer.windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace cl.uv.multimodalvisualizer.views
{
    public class PersonView
    {
        private Person Person;
        private bool combosGenerated = false;

        public List<Tuple<PostureType, ComboBox, RowDefinition>>
                   visiblePostures
        { get; private set; } = null;

        public Grid postureGroupsGrid { get; private set; } = null;
        public StackPanel ComboStackPanel { get; private set; }
        public Label Label { get; private set; }

        public PersonView(Person person)
        {
            this.Person = person;
            this.initControlsAndBorders();
            
        }

        private void generateCombos()
        {
            int maxVisiblePostureTypes = Convert.ToInt32(Properties.Resources.MaxPostureIntervalGroupViewPerUser);

            visiblePostures = new List<Tuple<PostureType, ComboBox, RowDefinition>>();

            postureGroupsGrid = new Grid();
            //foreach (SceneFrame sceneFrame in Scene.Instance.Frames)
            for(int i=0; i < Scene.Instance.TickCount; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(5, GridUnitType.Pixel);
                postureGroupsGrid.ColumnDefinitions.Add(colDef);
            }
            Grid.SetRow(postureGroupsGrid, Person.ListIndex);
            Grid.SetColumn(postureGroupsGrid, 0);
            Grid.SetColumnSpan(postureGroupsGrid, Scene.Instance.TickCount);
            postureGroupsGrid.Margin = new Thickness(0, 6, 0, 0);

            List<PostureType> posturesDetectedInPerson = new List<PostureType>();
            foreach (var group in Person.PostureIntervalGroups)
            {
                if (!posturesDetectedInPerson.Contains(group.PostureType))
                    posturesDetectedInPerson.Add(group.PostureType);
            }
            

            //visiblePostureTypes = new PostureType[maxVisiblePostureTypes];
            PostureType postureType;
            for (int i = 0; i < maxVisiblePostureTypes && i < posturesDetectedInPerson.Count; i++)
            {
                //postureType = i + 1 < PostureType.availablesPostureTypes.Count ?
                //                PostureType.availablesPostureTypes[i + 1] :
                //                PostureType.none;

                postureType = posturesDetectedInPerson[i];


                ComboBox combo = new ComboBox();
                combo.HorizontalAlignment = HorizontalAlignment.Right;
                combo.VerticalAlignment = VerticalAlignment.Top;
                combo.SelectedIndex = 0;
                foreach (PostureType pt in posturesDetectedInPerson)
                {
                    combo.Items.Add(pt);
                }
                this.ComboStackPanel.Children.Add(combo);

                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(20, GridUnitType.Pixel);
                postureGroupsGrid.RowDefinitions.Add(rowDef);

                combo.SelectedItem = postureType;
                combo.SelectionChanged += Combo_SelectionChanged;
                visiblePostures.Add(
                    new Tuple<PostureType, ComboBox, RowDefinition>
                        (postureType, combo, rowDef)
                        );
            }
            combosGenerated = true;
        }

        //private void EditButton_Click()
        //{
        //    
        //}

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditPersonForm editForm = new EditPersonForm(Person);
            editForm.Show();
        }

        private void initControlsAndBorders()
        {
            Border border = new Border();
            border.Background = Brushes.GhostWhite;
            border.BorderBrush = Brushes.Silver;
            border.BorderThickness = new Thickness(3);
            border.CornerRadius = new CornerRadius(8, 8, 3, 3);
            Grid.SetColumn(border, 0);
            Grid.SetColumnSpan(border, 2);
            Grid.SetRow(border, Person.ListIndex);

            StackPanel LabelAndButtonStackPanel = new StackPanel();
            LabelAndButtonStackPanel.Orientation = Orientation.Vertical;
            LabelAndButtonStackPanel.Margin = new Thickness(5, 5, 0, 0);
            Grid.SetColumn(LabelAndButtonStackPanel, 0);
            Grid.SetRow(LabelAndButtonStackPanel, Person.ListIndex);


            Label = new Label();
            Label.Content = Person.Name;
            Label.HorizontalAlignment = HorizontalAlignment.Left;
            Label.VerticalAlignment = VerticalAlignment.Top;
            Label.Foreground = Person.Color;
            LabelAndButtonStackPanel.Children.Add(Label);

            

            Button editButton = new Button();
            editButton.Content = "Edit";
            editButton.Click += EditButton_Click;
            editButton.HorizontalAlignment = HorizontalAlignment.Left;
            editButton.VerticalAlignment = VerticalAlignment.Top;
            editButton.Margin = new Thickness(0, 10, 0, 0);
            LabelAndButtonStackPanel.Children.Add(editButton);

            //Label.Margin = new Thickness(5, 5, 0, 0);
            //Grid.SetColumn(Label, 0);
            //Grid.SetRow(Label, Person.ListIndex);

            ComboStackPanel = new StackPanel();
            ComboStackPanel.Orientation = Orientation.Vertical;
            ComboStackPanel.Margin = new Thickness(0, 6, 6, 0);
            Grid.SetColumn(ComboStackPanel, 1);
            Grid.SetRow(ComboStackPanel, Person.ListIndex);

            RowDefinition personCtrlRowDef = new RowDefinition();
            personCtrlRowDef.Height = new GridLength(77, GridUnitType.Pixel);
            Grid personLabelsGrid = MainWindow.Instance().personLabelsGrid;
            personLabelsGrid.RowDefinitions.Add(personCtrlRowDef);

            personLabelsGrid.Children.Add(border);
            personLabelsGrid.Children.Add(LabelAndButtonStackPanel);
            personLabelsGrid.Children.Add(ComboStackPanel);

            //---

            Border posturesViewBorder = new Border();
            posturesViewBorder.Background = Brushes.GhostWhite;
            posturesViewBorder.BorderBrush = Brushes.Silver;
            posturesViewBorder.BorderThickness = new Thickness(3);
            posturesViewBorder.CornerRadius = new CornerRadius(8, 8, 3, 3);
            Grid.SetColumn(posturesViewBorder, 0);
            Grid.SetColumnSpan(posturesViewBorder, Int32.MaxValue);
            Grid.SetRow(posturesViewBorder, Person.ListIndex);

            RowDefinition postureViewRowDef = new RowDefinition();
            postureViewRowDef.Height = new GridLength(77, GridUnitType.Pixel);
            Grid timeLineContentGrid = MainWindow.Instance().timeLineContentGrid;
            timeLineContentGrid.RowDefinitions.Add(postureViewRowDef);

            timeLineContentGrid.Children.Add(posturesViewBorder);

            //---

            RowDefinition verticalScrollSyncRowDef = new RowDefinition();
            verticalScrollSyncRowDef.Height = new GridLength(77, GridUnitType.Pixel);
            Grid verticalScrollSyncGrid = MainWindow.Instance().timeLineVerticalScrollViewSubGrid;
            verticalScrollSyncGrid.RowDefinitions.Add(verticalScrollSyncRowDef);
        }

        

        public void repaintIntervalGroups()
        {
            if(!combosGenerated) this.generateCombos();
            for (int i = 0; i < visiblePostures.Count; i++)
            {
                repaintIntervalGroup(visiblePostures[i].Item2);
            }
        }

        private void repaintIntervalGroup(ComboBox combo)
        {

            PostureType newPostureType = (PostureType)combo.SelectedItem;

            var visiblePosture = visiblePostures.Find(v => v.Item2 == combo);
            PostureType oldPostureType = visiblePosture.Item1;
            RowDefinition rowDef = visiblePosture.Item3;

            // CLEAR ROW OF INTERVAL GROUP
            int rowDefIndex = postureGroupsGrid.RowDefinitions.IndexOf(rowDef);
            //foreach (UIElement path in postureGroupsGrid.Children)
            for (int i = postureGroupsGrid.Children.Count - 1; i >= 0; i--)
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
            PostureIntervalGroup postureIntervalGroup = Person.PostureIntervalGroups
                                    .FirstOrDefault
                                    (g => g.PostureType == newPostureType);
            if (postureIntervalGroup != null)
            {
                //StackPanel pathStackPanel = new StackPanel();
                foreach (var interval in postureIntervalGroup.Intervals)
                {
                    Console.WriteLine("\t[" + interval.StartTime.ToString(@"mm\:ss") + ", "
                        + interval.EndTime.ToString(@"mm\:ss") + "]");

                    int intervalIniCol = Convert.ToInt32(interval.StartTime.TotalSeconds);
                    int intervalFinCol = Convert.ToInt32(interval.EndTime.TotalSeconds);

                    System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
                    path.Stroke = Person.Color;//System.Windows.Media.Brushes.Red; //person.Color; // postureIntervalGroup.postureType.color; //
                    path.StrokeThickness = 10;
                    path.Stretch = System.Windows.Media.Stretch.Fill;
                    Grid.SetRow(path, rowDefIndex);

                    int iniCol = (intervalIniCol - 1) >= 0 ? (intervalIniCol - 1) : 0;
                    Grid.SetColumn(path, iniCol);
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

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            repaintIntervalGroup(combo);
        }


    }
}
