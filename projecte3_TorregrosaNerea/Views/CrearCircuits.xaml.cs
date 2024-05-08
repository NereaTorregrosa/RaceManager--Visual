using BD_MySQL.Model;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projecte3_TorregrosaNerea.Views
{
    /// <summary>
    /// Lógica de interacción para CrearCircuits.xaml
    /// </summary>
    public partial class CrearCircuits : Page
    {
        private double widthAnterior;
        private double heightAnterior;
        public CrearCircuits()
        {
            InitializeComponent();
            widthAnterior = MainWindow.mainWindow.Width;
            heightAnterior = MainWindow.mainWindow.Height;
            int width = 500;
            int height = 600;
            MainWindow.mainWindow.MaxHeight = height;
            MainWindow.mainWindow.MaxWidth = width;
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        public CrearCircuits(BDCursa c) : this()
        {
            cboCategoria.ItemsSource = BDCategoria.getCategoriesFromEsport(c.EsportId);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.MaxHeight = heightAnterior;
            MainWindow.mainWindow.MaxWidth = widthAnterior;
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            MainWindow.navigationFrame.Navigate(new ConsultarCurses());
        }
    }
}
