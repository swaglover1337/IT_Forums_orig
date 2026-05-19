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
    /// Логика взаимодействия для ProgramsPage.xaml
    /// </summary>
    public partial class ProgramsPage : Page
    {
        public ProgramsPage()
        {
            InitializeComponent();
            ComboEvents.ItemsSource = ITForum2BDEntities1.GetInst().Event
                .Where(x => x.StartDate.Value.Year == 2026)
                .ToList();
        }

        // Выбор ивента в ListBox
        private void ComboEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEvent = ComboEvents.SelectedItem as Event;
            RefreshSessions(selectedEvent);
        }

        // Подгрузка комбобоксов залов
        private void ComboLocations_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.ItemsSource = ITForum2BDEntities1.GetInst().Location.ToList();
            }
        }

        // Кнопка сохранения изменений программы
        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ITForum2BDEntities1.GetInst().SaveChanges();
                MessageBox.Show("Программа мероприятия успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                var selectedEvent = ComboEvents.SelectedItem as Event;
                RefreshSessions(selectedEvent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Кнопка: + Добавить новую сессию
        private void BtnAddSession_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = ComboEvents.SelectedItem as Event;
            if (selectedEvent == null)
            {
                MessageBox.Show("Сначала выберите мероприятие в списке слева!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newSession = new Session
            {
                Event_ID = selectedEvent.Event_ID,
                Theme = "Новая сессия",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                Loc_ID = 1 // Значение по умолчанию для залов
            };

            ITForum2BDEntities1.GetInst().Session.Add(newSession);

            try
            {
                ITForum2BDEntities1.GetInst().SaveChanges();
                RefreshSessions(selectedEvent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при автоматическом добавлении сессии: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        // Кнопка: ❌ Удалить выбранную сессию
        private void BtnDeleteSession_Click(object sender, RoutedEventArgs e)
        {
            var currentSession = (sender as Button).DataContext as Session;

            if (currentSession != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить сессию '{currentSession.Theme}'?",
                                             "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ITForum2BDEntities1.GetInst().Session.Remove(currentSession);
                    ITForum2BDEntities1.GetInst().SaveChanges();

                    var selectedEvent = ComboEvents.SelectedItem as Event;
                    RefreshSessions(selectedEvent);
                }
            }
        }

        // Метод синхронизации данных на форме
        private void RefreshSessions(Event selectedEvent)
        {
            if (selectedEvent != null)
            {
                SessionsControl.ItemsSource = ITForum2BDEntities1.GetInst().Session
                    .Where(x => x.Event_ID == selectedEvent.Event_ID)
                    .ToList();
            }
            else
            {
                SessionsControl.ItemsSource = null;
            }
        }

    }
}
