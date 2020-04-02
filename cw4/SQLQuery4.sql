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
	declare @idEnrollment int = (select IdEnrollment from Enrollment WHERE Semester = (@semestr + 1) AND IdStudy = @zmienna);
	if(@idEnrollment is null)
		begin
			insert into Enrollment (IdEnrollment.Semester, IdStudy, StartDate) VALUES (@id, @semestr, @zmienna, GETDATE());
            set @idEnrollment = (SELECT IdEnrollment FROM Enrollment WHERE Semester = (@semestr + 1) AND IdStudy = @zmienna);
		end;
	 UPDATE Student SET IdEnrollment = @idEnrollment WHERE IdEnrollment = (SELECT IdEnrollment FROM Enrollment WHERE Semester = @semestr AND IdStudy = @zmienna)
	end;
