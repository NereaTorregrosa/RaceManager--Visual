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
            mostrarDetallParticipant();
        }
        private void launchGetParticipants()
        {
            dgParticipants.ItemsSource = BDParticipant.getParticipantsFromCursa(cursaActual.Id,txbFiltre.Text);
        }

        private void mostrarDetallParticipant()
        {
            if (dgParticipants.SelectedItem != null)
            {
                BDParticipant participantSeleccionat = dgParticipants.SelectedItem as BDParticipant;
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
                BDInscripcio inscripcioActual = BDInscripcio.getDadesInscripcioFromParticipant(participantSeleccionat.Id,cursaActual.Id);
                txbDorsal.Text = inscripcioActual.Dorsal.ToString();
                string beaconCode = BDBeacon.getCodeFromBeaconById(inscripcioActual.IdBeacon);
                txbBeacon.Text = beaconCode;
                if (inscripcioActual.Retirat == true)
                {
                    txbRetirat.Text = "Sí";
                }
                else
                {
                    txbRetirat.Text = "No";
                }
            }
           
        }

    }
}
