// NAME          : OrderLine.cs
// PROJECT       : PROG2111 - Assignment 3
// PROGRAMMER    : Lidiia Zhitova
// FIRST VERSION : 2019-12-01
// DESCRIPTION   : OrderLine class implementation

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_A3
{
    /*
     * CLASS NAME  : OrderLine
     * DESCRIPTION : Represents orderLine table record from the DB. Each field represents table's attribute.
     */
    public class OrderLine
    {
        public int OrderID { get; set; }
        public int SKU { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
