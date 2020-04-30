// NAME          : CustomerSelection.xaml.cs
// PROJECT       : PROG2111 - Assignment 3
// PROGRAMMER    : Lidiia Zhitova
// FIRST VERSION : 2019-12-01

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
using System.Text.RegularExpressions;


namespace DB_A3
{
    /// <summary>
    /// Interaction logic for CustomerSelection.xaml
    /// </summary>
    public partial class CustomerSelection : UserControl
    {
        public static CustomerSelection Instance { get; private set; }
        public Order order = Tabs.Instance.order;


        public CustomerSelection()
        {
            InitializeComponent();
            Instance = this;

        }



        //NAME        : Button_Click
        //DESCRIPTION : adds a new customer to the database + checks for input validity
        //PARAMETERS  : object sender, RoutedEventArgs e
        //RETURNS     : void
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string fn = newFirstName.Text;
            string ln = newLastName.Text;
            string pn = newPhone.Text;


            //check if all fields are filled out
            if (fn == "" || ln == "" || pn == "")
            {
                MessageBox.Show("To create a new customer you must fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //check if name fields only contain letters
            if (!Regex.IsMatch(fn, @"^[a-zA-Z]+$") || !Regex.IsMatch(ln, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("First name and last name should contain letters only", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            //check if the phone number is valid
            if (!Regex.IsMatch(pn, @"^[0-9]+$") || pn.Length != 11)
            {
                MessageBox.Show("Invalid phone number. A valid phone number contains 11 numbers only", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataManager manager = new DataManager();

            if (manager.addNewCustomer(fn, ln, pn) == false)
            {
                MessageBox.Show("Customer already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int customerID = manager.getIdOfLastInsertedCustomer();
            order.customer = manager.getCustomer(customerID);
            
            manager.addOrder(order);

            MainWindow.Instance.Content = MainWindow.Instance.salesRecordLayout;
            MainWindow.Instance.salesRecordLayout.printReceipt(order);

        }



        //NAME        : searchByLastName_Click
        //DESCRIPTION : checks input validity, searches for customer by last name, displays results
        //PARAMETERS  : object sender, RoutedEventArgs e
        //RETURNS     : void
        private void searchByLastName_Click(object sender, RoutedEventArgs e)
        {
            string ln = lnSearchText.Text;

            if (!Regex.IsMatch(ln, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("Last name should contain letters only", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataManager manager = new DataManager();

            CustomerSearch window = new CustomerSearch();
            window.display(new DataManager().getCustomersByLastname(ln));
            window.Show();
        }



        //NAME        : searchByPhoneNumber_Click
        //DESCRIPTION : checks input validity, searches for customer by phone number, displays results
        //PARAMETERS  : object sender, RoutedEventArgs e
        //RETURNS     : void
        private void searchByPhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            string pn = pnSearchText.Text;

            if (!Regex.IsMatch(pn, @"^[0-9]+$"))
            {
                MessageBox.Show("Phone number should contain only 11 numbers", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CustomerSearch window = new CustomerSearch();
            window.display(new DataManager().getCustomersByPhone(pn));
            window.Show();

        }



        //NAME        : GoBack_Click
        //DESCRIPTION : displays the UeserControl with tabs
        //PARAMETERS  : object sender, RoutedEventArgs e
        //RETURNS     : void
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Content = MainWindow.Instance.tabsLayout;
        }
    }
}

