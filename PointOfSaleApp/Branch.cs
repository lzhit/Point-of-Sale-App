// NAME          : Branch.cs
// PROJECT       : PROG2111 - Assignment 3
// PROGRAMMER    : Lidiia Zhitova
// FIRST VERSION : 2019-12-01
// DESCRIPTION   : Branch class implementation


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_A3
{
    /*
     * CLASS NAME  : Branch
     * DESCRIPTION : Represents branch table in the DB. Consists of ID (column) and Name (column).
     */
    public class Branch
    {
        public int ID { get; set; }         // 1 = Sport's World; 2 = Waterloo; 3 = Cambridge Mall; 4 = St. Jacobs
        public string Name { get; set; }
    }
}
