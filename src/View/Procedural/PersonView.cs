using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using cl.uv.leikelen.src.Data;
using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Model.AccessLogic;

namespace cl.uv.leikelen.src.View.Procedural
{
    public class PersonView
    {
        private Person Person;
        private PersonInScene personInScene;
        private bool combosGenerated = false;
        private int sceneSeconds;

        public List<Tuple<Tuple<ModalType, SubModalType>, ComboBox, RowDefinition>>
                    visiblePostures
        { get; private set; } = null;

        public Grid postureGroupsGrid { get; private set; } = null;
        public StackPanel ComboStackPanel { get; private set; }
        public Label Label { get; private set; }

        public PersonView(PersonInScene personInScene, int sceneSeconds)
        {
            this.personInScene = personInScene;
            this.Person = personInScene.Person;
            this.sceneSeconds = sceneSeconds;
            this.initControlsAndBorders();

        }

        private void generateCombos()
        {
            int maxVisiblePostureTypes = Convert.ToInt32(Properties.Resources.MaxPostureIntervalGroupViewPerUser);

            visiblePostures = new List<Tuple<Tuple<ModalType, SubModalType>, ComboBox, RowDefinition>>();

            postureGroupsGrid = new Grid();
            for (int i = 0; i < this.sceneSeconds; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(5, GridUnitType.Pixel);
                postureGroupsGrid.ColumnDefinitions.Add(colDef);
            }
            Grid.SetRow(postureGroupsGrid, Person.ListIndex);
            Grid.SetColumn(postureGroupsGrid, 0);
            Grid.SetColumnSpan(postureGroupsGrid, this.sceneSeconds);
            postureGroupsGrid.Margin = new Thickness(0, 6, 0, 0);

            List<Tuple<ModalType, SubModalType>> posturesDetectedInPerson = new List<Tuple<ModalType, SubModalType>>();

            foreach(var type in this.personInScene.ModalTypes)
            {
                foreach(var subType in type.SubModalTypes)
                {
                    if(subType.IntervalGroup != null)
                    {
                        var typeTuple = new Tuple<ModalType, SubModalType>(type, subType);
                        if(!posturesDetectedInPerson.Contains(typeTuple))
                        {
                            posturesDetectedInPerson.Add(typeTuple);
                        }
                    }
                }
            }

            for (int i = 0; i < maxVisiblePostureTypes && i < posturesDetectedInPerson.Count; i++)
            {
                Tuple<ModalType, SubModalType>  postureType = posturesDetectedInPerson[i];

                ComboBox combo = new ComboBox();
                combo.HorizontalAlignment = HorizontalAlignment.Right;
                combo.VerticalAlignment = VerticalAlignment.Top;
                combo.SelectedIndex = 0;
                foreach (var pt in posturesDetectedInPerson)
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
                    new Tuple<Tuple<ModalType, SubModalType>, ComboBox, RowDefinition>
                        (postureType, combo, rowDef)
                        );
            }
            combosGenerated = true;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: hacer formulario de persona
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
            Label.Foreground = StaticScene.boneColors[Person.ListIndex];
            LabelAndButtonStackPanel.Children.Add(Label);



            Button editButton = new Button();
            editButton.Content = "Edit";
            editButton.Click += EditButton_Click;
            editButton.HorizontalAlignment = HorizontalAlignment.Left;
            editButton.VerticalAlignment = VerticalAlignment.Top;
            editButton.Margin = new Thickness(0, 10, 0, 0);
            LabelAndButtonStackPanel.Children.Add(editButton);

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
            if (!combosGenerated) this.generateCombos();
            for (int i = 0; i < visiblePostures.Count; i++)
            {
                repaintIntervalGroup(visiblePostures[i].Item2);
            }
        }

        private void repaintIntervalGroup(ComboBox combo)
        {

            Tuple<ModalType, SubModalType> newPostureType = (Tuple<ModalType, SubModalType>)combo.SelectedItem;

            var visiblePosture = visiblePostures.Find(v => v.Item2 == combo);
            Tuple<ModalType, SubModalType> oldPostureType = visiblePosture.Item1;
            RowDefinition rowDef = visiblePosture.Item3;

            // CLEAR ROW OF INTERVAL GROUP
            int rowDefIndex = postureGroupsGrid.RowDefinitions.IndexOf(rowDef);
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
                new Tuple<Tuple<ModalType, SubModalType>, ComboBox, RowDefinition>
                    (newPostureType, combo, rowDef)
                );

            List<IntervalData> postureIntervalGroup = findIntervalGroup(newPostureType);

            if (postureIntervalGroup != null)
            {
                foreach (var interval in postureIntervalGroup)
                {
                    Console.WriteLine("\t[" + interval.StartTime.ToString(@"mm\:ss") + ", "
                        + interval.EndTime.ToString(@"mm\:ss") + "]");

                    int intervalIniCol = Convert.ToInt32(interval.StartTime.TotalSeconds);
                    int intervalFinCol = Convert.ToInt32(interval.EndTime.TotalSeconds);

                    System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
                    path.Stroke = StaticScene.boneColors[Person.ListIndex];
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
                    postureGroupsGrid.Children.Add(path);
                }
            }
        }

        private List<IntervalData> findIntervalGroup(Tuple<ModalType, SubModalType> newPostureType)
        {
            foreach (var type in this.personInScene.ModalTypes)
            {
                foreach (var subType in type.SubModalTypes)
                {
                    if (subType.IntervalGroup != null)
                    {
                        if (newPostureType.Item1 == type && newPostureType.Item2 == subType)
                        {
                            return subType.IntervalGroup;
                        }
                    }
                }
            }
            return null;
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            repaintIntervalGroup(combo);
        }


    }
}
