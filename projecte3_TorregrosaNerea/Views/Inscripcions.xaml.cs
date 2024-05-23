using BD_MySQL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private BDParticipant nouParticipant;
        private BDInscripcio inscripcioNova;
        private int idParticipant = 0;
        private int numFederat = 0;
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
            lblNumeroFederat.Visibility = Visibility.Visible;
            txtNumeroFederat.Visibility = Visibility.Visible;
        }

        private void chkFederat_Unchecked(object sender, RoutedEventArgs e)
        {
            lblNumeroFederat.Visibility = Visibility.Collapsed;
            txtNumeroFederat.Visibility = Visibility.Collapsed;
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
                    idParticipant = participantAInscriure.Id;
                    txtNumeroFederat.Text = participantAInscriure.NumFederat.ToString();

                    
                }

            }
        }

        private void String_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Background = Brushes.Red;
                btnEnviar.IsEnabled = false;
            }
            else
            {
                textBox.Background = Brushes.White;
                btnEnviar.IsEnabled = true;
            }

            CheckFormValidity();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text) && !Utils.EsEmailValid(textBox.Text))
            {
                textBox.Background = Brushes.Red;
                btnEnviar.IsEnabled = false;
            }
            else
            {
                textBox.Background = Brushes.White;
                btnEnviar.IsEnabled = true;
            }

            CheckFormValidity();
        }

        private void txtNumeroFederat_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text) && !Utils.EsNumeroEnter(textBox.Text))
            {
                textBox.Background = Brushes.Red;
                btnEnviar.IsEnabled = false;
            }
            else
            {
                textBox.Background = Brushes.White;
                btnEnviar.IsEnabled = true;
            }

            CheckFormValidity();
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Hola");
            novaInscripcio();
        }
        private void novaInscripcio()
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
            if (chkFederat.IsChecked == true)
            {
                numFederat = Int32.Parse(txtNumeroFederat.Text);
            }
            bool ok = false;
            if (participantAInscriure != null)
            {
                idParticipant = participantAInscriure.Id;
                int idCircuitCategoria = BDCircuitCategoria.getCircuitCategoriaId(circuitActual.Id);
                inscripcioNova = new BDInscripcio(idParticipant, DateTime.Now.Date, 0, false, 0, idCircuitCategoria);
                bool okInscripcio = BDInscripcio.insertInscripcio(inscripcioNova);
                if (okInscripcio)
                {

                    MessageBox.Show("Inscripció correcta!");
                    MainWindow.navigationFrame.Navigate(new ConsultarCurses());
                }
                else
                {
                    MessageBox.Show("No s'ha pogut fer la inscripció");
                }
            }
            else
            {
                if(dpDataNaixement.SelectedDate != null)
                {
                    nouParticipant = new BDParticipant(txtNom.Text, txtCognoms.Text, dpDataNaixement.SelectedDate.Value, txtTelefon.Text, txtEmail.Text, esFederat, txtDNI.Text, numFederat);
                    ok = BDParticipant.insertParticipant(nouParticipant);
                    idParticipant = BDParticipant.ObtenirUltimParticipantId();
                    if (ok)
                    {
                        int idCircuitCategoria = BDCircuitCategoria.getCircuitCategoriaId(circuitActual.Id);
                        inscripcioNova = new BDInscripcio(idParticipant, DateTime.Now.Date, 0, false, 0, idCircuitCategoria);
                        bool okInscripcio = BDInscripcio.insertInscripcio(inscripcioNova);
                        if (okInscripcio)
                        {

                            MessageBox.Show("Inscripció correcta!");
                            MainWindow.navigationFrame.Navigate(new ConsultarCurses());
                        }
                        else
                        {
                            MessageBox.Show("No s'ha pogut fer la inscripció");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("La data de naixement no pot estar buida.");
                }
            }


        }

        private void CheckFormValidity()
        {
            bool isTextNomValid = !string.IsNullOrEmpty(txtNom.Text);
            bool isTextCognomValid = !string.IsNullOrEmpty(txtCognoms.Text);
            bool isTextTelfValid = !string.IsNullOrEmpty(txtTelefon.Text);
            bool isTextMailValid = !string.IsNullOrEmpty(txtEmail.Text) && Utils.EsEmailValid(txtEmail.Text);

            if (isTextNomValid && isTextCognomValid && isTextTelfValid && isTextMailValid)
            {
                btnEnviar.IsEnabled = true;
            }
            else
            {
                btnEnviar.IsEnabled = false;
            }
        }


    }
}
