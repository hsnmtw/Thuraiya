DROP DATABASE IF EXISTS thrdb;
CREATE DATABASE thrdb DEFAULT CHARACTER SET utf8  DEFAULT COLLATE utf8_general_ci;
USE thrdb;
SET NAMES utf8;

CREATE TABLE thrdb.nationality(
	id 		int not null primary key,
	country varchar(50) not null unique
) engine=innodb;

CREATE TABLE thrdb.client(
  mobile varchar(10) not null primary key,
  name varchar(100) not null,
  nationality int not null,
  govid varchar(10) not null unique,
  govid_poi varchar(35),
  govid_doi date,
  address varchar(200),
  tel1 varchar(10),
  tel2 varchar(10),
  foreign key(nationality) references nationality(id)
) engine = innodb;

CREATE TABLE thrdb.contract(
  id bigint not null primary key,
  client varchar(10) not null,
  on_date date not null unique,
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
	english varchar(150) not null primary key,
	arabic  varchar(150) not null
) engine = innodb;

CREATE TABLE thrdb.dates(
	greg  DATE not null primary key,
	hijri VARCHAR(10) not null unique
) engine = innodb;

CREATE TABLE thrdb.config(
	prp varchar(50)  not null primary key,
	val varchar(100) 
) engine=innodb;

CREATE VIEW thrdb.v_contracts AS
select a.id as `ID`,a.on_date as `On Date`,b.name as `Client`,b.govid NID,a.price as `Price`,sum(c.paied) as `Paied`, a.price-sum(c.paied) `Balance` 
  from thrdb.contract a 
  join thrdb.client b 
	on a.client=b.mobile
  join thrdb.payslip c
    on a.id=c.contract
 group by a.id,b.name,a.on_date,a.price,b.govid,a.occasion	
 order by 1 asc;

CREATE VIEW thrdb.v_clients AS
select c.mobile,c.name,n.country,c.govid,c.govid_doi,c.govid_poi,c.address,c.tel1,c.tel2 
  from thrdb.client c, thrdb.nationality n
 where c.nationality=n.id
 order by 2 asc;

begin;

insert into thrdb.config(prp,val) values
  ('m_Menu','m_File,m_Reports,m_Help'),
  ('m_File','Calendar,-,Contracts,Occasions,-,Clients,Pay_Slips,-,Nationalities,-,Exit'),
  ('m_Edit',''),
  ('m_Reports',''),
  ('m_Help','Date_Conversion,-,About'),
  ('Calendar.icon','assets\\tear_off_calendar-26.png'),
  ('Pay_Slips.icon','assets\\banknotes-26.png'),
  ('Exit.icon','assets\\shutdown-26.png'),
  ('About.icon','assets\\accessibility2-26.png'),
  ('Nationalities.icon','assets\\globe-26.png'),
  ('Contracts.icon','assets\\survey-26.png'),
  ('Clients.icon','assets\\businessman-26.png'),
  ('Occasions.icon','assets\\conference_call-26.png'),
  ('Date_Conversion.icon','assets\\date_from-26.png'),
  ('Contracts.data','data\\Contracts.csv'),
  ('Clients.data','data\\Clients.csv'),
  ('delete.sign','assets\\Programming\\delete_sign-26.png'),
  ('Days','SU,MO,TU,WE,TH,FR,SA'),
  ('RTL','true')
;

insert into thrdb.nationality(id,country) values
 (701, 'السعودية'),
 (702, 'غير سعودي')
;

insert into thrdb.client(mobile,name,nationality,govid) values 
  ('0508456745','احمد المطوع' , 701 ,'112340001'),
  ('0500012345','يوسف الشقاق', 701 ,'101526002'),
  ('0500000000','محمد صالح'  , 702 ,'206441152')
;
  
insert into thrdb.contract(id,client,on_date,price) values
 (10001,'0508456745','2015-01-05',1000.00),
 (10002,'0508456745','2015-01-07',2000.00),
 (10003,'0508456745','2015-10-05',1500.00),
 (10004,'0500012345','2015-01-15',3000.00),
 (10005,'0500000000','2015-02-07',1550.00),
 (10006,'0508456745','2015-11-05',1700.00)
;

insert into thrdb.payslip (id,contract,on_date,paied) values
 (1051,10001,'2015-01-05', 500.00),
 (1052,10002,'2015-01-08',1000.00),
 (1053,10003,'2015-10-05',1000.00),
 (1054,10004,'2015-01-15',3000.00),
 (1055,10005,'2015-02-07',1050.00),
 (1056,10006,'2015-11-05',1500.00)
;


insert into thrdb.lang(english,arabic) values
('File','صيانة البيانات'),
('Client','الزبون'),
('Address','العنوان'),
('Tel 1','الهاتف 1'),
('Tel 2','الهاتف 2'),
('tel1','الهاتف 1'),
('tel2','الهاتف 2'),
('Mobile','الجوال'),
('NID DOI','تاريخ الاصدار'),
('NID POI','مكان الاصدار'),
('NID','رقم الهوية'),

('govid_doi','تاريخ الاصدار'),
('govid_poi','مكان الاصدار'),
('govid','رقم الهوية'),

('New Payment','دفعة جديدة'),
('Warning','تحذير'),
('Are you sure of deleting this record?','هل انت متأكد من حذف السجل؟'),
('Thuraiya - Wedding Occations Program','الثريا - برنامج تنظيم قاعات الافراح - برمجة حسين المطوع'),
('Edit','تحرير'),
('Client Form','نموذج الزبائن'),
('Nationality','الجنسية'),
('Pay Slips Dialog','نموذج الدفعات'),
('Nationalities','الجنسيات'),
('Date_Conversion','تحويل التاريخ'),
('Date Conversion','تحويل التاريخ'),
('Nationality Form','نموذج الجنسيات'),
('Occasions','المناسبات'),
('Occasion','المناسبة'),
('Country','الدولة'),
('Contract Form','نموذج العقود'),
('Exit','خروج'),
('Clients','الزبائن'),
('Calendar','التقويم'),
('Pay_Slips','وصولات الدفع'),
('Pay Slips','وصولات الدفع'),
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
('New Client','زبون جديد'),
('Failed to process your request','حدث خطأ أثناء معالجة الطلب - حاول مرة أخرى'),
('Your request has been successfully processed','تم تنفيذ الطلب بنجاح'),
('Error','خطأ'),
('Success','نجاح'),
('Paied','المدفوع'),
('Balance','المتبقي')
;

   
commit;
