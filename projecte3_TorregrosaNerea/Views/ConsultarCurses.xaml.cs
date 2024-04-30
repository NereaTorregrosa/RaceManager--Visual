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
        public ConsultarCurses()
        {
            InitializeComponent();
            omplirCboFiltreiInicialitzarRB();
            inicialitzarBotons();
        }

        private void cboFiltre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboFiltre.SelectedItem != null)
            {
                string seleccio = ((ComboBoxItem)cboFiltre.SelectedItem).Content.ToString();
                switch(seleccio) {
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

        private void dgCurses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dgCurses.SelectedItem != null)
            {
                btnGestionarCircuits.IsEnabled = true;
                btnEditarCursa.IsEnabled = true;
            }
        }
        public void omplirCboFiltreiInicialitzarRB()
        {
            cboFiltre.Items.Clear();
            cboFiltre.Items.Add("Filtrar per nom");
            cboFiltre.Items.Add("Filtrar per data");
            cboFiltre.Items.Add("Filtrar per estat");
            rbActives.IsChecked = true;
        }
        public void inicialitzarBotons() { 
            btnFiltrar.IsEnabled = false;
            btnNetejarFiltres.IsEnabled = false;
            btnEditarCursa.IsEnabled = false;
            btnGestionarCircuits.IsEnabled = false;
        }

        
    }
}
