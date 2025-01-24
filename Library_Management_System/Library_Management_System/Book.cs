using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System
{
    public class Book
    {
        public int Book_ID { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }
        public string ISBN { get; set; }

        public string Publication_Year { get; set; }

        public string Genre { get; set; }
        public string Book_Status { get; set; }
    }
}
