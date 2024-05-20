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
        private void launchGetParticipants()
        {
            dgParticipants.ItemsSource = BDParticipant.getParticipantsFromCursa(cursaActual.Id,txbFiltre.Text);
        }


    }
}
