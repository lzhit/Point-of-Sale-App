// NAME          : CustomerSearch.xaml.cs
// PROJECT       : PROG2111 - Assignment 3
// PROGRAMMER    : Lidiia Zhitova
// FIRST VERSION : 2019-12-01


using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Navigation;



namespace DB_A3
{
    /// <summary>
    /// Interaction logic for CustomerSearch.xaml
    /// </summary>
    public partial class CustomerSearch : Window
    {

        public CustomerSearch()
        {
            InitializeComponent();

        }



        //NAME        : display
        //DESCRIPTION : displays customers found from search by last anme or by phone number
        //PARAMETERS  : DataTableReader reader 
        //RETURNS     : void
        public void display(DataTableReader reader)
        {

            if (!reader.HasRows)
            {
                searchInfoLabel.Content = "Nothing found!";
                return;
            }

            searchInfoLabel.Content = "Found these items: ";
            while(reader.Read())
            {
                searchCustomerList.Items.Add(reader[0] + " / " + reader[1] + " / " + reader[2] + " / " + reader[3]);
            }
        }



        //NAME        : SearchGoBack_Click
        //DESCRIPTION : Closes this window 
        //PARAMETERS  : object sender, RoutedEventArgs e
        //RETURNS     : void
        private void SearchGoBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        //NAME        : searchCustomerList_SelectionChanged
        //DESCRIPTION : Enables 'select' button once the user clicks on (and therefore selects) 
        //              a customer for completing checkout
        //PARAMETERS  : object sender, SelectionChangedEventArgs e
        //RETURNS     : void
        private void searchCustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            searchSelect_Button.IsEnabled = true;
        }



        //NAME        : SearchSelect_Click
        //DESCRIPTION : assigns a customer to an order, closes this window and displays a receipt
        //PARAMETERS  : object sender, RoutedEventArgs e
        //RETURNS     : void
        private void SearchSelect_Click(object sender, RoutedEventArgs e)
        {
            DataManager manager = new DataManager();

            //if no customer is selected, display an error message
            if (searchCustomerList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            string selection = searchCustomerList.SelectedItem.ToString();
            int i = selection.IndexOf('/');
            int customerID = Int32.Parse(selection.Substring(0, i-1));
            Tabs.Instance.order.customer = manager.getCustomer(customerID);
            manager.addOrder(Tabs.Instance.order);

            MainWindow.Instance.Content = MainWindow.Instance.salesRecordLayout;
            MainWindow.Instance.salesRecordLayout.printReceipt(Tabs.Instance.order);

            this.Close();

        }
    }
}
