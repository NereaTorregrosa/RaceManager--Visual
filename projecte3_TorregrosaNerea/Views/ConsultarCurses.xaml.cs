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
    /// Lógica de interacción para ConsultarCurses.xaml
    /// </summary>
    public partial class ConsultarCurses : Page
    {
        private List<BDCircuit> circuitsCursa;
        public ConsultarCurses()
        {
            InitializeComponent();
            omplirCboFiltreiInicialitzarRB();
            inicialitzarBotons();
            launchGetCurses();
            circuitsCursa = new List<BDCircuit>();
        }

        private void cboFiltre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboFiltre.SelectedItem != null)
            {
                btnFiltrar.IsEnabled = true;
                btnNetejarFiltres.IsEnabled = true;
                if (cboFiltre.SelectedItem is string)
                {
                    string seleccio = (string)cboFiltre.SelectedItem;
                    switch (seleccio)
                    {
                        case "Filtrar per nom":
                            dpFiltreData.Visibility = Visibility.Collapsed;
                            txbFiltreNom.Visibility = Visibility.Visible;
                            spEstats.Visibility = Visibility.Collapsed;
                            break;
                        case "Filtrar per data":
                            dpFiltreData.Visibility = Visibility.Visible;
                            txbFiltreNom.Visibility = Visibility.Collapsed;
                            spEstats.Visibility = Visibility.Collapsed;
                            break;
                        case "Filtrar per estat":
                            dpFiltreData.Visibility = Visibility.Collapsed;
                            txbFiltreNom.Visibility = Visibility.Collapsed;
                            spEstats.Visibility = Visibility.Visible;
                            break;
                    }
                }

            }
        }

        private void dgCurses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dgCurses.SelectedItem != null)
            {
                grdDetallCursa.Visibility = Visibility.Visible;
                btnGestionarCircuits.IsEnabled = true;
                btnEditarCursa.IsEnabled = true;
                mostrarDetallCursa();
            }
        }

        private void btnCrearCursa_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new CrearCurses());
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            launchGetCurses();
        }

        private void btnNetejarFiltres_Click(object sender, RoutedEventArgs e)
        {
            txbFiltreNom.Text = "";
            dpFiltreData.SelectedDate = null;
            rbActives.IsChecked = false;
            rbCancelades.IsChecked = false;
            rbTancades.IsChecked = false;
            launchGetCurses();
        }
        public void omplirCboFiltreiInicialitzarRB()
        {
            cboFiltre.Items.Clear();
            cboFiltre.Items.Add("Filtrar per nom");
            cboFiltre.Items.Add("Filtrar per data");
            cboFiltre.Items.Add("Filtrar per estat");
        }
        public void inicialitzarBotons() { 
            btnFiltrar.IsEnabled = false;
            btnNetejarFiltres.IsEnabled = false;
            btnEditarCursa.IsEnabled = false;
            btnGestionarCircuits.IsEnabled = false;
            grdDetallCursa.Visibility = Visibility.Collapsed;
        }

        private void launchGetCurses()
        {
            int estatCursa = estatSelected();
            DateTime dataSeleccionada;
            dataSeleccionada = dpFiltreData.SelectedDate ?? DateTime.MinValue;
            dgCurses.ItemsSource = BDCursa.getCurses(txbFiltreNom.Text,dataSeleccionada,estatCursa);
        }

        private int estatSelected() {
            if (rbActives.IsChecked == true)
            {
                return 1;
            }
            else if (rbCancelades.IsChecked == true)
            {
                return 4;
            }
            else if (rbTancades.IsChecked == true)
            {
                return 5;
            }
            else {
                return 0;
            }
        }

        private void mostrarDetallCursa()
        {
            BDCursa cursaSeleccionada = dgCurses.SelectedItem as BDCursa;

            txbNomCursa.Text = cursaSeleccionada.Nom;
            txbDataCursa.Text = cursaSeleccionada.DataInici.ToString("dd/MM/yyyy");
            txbLlocCursa.Text = cursaSeleccionada.Lloc;
            txbEsportiCategoria.Text = cursaSeleccionada.EsportId.ToString();
            txbEstatCursa.Text = cursaSeleccionada.EstatId.ToString();
            txbNumInscritsiLimit.Text = cursaSeleccionada.LimitInscripcions.ToString();
            BitmapImage bitmapImage = new BitmapImage(new Uri(cursaSeleccionada.UrlFoto));
            imgCursa.Source = bitmapImage;

            circuitsCursa = BDCircuit.getCircuitsFromCursa(cursaSeleccionada.Id);
            lsvCircuits.ItemsSource = circuitsCursa;
        }

    }
}
