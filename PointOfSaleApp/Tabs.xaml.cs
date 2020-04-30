// NAME          : Tabs.xaml.cs
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
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;


namespace DB_A3
{
    /// <summary>
    /// Interaction logic for Tabs.xaml
    /// </summary>
    public partial class Tabs : UserControl
    {
        public static Tabs Instance { get; private set; }       //the only MainTabs instance that will be used
        public Order order;

        const double MARKUP = 1.4;



        // NAME         : MainTabs
        // DESCRIPTION  : Constructor. Creates a MainTabs instance + fills in components
        //                with database items + intiates a new order
        // PARAMETERS   : void
        // RETURNS      : n/a
        public Tabs()
        {
            InitializeComponent();
            Instance = this;
            Instance.fillBranchSelection();

            order = new Order { status = 1 };
        }


        #region OrderTab

        public void fillBranchSelection()
        {
            DataManager data = new DataManager();
            DataTableReader reader = data.getBranches();

            while (reader.Read())
            {
                branchSelection.Items.Add(reader[1].ToString());
            }
        }




        // NAME         : fillProductList
        // DESCRIPTION  : calls a method from the DataManager class to get product data
        //                then fills the product listbox using the data
        // PARAMETERS   : void
        // RETURNS      : void
        public void fillProductList()
        {
            DataManager data = new DataManager();
            int branchId = branchSelection.SelectedIndex+1;

            DataTableReader reader = data.getProductList(branchId);

            while (reader.Read())
            {
                double price = Double.Parse(reader[2].ToString()) * MARKUP;

                productList.Items.Add(reader[0] + ": " + reader[1] + " $" + Math.Round(price, 2));
            }
        }



        // NAME         : branchSelection_Selected
        // DESCRIPTION  : populates productList when the user selects a branch
        // PARAMETERS   : object sender, RoutedEventArgs e
        // RETURNS      : void
        private void branchSelection_Selected(object sender, RoutedEventArgs e)
        {
            productList.Items.Clear();
            Instance.fillProductList();
        }




        // NAME         : updateOrderList
        // DESCRIPTION  : updates the order ListBox
        // PARAMETERS   : void
        // RETURNS      : void
        public void updateOrderList()
        {
            //create a local reference to products list that belongs to Order
            List<OrderLine> orderProducts = order.products;

            //refill the list box with updated products
            orderList.Items.Clear();
            foreach (OrderLine line in orderProducts)
            {
                orderList.Items.Add(line.Name + " $" + line.Price + " x " + line.Quantity);
            }

            discardOrderButton.IsEnabled = true;

        }



        // NAME         : checkoutButton_Click
        // DESCRIPTION  : assigns the current branch to the order and proceeds to the next
        //                user control
        // PARAMETERS   : object sender, RoutedEventArgs e
        // RETURNS      : void
        private void checkoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (branchSelection.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a branch first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (order.products.Count == 0)
            {
                MessageBox.Show("Order is empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            order.branch = new DataManager().getBranch(branchSelection.SelectedItem.ToString(),-1);
             

            MainWindow.Instance.Content = MainWindow.Instance.customerSelectionLayout;
        }



        // NAME         : addProduct_Click
        // DESCRIPTION  : adds (or updates) a product to order listBox when user presses
        //                the addProduct button
        // PARAMETERS   : void
        // RETURNS      : void
        private void addProduct_Click(object sender, RoutedEventArgs e)
        {
            //if no product is select in the product listBox, display an error message
            if (productList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string itemString = productList.SelectedItem.ToString();
            int y = itemString.IndexOf(':');
            int sku = Int32.Parse(itemString.Substring(0, y));
            int branchId = branchSelection.SelectedIndex;

            if (branchId == -1)
            {
                MessageBox.Show("Please select a branch", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (order.addProduct(new DataManager().getProduct(sku, branchId + 1)) == false)
            {
                MessageBox.Show("Sorry, we don't have more of this item in stock :(", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            updateOrderList();

            lblSubtotal.Content = "$" + Math.Round(order.subtotal, 2);
        }




        // NAME         : discardOrder_Click
        // DESCRIPTION  : disposes the Order object + clears the order ListBox
        //                + updates the subtotal + creates a new Order object         
        // PARAMETERS   : object sender, RoutedEventArgs e
        // RETURNS      : void
        public void discardOrder_Click(object sender, RoutedEventArgs e)
        {
            order.Dispose();
            orderList.Items.Clear();
            lblSubtotal.Content = "$0.00";

            order = new Order { status = 1 };

        }



        #endregion


        #region OrderLookUpTab

        // NAME         : searchOrder_Click
        // DESCRIPTION  : searchs for an order by its ID and displays its sales record                
        // PARAMETERS   : object sender, RoutedEventArgs e
        // RETURNS      : void
        private void searchOrder_Click(object sender, RoutedEventArgs e)
        {
            string productList = "";
            string stringId = orderIdTextBox.Text;
            Order order = null;


            //check input validity and find the order if the input is valid
            if (!Regex.IsMatch(stringId, @"^[0-9]+$"))
            {
                MessageBox.Show("Invalid ID. Only numeric values are accepted", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            order = new DataManager().GetOrder(Int32.Parse(stringId));

            if(order == null)
            {
                MessageBox.Show("Not Found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            //display the sales record
            foreach (OrderLine ol in order.products)
            {
                productList += ol.Name + " " + ol.Quantity + " x " + ol.Price + " = $" + ol.Quantity * ol.Price + "\n";
            }

            Refund r = new Refund();
            r.order = order;

            if (r.order.status == 2)
            {
                RefundButton.IsEnabled = false;
            }
            else
            {
                RefundButton.IsEnabled = true;
            }



            receipt.Content = "Order ID: " + order.ID +
                              "\nBranch: " + order.branch.Name +
                              "\nDate: " + order.date +
                              "\nCustomer ID: " + order.customer.ID +
                              "\nStatus: " + order.status +
                              "\n\nProducts purchased:\n" +
                              productList;

            receiptCanvas.Visibility = Visibility.Visible;

        }



        // NAME         : updateStockLevel_Click
        // DESCRIPTION  : refunds the order and prints the updated sales record
        // PARAMETERS   : void
        // RETURNS      : void
        private void Refund_Click(object sender, RoutedEventArgs e)
        {
            Refund.Instance.refundOrder();
            searchOrder_Click(sender, e); //update the search screen to indicate the order has been refunded

            MainWindow.Instance.Content = MainWindow.Instance.salesRecordLayout;
            MainWindow.Instance.salesRecordLayout.printReceipt(Refund.Instance.order);
        }

        #endregion


        #region StockLevels


        // NAME         : updateStockLevel_Click
        // DESCRIPTION  : displays the current stock levels for all products
        // PARAMETERS   : object sender, RoutedEventArgs e
        // RETURNS      : void
        public void updateStockLevel_Click(object sender, RoutedEventArgs e)
        {
            DataManager manager = new DataManager();

            int branchId = branchSelection.SelectedIndex;

            if (branchId == -1)
            {
                MessageBox.Show("Please select a branch", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataTableReader productData = manager.getProductList(branchId);
            

            ProductStock.DataContext = productData;

        }


        #endregion


    }
}
