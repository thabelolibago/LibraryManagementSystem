using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Library_Management_System
{
    public partial class ManageMembers : Window
    {
        public ManageMembers()
        {
            InitializeComponent();
            
        }

      
        private void BackManageButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

       
        private void CloseManageButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchedMember.Items.Clear(); 

           
            string selectedFilter = (FilterButton.SelectedItem as ComboBoxItem)?.Content.ToString();
            string searchTerm = SearchTextBox.Text;

           
            string filterColumn = "";
            switch (selectedFilter)
            {
                case "Member Number":
                    filterColumn = "Member_ID";
                    break;
                case "Names":
                    filterColumn = "Names";
                    break;
                case "Contact Number":
                    filterColumn = "Contact_Number";
                    break;
                case "Email":
                    filterColumn = "Email";
                    break;
            }

           
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
                string query = $"SELECT * FROM MEMBER WHERE {filterColumn} LIKE @SearchTerm";

                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    
                    while (reader.Read())
                    {
                        var member = new Member
                        {
                            Member_ID = Convert.ToInt32(reader["Member_ID"]),
                            Names = reader["Names"].ToString(),
                            Member_Address = reader["Member_Address"].ToString(),
                            Contact_Number = reader["Contact_Number"].ToString(),
                            Email = reader["Email"].ToString()
                        };

                        
                        SearchedMember.Items.Add(member);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a search term.");
            }
        }

        
        private void EditMemberButton(object sender, RoutedEventArgs e)
        {
            if (SearchedMember.SelectedItem != null)
            {
                Member selectedMember = (Member)SearchedMember.SelectedItem;

               
                EditMember editWindow = new EditMember(
                    selectedMember.Member_ID.ToString(),
                    selectedMember.Names,
                    selectedMember.Member_Address,
                    selectedMember.Contact_Number,
                    selectedMember.Email);

                editWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a member to edit.");
            }
        }


        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchedMember.SelectedItem != null)
            {
                Member selectedMember = (Member)SearchedMember.SelectedItem;

                string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";

                string query = "DELETE FROM MEMBER WHERE Member_ID = @Member_ID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Member_ID", selectedMember.Member_ID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                SearchedMember.Items.Remove(selectedMember);
                MessageBox.Show("Member deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please select a member to delete.");
            }
        }





        private void AddNewMember(object sender, RoutedEventArgs e)
        {
            AddNewMember addNewMemberWindow = new AddNewMember();
            addNewMemberWindow.Show();
            this.Close();
        }
    }
}


