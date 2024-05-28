using BD_MySQL;
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
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace projecte3_TorregrosaNerea.Views
{
    /// <summary>
    /// Lógica de interacción para LiveResults.xaml
    /// </summary>
    public partial class LiveResults : Page
    {
        private BDCursa cursaActual;
        private BDCircuit circuitActual;
        private DispatcherTimer timer;
        private OC<BDRegistres> registresActuals;
        private BDUsuari usuariActual;
        public LiveResults()
        {
            InitializeComponent();
            InitializeTimer();
            registresActuals = new OC<BDRegistres>();
            grdDetallRegistre.Visibility = Visibility.Collapsed;
        }

        public LiveResults(BDCursa c,BDCircuit ci,BDUsuari u ) : this()
        {
            cursaActual = c;
            circuitActual = ci;
            usuariActual = u;
            launchGetResults();

        }

        public LiveResults(BDCursa c, BDCircuit ci) : this()
        {
            cursaActual = c;
            circuitActual = ci;
            launchGetResults();

        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            BDRegistres registre = (BDRegistres)stackPanel.Tag;
            int numChk = BDCheckpoints.numeroCheckpoints(circuitActual.Id);
            
            foreach (BDRegistres actual in registresActuals)
            {
                int passedCheckpoints = BDRegistres.numeroCheckpointsPassats(circuitActual.Id,actual.IdParticipant,cursaActual.Id);
                SetImageColor(stackPanel,passedCheckpoints,numChk);
            }
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            launchGetResults();
        }

        private void btnNetejarFiltre_Click(object sender, RoutedEventArgs e)
        {
            txbFiltreNom.Text = "";
            txbFiltreDorsal.Text = "";
            launchGetResults();
        }

        private void btnViewDetail_Click(object sender, RoutedEventArgs e)
        {
            
            Button button = sender as Button;

            BDRegistres registre = button.DataContext as BDRegistres;
            if (registre != null)
            {
                txbNomParticipant.Text = registre.NomParticipant;
                txbDorsalParticipant.Text = registre.DorsalParticipant.ToString();
                grdDetallRegistre.Visibility = Visibility.Visible;
                lsvCheckpoints.ItemsSource = BDRegistres.registresCursaTotal(cursaActual.Id,circuitActual.Id,registre.IdParticipant);
            }

            
        }

        private void btnTornarAConsultarCurses_Click(object sender, RoutedEventArgs e)
        {
            if(usuariActual != null)
            {
                MainWindow.navigationFrame.Navigate(new ConsultarCurses(usuariActual));
            }
            else
            {
                MainWindow.navigationFrame.Navigate(new ConsultarCursesAnonim());
            }
            
        }

        public void launchGetResults()
        {

            int dorsalFilter = 0;
            bool applyDorsalFilter = true;
            if (string.IsNullOrWhiteSpace(txbFiltreDorsal.Text))
            {
                applyDorsalFilter = false;
            }
            else
            {
                if (!int.TryParse(txbFiltreDorsal.Text, out dorsalFilter))
                {
                    MessageBox.Show("Siusplau, entra un numero vàlid per el filtre de dorsal.");
                    return;
                }
            }

            if (applyDorsalFilter)
            {
                registresActuals = BDRegistres.registresCursaParcial(cursaActual.Id, circuitActual.Id, txbFiltreNom.Text, dorsalFilter);
                dgResults.ItemsSource = registresActuals;
            }
            else
            {
                registresActuals = BDRegistres.registresCursaParcial(cursaActual.Id, circuitActual.Id, txbFiltreNom.Text);
                dgResults.ItemsSource = registresActuals;

            }
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            launchGetResults();
        }

        private void SetImageColor(StackPanel stackPanel, int passedCheckpoints, int totalCheckpoints)
        {
            string imagePath = passedCheckpoints >= totalCheckpoints ? "pack://application:,,,/assets/green_dot.png" : "pack://application:,,,/assets/grey_dot.png";

            for (int i = 0; i < totalCheckpoints; i++)
            {
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Width = 20;
                img.Height = 20;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();
                img.Source = bitmap;
                stackPanel.Children.Add(img);
            }
        }


    }
}
