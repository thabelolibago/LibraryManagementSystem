using System;
using System.Windows;

namespace Library_Management_System
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void SearchForABookButton(object sender, RoutedEventArgs e)
        {
           
            SearchBook searchBookWindow = new SearchBook();
            this.Hide();
            searchBookWindow.Show();


        }

        private void ReturnBookButton(object sender, RoutedEventArgs e)
        {
            ReturnPage returnBookWindow = new ReturnPage();
            returnBookWindow.Show();
            this.Hide();
        }

        private void ManageMemberButton(object sender, RoutedEventArgs e)
        {
            ManageMembers manageMembers = new ManageMembers();
            manageMembers.Show();
            this.Hide();
         
        }
      
        private void TransactionButton(object sender, RoutedEventArgs e)
        {
            Transaction transactionWindow = new Transaction();
            transactionWindow.Show();
            this.Hide();
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
    }
}
