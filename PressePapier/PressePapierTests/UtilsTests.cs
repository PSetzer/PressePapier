using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PressePapier;

namespace PressePapierTests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void GetNomFichierTest()
        {
            string actual;
            string expected;

            actual = Utils.GetNomFichier("C:\\truc\\bidule\\test.1.2.3.xml");
            expected = "test.1.2.3";
            Assert.AreEqual<string>(expected, actual);

            actual = Utils.GetNomFichier("test.1.2.3.xml");
            expected = "test.1.2.3";
            Assert.AreEqual<string>(expected, actual);

            actual = Utils.GetNomFichier("C:\\truc\\bidule\\test123.xml");
            expected = "test123";
            Assert.AreEqual<string>(expected, actual);

            actual = Utils.GetNomFichier("test123.xml");
            expected = "test123";
            Assert.AreEqual<string>(expected, actual);

            actual = Utils.GetNomFichier("test123");
            expected = "test123";
            Assert.AreEqual<string>(expected, actual);

            actual = Utils.GetNomFichier("");
            expected = "";
            Assert.AreEqual<string>(expected, actual);
        }
    }
}
