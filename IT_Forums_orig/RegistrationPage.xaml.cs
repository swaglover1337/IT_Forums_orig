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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        // Загружаем справочники в ComboBox при открытии страницы
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = ITForum2BDEntities1.GetInst();

                ComboEvents.ItemsSource = db.Event.ToList();
                ComboGroups.ItemsSource = db.Groups.ToList();
                ComboTickets.ItemsSource = db.Ticket.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки справочников: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // КНОПКА: Регистрация
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем, что всё выбрано и заполнено
            var selectedEvent = ComboEvents.SelectedItem as Event;
            var selectedGroup = ComboGroups.SelectedItem as Groups;
            var selectedTicket = ComboTickets.SelectedItem as Ticket;

            if (selectedEvent == null || selectedGroup == null || selectedTicket == null)
            {
                MessageBox.Show("Пожалуйста, выберите мероприятие, группу и тип билета!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtLastName.Text) || string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show("Поля 'Фамилия' и 'Имя' обязательны для заполнения!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var db = ITForum2BDEntities1.GetInst();

                // 2. Создаем нового участника (в таблице Members)
                var newMember = new Members
                {
                    SecondName = TxtLastName.Text.Trim(),
                    Name = TxtName.Text.Trim(),
                    PapaName = TxtMiddleName.Text.Trim(),
                    PhoneNumber = TxtPhone.Text.Trim(),
                    Email = TxtEmail.Text.Trim(),
                    Group_ID = selectedGroup.Group_ID // Привязываем выбранную группу
                };

                db.Members.Add(newMember);

                // Сохраняем, чтобы Entity Framework сгенерировал Member_ID для этого человека
                db.SaveChanges();

                // 3. Создаем запись о его регистрации на конкретное событие
                var newRegistration = new Registration
                {
                    Member_ID = newMember.Member_ID, // ID только что созданного юзера
                    Event_ID = selectedEvent.Event_ID,
                    Ticket_ID = selectedTicket.Ticket_ID
                };

                db.Registration.Add(newRegistration);
                db.SaveChanges();

                // 4. Уведомляем об успехе и чистим поля формы
                MessageBox.Show($"Участник {newMember.SecondName} {newMember.Name} успешно зарегистрирован!\nНомер билета: № {newRegistration.Reg_ID}",
                                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проведении регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Метод очистки текстовых полей после успешной операции
        private void ClearForm()
        {
            TxtLastName.Clear();
            TxtName.Clear();
            TxtMiddleName.Clear();
            TxtPhone.Clear();
            TxtEmail.Clear();
            ComboEvents.SelectedIndex = -1;
            ComboGroups.SelectedIndex = -1;
            ComboTickets.SelectedIndex = -1;
        }
    }
}

