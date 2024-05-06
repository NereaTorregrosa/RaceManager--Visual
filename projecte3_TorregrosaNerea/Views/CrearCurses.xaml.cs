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
    /// Lógica de interacción para CrearCurses.xaml
    /// </summary>
    public partial class CrearCurses : Page
    {
        public CrearCurses()
        {
            InitializeComponent();
            omplirComboBoxEsports();
        }

        private void cboTipus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTipus.SelectedItem != null)
            {
                BDEsport esportSeleccionat = cboTipus.SelectedItem as BDEsport;
                cboCategoria.ItemsSource = BDCategoria.getCategoriesFromEsport(esportSeleccionat.Id);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new ConsultarCurses());
        }
        public void omplirComboBoxEsports()
        {
            cboTipus.ItemsSource = BDEsport.getEsports();
            
        }

        
    }
}
