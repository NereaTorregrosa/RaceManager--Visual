using BD_MySQL.Model;
using BD_MySQL;
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
using System.Collections;

namespace projecte3_TorregrosaNerea.Views
{
    /// <summary>
    /// Lógica de interacción para ConsultarCursesAnonim.xaml
    /// </summary>
    public partial class ConsultarCursesAnonim : Page
    {
        private List<BDCircuit> circuitsCursa;
        private List<BDCursa> curses;
        private BDCursa cursaSeleccionada;
        private OC<BDCheckpoints> checkpoints;
        private BDCheckpoints checkpointEditat;
        private BDCheckpoints checkpointClonat;
        private BDCircuit circuitSeleccionat;
        private BDCheckpoints checkpointAbansEditar;
        private string seleccioEstat;
        public ConsultarCursesAnonim()
        {
            InitializeComponent();
            omplirCboFiltreiInicialitzarRB();
            inicialitzarBotons();
            launchGetCurses();
            omplirCboEstats();
            grdDetallCursa.Visibility = Visibility.Collapsed;
            circuitsCursa = new List<BDCircuit>();
            checkpoints = new OC<BDCheckpoints>();
            curses = new List<BDCursa>();
        }


        private void cboFiltre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboFiltre.SelectedItem != null)
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
                            cboEstats.Visibility = Visibility.Collapsed;
                            break;
                        case "Filtrar per data":
                            dpFiltreData.Visibility = Visibility.Visible;
                            txbFiltreNom.Visibility = Visibility.Collapsed;
                            cboEstats.Visibility = Visibility.Collapsed;
                            break;
                        case "Filtrar per estat":
                            dpFiltreData.Visibility = Visibility.Collapsed;
                            txbFiltreNom.Visibility = Visibility.Collapsed;
                            cboEstats.Visibility = Visibility.Visible;
                            break;
                    }
                }

            }
        }

        private void dgCurses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarDesactivarBotons();
        }



        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            launchGetCurses();
        }

        private void btnNetejarFiltres_Click(object sender, RoutedEventArgs e)
        {
            txbFiltreNom.Text = "";
            dpFiltreData.SelectedDate = null;
            cboEstats.SelectedItem = null;
            launchGetCurses();
        }

        private void lsvCircuits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvCircuits.SelectedItem != null)
            {
                mostrarCheckpointsCursa();
                circuitSeleccionat = lsvCircuits.SelectedItem as BDCircuit;
                if (cursaSeleccionada.EstatId == 3 && cursaSeleccionada.NumParticipants < cursaSeleccionada.LimitInscripcions)
                {
                    btnInscripcio.IsEnabled = true;
                }
                else
                {
                    btnInscripcio.IsEnabled = false;
                }


            }

            activarDesactivarBotons();
        }


        private void dgKilometresCircuit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        private void cboEstats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboEstats.SelectedItem != null)
            {
                btnFiltrar.IsEnabled = true;
                btnNetejarFiltres.IsEnabled = true;
                if (cboEstats.SelectedItem is string)
                {
                    seleccioEstat = (string)cboEstats.SelectedItem;


                }
            }
        }


        private void btnInscripcio_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new Inscripcions(circuitSeleccionat));
        }



        private void btnVeureResultats_Click(object sender, RoutedEventArgs e)
        {
            if(cursaSeleccionada.EstatId == 6)
            {
                MainWindow.navigationFrame.Navigate(new LiveResults(cursaSeleccionada, circuitSeleccionat));
            }else if (cursaSeleccionada.EstatId == 7)
            {
                MainWindow.navigationFrame.Navigate(new ResultatsFinals(cursaSeleccionada, circuitSeleccionat));
            }
           
        }

        private void btnSortir_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new Login());
        }

        #region Mètodes
        public void omplirCboFiltreiInicialitzarRB()
        {
            cboFiltre.Items.Clear();
            cboFiltre.Items.Add("Filtrar per nom");
            cboFiltre.Items.Add("Filtrar per data");
            cboFiltre.Items.Add("Filtrar per estat");
        }
        public void inicialitzarBotons()
        {
            btnFiltrar.IsEnabled = false;
            btnNetejarFiltres.IsEnabled = false;
            btnInscripcio.IsEnabled = false;
            btnVeureResultats.IsEnabled = false;
        }

        private void launchGetCurses()
        {
            int estatCursa = 0;
            if (cboEstats.SelectedItem != null)
            {
                estatCursa = estatSelected(seleccioEstat);
            }
            DateTime dataSeleccionada;
            dataSeleccionada = dpFiltreData.SelectedDate ?? DateTime.MinValue;
            curses = BDCursa.getCurses(txbFiltreNom.Text, dataSeleccionada, estatCursa);
            curses.RemoveAll(c => c.EstatId == 1 || c.EstatId == 2 || c.EstatId == 4);
            dgCurses.ItemsSource = curses;
        }

        private int estatSelected(string seleccio)
        {
            if (seleccio.Equals("En preparació"))
            {
                return 1;
            }
            else if (seleccio.Equals("Esborrades"))
            {
                return 2;
            }
            else if (seleccio.Equals("Inscripció oberta"))
            {
                return 3;
            }
            else if (seleccio.Equals("Inscripció tancada"))
            {
                return 5;
            }
            else if (seleccio.Equals("En curs"))
            {
                return 6;
            }
            else if (seleccio.Equals("Finalitzades"))
            {
                return 7;
            }
            else if (seleccio.Equals("Cancel·lades"))
            {
                return 4;
            }
            else
            {
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
            txbNumInscritsiLimit.Text = numParticipants.ToString() + "/" + cursaSeleccionada.LimitInscripcions.ToString() + " participants";
            BitmapImage bitmapImage = new BitmapImage(new Uri(cursaSeleccionada.UrlFoto));
            imgCursa.Source = bitmapImage;

            circuitsCursa = BDCircuit.getCircuitsFromCursa(cursaSeleccionada.Id);
            lsvCircuits.ItemsSource = circuitsCursa;
        }

        private void mostrarCheckpointsCursa()
        {
            if (lsvCircuits.SelectedItem != null)
            {
                circuitSeleccionat = lsvCircuits.SelectedItem as BDCircuit;
                checkpoints = BDCheckpoints.getCheckpointsFromCircuit(circuitSeleccionat.Id);
                dgKilometresCircuit.ItemsSource = null;
                dgKilometresCircuit.ItemsSource = checkpoints;
            }
        }







        private void activarDesactivarBotons()
        {
            if (dgCurses.SelectedItem != null)
            {
                grdDetallCursa.Visibility = Visibility.Visible;

                mostrarDetallCursa();

                if ((cursaSeleccionada.EstatId == 6 || cursaSeleccionada.EstatId == 7) && circuitSeleccionat != null)
                {
                    btnVeureResultats.IsEnabled = true;
                }
                else
                {
                    btnVeureResultats.IsEnabled = false;
                }
            }
        }

        public void omplirCboEstats()
        {
            List<String> estats = new List<String>();
            estats.Add("Inscripció oberta");
            estats.Add("Inscripció tancada");
            estats.Add("En curs");
            estats.Add("Finalitzades");

            cboEstats.ItemsSource = estats;
        }


        #endregion

    }
}
