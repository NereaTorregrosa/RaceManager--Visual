﻿using BD_MySQL.Model;
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
    /// Lógica de interacción para EditarCursa.xaml
    /// </summary>
    public partial class EditarCursa : Page
    {
        private BDCursa cursaActual;
        private string pathNouLogo;
        public EditarCursa()
        {
            InitializeComponent();
        }

        public EditarCursa(BDCursa c) : this()
        {
            if(c !=null)
            {
                cursaActual = c;
            }

            mostrarCursa();
            
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

            CheckFormValidity();
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

                CheckFormValidity();
            }
        }


        private void cboTipus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnElegirFoto_Click(object sender, RoutedEventArgs e)
        {
            elegirFoto();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            editarCursa();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new ConsultarCurses());
        }

        public void mostrarCursa()
        {
            omplirComboBoxEsports();

            txbNom.Text = cursaActual.Nom;
            dpDataInici.SelectedDate = cursaActual.DataInici;
            dpDataFi.SelectedDate = cursaActual.DataFi;
            txbLloc.Text = cursaActual.Lloc;
            if(cursaActual.Descripcio != null)
            {
                txbDesc.Text = cursaActual.Descripcio;
            }
            if(cursaActual.UrlWeb != null)
            {
                txbWebsite.Text = cursaActual.UrlWeb;
            }
            txbLimitInscripcions.Text = cursaActual.LimitInscripcions.ToString();
            BDEsport tipusCursa = BDEsport.getEsportById(cursaActual.EsportId);
            foreach (BDEsport item in cboTipus.ItemsSource)
            {
                if (item.Equals(tipusCursa))
                {
                    cboTipus.SelectedItem = item;
                    break;
                }
            }
            BitmapImage bitmapImage = new BitmapImage(new Uri(cursaActual.UrlFoto));
            imgCursa.Source = bitmapImage;
        }

        public void omplirComboBoxEsports()
        {
            cboTipus.ItemsSource = BDEsport.getEsports();

        }

        public void editarCursa()
        {
            bool okTipusElegit = false;
            cursaActual.Nom =  txbNom.Text;
            if (dpDataInici.SelectedDate!= null)
            {
                cursaActual.DataInici = dpDataInici.SelectedDate.Value;
            }

            if (dpDataFi.SelectedDate != null)
            {
                cursaActual.DataFi = dpDataFi.SelectedDate.Value;
            }
            
            
            cursaActual.Lloc = txbLloc.Text;
            int esportId = 0;
            if (cboTipus.SelectedItem != null)
            {
                BDEsport e = cboTipus.SelectedItem as BDEsport;
                esportId = e.Id;
                okTipusElegit = true;

            }
            else
            {
                okTipusElegit = false;
            }
            cursaActual.EsportId = esportId;
            cursaActual.Descripcio = txbDesc.Text;
            cursaActual.LimitInscripcions = Int32.Parse(txbLimitInscripcions.Text);
            string urlWeb = txbWebsite.Text;

            cursaActual.UrlWeb = urlWeb;
            

            bool dataOk = Utils.EsDataFutura(cursaActual.DataInici, cursaActual.DataFi);
            if (pathNouLogo != null)
            {
                cursaActual.UrlFoto = pathNouLogo;
                if (dpDataInici.SelectedDate != null || dpDataFi.SelectedDate != null)
                {
                    if (dpDataInici.SelectedDate.Value < DateTime.Now)
                    {
                        MessageBox.Show("Data Inici no pot ser anterior a la data d'avui.");
                    }
                    else
                    {
                        if (dataOk)
                        {
                            if (okTipusElegit)
                            {
                                bool ok = BDCursa.updateCursa(cursaActual);
                                if (ok)
                                {
                                    MessageBox.Show("Cursa actualitzada correctament.");
                                    MainWindow.navigationFrame.Navigate(new ConsultarCurses());
                                }
                                else
                                {
                                    MessageBox.Show("No s'ha pogut actualitzar la cursa.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("S'ha d'elegir un tipus per la cursa.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Data Fi no pot ser anterior a Data Inici.");

                        }
                    }

                }
                else
                {
                    MessageBox.Show("Data Inici i Data Fi no poden estar buides.");
                }
            }
            else
            {
                MessageBox.Show("La foto de la carrera no pot estar buida.");
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

        private void CheckFormValidity()
        {
            bool isTextNomValid = !string.IsNullOrEmpty(txbNom.Text);
            bool isLimitInscripcionsValid = Utils.EsNumeroEnter(txbLimitInscripcions.Text) && !string.IsNullOrEmpty(txbLimitInscripcions.Text);
            bool isTextLLocValid = !string.IsNullOrEmpty(txbLloc.Text);

            if (isTextNomValid && isLimitInscripcionsValid && isTextLLocValid)
            {
                btnGuardar.IsEnabled = true;
            }
            else
            {
                btnGuardar.IsEnabled = false;
            }
        }
    }
}
