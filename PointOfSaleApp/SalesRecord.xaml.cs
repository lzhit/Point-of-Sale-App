// NAME          : SalesRecord.xaml.cs
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

namespace DB_A3
{
    /// <summary>
    /// Interaction logic for SalesRecord.xaml
    /// </summary>
    public partial class SalesRecord : Page
    {

        public SalesRecord()
        {
            InitializeComponent();
        }



        // NAME         : printReceipt
        // DESCRIPTION  : displays the sales record for the order                
        // PARAMETERS   : Order order
        // RETURNS      : void
        public void printReceipt(Order order)
        {
            DataManager manager = new DataManager();
            string customerName = order.customer.FirstName + " " + order.customer.LastName;
            string branch = order.branch.Name;
            int orderID = order.ID;
            double subtotal = order.subtotal;
            string productList = "";


            foreach (OrderLine ol in order.products)
            {
                productList += ol.Name + " " + ol.Quantity + " x " + ol.Price + " = $" + ol.Quantity * ol.Price + "\n";
            }
           

            string receiptContent = "******************************\n" +
                                    "Thank you for shopping at\n" +
                                    "Wally's World, " + branch + "\n" +
                                    "On " + DateTime.Today + ", " + customerName +
                                    "\n\n" +
                                    "Order ID: " + orderID +
                                    "\n\n" +
                                    productList +
                                    "\n\n" +
                                    "Subtotal = $" + subtotal + "\n" +
                                    "HST (13%) = $" + Math.Round(((subtotal * 1.13) - subtotal), 2) + "\n" +
                                    "Total = $" + Math.Round((subtotal * 1.13), 2) + "\n" +
                                    "\n\n" +
                                     manager.getOrderStatusName(order.status) +  " - Thank you!";

            receipt.Content = receiptContent;
        }




        // NAME         : Button_Click
        // DESCRIPTION  : Clears all textboxes in CustomerSelection UserControl +
        //                discards the order + displays the tabs user control
        // PARAMETERS   : object sender, RoutedEventArgs e
        // RETURNS      : void
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox txtbox in CustomerSelection.Instance.textboxes.Children)
            {
                txtbox.Text = String.Empty;
            }
            Tabs.Instance.discardOrder_Click( sender, e);
            MainWindow.Instance.Content = MainWindow.Instance.tabsLayout;

        }
    }
}
