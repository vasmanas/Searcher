using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Searcher.Core.Tests
{
    [TestClass]
    public class TreePartEndOfTests
    {
        public const string Category = "TreePartEndOf";

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Top_With_Null()
        {
            var part = new TreePartEndOf();

            var result = part.IsSatisfiedTopBy(null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Top_With_Empty()
        {
            var part = new TreePartEndOf();

            var result = part.IsSatisfiedTopBy(string.Empty);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Top_With_Something()
        {
            var part = new TreePartEndOf();

            var result = part.IsSatisfiedTopBy("AA");

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Null()
        {
            var part = new TreePartEndOf();

            var result = part.IsSatisfiedDeepBy(null);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Empty()
        {
            var part = new TreePartEndOf();

            var result = part.IsSatisfiedDeepBy(string.Empty);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Something()
        {
            var part = new TreePartEndOf();

            var result = part.IsSatisfiedDeepBy("AA");

            Assert.AreEqual(0, result);
        }
    }
}
