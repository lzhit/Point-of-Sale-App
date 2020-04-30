// NAME          : DataManager.cs
// PROJECT       : PROG2111 - Assignment 3
// PROGRAMMER    : Lidiia Zhitova
// FIRST VERSION : 2019-12-01
// DESCRIPTION   : Implementation of DataManager class which handles access to the DB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;


namespace DB_A3
{    
    /*
     * CLASS NAME  : DataManager
     * DESCRIPTION : This class can get data from the database or insert new data into it
     */
    class DataManager
    {
        private string connectionString = "Server=127.0.0.1; Port=3306; Database=LZWally; Uid=root; Pwd=Conestoga1;";


        //NAME        : getData
        //DESCRIPTION : This method handles general data retrieval. It retrieves data 
        //              based on the query provided as an argument
        //PARAMETERS  : string query - query to be executed
        //              string tableName - name for the table that will be returned
        //RETURNS     : DataTableReader - reader for the table where the retrieved data is stored
        private DataTableReader getData(string query, string tableName)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            DataSet dataset = new DataSet();

            // Fill the Set with the data
            using (connection)
            {
                MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                da.Fill(dataset, tableName);
            }

            DataTable dataTable = dataset.Tables[tableName];

            connection.Close();

            return dataTable.CreateDataReader();
        }




        //NAME        : getProductList
        //DESCRIPTION : retrieves the whole product table from the DB
        //PARAMETERS  : void
        //RETURNS     : DataTableReader - reader for the table where the retrieved data is stored
        public DataTableReader getProductList(int branchId)
        {
            string query = @"SELECT * FROM product, stockLevel WHERE stockLevel.BranchID = " + branchId + " and product.SKU = stocklevel.SKU";

            return getData(query, "Products");

        }




        //NAME        : getProduct
        //DESCRIPTION : gets data about a product from product table based on 
        //              the product's SKU and creates a new Product object based
        //              on the retrieved data
        //PARAMETERS  : int SKU - desired product's SKU
        //RETURNS     : Product - newly created Product object
        public Product getProduct(int SKU, int branchId)
        {
            string query = @"SELECT * FROM product WHERE SKU = " + SKU;

            DataTableReader reader = getData(query, "Product");

            reader.Read();
            Product product = new Product{ SKU = Convert.ToInt32(reader[0]),
                                           Name = (string)reader[1],
                                           WholePrice = (double)reader[2] };

            query = @"SELECT Stock FROM stockLevel WHERE SKU = " + SKU + " AND BranchID = " + branchId;
            reader = getData(query, "Stock");
            reader.Read();
            product.Stock = Convert.ToInt32(reader[0]);


            return product;

        }




        //NAME        : getBranches
        //DESCRIPTION : retrieves the entire branch table from the DB
        //PARAMETERS  : void
        //RETURNS     : DataTableReader - reader for the table where the retrieved data is stored
        public DataTableReader getBranches()
        {
            string query = @"SELECT * FROM branch";
            
            return getData(query, "Branches");
        }

        


        //NAME        : getBranch
        //DESCRIPTION : Creates a Branch object based on the data retrieved from the DB.
        //              It will search for a branch based on either its name or ID.
        //              If name is not null, the search will be based on the name; on ID
        //              otherwise
        //PARAMETERS  : string name - branch's name
        //              int id - branch's ID
        //RETURNS     : Branch - newly created Branch object
        public Branch  getBranch(string name, int id)
        {
            string query = "";

            if (name != null)
            {
                query = @"SELECT * FROM branch WHERE BranchName = '" + name + "'";

            }
            else
            {
                query = @"SELECT * FROM branch WHERE BranchID = '" + id + "'";
            }

            DataTableReader reader = getData(query, "Branches");
            reader.Read();

            return new Branch { ID = (Int32) reader[0], Name = (string) reader[1] };
        }




        //NAME        : getCustomer
        //DESCRIPTION : Retirieves customer data based on customer's ID and stores it
        //              into a Customer object
        //PARAMETERS  : int id - customer ID
        //RETURNS     : Customer - newly created Customer object
        public Customer getCustomer(int id)
        {
            string query = @"SELECT * FROM customer WHERE CustomerID = '" + id + "'";

            DataTableReader reader = getData(query, "Customers");
            reader.Read();

            return new Customer { ID = (int) reader[0],
                                  FirstName = (string) reader[1],
                                  LastName = (string) reader[2],
                                  PhoneNumber = (string) reader[3] };
        }




        //NAME        : addNewCustomer
        //DESCRIPTION : inserts new customer record into customer table
        //PARAMETERS  : string firstName - customer's first name
        //              string lastName - customer's last name 
        //              string phoneNum - customer's phone number
        //RETURNS     : bool - true if a new row has been inserted, false otherwise
        public bool addNewCustomer(string firstName, string lastName, string phoneNum)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = @"SELECT * FROM customer 
                                    WHERE LastName = '" + lastName + 
                                    "'AND FirstName = '" + firstName +
                                    "'AND PhoneNumber = '" + phoneNum + "'";

            DataTableReader reader = getData(query, "Customers");
            
            //if such customer already exists the record will not be inserted
            if(reader.HasRows)
            {
                return false;
            }

            //if such customer doesn't exist, insert the record into the DB
            MySqlCommand comm = connection.CreateCommand();
            comm.CommandText = "INSERT INTO customer (FirstName, LastName, PhoneNumber) VALUES (@fn, @ln, @pn)";
            comm.Parameters.AddWithValue("@fn", firstName);
            comm.Parameters.AddWithValue("@ln", lastName);
            comm.Parameters.AddWithValue("@pn", phoneNum);
            comm.ExecuteNonQuery();
            connection.Close();

            return true;
        }




        //NAME        : customerExistsLastname
        //DESCRIPTION : Checks if customer's record exists in the DB based on customer's last name
        //PARAMETERS  : string lastName - customer's last name
        //RETURNS     : bool - true if the record exists, false otherwise
        public bool customerExistsLastname(string lastName)
        {
            string query = @"SELECT * FROM customer WHERE LastName = '" + lastName+"'";

            DataTableReader reader = getData(query, "Customers");
            return reader.HasRows;
        }




        //NAME        : customerExistsPhone 
        //DESCRIPTION : Checks if customer's record exists in the DB based on customer's phone number
        //PARAMETERS  : string phoneNumber - customer's phone number 
        //RETURNS     : bool - true if the record exists, false otherwise
        public bool customerExistsPhone(string phoneNumber)
        {
            string query = @"SELECT * FROM customer WHERE PhoneNumber = '" + phoneNumber + "'";

            DataTableReader reader = getData(query, "Customers");
            return reader.HasRows;
        }




        //NAME        : getCustomersByLastname
        //DESCRIPTION : gets a customer record based on their last name
        //PARAMETERS  : string ln - customer's last name
        //RETURNS     : DataTableReader - reader for the table where the retrieved data is stored
        public DataTableReader getCustomersByLastname(string ln)
        {
            string query = @"SELECT * FROM customer WHERE LastName = '" + ln + "'";

            DataTableReader reader = getData(query, "Customers");

            return reader;
        }



        //NAME        : getCustomersByPhone
        //DESCRIPTION : gets a customer record based on their phone number 
        //PARAMETERS  : string pn - customer's phone numebr 
        //RETURNS     : DataTableReader - reader for the table where the retrieved data is stored
        public DataTableReader getCustomersByPhone(string pn)
        {
            string query = @"SELECT * FROM customer WHERE PhoneNumber = '" + pn + "'";

            DataTableReader reader = getData(query, "Customers");

            return reader;
        }



        //NAME        : getIdOfLastInsertedCustomer
        //DESCRIPTION : Gets the ID of the lat inserted customer
        //PARAMETERS  : void 
        //RETURNS     : int - the ID
        public int getIdOfLastInsertedCustomer()
        {
            string query = "SELECT (MAX(CustomerID)) FROM customer";

            DataTableReader reader = getData(query, "MaxCustomerID");
            reader.Read();

            return (Int32)reader[0];
        }




        //NAME        : addOrder
        //DESCRIPTION : Inserts a new record into the order table
        //PARAMETERS  : Order order - order to be inserted
        //RETURNS     : void
        public void addOrder(Order order)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //insert order into the DB
            MySqlCommand comm = connection.CreateCommand();
            comm.CommandText = "INSERT INTO `order` (`Date`, StatusID, BranchID, CustomerID) VALUES(curdate(), 1, @branch, @customerID)";
            comm.Parameters.AddWithValue("@branch", order.branch.ID);
            comm.Parameters.AddWithValue("@customerID", order.customer.ID);
            comm.ExecuteNonQuery();

            
            //get the order's ID
            string query = "SELECT (MAX(OrderID)) FROM `order`";

            DataTableReader reader = getData(query, "MaxOrderID");
            reader.Read();
            int orderID = (Int32) reader[0];



            //insert orderLine into the DB
            foreach (OrderLine ol in order.products)
            {
                comm = connection.CreateCommand();
                comm.CommandText = "INSERT INTO orderLine (OrderID, SKU, Quantity, sPrice) VALUES (@orderID, @sku, @quantity, @price)";
                comm.Parameters.AddWithValue("@orderID", orderID);
                comm.Parameters.AddWithValue("@sku", ol.SKU);
                comm.Parameters.AddWithValue("@quantity", ol.Quantity);
                comm.Parameters.AddWithValue("@price", ol.Price);
                comm.ExecuteNonQuery();

                //update item stock
                comm = connection.CreateCommand();
                comm.CommandText = "update stockLevel set Stock = (select Stock where SKU = @sku AND BranchID = @branchId) - @quantity where SKU = @sku AND BranchID = @branchId";
                comm.Parameters.AddWithValue("@sku", ol.SKU);
                comm.Parameters.AddWithValue("@branchId", order.branch.ID);
                comm.Parameters.AddWithValue("@quantity", ol.Quantity);
                comm.ExecuteNonQuery();

            }

            order.ID = orderID;

            connection.Close();
        }




        //NAME        : GetOrder
        //DESCRIPTION : gets order from the DB based on the order ID
        //PARAMETERS  : int id - Order ID
        //RETURNS     : Order - newly created Order object
        public Order GetOrder(int id)
        {
            Order order = new Order();
            OrderLine ol = null;
            DataTableReader reader = null;

            string query = @"SELECT * FROM `order` WHERE OrderID = " + id;

            reader = getData(query, "Order");

            //return null if the order was not found in the DB
            if (!reader.HasRows)
            {
                return null;
            }

            reader.Read();
            
            order.ID = (Int32) reader[0];
            order.date = Convert.ToDateTime( reader[1]);
            order.status = (Int32) reader[2];
            order.branch = getBranch(null, (Int32) reader[3]);
            order.customer = getCustomer((Int32)reader[4]);

            query = @"SELECT * FROM orderLine WHERE OrderID = " + id;

            reader = getData(query, "OrderLine");

            while(reader.Read())
            {
                ol = new OrderLine();
                ol.OrderID = (Int32) reader[0];
                ol.SKU = (Int32)reader[1];
                ol.Quantity = (Int32)reader[2];
                ol.Price = (Double)reader[3];

                ol.Name = getProduct(ol.SKU, order.branch.ID).Name;

                order.products.Add(ol);
            }

            return order;
        }




        //NAME        : updateProductStock
        //DESCRIPTION : updates Stock attribute for a given product in Product table 
        //PARAMETERS  : Product prod - product to update
        //              int num - number to add or subtract from Stock value 
        //              (positive number to add, negative number to subtract)
        //RETURNS     : bool - true on success, false otherwise
        public bool updateProductStock(Product prod, int num, int branchId)
        {
            if (prod.Stock + num < 0)
            {
                return false;
            }

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand comm = connection.CreateCommand();
            comm = connection.CreateCommand();
            comm.CommandText = "UPDATE stockLevel SET Stock = @num WHERE SKU = @sku AND branchID = @branch";
            comm.Parameters.AddWithValue("@num", prod.Stock + num);
            comm.Parameters.AddWithValue("@sku", prod.SKU);
            comm.Parameters.AddWithValue("@branch", branchId);
            comm.ExecuteNonQuery();

            connection.Close();

            return true;
        }




        //NAME        : getOrderStatusName
        //DESCRIPTION : links status ID to the actual status (status table)
        //PARAMETERS  : int statusID
        //RETURNS     : string - status name
        public string getOrderStatusName(int statusID)
        {
            string name = "";

            if (statusID != 1 && statusID != 2)
            {
                return name;
            }

            string query = @"SELECT `Name` FROM `status` WHERE StatusID = " + statusID;

            DataTableReader reader = getData(query, "Status");
            reader.Read();
            name = (string)reader[0];

            return name;
        }




        //NAME        : refundOrder
        //DESCRIPTION : updates records in order and orderLine tables to indicate a refund. 
        //              I.e. sets order statusID to 2, and each orderLine's record (related
        //              to the order) quantity to 0
        //PARAMETERS  : Order order - order to be updated
        //RETURNS     : void
        public void refundOrder(Order order)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand comm = connection.CreateCommand();
            comm = connection.CreateCommand();
            comm.CommandText = "UPDATE `order`, orderLine SET `order`.StatusID = @status, orderLine.Quantity = @quantity " +
                               "WHERE `order`.OrderID = @id AND orderLine.OrderID = @id";
            comm.Parameters.AddWithValue("@id", order.ID);
            comm.Parameters.AddWithValue("@status", order.status);
            comm.Parameters.AddWithValue("@quantity", 0);
            comm.ExecuteNonQuery();

            connection.Close();
        }

    }
}
