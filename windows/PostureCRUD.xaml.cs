﻿using cl.uv.multimodalvisualizer.db;
using cl.uv.multimodalvisualizer.models;
using cl.uv.multimodalvisualizer.utils;
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

namespace cl.uv.multimodalvisualizer.windows
{
    /// <summary>
    /// Interaction logic for PostureCRUD.xaml
    /// </summary>
    public partial class PostureCRUD : Window
    {
        public PostureCRUD()
        {
            InitializeComponent();
            refreshList();
        }

        private void addPostureButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditPostureType addPostureView = new AddOrEditPostureType();
            addPostureView.Show();
        }
        public void refreshList()
        {
            postureCrudDataGrid.ItemsSource = PostureTypeContext.db.PostureType.ToList();
        }

        private void removePostureButton_Click(object sender, RoutedEventArgs e)
        {
            //int idx = postureCrudDataGrid.SelectedIndex;
            PostureType selectedItem = (PostureType) postureCrudDataGrid.SelectedItem;
            if (selectedItem != PostureType.none)
            {
                PostureTypeContext.db.PostureType.Remove(PostureTypeContext.db.PostureType.FirstOrDefault(p => p.PostureTypeId == selectedItem.PostureTypeId));
                PostureTypeContext.db.SaveChanges();
                this.refreshList();
            }
        }

        private void editPostureButton_Click(object sender, RoutedEventArgs e)
        {
            PostureType selectedPosture = (PostureType)postureCrudDataGrid.SelectedItem;
            selectedPosture = PostureTypeContext.db.PostureType.FirstOrDefault(p => p.PostureTypeId == selectedPosture.PostureTypeId);
            AddOrEditPostureType addPostureView = new AddOrEditPostureType(selectedPosture);
            addPostureView.Show();
        }

        private void AssociatePosturesButton_Click(object sender, RoutedEventArgs e)
        {
            AssociatePostures associatePsView = new AssociatePostures();
            associatePsView.Show();

            //GesturesCRUD gcrud = new GesturesCRUD();
            //gcrud.Show();
        }
    }
}
