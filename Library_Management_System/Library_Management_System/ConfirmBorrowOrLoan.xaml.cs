using System;
using System.Data.SqlClient;
using System.Windows;

namespace Library_Management_System
{
    public partial class ConfirmBorrowOrLoan : Window
    {
        private int memberId;
        private string memberNames;
        private int bookId;
        private string bookTitle;
        private string isbn;
        private string previousPage;

        public ConfirmBorrowOrLoan(int memberId, string memberNames, int bookId, string bookTitle, string isbn, string previousPage)
        {
            InitializeComponent();
            this.memberId = memberId;
            this.memberNames = memberNames;
            this.bookId = bookId;
            this.bookTitle = bookTitle;
            this.isbn = isbn;
            this.previousPage = previousPage;

            
            LoanDateTextBlock.Text = DateTime.Now.ToShortDateString();

            DateTime dueDate = DateTime.Now.AddDays(14);
            DueDateTextBlock.Text = dueDate.ToShortDateString();

            ReturnDatePicker.DisplayDateEnd = dueDate;
            ReturnDatePicker.SelectedDate = dueDate;

            
            var memberInfo = new
            {
                Member_ID = memberId,
                Names = memberNames,
                Book_ID = bookId,
                Title = bookTitle
            };

            VerifyInfo.Items.Clear();
            VerifyInfo.Items.Add(memberInfo); 
        }

        private void ReturnDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ReturnDatePicker.SelectedDate.HasValue)
            {
                DueDateTextBlock.Text = DateTime.Now.AddDays(14).ToShortDateString();
            }
        }

        private void BackConfirmButton(object sender, RoutedEventArgs e)
        {
            if (previousPage == "NewMember")
            {
                NewMember newMemberPage = new NewMember(bookId, bookTitle, isbn, memberNames);
                newMemberPage.Show();
            }
            else if (previousPage == "ExistingMember")
            {
                ExistingMember existingMemberPage = new ExistingMember(bookId, bookTitle, isbn);
                existingMemberPage.Show();
            }

            this.Close(); 
        }

        private void CloseConfirmButton(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void BorrowButton(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                  
                    string updateBookStatusQuery = "UPDATE Book SET Book_Status = 'Borrowed' WHERE Book_ID = @Book_ID";
                    using (SqlCommand cmd = new SqlCommand(updateBookStatusQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Book_ID", bookId);
                        cmd.ExecuteNonQuery();
                    }

                    int loanId;
                    string insertLoanQuery = "INSERT INTO Loan (Member_ID, Book_ID, Loan_Date, Due_Date) " +
                                              "VALUES (@Member_ID, @Book_ID, @Loan_Date, @Due_Date); " +
                                              "SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(insertLoanQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Member_ID", memberId);
                        cmd.Parameters.AddWithValue("@Book_ID", bookId);
                        cmd.Parameters.AddWithValue("@Loan_Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Due_Date", ReturnDatePicker.SelectedDate);

                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            loanId = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Failed to retrieve Loan ID. Please check the insert operation.");
                            return;
                        }
                    }

                    
                    string insertBorrowedBooksQuery = "INSERT INTO Borrowed_Books (Book_ID, Member_ID, Loan_ID, Loan_Date, Due_Date, ISBN, Title, Names) " +
                                                       "VALUES (@Book_ID, @Member_ID, @Loan_ID, @Loan_Date, @Due_Date, @ISBN, @Title, @Names)";

                    using (SqlCommand cmd = new SqlCommand(insertBorrowedBooksQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Book_ID", bookId);
                        cmd.Parameters.AddWithValue("@Member_ID", memberId);
                        cmd.Parameters.AddWithValue("@Loan_ID", loanId);
                        cmd.Parameters.AddWithValue("@Loan_Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Due_Date", ReturnDatePicker.SelectedDate);
                        cmd.Parameters.AddWithValue("@ISBN", isbn);
                        cmd.Parameters.AddWithValue("@Title", bookTitle);
                        cmd.Parameters.AddWithValue("@Names", memberNames);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Borrowing transaction recorded successfully.");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while processing your request: " + ex.Message);
            }
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
