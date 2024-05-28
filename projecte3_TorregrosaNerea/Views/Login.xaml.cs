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
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool ok = false;
            ok = BDUsuari.isLoginCorrect(txtUser.Text, txtPass.Password);
            if (ok)
            {
                BDUsuari user = new BDUsuari(txtUser.Text, txtPass.Password);
                MainWindow.navigationFrame.Navigate(new ConsultarCurses(user));
            } else
            {
                MessageBox.Show("Nom d'usuari o contrasenya incorrectes.");
            }
        }

        private void btnAnonymous_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.navigationFrame.Navigate(new ConsultarCursesAnonim());
        }
    }
}
