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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            EventsList.ItemsSource = ITForum2BDEntities1.GetInst().Event.ToList();
        }

        private void EventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем объект, на который нажал пользователь
            var selectedEvent = EventsList.SelectedItem as Event;

            if (selectedEvent != null)
            {
                // Переходим на страницу деталей и передаем туда наш объект
                NavigationService.Navigate(new EventDetailPage(selectedEvent));

                // Сбрасываем выделение, чтобы можно было нажать на ту же карточку снова
                EventsList.SelectedItem = null;
            }
        }
    }
}
