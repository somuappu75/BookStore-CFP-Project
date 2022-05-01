use  BookStoreDB

---Creating Cart tabel----
create table Carts
(
	CartId int identity(1,1) not null primary key,
	OrderQuantity int default 1,
	UserId int foreign key references UserRegister(UserId) on delete no action,
	BookId int foreign key references Book(BookId) on delete no action
);

select *from Carts

----Store Procedure for Cart tables----

--------Add To Cart Store Procedure------

create or alter proc spAddCart
(
@OrderQuantity int,
@UserId int,
@BookId int
)
as
begin 
		if(exists(select * from Book where BookId=@BookId))
		begin
		insert into Carts(OrderQuantity,UserId,BookId)
		values(@OrderQuantity,@UserId,@BookId)
		end
		else
		begin
		select 1
		end
end;


----Get Cart ByUser Id Sp----
create proc spGetCartByUserId
(
	@UserId int
)
as
begin
	select CartId,OrderQuantity,UserId,c.BookId,BookName,AuthorName,
	DiscountPrice,ActualPrice,BookImage from Carts c join Book b on c.BookId=b.BookId 
	where UserId=@UserId;
end;

---Update Cart By  book,cart,user ,Store Procedure-------
create proc spUpdateCart
(
	@OrderQuantity int,
	@BookId int,
	@UserId int,
	@CartId int
)
as
begin
update Carts set BookId=@BookId,
				UserId=@UserId,
				OrderQuantity=@OrderQuantity
				where CartId=@CartId;
end;

---Delete  BY Cart Id ANd Book Id By Store Procedure-------
create proc spDeleteCart
(
	@CartId int,
	@BookId int
)
as
begin
delete Carts where
		CartId=@CartId and BookId=@BookId;
end;

select * from Carts