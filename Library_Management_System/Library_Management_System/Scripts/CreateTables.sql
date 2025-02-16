USE LibraryDB;

CREATE TABLE Book (
  Book_ID INT PRIMARY KEY IDENTITY(1,1),
  Title VARCHAR(250),
  Author VARCHAR(250),
  ISBN VARCHAR(250),
  Publication_Year INT,  
  Genre VARCHAR(250),
  Book_Status VARCHAR(50) CHECK (Book_Status IN ('Available', 'Borrowed'))
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
  Return_Date DATE NULL  
);


CREATE TABLE LIBRARIAN (
  Username NVARCHAR(100) NOT NULL PRIMARY KEY,  
  User_Password VARCHAR(255) NOT NULL
);


CREATE TABLE Borrowed_Books (
  BorrowedBook_ID INT PRIMARY KEY IDENTITY(1,1),
  Book_ID INT NOT NULL,
  Member_ID INT NOT NULL,
  Loan_ID INT NOT NULL,
  Loan_Date DATETIME NOT NULL,
  Due_Date DATETIME NOT NULL,
  ISBN VARCHAR(20),
  Title VARCHAR(255) NOT NULL,
  Names VARCHAR(255) NOT NULL,
  FOREIGN KEY (Book_ID) REFERENCES Book(Book_ID), 
  FOREIGN KEY (Member_ID) REFERENCES Member(Member_ID),
  FOREIGN KEY (Loan_ID) REFERENCES Loan(Loan_ID) 
);


CREATE TABLE Returned_Books (
  ReturnedBook_ID INT PRIMARY KEY IDENTITY(1,1),
  Book_ID INT NOT NULL,
  Member_ID INT NOT NULL,
  Loan_ID INT NOT NULL,
  Return_Date DATETIME NOT NULL,
  Due_Date DATETIME NOT NULL,
  ISBN VARCHAR(20),
  Title VARCHAR(255) NOT NULL,
  Names VARCHAR(255) NOT NULL,
  FOREIGN KEY (Book_ID) REFERENCES Book(Book_ID),
  FOREIGN KEY (Member_ID) REFERENCES Member(Member_ID),
  FOREIGN KEY (Loan_ID) REFERENCES Loan(Loan_ID)
);