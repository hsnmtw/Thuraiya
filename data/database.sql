-- DROP DATABASE IF EXISTS thrdb;
-- CREATE DATABASE thrdb DEFAULT CHARACTER SET utf8  DEFAULT COLLATE utf8_general_ci;
-- USE thrdb;
-- SET NAMES utf8;

--exit;

DROP TABLE IF EXISTS  nationality;
CREATE TABLE nationality(
	id 		int not null primary key,
	country varchar(50) not null unique
) ;

DROP TABLE IF EXISTS  client;
CREATE TABLE client(
  mobile varchar(10) not null primary key,
  name varchar(100) not null,
  nationality int not null,
  govid varchar(10) not null unique,
  govid_poi varchar(35),
  govid_doi date,
  address varchar(200),
  tel1 varchar(10),
  tel2 varchar(10)
) ;

DROP TABLE IF EXISTS  contract;
CREATE TABLE contract(
  id bigint not null primary key,
  client varchar(10) not null,
  on_date date not null unique,
  price decimal(8,2) not null,
  occasion varchar(200) 
);

DROP TABLE IF EXISTS  payslip;
CREATE TABLE payslip(
  id bigint not null primary key,
  contract bigint not null,
  paied decimal(8,2) not null,
  on_date date not null,
  notes varchar(200)
);

DROP TABLE IF EXISTS  lang;
CREATE TABLE lang(
	english varchar(150) not null primary key,
	arabic  varchar(150) not null
) ;

DROP TABLE IF EXISTS  dates;
CREATE TABLE dates(
	greg  DATE not null primary key,
	hijri VARCHAR(10) not null unique
) ;

DROP TABLE IF EXISTS  config;
CREATE TABLE config(
	prp varchar(50)  not null primary key,
	val varchar(100) 
) ;

DROP VIEW IF EXISTS v_contracts;
CREATE VIEW v_contracts AS
select a.id as `ID`,a.on_date as `On Date`,b.name as `Client`,b.govid NID,a.price as `Price`,sum(c.paied) as `Paied`, a.price-sum(c.paied) `Balance` 
  from contract a 
  join client b 
	on a.client=b.mobile left
  join payslip c
    on a.id=c.contract
 group by a.id,b.name,a.on_date,a.price,b.govid,a.occasion	
 order by 1 asc;

DROP VIEW IF EXISTS v_clients;
CREATE VIEW v_clients AS
select c.mobile,c.name,n.country,c.govid,c.govid_doi,c.govid_poi,c.address as `Address`,c.tel1,c.tel2 
  from client c, nationality n
 where c.nationality=n.id
 order by 2 asc;

DROP VIEW IF EXISTS v_payslips;
CREATE VIEW v_payslips AS
 select a.id `ID`, a.contract `Contract`, a.paied `Paied`, a.on_date `On Date`,a.notes `Notes`,c.name `Client`
   from payslip a,
	contract b,
	client c
  where a.contract = b.id
    and b.client = c.mobile
  order by c.name,b.id,a.id; 

DROP VIEW IF EXISTS v_calendar;
CREATE VIEW v_calendar AS
select on_date
  from contract
 where on_date between '2015-01-01' and '2015-12-31';

