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
    /// Lógica de interacción para ExitDialog.xaml
    /// </summary>
    public partial class AcceptCancelDialog : UserControl
    {
        public AcceptCancelDialog(string text, RoutedEventHandler AcceptHandler)
        {
            InitializeComponent();
            AcceptCancelDialogLabel.Content = text;
            AcceptBtn.Click += AcceptHandler;
        }
    }
}