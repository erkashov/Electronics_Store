using Microsoft.VisualStudio.TestTools.UnitTesting;
using Electronics_Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics_Store.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        [TestMethod()]
        public void AuthTest1()
        {
            string login = "admin";
            string password = "admin";

            User expected = BooksShopEntities.GetContext().Users.
                Where(p => p.login == login && p.password == password).FirstOrDefault();
            User actual = MainWindow.Auth(login, password);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AuthTest2()
        {
            string login = "";
            string password = "";

            User expected = null;
            User actual = MainWindow.Auth(login, password);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AuthTest3()
        {
            string login = "admin      ";
            string password = "admin      ";

            User expected = BooksShopEntities.GetContext().Users.
                Where(p => p.login == login && p.password == password).FirstOrDefault();
            User actual = MainWindow.Auth(login, password);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AuthTest4()
        {
            string login = "ADMIN      ";
            string password = "ADMIN      ";

            User expected = BooksShopEntities.GetContext().Users.
                Where(p => p.login == login && p.password == password).FirstOrDefault();
            User actual = MainWindow.Auth(login, password);

            Assert.AreEqual(expected, actual);
        }
    }
}