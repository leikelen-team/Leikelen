﻿using System;
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

namespace cl.uv.leikelen.View.Widget
{
    /// <summary>
    /// Lógica de interacción para PreferencesBD.xaml
    /// </summary>
    public partial class PreferencesBD : TabItem
    {
        public PreferencesBD()
        {
            InitializeComponent();

            AcceptBtn.Click += AcceptBtn_Click;
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
