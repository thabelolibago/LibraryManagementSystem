using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Library_Management_System
{
    public partial class NewMember : Window
    {
       
        private int bookId;        
        private string bookTitle;  
        private string isbn;       
        private string memberNames; 
        private bool isMemberAdded;

        
        public NewMember(int bookId, string bookTitle, string isbn, string memberNames)
        {
            InitializeComponent();
            this.bookId = bookId;
            this.bookTitle = bookTitle;
            this.isbn = isbn;
            this.memberNames = memberNames;

           
            SelectedBook.Items.Add(new { Book_ID = bookId, Title = bookTitle, ISBN = isbn });
            isMemberAdded = false;
        }

        private void SubmitButton(object sender, RoutedEventArgs e)
        {
            string namePattern = @"^[A-Za-z\s]+$";
            string addressPattern = @"^[A-Za-z\s]+$";
            string contactPattern = @"^\d{10}$";
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            string names = NamesTextBox.Text.Trim();
            string address = Member_AddressTextBox.Text.Trim();
            string contact = Contact_NumberTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();

            
            if (string.IsNullOrEmpty(names) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(names, namePattern))
            {
                MessageBox.Show("Please enter a valid name (A-Z only).");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(address, addressPattern))
            {
                MessageBox.Show("Please enter a valid address (A-Z only).");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(contact, contactPattern))
            {
                MessageBox.Show("Please enter a valid 10-digit South African phone number.");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Please enter a valid email address.");
                return;
            }

            
            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            string insertQuery = "INSERT INTO Member (Names, Member_Address, Contact_Number, Email) VALUES (@Names, @Member_Address, @Contact_Number, @Email)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Names", names);
                        cmd.Parameters.AddWithValue("@Member_Address", address);
                        cmd.Parameters.AddWithValue("@Contact_Number", contact);
                        cmd.Parameters.AddWithValue("@Email", email);

                        cmd.ExecuteNonQuery();
                        isMemberAdded = true;

                         memberNames = NamesTextBox.Text.Trim();
                         
                        MessageBox.Show("New member added successfully.");
                        ClearForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ClearForm()
        {
            NamesTextBox.Clear();
            Member_AddressTextBox.Clear();
            Contact_NumberTextBox.Clear();
            EmailTextBox.Clear();
        }

        private void BackMemberButton(object sender, RoutedEventArgs e)
        {
            SearchBook searchBookWindow = new SearchBook();
            searchBookWindow.Show();
            this.Close();
        }

        private void CloseMemberButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ContinueButton(object sender, RoutedEventArgs e)
        {
            if (!isMemberAdded)
            {
                MessageBox.Show("Please add a member before proceeding.");
                return;
            }

            int memberId = GetLastMemberId();
            

            ConfirmBorrowOrLoan confirmBorrowOrLoanWindow = new ConfirmBorrowOrLoan(memberId, memberNames, bookId, bookTitle, isbn, "NewMember");
            confirmBorrowOrLoanWindow.Show();
            this.Close();
        }

        private int GetLastMemberId()
        {
            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            int memberId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 Member_ID FROM Member ORDER BY Member_ID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        memberId = Convert.ToInt32(result);
                    }
                }
            }

            return memberId;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
               
                if (textBox.Text == "Thabelo Libago" ||
                    textBox.Text == "City South Africa" ||
                    textBox.Text == "079 223 0111" ||
                    textBox.Text == "thabelo@library.com")
                {
                    textBox.Clear();
                    textBox.Foreground = Brushes.Black;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                
                if (textBox.Name == "NamesTextBox")
                    textBox.Text = "Thabelo Libago";
                else if (textBox.Name == "Member_AddressTextBox")
                    textBox.Text = "1234 Example Street, City, 1000, South Africa";
                else if (textBox.Name == "Contact_NumberTextBox")
                    textBox.Text = "079 223 0111";
                else if (textBox.Name == "EmailTextBox")
                    textBox.Text = "thabelo@library.com";

                textBox.Foreground = new SolidColorBrush(Color.FromRgb(193, 188, 188));
            }
        }
    }
}
