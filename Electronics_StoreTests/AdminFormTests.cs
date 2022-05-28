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
    public class AdminFormTests
    {
        [TestMethod()]
        public void AddTovarTest1()
        {
            Tovar t = null;

            string expected = "Укажите товар";
            string actual = AdminForm.AddTovar(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddTovarTest2()
        {
            Tovar t = new Tovar() { author="", category="", name="", number=0, price=0 };

            string expected = "Заполните все поля";
            string actual = AdminForm.AddTovar(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddTovarTest3()
        {
            Tovar t = new Tovar() { author = "Толстой", category = "Худ. литература", name = "Война и мир", number = 100, price = 1200 };

            string expected = "Успешно";
            string actual = AdminForm.AddTovar(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddTovarTest4()
        {
            Tovar t = new Tovar() { author = "Толстой", category = "Худ. литература", name = "Война и мир", number = 100, price = 1200 };

            string expected = "Данный товар уже существует";
            string actual = AdminForm.AddTovar(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddTovarTest5()
        {
            Tovar t = new Tovar() { author = "Пушкин", category = "Художественная литература", name = "Евгений Онегин", number = 100, price = 1200 };

            string expected = "Конфликт инструкции INSERT с ограничением FOREIGN KEY \"FK_Tovar_Category\".";
            string actual = AdminForm.AddTovar(t);
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod()]
        public void ChangeTovTest1()
        {
            Tovar t = null;

            string expected = "Укажите товар";
            string actual = AdminForm.ChangeTov(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ChangeTovTest2()
        {
            Tovar t = new Tovar() { author = "Толстой", category = "Худ. литература", name = "Война и мир", number = 0, price = 1200 };

            string expected = "Заполните все поля";
            string actual = AdminForm.ChangeTov(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ChangeTovTest3()
        {
            Tovar t = new Tovar() { author = "Толстой", category = "Худ. литература", name = "Война и мир", number = 0, price = 1200 };

            string expected = "Заполните все поля";
            string actual = AdminForm.ChangeTov(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ChangeTovTest4()
        {
            Tovar t = new Tovar() { author = "Толстой", category = "Худ. литература", name = "Война и мир", number = 10, price = 1200, id = -1 };

            string expected = "Товара с таким id не найдено";
            string actual = AdminForm.ChangeTov(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ChangeTovTest5()
        {
            Tovar t = BooksShopEntities.GetContext().Tovars.Where(p => p.author == "Толстой" && p.name == "Война и мир").FirstOrDefault();
            t.price = 1300;
            string expected = "Успешно";
            string actual = AdminForm.ChangeTov(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveTovTest1()
        {
            Tovar t = null;
            string expected = "Укажите товар";
            string actual = AdminForm.RemoveTov(t);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveTovTest2()
        {
            Tovar t = BooksShopEntities.GetContext().Tovars.Where(p => p.author == "Толстой" && p.name == "Война и мир").FirstOrDefault();
            string expected = "Успешно";
            string actual = AdminForm.RemoveTov(t);

            Assert.AreEqual(expected, actual);
        }
    }
}