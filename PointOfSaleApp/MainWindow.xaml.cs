// NAME          : MainWindow.xaml.cs
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //this instance
        public static MainWindow Instance { get; private set; }

        //UserControls
        public readonly Tabs tabsLayout;
        public readonly CustomerSelection customerSelectionLayout;
        public SalesRecord salesRecordLayout;


        // NAME         : MainWindow
        // DESCRIPTION  : Constructor. Creates a MainWindow instance + UserControls
        // PARAMETERS   : void
        // RETURNS      : n/a
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;


            tabsLayout = new Tabs();
            customerSelectionLayout = new CustomerSelection();
            salesRecordLayout = new SalesRecord();

            this.Content = tabsLayout;

        }

        
    }
}
