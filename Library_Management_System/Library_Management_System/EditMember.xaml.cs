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
   
    public partial class EditMember : Window
    {
        public EditMember(string member_id, string names, string address, string contactNumber, string email)
        {
            InitializeComponent();
            Member_IDTextBlock.Text = member_id;
            Names.Text = names;
            Address.Text = address;
            Contact_Number.Text = contactNumber;
            Email.Text = email;
        }

        private void BackEditMemberButton(object sender, RoutedEventArgs e)
        {
            ManageMembers manageMember = new ManageMembers();
            manageMember.Show();
            this.Close();
        }
        private void CloseEditMemberButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateButton(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            string updateQuery = "UPDATE Member SET Names = @Names, Member_Address = @Member_Address, Contact_Number = @Contact_Number, Email = @Email WHERE Member_ID = @Member_ID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@Member_ID", Member_IDTextBlock.Text);
                    cmd.Parameters.AddWithValue("@Names", Names.Text);
                    cmd.Parameters.AddWithValue("@Member_Address", Address.Text);
                    cmd.Parameters.AddWithValue("@Contact_Number", Contact_Number.Text);
                    cmd.Parameters.AddWithValue("@Email", Email.Text);
                    

                    
                    cmd.ExecuteNonQuery();

                   
                    MessageBox.Show("Member information updated successfully.");
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating member: " + ex.Message);
                }

            }

        }
    }
}
