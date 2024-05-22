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
    /// Lógica de interacción para AssiganrBeaconIDorsal.xaml
    /// </summary>
    public partial class AssiganrBeaconIDorsal : Page
    {
        private BDCursa cursaActual;
        private BDParticipant participantActual;
        private BDInscripcio inscripcioActual;
        public AssiganrBeaconIDorsal()
        {
            InitializeComponent();
        }

        public AssiganrBeaconIDorsal(BDCursa c,BDParticipant p,BDInscripcio i) : this()
        {
            cursaActual = c;
            participantActual = p;
            inscripcioActual = i;
            omplirCboBeacons();
        }

        private void txbDorsal_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text) && !Utils.EsNumeroEnter(txbDorsal.Text))
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

        private void cboBeaconCodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            guardarDorsalIBeacon();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new RecepcioCorredors(cursaActual));
        }
        public void omplirCboBeacons()
        {
            cboBeaconCodes.ItemsSource = BDBeacon.getBeaconsSenseUtilitzar(cursaActual.Id);
        }

        private void CheckFormValidity()
        {
            bool isTextDorsalValid = Utils.EsNumeroEnter(txbDorsal.Text) && !string.IsNullOrEmpty(txbDorsal.Text);

            if (isTextDorsalValid)

            {
                btnGuardar.IsEnabled = true;
            }
            else
            {
                btnGuardar.IsEnabled = false;
            }
        }

        private void guardarDorsalIBeacon()
        {
            if(cboBeaconCodes.SelectedItem != null)
            {
                string beaconCode = cboBeaconCodes.SelectedItem.ToString();
                int id = BDBeacon.getIdFromBeaconByCode(beaconCode);
                inscripcioActual.IdBeacon = id;
                inscripcioActual.Dorsal = Int32.Parse(txbDorsal.Text);
                if(BDInscripcio.IsDorsalDuplicated(inscripcioActual.IdCircuitCategoria, inscripcioActual.Dorsal))
                {
                    MessageBox.Show("Dorsal no disponible.");
                }
                else
                {
                    bool ok = BDInscripcio.updateInscripcio(inscripcioActual);
                    if (ok)
                    {
                        MessageBox.Show("Assignació feta correctament.");
                        MainWindow.navigationFrame.Navigate(new RecepcioCorredors(cursaActual));

                    }
                    else
                    {
                        MessageBox.Show("No s'ha pogut fer l'assignació.");
                    }
                }
                
            }
            else
            {
                MessageBox.Show("No es pot deixar el beacon code buit.");
            }
        }

    }
}
