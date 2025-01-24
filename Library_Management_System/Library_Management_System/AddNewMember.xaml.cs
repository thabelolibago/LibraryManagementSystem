using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library_Management_System
{

    public partial class AddNewMember : Window
    {
        public AddNewMember()
        {
            InitializeComponent();
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

            string insertQuery = "INSERT INTO Member (Names, Member_Address, Contact_Number, Email) " +
                                 "VALUES (@Names, @Member_Address, @Contact_Number, @Email)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@Names", NamesTextBox.Text);
                        cmd.Parameters.AddWithValue("@Member_Address", Member_AddressTextBox.Text);
                        cmd.Parameters.AddWithValue("@Contact_Number", Contact_NumberTextBox.Text);
                        cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);

                      
                        cmd.ExecuteNonQuery();

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

        private void BackAddNewMemberButton(object sender, RoutedEventArgs e)
        {
            ManageMembers manageMembers = new ManageMembers();
            manageMembers.Show();
            this.Hide();
        }

        private void CloseAddNewMemberButton(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {

                if (textBox.Text == "Thabelo Libago" ||
                    textBox.Text == "1234 Example Street, City, 1000, South Africa" ||
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
