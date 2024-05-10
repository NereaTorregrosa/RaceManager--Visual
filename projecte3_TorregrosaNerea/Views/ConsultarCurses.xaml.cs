using BD_MySQL;
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
        private BDCursa cursaSeleccionada;
        private OC<BDCheckpoints> checkpoints;
        private BDCheckpoints checkpointEditat;
        private BDCircuit circuitSeleccionat;
        public ConsultarCurses()
        {
            InitializeComponent();
            omplirCboFiltreiInicialitzarRB();
            inicialitzarBotons();
            launchGetCurses();
            circuitsCursa = new List<BDCircuit>();
            checkpoints = new OC<BDCheckpoints>();
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
                btnEditarCircuit.IsEnabled = true;
                btnCrearCircuit.IsEnabled = true;
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

        private void btnCrearCircuit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new CrearCircuits(cursaSeleccionada));
        }

        private void lsvCircuits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvCircuits.SelectedItem != null)
            {
                btnCrearKMChk.IsEnabled = true;
                btnGuardarKMChk.IsEnabled = true;
                btnEliminarKMChk.IsEnabled = true;
                mostrarCheckpointsCursa();
                circuitSeleccionat = lsvCircuits.SelectedItem as BDCircuit;

            }
            else
            {
                btnCrearKMChk.IsEnabled = false;
                btnGuardarKMChk.IsEnabled = false;
                btnEliminarKMChk.IsEnabled = false;
            }
        }

        private void btnCrearKMChk_Click(object sender, RoutedEventArgs e)
        {
            checkpoints.Add(new BDCheckpoints());
            dgKilometresCircuit.ItemsSource = checkpoints;
        }

        private void btnGuardarKMChk_Click(object sender, RoutedEventArgs e)
        {
            afegirKilometreACircuit();
        }


        private void dgKilometresCircuit_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGridRow row = e.Row;
            checkpointEditat = (BDCheckpoints)row.Item;
            checkpointEditat.IdCircuit = circuitSeleccionat.Id;

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
            btnEditarCircuit.IsEnabled = false;
            btnCrearCircuit.IsEnabled = false;
            grdDetallCursa.Visibility = Visibility.Collapsed;
            btnCrearKMChk.IsEnabled = false;
            btnGuardarKMChk.IsEnabled = false;
            btnEliminarKMChk.IsEnabled = false;
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
            cursaSeleccionada = dgCurses.SelectedItem as BDCursa;

            txbNomCursa.Text = cursaSeleccionada.Nom.ToUpper();
            txbDataCursa.Text = cursaSeleccionada.DataInici.ToString("dd/MM/yyyy");
            txbLlocCursa.Text = cursaSeleccionada.Lloc.ToUpper();
            txbEsportiCategoria.Text = BDEsport.getEsportById(cursaSeleccionada.EsportId).ToUpper();
            txbEstatCursa.Text = BDEstat.getEstatById(cursaSeleccionada.EstatId).ToUpper();
            txbNumInscritsiLimit.Text = cursaSeleccionada.LimitInscripcions.ToString();
            BitmapImage bitmapImage = new BitmapImage(new Uri(cursaSeleccionada.UrlFoto));
            imgCursa.Source = bitmapImage;

            circuitsCursa = BDCircuit.getCircuitsFromCursa(cursaSeleccionada.Id);
            lsvCircuits.ItemsSource = circuitsCursa;
        }

        private void mostrarCheckpointsCursa()
        {
            if(lsvCircuits.SelectedItem != null) { 
                BDCircuit circuitActual = lsvCircuits.SelectedItem as BDCircuit;
                checkpoints = BDCheckpoints.getCheckpointsFromCircuit(circuitActual.Id);
                dgKilometresCircuit.ItemsSource = null;
                dgKilometresCircuit.ItemsSource = checkpoints;
            }
        }

        private void afegirKilometreACircuit()
        {
            if (comprovarValorGridCheckpoints()) { 
                btnGuardarKMChk.IsEnabled = true;
                btnCrearKMChk.IsEnabled = true;
                bool ok = BDCheckpoints.insertCheckpoint(checkpointEditat);
                if (ok)
                {
                    MessageBox.Show("Kilòmetre inserit correctament.");
                    mostrarCheckpointsCursa();

                }
                else
                {
                    MessageBox.Show("No s'ha pogut inserir el kilòmetre.");
                }
            }
            else
            {
                btnGuardarKMChk.IsEnabled = false;
                btnCrearKMChk.IsEnabled = false;
            }
        }

        private bool comprovarValorGridCheckpoints()
        {
            foreach (var item in dgKilometresCircuit.Items)
            {
                var row = (DataGridRow)dgKilometresCircuit.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null && Validation.GetHasError(row))
                {
                    MessageBox.Show("El kilòmetres introduït ha de ser numèric", "Afegir Kilòmetre", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                    break;
                    
                }
                else
                {
                    return true;
                }
            }
            return false;
        }


    }
}
