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
    /// Lógica de interacción para Inscripcions.xaml
    /// </summary>
    public partial class Inscripcions : Page
    {
        private BDCircuit circuitActual;
        private BDParticipant participantAInscriure;
        public Inscripcions()
        {
            InitializeComponent();
        }

        public Inscripcions(BDCircuit ci) : this()
        {
            circuitActual = ci;
        }

        private void chkFederat_Checked(object sender, RoutedEventArgs e)
        {
            //lblNumeroFederat.Visibility = Visibility.Visible;
            //txtNumeroFederat.Visibility = Visibility.Visible;
        }

        private void chkFederat_Unchecked(object sender, RoutedEventArgs e)
        {
            //lblNumeroFederat.Visibility = Visibility.Collapsed;
            //txtNumeroFederat.Visibility = Visibility.Collapsed;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Formulari enviat!");
            MainWindow.navigationFrame.Navigate(new ConsultarCurses());
        }

        private async void txtDNI_TextChanged(object sender, TextChangedEventArgs e)
        {
            string dni = txtDNI.Text.Trim();

            if (dni.Length >= 5)
            {
                bool exists = await BDParticipant.checkIfParticipantExist(dni);
                if (exists)
                {
                    participantAInscriure = BDParticipant.getParticipantByDNI(dni);
                    txtNom.Text = participantAInscriure.Nom;
                    txtCognoms.Text = participantAInscriure.Cognoms;
                    txtEmail.Text = participantAInscriure.Email;   
                    txtTelefon.Text = participantAInscriure.Telefon;
                    dpDataNaixement.SelectedDate = participantAInscriure.Data_naixement.Date;
                    if(participantAInscriure.EsFederat == true)
                    {
                        chkFederat.IsChecked = true;
                    }
                    else
                    {
                        chkFederat.IsChecked = false;
                    }

                    
                }
                else
                {
                    bool esFederat = false;
                    if (chkFederat.IsChecked == true)
                    {
                        esFederat = true;
                    }
                    else
                    {
                        esFederat = false;
                    }
                    participantAInscriure = new BDParticipant(txtNom.Text, txtCognoms.Text,dpDataNaixement.SelectedDate.Value, txtTelefon.Text, txtEmail.Text,esFederat,dni);
                    
                }
            }
        }
    }
}
