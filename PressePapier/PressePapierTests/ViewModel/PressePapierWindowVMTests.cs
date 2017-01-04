using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PressePapier.ViewModel;
using System.Windows.Forms;
using OuelletKeyHandler;

namespace PressePapierTests.ViewModel
{
    /// <summary>
    /// Summary description for PressePapierWindowVMTests
    /// </summary>
    [TestClass]
    public class PressePapierWindowVMTests
    {
        PressePapierWindowVM viewModelTest;
        NotifyIcon nIcon;
        
        public PressePapierWindowVMTests()
        {
            nIcon = new NotifyIcon();
            viewModelTest = new PressePapierWindowVM(nIcon);

            foreach (var p in viewModelTest.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)))
                p.SetValue(viewModelTest, "");
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void CleanupKeys()
        {
            foreach (HotKey hotKey in viewModelTest.lstHotKeys)
                hotKey.Unregister();
            viewModelTest.lstHotKeys.Clear();
        }
        
        #endregion

        [TestMethod]
        public void GetTextesTest()
        {
            var actual = new Dictionary<string, string>();
            var expected = new Dictionary<string, string>();

            viewModelTest.TextTB1 = "testTexteTB1";
            viewModelTest.TextTB2 = "testTexteTB2";
            viewModelTest.TextTB3 = "testTexteTB3";
            viewModelTest.TextTB4 = "testTexteTB4";
            viewModelTest.TextTB5 = "testTexteTB5";
            viewModelTest.TextTB6 = "testTexteTB6";
            viewModelTest.TextTB7 = "testTexteTB7";
            viewModelTest.TextTB8 = "testTexteTB8";
            viewModelTest.TextTB9 = "testTexteTB9";
            viewModelTest.TextTB10 = "testTexteTB10";

            expected.Add("TextTB1", "testTexteTB1");
            expected.Add("TextTB2", "testTexteTB2");
            expected.Add("TextTB3", "testTexteTB3");
            expected.Add("TextTB4", "testTexteTB4");
            expected.Add("TextTB5", "testTexteTB5");
            expected.Add("TextTB6", "testTexteTB6");
            expected.Add("TextTB7", "testTexteTB7");
            expected.Add("TextTB8", "testTexteTB8");
            expected.Add("TextTB9", "testTexteTB9");
            expected.Add("TextTB10", "testTexteTB10");

            actual = viewModelTest.GetTextes();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetTextesTest()
        {
            var dicTest = new Dictionary<string, string>();

            dicTest.Add("TextTB1", "testTexteTB1");
            dicTest.Add("TextTB2", "testTexteTB2");
            dicTest.Add("TextTB3", "testTexteTB3");
            dicTest.Add("TextTB4", "testTexteTB4");
            dicTest.Add("TextTB5", "testTexteTB5");
            dicTest.Add("TextTB6", "testTexteTB6");
            dicTest.Add("TextTB7", "testTexteTB7");
            dicTest.Add("TextTB8", "testTexteTB8");
            dicTest.Add("TextTB9", "testTexteTB9");
            dicTest.Add("TextTB10", "testTexteTB10");

            viewModelTest.SetTextes(dicTest);

            Assert.AreEqual("testTexteTB1", viewModelTest.TextTB1);
            Assert.AreEqual("testTexteTB2", viewModelTest.TextTB2);
            Assert.AreEqual("testTexteTB3", viewModelTest.TextTB3);
            Assert.AreEqual("testTexteTB4", viewModelTest.TextTB4);
            Assert.AreEqual("testTexteTB5", viewModelTest.TextTB5);
            Assert.AreEqual("testTexteTB6", viewModelTest.TextTB6);
            Assert.AreEqual("testTexteTB7", viewModelTest.TextTB7);
            Assert.AreEqual("testTexteTB8", viewModelTest.TextTB8);
            Assert.AreEqual("testTexteTB9", viewModelTest.TextTB9);
            Assert.AreEqual("testTexteTB10", viewModelTest.TextTB10);
        }
    }
}
