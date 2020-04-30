// NAME          : Customer.cs
// PROJECT       : PROG2111 - Assignment 3
// PROGRAMMER    : Lidiia Zhitova
// FIRST VERSION : 2019-12-01
// DESCRIPTION   : Customer class implementation


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_A3
{
    /*
     * CLASS NAME  : Customer
     * DESCRIPTION : Represents customer table in the DB. Each field represents table's attribute.
     */
    public class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
