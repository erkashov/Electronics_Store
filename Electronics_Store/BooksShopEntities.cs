using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics_Store
{
    public partial class BooksShopEntities : DbContext
    {
        private static BooksShopEntities _context;

        public static BooksShopEntities GetContext()
        {
            if (_context == null) _context = new BooksShopEntities();
            return _context;
        }
    }

    public partial class User
    {
        public User(string fullname, string name, string papaname, string login, string password, string phone, string email, string role = "Пользователь")
        {
            this.fullname = fullname;
            this.name = name;
            this.papaname = papaname;
            this.login = login;
            this.password = password;
            this.phone = phone;
            this.email = email;
            this.role = role;
        }
    }

    public partial class Sell
    {
        public string FIO
        {
            get
            {
                return $"{User.fullname} {User.name}";
            }
        }
    }
}
