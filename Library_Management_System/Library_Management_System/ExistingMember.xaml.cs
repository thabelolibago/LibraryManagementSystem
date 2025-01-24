using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace Library_Management_System
{
    public partial class ExistingMember : Window
    {
        private ObservableCollection<Member> existingBooks; 
        private ObservableCollection<Member> searchResults; 

        public ExistingMember(int bookId, string bookTitle, string bookISBN)
        {
            InitializeComponent();

            existingBooks = new ObservableCollection<Member>(); 
            searchResults = new ObservableCollection<Member>(); 

            
            var selectedBook = new Member 
            {
                Book_ID = bookId, 
                Title = bookTitle,   
                ISBN = bookISBN     
            };

            existingBooks.Add(selectedBook);
            SelectedExistingMemberBooks.ItemsSource = existingBooks; 
        }

        private void SearchButton(object sender, RoutedEventArgs e)
        {
            searchResults.Clear(); 

            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            string query = "SELECT * FROM Member WHERE ";
            string searchTerm = searhBox.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            if (NamesRadioButton.IsChecked == true)
            {
                query += "Names LIKE @SearchTerm";
                searchTerm = "%" + searchTerm + "%"; 
            }
            else if (MemberNumberRadioButton.IsChecked == true)
            {
                query += "Member_ID = @SearchTerm";
            }
            else
            {
                MessageBox.Show("Please select a search option.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                searchResults.Add(new Member 
                                {
                                    Member_ID = (int)reader["Member_ID"],
                                    Names = reader["Names"].ToString(),
                                    Member_Address = reader["Member_Address"].ToString(),
                                    Contact_Number = reader["Contact_Number"].ToString(),
                                    Email = reader["Email"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occurred while searching for members: " + ex.Message);
                }
            }

            SearchedMemberResults.ItemsSource = searchResults; 
        }

        private void ContinueButtonExistingMember(object sender, RoutedEventArgs e)
        {
            if (SearchedMemberResults.SelectedItem != null && existingBooks.Count > 0)
            {
                var selectedMember = SearchedMemberResults.SelectedItem as Member; 
                var selectedBook = existingBooks[0]; 

                ConfirmBorrowOrLoan confirmPage = new ConfirmBorrowOrLoan(
                    selectedMember.Member_ID,
                    selectedMember.Names,
                    selectedBook.Book_ID, 
                    selectedBook.Title,     
                    selectedBook.ISBN,     
                    "ExistingMember"
                );

                confirmPage.Show();
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Please select a member to continue.");
            }
        }

        private void BackExistingButton(object sender, RoutedEventArgs e)
        {
            SearchBook searchBookWindow = new SearchBook();
            searchBookWindow.Show(); 
            this.Close(); 
        }

        private void CloseExiButton(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
