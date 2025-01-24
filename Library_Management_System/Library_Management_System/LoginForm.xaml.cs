using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Library_Management_System
{
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

           
            if (string.IsNullOrWhiteSpace(username))
            {
                ShowErrorPopup("Username cannot be empty.");
                return;
            }

            if (!IsValidPasswordFormat(password))
            {
                ShowErrorPopup("Password must contain at least one uppercase letter, one symbol, and one number.");
                return;
            }

            
            if (ValidateCredentials(username, password))
            {
                MainWindow mainWindow = new MainWindow();
                this.Hide();
                mainWindow.Show();
            }
            else
            {
                ShowErrorPopup("Invalid username or password.");
            }
        }

        
        private bool IsValidPasswordFormat(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return passwordRegex.IsMatch(password);
        }

        
        private bool ValidateCredentials(string username, string password)
        {
            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            string query = "SELECT COUNT(1) FROM LIBRARIAN WHERE Username = @Username AND User_Password = @User_Password";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@User_Password", password);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count == 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while connecting to the database: " + ex.Message);
                    return false;
                }
            }
        }

        
        private void ShowErrorPopup(string message)
        {
            PopupMessage.Text = message;
            ErrorPopup.IsOpen = true;
        }
    }
}
