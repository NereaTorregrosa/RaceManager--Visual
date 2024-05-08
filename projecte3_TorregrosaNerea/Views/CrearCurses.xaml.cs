using BD_MySQL.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para CrearCurses.xaml
    /// </summary>
    public partial class CrearCurses : Page
    {
        private string pathNouLogo;
        public CrearCurses()
        {
            InitializeComponent();
            omplirComboBoxEsports();
        }

        private void cboTipus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTipus.SelectedItem != null)
            {
                BDEsport esportSeleccionat = cboTipus.SelectedItem as BDEsport;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new ConsultarCurses());
        }
        public void omplirComboBoxEsports()
        {
            cboTipus.ItemsSource = BDEsport.getEsports();
            
        }

        private void TextString_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txbLimitInscripcions_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Utils.EsNumeroEnter(txbLimitInscripcions.Text) || string.IsNullOrEmpty(txbLimitInscripcions.Text))
            {

                txbLimitInscripcions.Background = Brushes.Red;
                btnGuardar.IsEnabled = false;
            }
            else
            {
                txbLimitInscripcions.Background = Brushes.White;
                btnGuardar.IsEnabled = true;
            }
        }

        private void btnElegirFoto_Click(object sender, RoutedEventArgs e)
        {
            elegirFoto();
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            afegirCursa();
        }
        public void afegirCursa()
        {
             string nom = txbNom.Text;
             DateTime dataInici = dpDataInici.SelectedDate.Value;
             DateTime dataFi = dpDataFi.SelectedDate.Value;
             string lloc = txbLloc.Text;
            int esportId = 0;
            if (cboTipus.SelectedItem != null)
            {
                BDEsport e = cboTipus.SelectedItem as BDEsport;
                esportId = e.Id;
            }
             
             string descripcio = txbDesc.Text;
             int limitInscripcions = Int32.Parse(txbLimitInscripcions.Text);
             string urlFoto = pathNouLogo;
            string urlWeb = txbWebsite.Text;
            if (("").Equals(pathNouLogo) || pathNouLogo == null) {
                MessageBox.Show("La foto de la carrera no pot estar buida.");
            }
            else
            {
                BDCursa c = new BDCursa(nom,dataInici,dataFi,lloc,esportId,1,descripcio,limitInscripcions,urlFoto,urlWeb);
                bool ok = BDCursa.insertCursa(c);
                if(ok)
                {
                    MessageBox.Show("Cursa inserida correctament.");
                    MainWindow.navigationFrame.Navigate(new ConsultarCurses());
                }
                else
                {
                    MessageBox.Show("No s'ha pogut inserir la cursa.");
                }
            }
            
             

        }

        public async void elegirFoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagenes|*.jpg;*.png";

            // Mostrar el cuadro de diálogo para seleccionar un archivo
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string iconsFolderPath = System.IO.Path.Combine(localAppDataPath, "icons");
                    if (!Directory.Exists(iconsFolderPath))
                    {
                        Directory.CreateDirectory(iconsFolderPath);
                    }


                    string fileName = $"{DateTime.Now:yyyyMMddhhmmss}_{System.IO.Path.GetFileName(filePath)}";
                    string newFilePath = System.IO.Path.Combine(iconsFolderPath, fileName);
                    File.Copy(filePath, newFilePath);

                    BitmapImage bitmapImage = new BitmapImage(new Uri(newFilePath));

                    imgCursa.Source = bitmapImage;

                    pathNouLogo = newFilePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al copiar y mostrar la imagen: {ex.Message}");
                }
            }
        }


    }
}
