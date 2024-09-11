using System;
using System.Linq;
using UnitTestLearning.Models;

namespace UnitTestLearning.Tests
{
    public class TestProductDbSet : TestDbSet<Product>
    {
        public override Product Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.Id == (int)keyValues.Single());
        }
    }
}
