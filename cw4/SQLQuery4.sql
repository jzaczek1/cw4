select * from Student;
select * from Enrollment;
select * from Studies;

drop procedure promote;

create procedure promote
	@studies varchar(64), @semestr int
	as
	declare @zmienna int = (select IdStudy from Studies where name = @studies),	
			@info varchar(64),
			@id int = (select max(IdEnrollment) from Enrollment);
	begin
	if(@zmienna is null)
		begin	--https://docs.microsoft.com/en-us/sql/t-sql/language-elements/raiserror-transact-sql?view=sql-server-ver15
			RAISERROR (15600,-1,-1, 'mysp_CreateCustomer');  
		end;
	declare @idEnrollment int = (select IdEnrollment from Enrollment where Semester = (@semestr + 1) and IdStudy = @zmienna);
	if(@idEnrollment is null)
		begin
			insert into Enrollment (IdEnrollment.Semester, IdStudy, StartDate) values (@id, @semestr, @zmienna, GETDATE());
            set @idEnrollment = (select IdEnrollment from Enrollment where Semester = (@semestr + 1) and IdStudy = @zmienna);
		end;
	 update Student set IdEnrollment = @idEnrollment where IdEnrollment = (select IdEnrollment from Enrollment where Semester = @semestr and IdStudy = @zmienna)
	end;
