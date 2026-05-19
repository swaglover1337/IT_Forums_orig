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
    /// Логика взаимодействия для EditEventPage.xaml
    /// </summary>
    public partial class EditEventPage : Page
    {
        public EditEventPage(Event selectedEvent)
        {
            InitializeComponent();
            if (selectedEvent == null)
                selectedEvent = new Event 
                { 
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now 

                };

            DataContext = selectedEvent;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            
            var currentEvent = DataContext as Event;

            if (string.IsNullOrWhiteSpace(currentEvent.Name))
            {
                MessageBox.Show("Введите название мероприятия!");
                return;
            }

            if (currentEvent.Event_ID == 0)
            {
                ITForum2BDEntities1.GetInst().Event.Add(currentEvent);
            }

            try
            {
               
                ITForum2BDEntities1.GetInst().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
    }
}
