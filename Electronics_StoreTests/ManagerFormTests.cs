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
    public class ManagerFormTests
    {
        [TestMethod()]
        public void AddUserTest1()
        {
            User user = null;

            string expected = "Укажите пользователя";
            string actual = ManagerForm.Reg(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddUserTest2()
        {
            User user = new User("", "", "", "", "", "","");

            string expected = "Не все поля заполнены";
            string actual = ManagerForm.Reg(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddUserTest3()
        {
            User user = new User("Иванов12", "Иван", "Иванович", "ivan", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "ФИО не должно содержать цифры";
            string actual = ManagerForm.Reg(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddUserTest4()
        {
            User user = new User("Иванов", "Иван", "Иванович", "ivan", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "Успешно";
            string actual = ManagerForm.Reg(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddUserTest5()
        {
            User user = new User("Иванов", "Иван", "Иванович", "ivan", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "Пользователь с таким логином существует";
            string actual = ManagerForm.Reg(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddUserTest6()
        {
            User user = new User("Иванов", "Иван", "Иванович", "ivan2003", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "Пользователь с таким телефоном существует";
            string actual = ManagerForm.Reg(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ChangeUserTest1()
        {
            User user = new User("Иванов", "", "", "ivan2003", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "Не все поля заполнены";
            string actual = ManagerForm.Change(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ChangeUserTest2()
        {
            User user = new User("Ива152нов", "Иван", "Иванович", "ivan2003", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "ФИО не должно содержать цифры";
            string actual = ManagerForm.Change(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ChangeUserTest3()
        {
            User user = new User("Иванов", "Ива4826н", "Иванович", "ivan", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "ФИО не должно содержать цифры";
            string actual = ManagerForm.Change(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ChangeUserTest4()
        {
            User user = new User("Иванов", "Иван", "Иванович", "ivan", "ivan123", "9391049223", "ivan@ivan.ru");

            string expected = "Пользователь с таким номером уже существует";
            string actual = ManagerForm.Change(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ChangeUserTest5()
        {
            User user = new User("Иванов", "Владимир", "Иванович", "ivan", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "Успешно";
            string actual = ManagerForm.Change(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveUserTest1()
        {
            User user = null;

            string expected = "Укажите пользователя";
            string actual = ManagerForm.RemoveUser(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveUserTest2()
        {
            User user = new User("Иванов", "Владимир", "Иванович", "999999999", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "Пользователя с таким логином не существует";
            string actual = ManagerForm.RemoveUser(user);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveUserTest3()
        {
            User user = new User("Иванов", "Владимир", "Иванович", "ivan", "ivan123", "9391049222", "ivan@ivan.ru");

            string expected = "Успешно";
            string actual = ManagerForm.RemoveUser(user);

            Assert.AreEqual(expected, actual);
        }
    }
}