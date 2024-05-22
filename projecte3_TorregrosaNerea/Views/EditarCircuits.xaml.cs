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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projecte3_TorregrosaNerea.Views
{
    /// <summary>
    /// Lógica de interacción para EditarCircuits.xaml
    /// </summary>
    public partial class EditarCircuits : Page
    {
        private int numCheckpoints;
        private BDCircuit circuitActual;
        private double widthAnterior;
        private double heightAnterior;
        public EditarCircuits()
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

        public EditarCircuits(int numCheckpoints,BDCircuit circuitActual,BDCursa cursaActual) : this()
        {
            cboCategoria.ItemsSource = BDCategoria.getCategoriesFromEsport(cursaActual.EsportId);
            this.numCheckpoints = numCheckpoints;
            this.circuitActual = circuitActual;
            txbNumCheckpoints.Text = this.numCheckpoints.ToString();
            mostrarCircuit();
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
            CheckFormValidity();
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
            CheckFormValidity();
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
            CheckFormValidity();
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
            CheckFormValidity();
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

            CheckFormValidity();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            modificarCircuit();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tornarAConusltarCurses();
        }

        #region Mètodes
        public void mostrarCircuit()
        {
            txbNom.Text = circuitActual.Nom;
            txbDistancia.Text = circuitActual.Distancia.ToString();
            txbPreu.Text = circuitActual.Preu.ToString();
            txbTempsEstimat.Text = Utils.convertirDateTimeAString(circuitActual.TempsEstimat);
            int idCategoria = BDCircuitCategoria.getCategoriaId(circuitActual.Id);
            BDCategoria c = BDCategoria.getCategoriaById(idCategoria);
            foreach (BDCategoria item in cboCategoria.ItemsSource)
            {
                if (item.Equals(c)) {
                    cboCategoria.SelectedItem = item; 
                    break;
                }
            }   
            txbNumero.Text = circuitActual.Numero.ToString();
        }

        private void tornarAConusltarCurses()
        {
            MainWindow.mainWindow.MaxHeight = heightAnterior;
            MainWindow.mainWindow.MaxWidth = widthAnterior;
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            MainWindow.navigationFrame.Navigate(new ConsultarCurses());
        }

        public void modificarCircuit()
        {
            bool okBorrarCircuitCategoria = false;
            bool okCategoria = false;
            if (circuitActual != null)
            {
                circuitActual.Nom = txbNom.Text;
                int categoriaId = 0;
                if (cboCategoria.SelectedItem != null)
                {
                    BDCategoria e = cboCategoria.SelectedItem as BDCategoria;
                    string nomCategoriaCbo = e.Nom.Trim();
                    if(circuitActual.Categoria != null)
                    {
                        string categoriaCircuitActual = circuitActual.Categoria.Trim();
                        if (!nomCategoriaCbo.Equals(categoriaCircuitActual))
                        {
                            categoriaId = e.Id;
                            okBorrarCircuitCategoria = BDCircuitCategoria.deleteCircuitCategoria(circuitActual.Id);
                            circuitActual.Numero = Int32.Parse(txbNumero.Text);
                            circuitActual.Distancia = Double.Parse(txbDistancia.Text);
                            circuitActual.Preu = Decimal.Parse(txbPreu.Text);
                            DateTime tempsEstimat = Utils.aconseguirTempsEstimat(txbTempsEstimat.Text);
                            circuitActual.TempsEstimat = tempsEstimat;
                            if (okBorrarCircuitCategoria)
                            {
                                bool ok = BDCircuit.updateCircuit(circuitActual);

                                if (ok)
                                {
                                    int idUltimCircuit = BDCircuit.ObtenirUltimCircuitId();
                                    BDCircuitCategoria.insertCircuitCategoria(categoriaId, idUltimCircuit);
                                    MessageBox.Show("Circuit modificat correctament.");
                                    tornarAConusltarCurses();

                                }
                                else
                                {
                                    MessageBox.Show("No s'ha pogut modificar el circuit.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error inseperat.");
                            }
                        }
                        else
                        {
                            circuitActual.Numero = Int32.Parse(txbNumero.Text);
                            circuitActual.Distancia = Double.Parse(txbDistancia.Text);
                            circuitActual.Preu = Decimal.Parse(txbPreu.Text);
                            DateTime tempsEstimat = Utils.aconseguirTempsEstimat(txbTempsEstimat.Text);
                            circuitActual.TempsEstimat = tempsEstimat;
                            bool ok = BDCircuit.updateCircuit(circuitActual);

                            if (ok)
                            {
                                int idUltimCircuit = BDCircuit.ObtenirUltimCircuitId();
                                BDCircuitCategoria.insertCircuitCategoria(categoriaId, idUltimCircuit);
                                MessageBox.Show("Circuit modificat correctament.");
                                tornarAConusltarCurses();

                            }
                            else
                            {
                                MessageBox.Show("No s'ha pogut modificar el circuit.");
                            } 

                        }
                    }
                    
                }
                else
                {
                    MessageBox.Show("La categoria no pot estar buida.");
                }
            }
        }

        private void CheckFormValidity()
        {
            bool isTextNomValid = !string.IsNullOrEmpty(txbNom.Text);
            bool isDistanciaValid = Utils.EsDouble(txbDistancia.Text) && !string.IsNullOrEmpty(txbDistancia.Text);
            bool isPreuValid = Utils.EsDecimal(txbPreu.Text) && !string.IsNullOrEmpty(txbPreu.Text);
            bool isNumeroValid = Utils.EsNumeroEnter(txbNumero.Text) && !string.IsNullOrEmpty(txbPreu.Text);
            bool isTempsEstimatValid = Utils.TeFormatHora(txbTempsEstimat.Text) && !string.IsNullOrEmpty(txbTempsEstimat.Text);
            if (isTextNomValid && isDistanciaValid && isPreuValid)

            {
                btnGuardar.IsEnabled = true;
            }
            else
            {
                btnGuardar.IsEnabled = false;
            }
        }
        #endregion

    }
}
