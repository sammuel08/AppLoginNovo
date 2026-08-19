drop database if exists LoginCore;
create database LoginCore;
use LoginCore;

create table Cliente(
Id int auto_increment primary key,
Nome Varchar(50) not null,
Nascimento DateTime not null,
Sexo char(1),
CPF Varchar(11) not null,
Telefone Varchar(14) not null,
Email Varchar(50) not null,
Senha varchar(8) not null,
ConfirmacaoSenha Varchar(8) not null,
Situacao char(1) not null);

create table Colaborador(
Id int auto_increment primary key,
Nome Varchar(50) not null,
Email Varchar(50) not null,
Senha Varchar(8) not null,
Tipo Varchar(8) not null);

INSERT INTO Cliente
(Nome, Nascimento, Sexo, CPF, Telefone, Email, Senha, ConfirmacaoSenha, Situacao)
VALUES
('Sammuel', '2008-08-13', 'M', '12345678900',
'11999999999', 'sammuel10@teste.com', '123456','123456', 'A');

INSERT INTO Colaborador
(Nome, Email, Senha, Tipo)
VALUES
('Samuel', 'sammuel11@gmail.com', '12345678', 'admin');