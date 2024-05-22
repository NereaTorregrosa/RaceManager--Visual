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
    /// Lógica de interacción para RecepcioCorredors.xaml
    /// </summary>
    public partial class RecepcioCorredors : Page
    {
        private BDCursa cursaActual;
        private BDParticipant participantSeleccionat;
        private BDInscripcio inscripcioActual;

        public RecepcioCorredors()
        {
            InitializeComponent();
        }

        public RecepcioCorredors(BDCursa c) : this()
        {
            if (c != null)
            {
                cursaActual = c;
            }

            launchGetParticipants();
            grdDetallParticipant.Visibility = Visibility.Collapsed;
            btnAssiganrBeaconDorsal.IsEnabled = false;
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            launchGetParticipants();
        }

        private void btnNetejarFiltre_Click(object sender, RoutedEventArgs e)
        {
            txbFiltre.Text = "";
            launchGetParticipants();
        }
        private void dgParticipants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgParticipants.SelectedItem != null)
            {
                mostrarDetallParticipant();
            }
               
        }

        private void btnAssiganrBeaconDorsal_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new AssiganrBeaconIDorsal(cursaActual,participantSeleccionat,inscripcioActual));
        }

        private void btnRetirarParticipant_Click(object sender, RoutedEventArgs e)
        {
            retirarParticipant();
        }

        private void btnTornarAConsultarCurses_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new ConsultarCurses());
        }
        private void launchGetParticipants()
        {
            dgParticipants.ItemsSource = BDParticipant.getParticipantsFromCursa(cursaActual.Id,txbFiltre.Text);
        }

        private void mostrarDetallParticipant()
        {
            if (dgParticipants.SelectedItem != null)
            {
                grdDetallParticipant.Visibility = Visibility.Visible;
                participantSeleccionat = dgParticipants.SelectedItem as BDParticipant;
                txbNom.Text = participantSeleccionat.Nom + " " + participantSeleccionat.Cognoms;
                txbTelefon.Text = participantSeleccionat.Telefon;
                txbNif.Text = participantSeleccionat.Nif;
                txbEmail.Text = participantSeleccionat.Email;
                txbDataNaix.Text = Utils.convertirDateTimeAStringData(participantSeleccionat.Data_naixement);
                if(participantSeleccionat.EsFederat == true)
                {
                    txbFederat.Text = "Sí";
                }
                else
                {
                    txbFederat.Text = "No";
                }
                inscripcioActual = BDInscripcio.getDadesInscripcioFromParticipant(participantSeleccionat.Id,cursaActual.Id);
                txbDorsal.Text = inscripcioActual.Dorsal.ToString();
                if(inscripcioActual.IdBeacon != 0)
                {
                    string beaconCode = BDBeacon.getCodeFromBeaconById(inscripcioActual.IdBeacon);
                    txbBeacon.Text = beaconCode;
                    btnAssiganrBeaconDorsal.IsEnabled = false;
                }
                else
                {
                    txbBeacon.Text = "";
                    btnAssiganrBeaconDorsal.IsEnabled = true;
                }

                if (inscripcioActual.Retirat == true)
                {
                    txbRetirat.Text = "Sí";
                    btnRetirarParticipant.IsEnabled = false;
                }
                else
                {
                    txbRetirat.Text = "No";
                    btnRetirarParticipant.IsEnabled = true;

                }
            }
           
        }

        private void retirarParticipant()
        {
            MessageBoxResult result = MessageBox.Show("¿Estás segur de que vols retirar el participant?", "Confirmació de retirament de participant", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                inscripcioActual.Retirat = true;
                bool ok = BDInscripcio.updateInscripcio(inscripcioActual);
                if (ok)
                {
                    MessageBox.Show("Participant retirat correctament.");
                    grdDetallParticipant.Visibility = Visibility.Collapsed;

                }
                else
                {
                    MessageBox.Show("No s'ha pogut retirar el participant");
                }
            }
            
        }


    }
}
