-- NAME			 : LZWally.sql
-- PROJECT		 : PROG2111 - Assignment 3
-- PROGRAMMER	 : Lidiia Zhitova
-- FIRST VERSION : 2019-11-30
-- DESCRIPTION	 : Creates and fills LZWally database


-- CREATE A DATABASE FIRST

drop database if exists LZWally;
create database LZWally;
use LZWally;

-- ADD TABLES

create table customer
(
	CustomerID		int not null unique auto_increment,
    FirstName		varchar(20) not null,
    LastName		varchar(20) not null,
    PhoneNumber		varchar(20) not null,
    primary key(CustomerID)
);


create table branch
(
	BranchID 		int not null unique auto_increment,
    BranchName		varchar(20) not null,
    primary key(BranchID)
);


create table `status`
(
	StatusID	int unique not null auto_increment,
    `Name`		varchar(4),
    primary key(StatusID)
);


create table `order`
(
	OrderID 	int not null unique auto_increment,
    `Date`		date not null,
    StatusID	int not null,
	BranchID	int not null,
    CustomerID	int not null,
    primary key(orderID),
    foreign key(BranchID) references branch(BranchID),
    foreign key(CustomerID) references customer(CustomerID),
    foreign key(StatusID) references `status`(StatusID)
);


create table product
(
	SKU			int not null unique auto_increment,
    `Name`		varchar(50) not null,
    WPrice		double not null,
    Stock		int not null,
    primary key(SKU)
);


create table orderLine
(
	OrderID		int not null,
    SKU			int not null,
    Quantity	int not null,
    sPrice		double not null,
    foreign key (OrderID) references `order`(OrderID),
    foreign key (SKU) references product(SKU)
);




-- INSERT RECORDS

insert into customer (FirstName, LastName, PhoneNumber)
values	("Carlo", "Sgro", "15195550000"),
		("Norbert", "Mika", "14165551111"),
        ("Russell", "Foubert", "15195552222"),
        ("Sean", "Clarke", "15195553333");
        

insert into branch (BranchName)
values 	("Sports World"),
		("Waterloo"),
        ("Cambridge Mall"),
        ("St. Jacobs");
        

insert into product (`Name`, WPrice, Stock)
values	("Disco Queen Wallpaper (roll)", 12.95, 56),
		("Countryside Wallpaper (roll)", 11.95, 24),
        ("Victorian Lace Wallpaper (roll)", 14.95, 44),
        ("Drywall Tape (roll)", 3.95, 120),
        ("Drywall Tape (pkg 10)", 36.95, 30),
        ("Drywall Repair Compound (tube)", 6.95, 90);
 
 
 insert into `status` (`Name`)
 values	("PAID"),
		("RFND");
        
insert into `order` (`Date`, StatusID, BranchID, CustomerID)
values	("2019-09-20", 1, 1, 4),
		("2019-10-06", 1, 3, 3),
        ("2019-11-02", 1, 4, 2);
        
        
insert into orderLine (OrderID, SKU, Quantity, sPrice)
values	(1, 3, 4, (select product.WPrice from product where product.SKU = 3) * 1.4),
		(1, 6, 1, (select product.WPrice from product where product.SKU = 6) * 1.4),
        (1, 4, 2, (select product.WPrice from product where product.SKU = 4) * 1.4),
        (2, 2, 10, (select product.WPrice from product where product.SKU = 2) * 1.4),
        (3, 1, 12, (select product.WPrice from product where product.SKU = 1) * 1.4),
        (3, 4, 3, (select product.WPrice from product where product.SKU = 4) * 1.4);
        
        
-- Update stock levels
-- order 1
update product
set Stock = (select Stock where SKU = 3) - 4
where SKU = 3;

update product
set Stock = (select Stock where SKU = 6) - 1
where SKU = 6;

update product
set Stock = (select Stock where SKU = 4) - 2
where SKU = 4;

-- order 2
update product
set Stock = (select Stock where SKU = 2) - 10
where SKU = 2;

-- order 3
update product
set Stock = (select Stock where SKU = 1) - 12
where SKU = 1;

update product
set Stock = (select Stock where SKU = 4) - 3
where SKU = 4;

-- This query does the refund 
update `order`, orderLine
set `order`.StatusID = 2, orderLine.Quantity = 0
where `order`.OrderID = 3 AND orderLine.OrderID = 3;  

update product
set Stock = (select Stock where SKU = 1) + 12
where SKU = 1;

update product
set Stock = (select Stock where SKU = 4) + 3
where SKU = 4;

