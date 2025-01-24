use LibraryDB;
CREATE TABLE Book (

  Book_ID INT PRIMARY KEY IDENTITY(1,1),
  Title VARCHAR(250),
  Author VARCHAR(250),
  ISBN VARCHAR(250),
  Pblication_Year INT,
  Genre VARCHAR(250),
  Book_Status VARCHAR(250) CHECK (Book_Status IN('Available', 'Borrowed'))
);

CREATE TABLE Member (

 Member_ID INT PRIMARY KEY IDENTITY(1,1),
 Names VARCHAR(250),
 Member_Address VARCHAR(250),
 Contact_Number VARCHAR(250),
 Email VARCHAR(250)
);

CREATE TABLE Loan (
   
   Loan_ID INT PRIMARY KEY IDENTITY(1,1),
   Book_ID INT FOREIGN KEY REFERENCES Book(Book_ID)
               ON DELETE NO ACTION
			   ON UPDATE CASCADE,
   Member_ID INT FOREIGN KEY REFERENCES Member(Member_ID)
               ON DELETE NO ACTION
			   ON UPDATE CASCADE,
   Loan_Date DATE NOT NULL,
   Due_Date DATE NOT NULL,
   Return_Date DATE NOT NULL
);