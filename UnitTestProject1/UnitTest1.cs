using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfApp1;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MainWindow window = new WpfApp1.MainWindow();
            window.Tents = new bool[3, 3] { { false,false,false},{false,false,false },{false,false,false } };
            Assert.AreEqual(window.IsAvailableForTent(2, 1, 0),true);
        }
        [TestMethod]
        public void TestMethod2()
        {
            MainWindow window = new WpfApp1.MainWindow();
            window.Tents = new bool[3, 3] { { true, false, false }, { false, false, false }, { false, false, false } };
            Assert.AreEqual(window.IsAvailableForTent(2, 2, 0), false);
        }
    }
}
