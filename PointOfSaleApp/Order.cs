// NAME          : Order.cs
// PROJECT       : PROG2111 - Assignment 3
// PROGRAMMER    : Lidiia Zhitova
// FIRST VERSION : 2019-12-01
// DESCRIPTION   : Representation of a record from order table


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;


namespace DB_A3
{
    /*
     * CLASS NAME  : Order
     * DESCRIPTION : Represents order table record from the DB. Each field represents table's attribute.
     */
    public class Order : IDisposable
    {
        const double MARKUP = 1.4;

        public int ID { get; set; } = 0;
        public List<OrderLine> products = new List<OrderLine>();        // products on order
        public double subtotal { get; set; } = 0;
        public DateTime date { get; set; }
        public Customer customer { get; set; } = null;                   
        public Branch branch { get; set; } = null;                      
        public int status { get; set; }                                 // 1 = PAID; 2 = RFND

        //for object disposal
        Boolean disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);




        // NAME         : addProduct
        // DESCRIPTION  : Creates a new OrderLine object and adds it to products list; also 
        //                adds to the subtotal based on the products price
        // PARAMETERS   : Product prod - product to add
        // RETURNS      : bool - true if product has been added to order, false otherwise
        public bool addProduct(Product prod)
        {
            int index = findProduct(prod.SKU);

            //if there's no OrderLine for such product yet, add one;
            //otherwise just increment the quantity
            if (index == -1)
            {
                if (prod.Stock <= 0)
                {
                    return false;
                }

                products.Add(new OrderLine { OrderID = -1,
                                             Name = prod.Name,
                                             SKU = prod.SKU,
                                             Price = prod.WholePrice * MARKUP,
                                             Quantity = 1 } );
            }
            else
            {
                if (prod.Stock < products[index].Quantity + 1)
                {
                    return false;
                }

                products[index].Quantity += 1;
            }


            //update the subtotal
            subtotal += prod.WholePrice * MARKUP;


            return true;
        }



        // NAME         : Dispose
        // DESCRIPTION  : disposes the object and frees the memory
        // PARAMETERS   : void
        // RETURNS      : void
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }



        // NAME         : Dispose
        // DESCRIPTION  : disposes the object and frees the memory
        // PARAMETERS   : void
        // RETURNS      : void
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }




        // NAME         : findProduct
        // DESCRIPTION  : checks if the order already has an orderLine for such product
        // PARAMETERS   : int SKU - the product's sku
        // RETURNS      : void
        public int findProduct(int SKU)
        {
            int index = 0;

            foreach (OrderLine prod in products)
            {
                if (prod.SKU == SKU)
                {
                    return index;
                }

                index++;
            }

            index = -1;

            return index;
        }

    }
}
