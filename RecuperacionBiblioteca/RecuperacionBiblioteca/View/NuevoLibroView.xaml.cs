﻿using RecuperacionBiblioteca.Model;
using RecuperacionBiblioteca.Service;
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

namespace RecuperacionBiblioteca.View
{
    /// <summary>
    /// Lógica de interacción para NuevoLibroView.xaml
    /// </summary>
    public partial class NuevoLibroView : Window
    {
        public NuevoLibroView(BibliotecaService biblioService, LibroModel libroSelect)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.NuevoLibroViewModel(this, biblioService, libroSelect);

        }
    }
}
