using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PressePapier.Model;

namespace PressePapierTests.Model
{
    /// <summary>
    /// Summary description for TextUtilsTests
    /// </summary>
    [TestClass]
    public class TextUtilsTests
    {
        public TextUtilsTests()
        {
            //
            // TODO: Add constructor logic here
            //
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
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetNomFichierTest()
        {
            string actual;
            string expected;

            actual = TextUtils.GetNomFichier("C:\\truc\\bidule\\test.1.2.3.xml");
            expected = "test.1.2.3";
            Assert.AreEqual<string>(expected, actual);

            actual = TextUtils.GetNomFichier("test.1.2.3.xml");
            expected = "test.1.2.3";
            Assert.AreEqual<string>(expected, actual);

            actual = TextUtils.GetNomFichier("C:\\truc\\bidule\\test123.xml");
            expected = "test123";
            Assert.AreEqual<string>(expected, actual);

            actual = TextUtils.GetNomFichier("test123.xml");
            expected = "test123";
            Assert.AreEqual<string>(expected, actual);

            actual = TextUtils.GetNomFichier("test123");
            expected = "test123";
            Assert.AreEqual<string>(expected, actual);

            actual = TextUtils.GetNomFichier("");
            expected = "";
            Assert.AreEqual<string>(expected, actual);
        }

        [TestMethod]
        public void GetTooltipTextTest()
        {

        }
    }
}
