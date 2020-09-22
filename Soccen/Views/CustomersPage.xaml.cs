using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;
using System.Linq;
using Soccen.Models;
using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Soccen.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        soccenEntities context = new soccenEntities();
        CollectionViewSource streetViewSource;
        CollectionViewSource customersViewSource;
        CollectionViewSource socialTypesViewSource;
        CollectionViewSource customercustomersocialtypesViewSource;


        public CustomersPage()
        {
            InitializeComponent();
            customersViewSource = ((CollectionViewSource)(FindResource("customersViewSource")));
            streetViewSource = ((CollectionViewSource)(FindResource("streetViewSource")));
            socialTypesViewSource = ((CollectionViewSource)(FindResource("socialTypesViewSource")));
            customercustomersocialtypesViewSource = ((CollectionViewSource)(FindResource("customercustomersocialtypesViewSource")));
            DataContext = this;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.customers.Load();
            context.streets.Load();
            context.socialtypes.Load();
            socialTypesViewSource.Source = context.socialtypes.Local;
            customersViewSource.Source = context.customers.Local;
            streetViewSource.Source = context.streets.Local;
            
        }

        private void LastCommandHandler(object sender, RoutedEventArgs e)
        {
            customersViewSource.View.MoveCurrentToLast();
        }

        private void PreviousCommandHandler(object sender, RoutedEventArgs e)
        {
            customersViewSource.View.MoveCurrentToPrevious();
        }

        private void NextCommandHandler(object sender, RoutedEventArgs e)
        {
            customersViewSource.View.MoveCurrentToNext();
        }

        private void FirstCommandHandler(object sender, RoutedEventArgs e)
        {
            customersViewSource.View.MoveCurrentToFirst();
        }

        private void DeleteCommandHandler(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Ви точно хочете видалити запис?", "Підтвердження",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (customersViewSource.View.CurrentItem != null)
                    {
                        var cur = customersViewSource.View.CurrentItem as customer;

                        var cust = (from c in context.customers
                                    where c.IdCustomer == cur.IdCustomer
                                    select c).FirstOrDefault();

                        if (cust != null)
                        {
                            context.customers.Remove(cust);
                            foreach (bankaccount var in cust.bankaccounts)
                            {
                                context.bankaccounts.Remove(var);
                            }
                            foreach (customersocialtype var in cust.customersocialtypes)
                            {
                                context.customersocialtypes.Remove(var);
                            }
                            foreach (serviceexecution var in cust.serviceexecutions)
                            {
                                context.serviceexecutions.Remove(var);
                            }
                        }

                        MessageBox.Show("Видалення відбулося успішно!");
                        context.SaveChanges();
                        customersViewSource.View.Refresh();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Щойно відбувся оброблений виняток: " + ex.Message, "Помилка", MessageBoxButton.OK);
                }
            }

            

        }

        private void UpdateCommandHandler(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви точно хочете зберегти запис?", "Підтвердження",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                String errorString = null;
                if (newCustomerGrid.IsVisible)
                {
                    street custStreet = (street)add_streetComboBox.SelectedItem;
                    customer newCustomer = new customer()
                    {
                        RegistrationDate = (DateTime)add_registrationDateDatePicker.SelectedDate,
                        BirthDate = add_birthDateDatePicker.SelectedDate,
                        Surname = add_surnameTextBox.Text.Trim(),
                        Name = add_nameTextBox.Text.Trim(),
                        Patronymic = add_patronymicTextBox.Text.Trim(),
                        City = add_cityTextBox.Text.Trim(),
                        StreetId = custStreet.IdStreet,
                        Phonenumber = add_phonenumberTextBox.Text.Trim(),
                        Passport = add_passportTextBox.Text.Trim(),
                        House = add_houseTextBox.Text.Trim(),
                        DeathDate = add_deathDateDatePicker.SelectedDate

                    };

                    try
                    {
                        newCustomer.Apartment = Int32.Parse((string)add_apartmentTextBox.Text);
                    }
                    catch
                    {

                        errorString += "\nНомер квартири має бути цілим числом!";

                    }

                    if ((Regex.IsMatch(add_emailTextBox.Text, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", RegexOptions.IgnoreCase)) || (add_emailTextBox.Text == ""))
                    {
                        newCustomer.Email = add_emailTextBox.Text;
                    }
                    else
                    {
                        errorString += "\nНеправильний формат електроного адресу!";

                    }

                    if (Regex.IsMatch(add_identificationTextBox.Text, @"\d{10}", RegexOptions.IgnoreCase))
                    {
                        try
                        {
                            newCustomer.Identification = Int32.Parse((string)add_identificationTextBox.Text);
                        }
                        catch
                        {
                            errorString += "\nТам програміст натупив, скоро виправлять!";
                        }
                       
                    }
                    else
                    {
                        errorString += "\nІдетифікаційни номер має містити 10 цифр!";

                    }

                    if (errorString != null)
                    {
                        MessageBox.Show(errorString, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if ((bool)liveStatusCheckBox.IsChecked) { newCustomer.LiveStatus = 1; }

                    if ((bool)otgStatusCheckBox.IsChecked) { newCustomer.OtgStatus = 1; }
                    context.customers.Local.Insert(0, newCustomer);
                    customersViewSource.View.Refresh();
                    customersViewSource.View.MoveCurrentTo(newCustomer);
                    newCustomerGrid.Visibility = Visibility.Collapsed;
                    existingCustomerGrid.Visibility = Visibility.Visible;
                    customerSocialTypeGrid.Visibility = Visibility.Visible;

                }

                context.SaveChanges();
                customersViewSource.View.Refresh();
            }
            
        }


        private void AddCommandHandler(object sender, RoutedEventArgs e)
        {

            add_apartmentTextBox.Text = "";
            cityTextBox.Text = "Коломия";
            add_birthDateDatePicker.SelectedDate = DateTime.Today;
            add_deathDateDatePicker.SelectedDate = null;
            add_registrationDateDatePicker.SelectedDate = DateTime.Today;
            add_streetComboBox.SelectedIndex = 1;
            add_nameTextBox.Text = "";
            add_patronymicTextBox.Text = "";
            add_surnameTextBox.Text = "";
            add_otgStatusCheckBox.IsChecked = false;
            add_liveStatusCheckBox.IsChecked = false;
            add_identificationTextBox.Text = "";
            add_passportTextBox.Text = "";
            add_phonenumberTextBox.Text = "";
            add_houseTextBox.Text = "";
            add_emailTextBox.Text = "";

            existingCustomerGrid.Visibility = Visibility.Collapsed;
            newCustomerGrid.Visibility = Visibility.Visible;
            customerSocialTypeGrid.Visibility = Visibility.Collapsed;


        }

        private void CancelCommandHandler(object sender, RoutedEventArgs e)
        {

            if (newCustomerGrid.IsVisible)
            {
                add_apartmentTextBox.Text = "";
                cityTextBox.Text = "Коломия";
                add_birthDateDatePicker.SelectedDate = DateTime.Today;
                add_deathDateDatePicker.SelectedDate = null;
                add_registrationDateDatePicker.SelectedDate = DateTime.Today;
                add_streetComboBox.SelectedIndex = 1;
                add_nameTextBox.Text = "";
                add_patronymicTextBox.Text = "";
                add_surnameTextBox.Text = "";
                add_otgStatusCheckBox.IsChecked = false;
                add_liveStatusCheckBox.IsChecked = false;
                add_identificationTextBox.Text = "";
                add_passportTextBox.Text = "";
                add_phonenumberTextBox.Text = "";
                add_houseTextBox.Text = "";
                add_emailTextBox.Text = "";
            } 
            else if (existingCustomerGrid.IsVisible)
            {
                apartmentTextBox.Text = "";
                cityTextBox.Text = "Коломия";
                birthDateDatePicker.SelectedDate = DateTime.Today;
                deathDateDatePicker.SelectedDate = null;
                registrationDateDatePicker.SelectedDate = DateTime.Today;
                streetComboBox.SelectedIndex = 1;
                nameTextBox.Text = "";
                patronymicTextBox.Text = "";
                surnameTextBox.Text = "";
                otgStatusCheckBox.IsChecked = false;
                liveStatusCheckBox.IsChecked = false;
                identificationTextBox.Text = "";
                passportTextBox.Text = "";
                phonenumberTextBox.Text = "";
                houseTextBox.Text = "";
                emailTextBox.Text = "";
            }

        }

        private void customerDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            existingCustomerGrid.Visibility = Visibility.Visible;
            newCustomerGrid.Visibility = Visibility.Collapsed;
            customerSocialTypeGrid.Visibility = Visibility.Visible;
        }

        private void SelectSocialTypesCommandHandler(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            try
            {
                if ((SocialTypesList.SelectedItems.Count > 0))
                {

                    var cur = customersViewSource.View.CurrentItem as customer;
                    socialtype transsocialtype = (socialtype)SocialTypesList.SelectedItems[0];

                    foreach (customersocialtype var in cur.customersocialtypes)
                    {
                        if((var.IdCustomer == cur.IdCustomer) && (var.IdSocialType == transsocialtype.IdSocialType)){
                            MessageBox.Show("Підопічний уже має такий соціальний тип!", "Помилка");
                            return;
                        }
                    }

                    
                    
                    cur.customersocialtypes.Add(new customersocialtype()
                    {
                        customer = cur,
                        socialtype = transsocialtype
                    });

                    context.SaveChanges();
                    customercustomersocialtypesViewSource.View.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Щойно відбувся оброблений виняток: " + ex.Message, "Помилка", MessageBoxButton.OK);
            } 
        }
        private void DeleteSocialTypesCommandHandler(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {

            try
            {
                customersocialtype selectedItem = (customersocialtype)customerSocialTypesList.SelectedItems[0];

                if (selectedItem != null)
                {
                    context.customersocialtypes.Remove(selectedItem);
                }

                context.SaveChanges();
                customersViewSource.View.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Щойно відбувся оброблений виняток: " + ex.Message, "Помилка", MessageBoxButton.OK);
            }
        }

        
    }
}
