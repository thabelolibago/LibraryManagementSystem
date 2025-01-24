using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Library_Management_System
{
    public partial class SearchBook : Window
    {
        private ObservableCollection<Book> searchResults;
        private string selectedCriteria;

        public SearchBook()
        {
            InitializeComponent();
            searchResults = new ObservableCollection<Book>();
            ResultsListView.ItemsSource = searchResults;
        }


        private void SearchButton(object sender, RoutedEventArgs e)
        {
            searchResults.Clear();

            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            string query = "SELECT * FROM BOOK WHERE";
            string searchTerm = SearchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }


            if (selectedCriteria == "Title")
            {
                query += " Title LIKE @SearchTerm";
                searchTerm = "%" + searchTerm + "%";
            }
            else if (selectedCriteria == "Author")
            {
                query += " Author LIKE @SearchTerm";
                searchTerm = "%" + searchTerm + "%";
            }
            else if (selectedCriteria == "Year")
            {
                query += " Publication_Year = @SearchTerm";
            }
            else if (selectedCriteria == "ISBN")
            {
                query += " ISBN = @SearchTerm";
            }
            else if (selectedCriteria == "Genre")
            {
                query += " Genre LIKE @SearchTerm";
                searchTerm = "%" + searchTerm + "%";
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
                                searchResults.Add(new Book
                                {
                                    Book_ID = (int)reader["Book_ID"],
                                    Title = reader["Title"].ToString(),
                                    Author = reader["Author"].ToString(),
                                    Publication_Year = reader["Publication_Year"].ToString(),
                                    ISBN = reader["ISBN"].ToString(),
                                    Genre = reader["Genre"].ToString(),
                                    Book_Status = reader["Book_Status"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occurred while searching for books: " + ex.Message);
                }
            }

        }


        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                selectedCriteria = radioButton.Content.ToString();
            }
        }

        private void LoadAvailableBooks()
        {
            searchResults.Clear();
            string connectionString = "Data Source=exz_mp2hmrb1\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            string query = "SELECT * FROM BOOK WHERE Book_Status = 'Available'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            searchResults.Add(new Book
                            {
                                Book_ID = (int)reader["Book_ID"],
                                Title = reader["Title"].ToString(),
                                Author = reader["Author"].ToString(),
                                Publication_Year = reader["Publication_Year"].ToString(),
                                ISBN = reader["ISBN"].ToString(),
                                Genre = reader["Genre"].ToString(),
                                Book_Status = reader["Book_Status"].ToString()
                            });
                        }
                    }
                }
            }
        }


        private void ResultsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultsListView.SelectedItem != null)
            {
                var selectedBook = ResultsListView.SelectedItem as Book;
                if (selectedBook != null && selectedBook.Book_Status != "Available")
                {
                    ResultsListView.SelectedItem = null;
                    MessageBox.Show("This book is currently borrowed and cannot be selected.");
                }
            }
        }


        private void NewMemberButton(object sender, RoutedEventArgs e)
        {
            if (ResultsListView.SelectedItem != null)
            {
                var selectedBook = ResultsListView.SelectedItem as Book;
                if (selectedBook != null)
                {
                    if (selectedBook.Book_Status == "Available")
                    {
                        int bookId = selectedBook.Book_ID; 
                        string bookTitle = selectedBook.Title; 
                        string isbn = selectedBook.ISBN; 
                        string memberNames = "New Member";

                        NewMember newMemberWindow = new NewMember(bookId, bookTitle, isbn, memberNames);
                        newMemberWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("This book is currently borrowed. Please choose another book.");
                        ResultsListView.SelectedItem = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a book to borrow.");
            }
        }

        private void ExistingMemberMemberButton(object sender, RoutedEventArgs e)
        {
            if (ResultsListView.SelectedItem != null)
            {
                var selectedBook = ResultsListView.SelectedItem as Book;
                if (selectedBook != null)
                {
                    int memberId = selectedBook.Book_ID; 
                    string memberName = selectedBook.Title; 
                    string bookTitle = selectedBook.ISBN; 

                    ExistingMember existingMemberWindow = new ExistingMember(selectedBook.Book_ID, selectedBook.Title, selectedBook.ISBN);
                    existingMemberWindow.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Please select a book to borrow.");
            }
        }




        private void BackSearchBookButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }


        private void CloseSearchBookButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
       

      
    }
}
