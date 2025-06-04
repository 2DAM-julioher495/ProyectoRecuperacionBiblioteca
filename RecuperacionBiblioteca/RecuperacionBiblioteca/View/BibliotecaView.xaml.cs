using RecuperacionBiblioteca.Model;
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
    public partial class BibliotecaView : Window
    {
        public BibliotecaView(UsuarioModel usuario)
        {
            InitializeComponent();

            this.DataContext = new ViewModel.BibliotecaViewModel(this, usuario);
        }

        private void CheckBox_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
