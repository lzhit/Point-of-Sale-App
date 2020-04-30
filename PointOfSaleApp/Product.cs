// NAME          : Product.cs
// PROJECT       : PROG2111 - Assignment 3
// PROGRAMMER    : Lidiia Zhitova
// FIRST VERSION : 2019-12-01
// DESCRIPTION   : Implementation of Product class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_A3
{
    /*
     * CLASS NAME  : Product
     * DESCRIPTION : Represents product table record from the DB. Each field represents table's attribute.
     */
    public class Product
    {
        public int SKU { get; set; }
        public string Name { get; set;}
        public double WholePrice { get; set; }
        public int Stock { get; set; }


    }
}
