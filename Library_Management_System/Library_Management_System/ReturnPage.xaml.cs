using System;
using System.Data.SqlClient;
using System.Windows;

namespace Library_Management_System
{
    public partial class ReturnPage : Window
    {
        public ReturnPage()
        {
            InitializeComponent();
        }

        private void SubmitBtn(object sender, RoutedEventArgs e)
        {
            string memberNumber = MemberNumberTextBox.Text.Trim();
            string bookNumber = BookNumberTextBox.Text.Trim();

            if (string.IsNullOrEmpty(bookNumber))
            {
                MessageBox.Show("Please enter the Book Number.");
                return;
            }

            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); 

                try
                {
                    int bookId = -1;
                    string loanDate = string.Empty;
                    string dueDate = string.Empty;
                    string title = string.Empty;
                    string isbn = string.Empty;
                    string memberNames = string.Empty;
                    int loanId = -1;
                    string bookStatus = string.Empty;

                   
                    string checkMemberQuery = "SELECT COUNT(*) FROM Member WHERE Member_ID = @Member_ID";
                    using (SqlCommand cmd = new SqlCommand(checkMemberQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Member_ID", memberNumber);
                        int memberCount = (int)cmd.ExecuteScalar();

                        if (memberCount == 0)
                        {
                            MessageBox.Show("Member does not exist. Please enter a valid Member Number.");
                            transaction.Rollback();
                            return;
                        }
                    }

                    
                    string selectBookQuery = "SELECT Book_ID, ISBN, Book_Status FROM Book WHERE Book_ID = @Book_ID";
                    using (SqlCommand cmd = new SqlCommand(selectBookQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Book_ID", bookNumber);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bookId = reader.GetInt32(0);
                                isbn = reader.GetString(1);
                                bookStatus = reader.GetString(2);
                            }
                        }
                    }

                    
                    if (bookStatus == "Available")
                    {
                        MessageBox.Show("The book has already been returned.");
                        transaction.Rollback();
                        return;
                    }

                    if (bookId != -1)
                    {
                        
                        string loanQuery = "SELECT Loan_ID, Loan_Date, Due_Date, Member_ID FROM Loan WHERE Book_ID = @Book_ID AND Member_ID = @Member_ID";
                        using (SqlCommand cmd = new SqlCommand(loanQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Book_ID", bookId);
                            cmd.Parameters.AddWithValue("@Member_ID", memberNumber);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    loanId = reader.GetInt32(0);
                                    loanDate = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                                    dueDate = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                                }
                            }
                        }

                        
                        string bookQuery = "SELECT Title FROM Book WHERE Book_ID = @Book_ID";
                        using (SqlCommand cmd = new SqlCommand(bookQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Book_ID", bookId);
                            var result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                title = result.ToString();
                            }
                        }

                        
                        string memberQuery = "SELECT Names FROM Member WHERE Member_ID = @Member_ID";
                        using (SqlCommand cmd = new SqlCommand(memberQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Member_ID", memberNumber);
                            var result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                memberNames = result.ToString();
                            }
                        }

                        
                        string insertReturnedBooksQuery = "INSERT INTO Returned_Books (Book_ID, Member_ID, Loan_ID, Return_Date, Due_Date, ISBN, Title, Names) " +
                                                          "VALUES (@Book_ID, @Member_ID, @Loan_ID, @Return_Date, @Due_Date, @ISBN, @Title, @Names)";
                        using (SqlCommand cmd = new SqlCommand(insertReturnedBooksQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Book_ID", bookId);
                            cmd.Parameters.AddWithValue("@Member_ID", memberNumber);
                            cmd.Parameters.AddWithValue("@Loan_ID", loanId);
                            cmd.Parameters.AddWithValue("@Return_Date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Due_Date", dueDate);
                            cmd.Parameters.AddWithValue("@ISBN", isbn);
                            cmd.Parameters.AddWithValue("@Title", title);
                            cmd.Parameters.AddWithValue("@Names", memberNames);

                            cmd.ExecuteNonQuery();
                        }

                       
                        string updateBookStatusQuery = "UPDATE Book SET Book_Status = 'Available' WHERE Book_ID = @Book_ID";
                        using (SqlCommand cmd = new SqlCommand(updateBookStatusQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Book_ID", bookId);
                            cmd.ExecuteNonQuery();
                        }

                        
                        transaction.Commit();
                        MessageBox.Show("Book returned successfully, transaction recorded, and book status updated.");
                    }
                    else
                    {
                        MessageBox.Show("Book not found. Please check the Book Number.");
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    
                    transaction.Rollback();
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

          
            MemberNumberTextBox.Clear();
            BookNumberTextBox.Clear();
        }

        private void BackReturnButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void CloseReturnButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}











