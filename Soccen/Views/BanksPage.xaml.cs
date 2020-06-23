using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;
using System.Linq;
using Soccen.Models;
using System;

namespace Soccen.Views
{
    /// <summary>
    /// Логика взаимодействия для BanksPage.xaml
    /// </summary>
    public partial class BanksPage : Page
    {
        // Оголошення та заповнення даними про моделі класів контексту моделі БД
        soccenEntities context = new soccenEntities();
        // Оголошення колекціх
        CollectionViewSource banksViewSource;
        public BanksPage()
        {
            InitializeComponent();
            banksViewSource = ((CollectionViewSource)(FindResource("banksViewSource")));
            // Присвоювання полю полів цієї сторінки
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Загрузка даних про банки в пам'ять
            context.banks.Load();
            // Присвоювання даних про банки в колекцію
            banksViewSource.Source = context.banks.Local;
        }
        //Перехід до кінцевого елементу колекції
        private void LastCommandHandler(object sender, RoutedEventArgs e)
        {
            banksViewSource.View.MoveCurrentToLast();
        }
        //Перехід до попереднього елементу колекції
        private void PreviousCommandHandler(object sender, RoutedEventArgs e)
        {
            banksViewSource.View.MoveCurrentToPrevious();
        }
        //Перехід до наступного елементу колекції
        private void NextCommandHandler(object sender, RoutedEventArgs e)
        {
            banksViewSource.View.MoveCurrentToNext();
        }
        //Перехід до першого елементу колекції
        private void FirstCommandHandler(object sender, RoutedEventArgs e)
        {
            banksViewSource.View.MoveCurrentToFirst();
        }

        private void DeleteCommandHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                // Зміна cur дорівнює елементу, який наразі відображає колекція
                var cur = banksViewSource.View.CurrentItem as bank;
                // Зміна bk елементу з моделі таблиці банків, який дорівнбє зміній cur
                var bk = (from b in context.banks
                           where b.IdBank == cur.IdBank
                           select b).FirstOrDefault();
                // ПеревІрка чи зміна щось найшла та видалення
                if (bk != null)
                {
                    context.banks.Remove(bk);
                }

                MessageBox.Show("Видалення відбулося успішно!");
                // Збереження даних до БД
                context.SaveChanges();
                // Оновлення вигляду колекціх відображення
                banksViewSource.View.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Щойно відбувся оброблений виняток: " + ex.Message, "Помилка", MessageBoxButton.OK);
            }

        }


        private void UpdateCommandHandler(object sender, RoutedEventArgs e)
        {
            if (newBankGrid.IsVisible)
            {

                bank newBank = new bank
                {
                    Title = add_titleTextBox.Text,
                };
                context.banks.Local.Insert(0, newBank);
                banksViewSource.View.Refresh();
                banksViewSource.View.MoveCurrentTo(newBank);
                newBankGrid.Visibility = Visibility.Collapsed;
                existingBankGrid.Visibility = Visibility.Visible;

            }

            context.SaveChanges();
            banksViewSource.View.Refresh();
        }


        private void AddCommandHandler(object sender, RoutedEventArgs e)
        {
            newBankGrid.Visibility = Visibility.Visible;
            existingBankGrid.Visibility = Visibility.Collapsed;

            add_titleTextBox.Text = "";
        }

        private void CancelCommandHandler(object sender, RoutedEventArgs e)
        {
            add_titleTextBox.Text = "";
        }

        private void bankDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            existingBankGrid.Visibility = Visibility.Visible;
            newBankGrid.Visibility = Visibility.Collapsed;
        }
    }
}
