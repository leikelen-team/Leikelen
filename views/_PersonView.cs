using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;


namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.views
{
    public class _PersonView
    {
        //private Person Person;
        //public Label Label { get; private set; }
        //public StackPanel ComboStackPanel { get; private set; }

        //public PersonView(Person person)
        //{
        //    this.Person = person;

        //    Border border = new Border();
        //    border.Background = Brushes.GhostWhite;
        //    border.BorderBrush = Brushes.Silver;
        //    border.BorderThickness = new Thickness(3);
        //    border.CornerRadius = new CornerRadius(8, 8, 3, 3);
        //    Grid.SetColumn(border, 0);
        //    Grid.SetColumnSpan(border, 2);
        //    Grid.SetRow(border, person.ListIndex);
            
        //    Label = new Label();
        //    Label.Content = person.Name;
        //    Label.HorizontalAlignment = HorizontalAlignment.Left;
        //    Label.VerticalAlignment = VerticalAlignment.Top;
        //    Label.Background = Person.Color;
        //    Grid.SetColumn(Label, 0);
        //    Grid.SetRow(Label, person.ListIndex);

        //    ComboStackPanel = new StackPanel();
        //    ComboStackPanel.Orientation = Orientation.Vertical;
        //    ComboStackPanel.Margin = new Thickness(0, 6, 7, 0);
        //    Grid.SetColumn(ComboStackPanel, 1);
        //    Grid.SetColumn(Label, 1);
        //    Grid.SetRow(ComboStackPanel, person.ListIndex);
            
        //    RowDefinition personCtrlRowDef = new RowDefinition();
        //    personCtrlRowDef.Height = new GridLength(77, GridUnitType.Pixel);
        //    Grid personLabelsGrid = MainWindow.Instance().personLabelsGrid;
        //    personLabelsGrid.RowDefinitions.Add(personCtrlRowDef);

        //    personLabelsGrid.Children.Add(border);
        //    personLabelsGrid.Children.Add(Label);
        //    personLabelsGrid.Children.Add(ComboStackPanel);

        //    //---

        //    Border posturesViewBorder = new Border();
        //    posturesViewBorder.Background = Brushes.GhostWhite;
        //    posturesViewBorder.BorderBrush = Brushes.Silver;
        //    posturesViewBorder.BorderThickness = new Thickness(3);
        //    posturesViewBorder.CornerRadius = new CornerRadius(8, 8, 3, 3);
        //    Grid.SetColumn(posturesViewBorder, 0);
        //    Grid.SetColumnSpan(posturesViewBorder, Int32.MaxValue);
        //    Grid.SetRow(posturesViewBorder, person.ListIndex);

        //    RowDefinition postureViewRowDef = new RowDefinition();
        //    postureViewRowDef.Height = new GridLength(77, GridUnitType.Pixel);
        //    Grid timeLineContentGrid = MainWindow.Instance().timeLineContentGrid;
        //    timeLineContentGrid.RowDefinitions.Add(postureViewRowDef);

        //    timeLineContentGrid.Children.Add(posturesViewBorder);

        //    //---

        //    RowDefinition verticalScrollSyncRowDef = new RowDefinition();
        //    verticalScrollSyncRowDef.Height = new GridLength(77, GridUnitType.Pixel);
        //    Grid verticalScrollSyncGrid = MainWindow.Instance().timeLineVerticalScrollViewSubGrid;
        //    verticalScrollSyncGrid.RowDefinitions.Add(verticalScrollSyncRowDef);

            
        //}
    }
}
