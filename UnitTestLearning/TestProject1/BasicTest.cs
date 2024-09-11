using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    internal class BasicTest
    {
        [TestMethod]
        public void PassingTest() => Assert.Equals(9, Multiply(3, 3));
        
        int Multiply(int a, int b)
        {
            return a * b;
        }

        bool IsEven(int value)
        {
            return value % 2 == 0;
        }
    }
}
