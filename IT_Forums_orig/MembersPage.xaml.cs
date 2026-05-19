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
    /// Логика взаимодействия для MembersPage.xaml
    /// </summary>
    public partial class MembersPage : Page
    {
        public MembersPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var registrationsList = ITForum2BDEntities1.GetInst().Registration
                    .Include("Members")
                    .Include("Members.Groups")
                    .Include("Ticket")
                    .Include("Event")
                    .ToList();

                ListViewMembers.ItemsSource = registrationsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

            private void ListViewMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedReg = ListViewMembers.SelectedItem as Registration;

            if (selectedReg != null && selectedReg.Members != null)
            {
                DetailsGrid.Visibility = Visibility.Visible;

                TxtEventName.Text = selectedReg.Event != null ? selectedReg.Event.Name : "Мероприятие не указано";
                TxtName.Text = selectedReg.Members.Name;
                TxtLastName.Text = selectedReg.Members.SecondName;
                TxtMiddleName.Text = selectedReg.Members.PapaName;
                TxtPhone.Text = selectedReg.Members.PhoneNumber;
                TxtEmail.Text = selectedReg.Members.Email;

                TxtGroup.Text = selectedReg.Members.Groups != null ? selectedReg.Members.Groups.Type : "Без группы";

                string ticketType = selectedReg.Ticket != null ? selectedReg.Ticket.Type : "Стандарт";
                TxtTicketInfo.Text = $"№ {selectedReg.Reg_ID} ({ticketType})";
            }
            else
            {
                DetailsGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedReg = ListViewMembers.SelectedItem as Registration;

            if (selectedReg != null && selectedReg.Members != null)
            {
                selectedReg.Members.Name = TxtName.Text.Trim();
                selectedReg.Members.SecondName = TxtLastName.Text.Trim();
                selectedReg.Members.PapaName = TxtMiddleName.Text.Trim();
                selectedReg.Members.PhoneNumber = TxtPhone.Text.Trim();
                selectedReg.Members.Email = TxtEmail.Text.Trim();

                try
                {
                    ITForum2BDEntities1.GetInst().SaveChanges();
                    MessageBox.Show("Данные участника успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ListViewMembers.Items.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedReg = ListViewMembers.SelectedItem as Registration;

            if (selectedReg != null)
            {
                var result = MessageBox.Show($"Удалить регистрацию участника {selectedReg.Members.SecondName} {selectedReg.Members.Name} на мероприятие '{selectedReg.Event.Name}'?",
                                             "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ITForum2BDEntities1.GetInst().Registration.Remove(selectedReg);
                        ITForum2BDEntities1.GetInst().SaveChanges();

                        MessageBox.Show("Регистрация успешно аннулирована.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadData();
                        DetailsGrid.Visibility = Visibility.Collapsed;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
