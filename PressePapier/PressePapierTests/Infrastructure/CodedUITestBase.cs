using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PressePapierTests.Infrastructure
{
    public abstract class CodedUITestBase<T> where T : ICodedUIApplication, new()
    {
        [TestInitialize]
        public void Setup()
        {
            Application = new T();
            Application.Start();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Application != null)
                Application.Close();
        }

        protected T Application { get; private set; }
    }
}