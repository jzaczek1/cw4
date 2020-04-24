select * from Student;
select * from Studies;
select * from Enrollment;

drop table Student;
drop table Studies;
drop table Enrollment;

-- tables
-- Table: Enrollment
CREATE TABLE Enrollment (
    IdEnrollment int  NOT NULL,
    Semester int  NOT NULL,
    IdStudy int  NOT NULL,
    StartDate date  NOT NULL,
    CONSTRAINT Enrollment_pk PRIMARY KEY  (IdEnrollment)
);

insert into Enrollment(IdEnrollment,Semester,IdStudy,StartDate)
	values (1,1,1,'2019-09-01'),(2,2,2,'2020-09-01');

-- Table: Student
CREATE TABLE Student (
    IndexNumber nvarchar(100)  NOT NULL,
    FirstName nvarchar(100)  NOT NULL,
    LastName nvarchar(100)  NOT NULL,
    BirthDate date  NOT NULL,
    IdEnrollment int  NOT NULL,
	Password nvarchar(1000),
    CONSTRAINT Student_pk PRIMARY KEY  (IndexNumber)
);

insert into Student(IndexNumber,FirstName,LastName,BirthDate,IdEnrollment,Password)
	values ('s1234','Maciej','Kowalski','1998-10-10',1,''),
			('s2222','Patryk','Pawe³','1997-07-07',2,'');

-- Table: Studies
CREATE TABLE Studies (
    IdStudy int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Studies_pk PRIMARY KEY  (IdStudy)
);

insert into Studies(IdStudy,Name)
	values(1,'Informatyka'),(2,'Grafika');

-- foreign keys
-- Reference: Enrollment_Studies (table: Enrollment)
ALTER TABLE Enrollment ADD CONSTRAINT Enrollment_Studies
    FOREIGN KEY (IdStudy)
    REFERENCES Studies (IdStudy);

-- Reference: Student_Enrollment (table: Student)
ALTER TABLE Student ADD CONSTRAINT Student_Enrollment
    FOREIGN KEY (IdEnrollment)
    REFERENCES Enrollment (IdEnrollment);

select * from Student;
select * from Studies;
select * from Enrollment;
