DROP DATABASE IF EXISTS thrdb;
CREATE DATABASE thrdb DEFAULT CHARACTER SET utf8  DEFAULT COLLATE utf8_general_ci;
USE thrdb;
SET NAMES utf8;

CREATE TABLE thrdb.client(
  mobile varchar(10) not null primary key,
  name varchar(100) not null,
  address varchar(200),
  tel1 varchar(10),
  tel2 varchar(10)
) engine = innodb;

CREATE TABLE thrdb.contract(
  id bigint not null primary key,
  client varchar(10) not null,
  on_date date not null,
  price decimal(8,2) not null,
  occasion varchar(200) ,
  foreign key (client) references thrdb.client(mobile)
) engine = innodb;

CREATE TABLE thrdb.payslip(
  id bigint not null primary key,
  contract bigint not null,
  paied decimal(8,2) not null,
  on_date date not null,
  notes varchar(200),
  foreign key (contract) references thrdb.contract(id)
) engine = innodb;

CREATE TABLE thrdb.lang(
	english varchar(100) not null primary key,
	arabic  varchar(100) not null
) engine = innodb;

CREATE TABLE thrdb.dates(
	greg  DATE not null primary key,
	hijri VARCHAR(10) not null unique
) engine = innodb;

begin;


insert into thrdb.lang(english,arabic) values
('File','صيانة البيانات'),
('Edit','تحرير'),
('Date_Conversion','تحويل التاريخ'),
('Occasions','المناسبات'),
('Exit','خروج'),
('Clients','الزبائن'),
('Calendar','التقويم'),
('Pay_Slips','وصولات الدفع'),
('Contracts','العقود'),
('Help','مساعده'),
('About','حول'),
('Reports','التقارير'),
('January','يناير'),
('February','فبراير'),
('March','مارس'),
('April','ابريل'),
('May','مايو'),
('June','يونيو'),
('July','يوليو'),
('August','اغسطس'),
('September','سبتمبر'),
('October','اكتوبر'),
('November','نوفمبر'),
('December','ديسمبر'),
('SA','السبت'),
('SU','الاحد'),
('MO','الاثنين'),
('TU','الثلاثاء'),
('WE','الاربعاء'),
('TH','الخميس'),
('FR','الجمعه'),
('Muharam','محرم'),
('Safar','صفر'),
('Rabie I','ربيع الاول'),
('Rabie II','ربيع الثاني'),
('Jumada I','جمادى الاول'),
('Jumada II','جمادى الثاني'),
('Rajab','رجب'),
('Shaban','شعبان'),
('Ramdhan','رمضان'),
('Shawal','شوال'),
('Dhul Quada','ذو القعده'),
('Dhul Hijja','ذو الحجة'),
('Add','اضافة'),
('Print','طباعة'),
('Save','حفظ'),
('OK','تم'),
('Delete','حذف'),
('Cancel','الغاء'),
('On Date','بتاريخ'),
('Name','الاسم'),
('Price','المبلغ'),
('ID','الرقم'),
('Hijri','هجري'),
('New Client','زبون جديد')
;

insert into thrdb.client(mobile,name) values 
  ('0508456745','Hussain'),
  ('0500012345','Ali'),
  ('0500000000','Ahmad');
  
insert into thrdb.contract(id,client,on_date,price) values
 (10001,'0508456745','2015-01-05',1000.00),
 (10002,'0508456745','2015-01-07',2000.00),
 (10003,'0508456745','2015-10-05',1500.00),
 (10004,'0500012345','2015-01-15',3000.00),
 (10005,'0500000000','2015-02-07',1550.00),
 (10006,'0508456745','2015-11-05',1700.00);
   
commit;
