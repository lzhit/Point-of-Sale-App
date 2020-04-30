// NAME          : Refund.cs
// PROJECT       : PROG2111 - Assignment 3
// PROGRAMMER    : Lidiia Zhitova
// FIRST VERSION : 2019-12-01
// DESCRIPTION   : Implementation of Refund class


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_A3
{
    /*
     * CLASS NAME  : Refund
     * DESCRIPTION : This class is for processing refunds
     */
    class Refund
    {
        public static Refund Instance { get; private set; }
        public Order order { get; set; }


        public Refund ()
        {
            Instance = this;
        }




        //NAME        : refundOrder
        //DESCRIPTION : adjusts Order object's properties for further refund processing
        //              i.e. sets order status to 2, updates product's stock and sets orderline 
        //              quantities to 0    
        //PARAMETERS  : void
        //RETURNS     : void
        public void refundOrder()
        {
            DataManager manager = new DataManager();
            Product prod = null;

            //zero quantity + adjust stocks
            foreach (OrderLine ol in order.products)
            {
                prod = manager.getProduct(ol.SKU, order.branch.ID);
                manager.updateProductStock(prod, ol.Quantity, order.branch.ID);

                ol.Quantity = 0;
            }


            //add refund order
            order.status = 2;
            order.subtotal = 0;
            order.date = DateTime.Today;
            manager.refundOrder(order);
  

        }
    }
}
