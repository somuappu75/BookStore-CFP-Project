create database BookStoreDB
use  BookStoreDB

----for creating User table----
create table UserRegister(
UserId int IDENTITY(1,1) NOT NULL, 
FullName varchar(70)not null,
EmailId  varchar(70)not null,
Password  varchar(70)not null,
MobileNumber  varchar(70)not null,
primary key(UserId)
)

select *from UserRegister

-----Store Procedure For User Registration-----------

Create procedure spRegisterUser     
(                 
    @FullName VARCHAR(60),        
    @EmailId VARCHAR(50),       
	@Password  VARCHAR(50),
	@MobileNumber VARCHAR(50)
    
)        
as         
Begin         
    Insert into UserRegister(FullName,EmailId, Password,MobileNumber)         
    Values (@FullName,@EmailId,@Password,@MobileNumber)         
End     




select *from UserRegister

-----For User Login Store Procedure----
create Proc spUserLogin
(
	@EmailId varchar(Max),
	@Password varchar(Max)
)
as
begin
	select * from UserRegister
	where
		EmailId=@EmailId
		and
		Password=@Password
end;



-----For Forgot Password Api-------sp
create proc spUserForgotPassword
(
	@EmailId varchar(Max)
)
as
begin
	update UserRegister
	set 
		Password ='Null'
	where 
		EmailId = @EmailId;
    select * from UserRegister where EmailId = @EmailId;
end;

-----For Reset Passord Store Procedure---------
create proc spUserResetPassword
(
	@EmailId varchar(Max),
	@Password varchar(Max)
)
AS
BEGIN
	update UserRegister
	SET 
		Password = @Password 
	where
		EmailId = @EmailId;
end;


select * from  UserRegister
