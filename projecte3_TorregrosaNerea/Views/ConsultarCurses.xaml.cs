using BD_MySQL;
using BD_MySQL.Model;
using System;
using System.Collections;
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
        private BDCheckpoints checkpointClonat;
        private BDCircuit circuitSeleccionat;
        private BDCheckpoints checkpointAbansEditar;
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
            activarDesactivarBotons();
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
            Console.WriteLine("Hola");
            if(checkpointClonat != null)
            {
                if (BDCheckpoints.CheckpointExists(checkpointClonat.IdCircuit, checkpointClonat.Kilometre))
                {
                    bool ok = BDCheckpoints.UpdateCheckpoint(checkpointEditat);
                    if (ok)
                    {
                        MessageBox.Show("Kilòmetre editat correctament.");
                        mostrarCheckpointsCursa();

                    }
                    else
                    {
                        MessageBox.Show("No s'ha pogut editar el kilòmetre.");
                    }
                }
            }
            else
            {
                afegirKilometreACircuit();
            }
            
        }


        private void dgKilometresCircuit_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGridRow row = e.Row;
            checkpointEditat = (BDCheckpoints)row.Item;
            checkpointEditat.IdCircuit = circuitSeleccionat.Id;

        }

        private void btnEditarCursa_Click(object sender, RoutedEventArgs e)
        {
            if(dgCurses.SelectedItem != null)
            {
                BDCursa cursaSeleccionada = dgCurses.SelectedItem as BDCursa;
                if(cursaSeleccionada.EstatId == 1)
                {
                    btnEditarCursa.IsEnabled = true;
                    MainWindow.navigationFrame.Navigate(new EditarCursa(cursaSeleccionada));
                }
            }
        }

        private void btnEditarCircuit_Click(object sender, RoutedEventArgs e)
        {
            if(lsvCircuits.SelectedItem != null)
            {
                BDCircuit circuitActual = lsvCircuits.SelectedItem as BDCircuit;
                MainWindow.navigationFrame.Navigate(new EditarCircuits(obtenirFilesGridCheckpoints(), circuitActual,cursaSeleccionada));
            }
            
        }

        private void dgKilometresCircuit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("Hola");
            if (dgKilometresCircuit.SelectedItem != null)
            {
                DataGridRow row = (DataGridRow)dgKilometresCircuit.ItemContainerGenerator.ContainerFromItem(dgKilometresCircuit.SelectedItem); ;
                if (IsRowEmpty(row))
                {
                    checkpointAbansEditar = (BDCheckpoints)dgKilometresCircuit.SelectedItem;
                    checkpointClonat = checkpointAbansEditar.Clone();
                }
                
            }
        }

        private void btnObrirInscripcioCursa_Click(object sender, RoutedEventArgs e)
        {

            cursaSeleccionada.EstatId = 3;
            bool ok = BDCursa.updateCursa(cursaSeleccionada);
            if (ok)
            {
                MessageBox.Show("Estat de la cursa modificat correctament.");
                grdDetallCursa.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("No s'ha pogut canviar l'estat de la cursa.");
            }
        }

        private void btnEliminarCursa_Click(object sender, RoutedEventArgs e)
        {
            confirmarEliminarCursa();    
        }
        private void btnCancelarCursa_Click(object sender, RoutedEventArgs e)
        {
            confirmarCancelarCursa();
        }

        #region Mètodes
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
            btnObrirInscripcioCursa.IsEnabled = false;
            btnEliminarCursa.IsEnabled = false;
            btnTancarInscripcionsCursa.IsEnabled = false;
            btnCancelarCursa.IsEnabled = false;
            btnIniciarCursa.IsEnabled = false;
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
            int numParticipants = BDCursa.getParticipantsCursa(cursaSeleccionada.Id);
            txbNomCursa.Text = cursaSeleccionada.Nom.ToUpper();
            txbDataCursa.Text = cursaSeleccionada.DataInici.ToString("dd/MM/yyyy");
            txbLlocCursa.Text = cursaSeleccionada.Lloc.ToUpper();
            txbEsportiCategoria.Text = BDEsport.getEsportById(cursaSeleccionada.EsportId).Nom.ToUpper();
            txbEstatCursa.Text = BDEstat.getEstatById(cursaSeleccionada.EstatId).ToUpper();
            txbNumInscritsiLimit.Text = numParticipants.ToString()+"/"+cursaSeleccionada.LimitInscripcions.ToString()+ " participants";
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

        private int obtenirFilesGridCheckpoints()
        {
            
            var sourceCollection = dgKilometresCircuit.ItemsSource as IEnumerable;
            if (sourceCollection != null)
            {
                return sourceCollection.Cast<object>().Count();
            }
            else return 0;
        }

        private bool IsRowEmpty(DataGridRow row)
        {
            return row.Item == null;
        }


        private void confirmarEliminarCursa()
        {
            MessageBoxResult result = MessageBox.Show("¿Estás segur de que vols eliminar la cursa?", "Confirmació d'eliminació de cursa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool ok = BDCursa.deleteCursa(cursaSeleccionada.Id,cursaSeleccionada);
                if (ok) {
                    MessageBox.Show("Cursa eliminada correctament.");
                    launchGetCurses();
                    grdDetallCursa.Visibility = Visibility.Collapsed;

                }
                else
                {
                    MessageBox.Show("No s'ha pogut eliminar la cursa: " + cursaSeleccionada.ErrorEliminar);
                }
            }
        }

        private void confirmarCancelarCursa()
        {
            MessageBoxResult result = MessageBox.Show("¿Estás segur de que vols cancel·lar la cursa?", "Confirmació de cancel·lació de cursa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                cursaSeleccionada.EstatId = 4;
                bool ok = BDCursa.updateCursa(cursaSeleccionada);
                if (ok)
                {
                    MessageBox.Show("Estat de la cursa canviat correctament.");
                    launchGetCurses();
                    grdDetallCursa.Visibility = Visibility.Collapsed;

                }
                else
                {
                    MessageBox.Show("No s'ha pogut canviar l'estat de la cursa: ");
                }
            }
        }

        private void activarDesactivarBotons()
        {
            if (dgCurses.SelectedItem != null)
            {
                grdDetallCursa.Visibility = Visibility.Visible;

                mostrarDetallCursa();
                if (cursaSeleccionada.EstatId == 1)
                {
                    btnObrirInscripcioCursa.IsEnabled = true;
                    btnEliminarCursa.IsEnabled = true;
                    btnEditarCursa.IsEnabled = true;
                    btnEditarCircuit.IsEnabled = true;
                    btnCrearCircuit.IsEnabled = true;
                }
                else
                {
                    btnObrirInscripcioCursa.IsEnabled = false;
                    btnEliminarCursa.IsEnabled = false;
                    btnEditarCursa.IsEnabled = false;
                    btnEditarCircuit.IsEnabled = false;
                    btnCrearCircuit.IsEnabled = false;
                }

                if (cursaSeleccionada.EstatId == 3 || cursaSeleccionada.EstatId == 6 || cursaSeleccionada.EstatId == 5)
                {
                    btnCancelarCursa.IsEnabled = true;
                }
                else
                {
                    btnCancelarCursa.IsEnabled = false;
                }

                if(cursaSeleccionada.EstatId == 3)
                {
                    btnTancarInscripcionsCursa.IsEnabled = true;
                }
                else
                {
                    btnTancarInscripcionsCursa.IsEnabled = false;
                }

                if(cursaSeleccionada.EstatId == 5)
                {
                    btnIniciarCursa.IsEnabled = true;
                }
                else
                {
                    btnIniciarCursa.IsEnabled = false;
                }
            }
        }

        #endregion


    }
}
