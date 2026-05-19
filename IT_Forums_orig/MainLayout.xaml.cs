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

namespace IT_Forums_orig
{
    /// <summary>
    /// Логика взаимодействия для MainLayout.xaml
    /// </summary>
    public partial class MainLayout : Page
    {
        public MainLayout()
        {
            InitializeComponent();
            InnerFrame.Navigate(new MainPage());
        }

        private void Events_Click(object sender, RoutedEventArgs e)
        {
            InnerFrame.Navigate(new MainPage());
        }

        private void EditEvent_Click(object sender, RoutedEventArgs e)
        {
            InnerFrame.Navigate(new EditEventPage(null));
        }

        private void Programs_Click(object sender, RoutedEventArgs e)
        {
            InnerFrame.Navigate(new ProgramsPage());
        }

        private void Members_Click(object sender, RoutedEventArgs e)
        {
            InnerFrame.Navigate(new MembersPage());
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            InnerFrame.Navigate(new RegistrationPage());
        }
    }
}
