using BD_MySQL.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private BDCursa cursaActual = null;
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
            cursaActual = c;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tornarAConusltarCurses();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            afegirCircuit();
        }

        private void txbNom_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Background = Brushes.Red;
                btnGuardar.IsEnabled = false;
            }
            else
            {
                textBox.Background = Brushes.White;
                btnGuardar.IsEnabled = true;
            }
        }

        private void txbDistancia_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!Utils.EsDouble(textBox.Text) || string.IsNullOrEmpty(textBox.Text))
            {

                textBox.Background = Brushes.Red;
                btnGuardar.IsEnabled = false;
            }
            else
            {
                textBox.Background = Brushes.White;
                btnGuardar.IsEnabled = true;
            }
        }

        private void txbPreu_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!Utils.EsDecimal(textBox.Text) || string.IsNullOrEmpty(textBox.Text))
            {

                textBox.Background = Brushes.Red;
                btnGuardar.IsEnabled = false;
            }
            else
            {
                textBox.Background = Brushes.White;
                btnGuardar.IsEnabled = true;
            }
        }

        private void TextBoxInt_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!Utils.EsNumeroEnter(textBox.Text) || string.IsNullOrEmpty(textBox.Text))
            {

                textBox.Background = Brushes.Red;
                btnGuardar.IsEnabled = false;
            }
            else
            {
                textBox.Background = Brushes.White;
                btnGuardar.IsEnabled = true;
            }
        }

        private void txbTempsEstimat_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!Utils.TeFormatHora(textBox.Text) || string.IsNullOrEmpty(textBox.Text))
            {

                textBox.Background = Brushes.Red;
                btnGuardar.IsEnabled = false;
            }
            else
            {
                textBox.Background = Brushes.White;
                btnGuardar.IsEnabled = true;
            }
        }
        public void afegirCircuit()
        {
            string nom = txbNom.Text;
            int categoriaId = 0;
            if (cboCategoria.SelectedItem != null)
            {
                BDCategoria e = cboCategoria.SelectedItem as BDCategoria;
                categoriaId = e.Id;
            }

            int numero = Int32.Parse(txbNumero.Text);
            double distancia = Double.Parse(txbDistancia.Text);
            decimal preu = Decimal.Parse(txbPreu.Text);
            DateTime tempsEstimat = Utils.aconseguirTempsEstimat(txbTempsEstimat.Text);

            BDCircuit ci = new BDCircuit(cursaActual.Id,numero,distancia,nom,preu,tempsEstimat);
            bool ok = BDCircuit.insertCircuit(ci);
            
            if (ok)
            {
                int idUltimCircuit = BDCircuit.ObtenirUltimCircuitId();
                BDCircuitCategoria.insertCircuitCategoria(categoriaId, idUltimCircuit);
                MessageBox.Show("Circuit inserit correctament.");
                tornarAConusltarCurses();

            }
            else
            {
                MessageBox.Show("No s'ha pogut inserir el circuit.");
            }
            
        }


        private void tornarAConusltarCurses()
        {
            MainWindow.mainWindow.MaxHeight = heightAnterior;
            MainWindow.mainWindow.MaxWidth = widthAnterior;
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            MainWindow.navigationFrame.Navigate(new ConsultarCurses());
        }



    }
}
