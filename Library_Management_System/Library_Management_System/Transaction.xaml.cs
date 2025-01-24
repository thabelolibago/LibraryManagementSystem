using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows;



namespace Library_Management_System
{
    public partial class Transaction : Window
    {
        private ObservableCollection<BorrowedBook> borrowedBooks;
        private ObservableCollection<ReturnedBook> returnedBooks;

        public Transaction()
        {
            InitializeComponent();
            borrowedBooks = new ObservableCollection<BorrowedBook>();
            returnedBooks = new ObservableCollection<ReturnedBook>();
            listOfBorrowedBooks.ItemsSource = borrowedBooks;
            listOfReturnedBooks.ItemsSource = returnedBooks;

            LoadBorrowedBooks();
            LoadReturnedBooks();
        }

        private void LoadBorrowedBooks()
        {
            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
               
                string selectBorrowedBooksQuery = @"
                    SELECT 
                        Loan.Book_ID, 
                        Loan.Member_ID, 
                        Loan.Loan_ID, 
                        Loan.Loan_Date, 
                        Loan.Due_Date,
                        Book.ISBN, 
                        Book.Title, 
                        Member.Names 
                    FROM Loan
                    JOIN Book ON Loan.Book_ID = Book.Book_ID
                    JOIN Member ON Loan.Member_ID = Member.Member_ID
                    WHERE Loan.Return_Date IS NULL"; 

                using (SqlCommand cmd = new SqlCommand(selectBorrowedBooksQuery, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var borrowedBook = new BorrowedBook
                        {
                            Book_ID = reader.GetInt32(0),         
                            Member_ID = reader.GetInt32(1),       
                            Loan_ID = reader.GetInt32(2),         
                            Loan_Date = reader.GetDateTime(3),    
                            Due_Date = reader.GetDateTime(4),     
                            ISBN = reader.GetString(5),           
                            Title = reader.GetString(6),          
                            Names = reader.GetString(7)           
                        };

                        borrowedBooks.Add(borrowedBook);
                    }
                }
            }
        }

        private void LoadReturnedBooks()
        {
            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                string selectReturnedBooksQuery = @"
                    SELECT 
                        Book_ID, 
                        Member_ID, 
                        Loan_ID, 
                        Return_Date, 
                        Due_Date, 
                        ISBN, 
                        Title, 
                        Names
                    FROM Returned_Books"; 

                using (SqlCommand cmd = new SqlCommand(selectReturnedBooksQuery, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var returnedBook = new ReturnedBook
                        {
                            Book_ID = reader.GetInt32(0),         
                            Member_ID = reader.GetInt32(1),       
                            Loan_ID = reader.GetInt32(2),         
                            Return_Date = reader.GetDateTime(3),  
                            Due_Date = reader.GetDateTime(4),     
                            ISBN = reader.GetString(5),           
                            Title = reader.GetString(6),          
                            Names = reader.GetString(7)           
                        };

                        returnedBooks.Add(returnedBook);
                    }
                }
            }
        }

        private void searchBorrowedBooks(object sender, RoutedEventArgs e)
        {
            string searchISBN = searchBorrowedBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchISBN))
            {
                borrowedBooks.Clear(); 

                string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string searchQuery = @"
                SELECT 
                    Loan.Book_ID, 
                    Loan.Member_ID, 
                    Loan.Loan_ID, 
                    Loan.Loan_Date, 
                    Loan.Due_Date,
                    Book.ISBN, 
                    Book.Title, 
                    Member.Names 
                FROM Loan
                JOIN Book ON Loan.Book_ID = Book.Book_ID
                JOIN Member ON Loan.Member_ID = Member.Member_ID
                WHERE Loan.Return_Date IS NULL AND Book.ISBN = @ISBN"; 

                    using (SqlCommand cmd = new SqlCommand(searchQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ISBN", searchISBN);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var borrowedBook = new BorrowedBook
                                {
                                    Book_ID = reader.GetInt32(0),
                                    Member_ID = reader.GetInt32(1),
                                    Loan_ID = reader.GetInt32(2),
                                    Loan_Date = reader.GetDateTime(3),
                                    Due_Date = reader.GetDateTime(4),
                                    ISBN = reader.GetString(5),
                                    Title = reader.GetString(6),
                                    Names = reader.GetString(7)
                                };

                                borrowedBooks.Add(borrowedBook);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter an ISBN to search.");
            }

        }

        private void SearchReturnedBooks(object sender, RoutedEventArgs e)
        {

            string searchMemberID = searchReturnedBox.Text.Trim(); 

            if (int.TryParse(searchMemberID, out int memberId)) 
            {
                returnedBooks.Clear(); 

                string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string searchQuery = @"
                SELECT 
                    Book_ID, 
                    Member_ID, 
                    Loan_ID, 
                    Return_Date, 
                    Due_Date, 
                    ISBN, 
                    Title, 
                    Names
                FROM Returned_Books
                WHERE Member_ID = @MemberID"; 

                    using (SqlCommand cmd = new SqlCommand(searchQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberId); 

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var returnedBook = new ReturnedBook
                                {
                                    Book_ID = reader.GetInt32(0),
                                    Member_ID = reader.GetInt32(1),
                                    Loan_ID = reader.GetInt32(2),
                                    Return_Date = reader.GetDateTime(3),
                                    Due_Date = reader.GetDateTime(4),
                                    ISBN = reader.GetString(5),
                                    Title = reader.GetString(6),
                                    Names = reader.GetString(7)
                                };

                                returnedBooks.Add(returnedBook); 
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Member ID to search.");
            }


        }

        private void RefreshBorrowedBooks(object sender, RoutedEventArgs e)
        {
            borrowedBooks.Clear();
            LoadBorrowedBooks();
        }

        private void RefreshReturnedBooks(object sender, RoutedEventArgs e)
        {
            returnedBooks.Clear();
            LoadReturnedBooks();
        }

        private void BackTransactionButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void CloseTransactionButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DownloadBorrowedBooks(object sender, RoutedEventArgs e)
        {
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("Book_ID,Member_ID,Loan_ID,Loan_Date,Due_Date,ISBN,Title,Names");

            foreach (var book in borrowedBooks)
            {
                csvContent.AppendLine($"{book.Book_ID},{book.Member_ID},{book.Loan_ID},{book.Loan_Date},{book.Due_Date},{book.ISBN},{book.Title},{book.Names}");
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            saveFileDialog.Title = "Save Borrowed Books List";
            saveFileDialog.FileName = "BorrowedBooks.csv";

            
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());
                MessageBox.Show("Borrowed books list downloaded as CSV.");
            }



        }

        private void DownloadReturnedBooks(object sender, RoutedEventArgs e)
        {
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("Book_ID,Member_ID,Loan_ID,Return_Date,Due_Date,ISBN,Title,Names");

            foreach (var book in returnedBooks)
            {
                csvContent.AppendLine($"{book.Book_ID},{book.Member_ID},{book.Loan_ID},{book.Return_Date},{book.Due_Date},{book.ISBN},{book.Title},{book.Names}");
            }

            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            saveFileDialog.Title = "Save Returned Books List";
            saveFileDialog.FileName = "ReturnedBooks.csv";

            
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());
                MessageBox.Show("Returned books list downloaded as CSV.");
            }
        }
    }


    public class BorrowedBook
    {
        public int Book_ID { get; set; }          
        public int Member_ID { get; set; }        
        public int Loan_ID { get; set; }          
        public DateTime Loan_Date { get; set; }   
        public DateTime Due_Date { get; set; }    
        public string ISBN { get; set; }          
        public string Title { get; set; }         
        public string Names { get; set; }          
    }

   
    public class ReturnedBook
    {
        public int Book_ID { get; set; }          
        public int Member_ID { get; set; }        
        public int Loan_ID { get; set; }          
        public DateTime Return_Date { get; set; } 
        public DateTime Due_Date { get; set; }   
        public string ISBN { get; set; }        
        public string Title { get; set; }         
        public string Names { get; set; }         
        public DateTime Loan_Date { get; set; }
    }
}

