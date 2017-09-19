using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Searcher.Core.Tests
{
    [TestClass]
    public class TreePartCollectionTests
    {
        public const string Category = "TreePartCollection";

        [TestMethod]
        [TestCategory(Category)]
        public void Empty_Collection_Check_Top_With_Null()
        {
            var part = new TreePartCollection();

            var result = part.IsSatisfiedTopBy(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Empty_Collection_Check_Top_With_Empty()
        {
            var part = new TreePartCollection();

            var result = part.IsSatisfiedTopBy(string.Empty);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Empty_Collection_Check_Top_With_Value()
        {
            var part = new TreePartCollection();

            var result = part.IsSatisfiedTopBy("ABC");

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Collection_Check_Top_With_Valid_Value()
        {
            var part = new TreePartCollection();
            var leaf = Substitute.For<TreePart>();
            leaf.IsSatisfiedTopBy(Arg.Is("ABC")).Returns(true);
            part.Leafs.Add(leaf);

            var result = part.IsSatisfiedTopBy("ABC");

            Assert.IsTrue(result);
            leaf.Received(1).IsSatisfiedTopBy(Arg.Is("ABC"));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Collection_Check_Top_With_Invalid_Value()
        {
            var part = new TreePartCollection();
            var leaf = Substitute.For<TreePart>();
            leaf.IsSatisfiedTopBy(Arg.Is("ABC")).Returns(false);
            part.Leafs.Add(leaf);

            var result = part.IsSatisfiedTopBy("ABC");

            Assert.IsFalse(result);
            leaf.Received(1).IsSatisfiedTopBy(Arg.Is("ABC"));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Collection_Check_Top_With_One_Of_Valid_Value()
        {
            var part = new TreePartCollection();

            var leaf1 = Substitute.For<TreePart>();
            leaf1.IsSatisfiedTopBy(Arg.Is("ABC")).Returns(false);
            part.Leafs.Add(leaf1);

            var leaf2 = Substitute.For<TreePart>();
            leaf2.IsSatisfiedTopBy(Arg.Is("ABC")).Returns(true);
            part.Leafs.Add(leaf2);

            var result = part.IsSatisfiedTopBy("ABC");

            Assert.IsTrue(result);
            leaf1.Received(1).IsSatisfiedTopBy(Arg.Is("ABC"));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Deep_Empty_Collection_Check_Top_With_Null()
        {
            var part = new TreePartCollection();

            var result = part.IsSatisfiedDeepBy(null);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Deep_Empty_Collection_Check_Top_With_Empty()
        {
            var part = new TreePartCollection();

            var result = part.IsSatisfiedDeepBy(string.Empty);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Deep_Empty_Collection_Check_Top_With_Value()
        {
            var part = new TreePartCollection();

            var result = part.IsSatisfiedDeepBy("ABC");

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Deep_Collection_Check_Top_With_Valid_Value()
        {
            var part = new TreePartCollection();
            var leaf = Substitute.For<TreePart>();
            leaf.IsSatisfiedDeepBy(Arg.Is("ABC")).Returns(3);
            part.Leafs.Add(leaf);

            var result = part.IsSatisfiedDeepBy("ABC");

            Assert.AreEqual(3, result);
            leaf.Received(1).IsSatisfiedDeepBy(Arg.Is("ABC"));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Deep_Collection_Check_Top_With_Invalid_Value()
        {
            var part = new TreePartCollection();
            var leaf = Substitute.For<TreePart>();
            leaf.IsSatisfiedDeepBy(Arg.Is("ABC")).Returns(-1);
            part.Leafs.Add(leaf);

            var result = part.IsSatisfiedDeepBy("ABC");

            Assert.AreEqual(-1, result);
            leaf.Received(1).IsSatisfiedDeepBy(Arg.Is("ABC"));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Deep_Collection_Check_Top_With_One_Of_Valid_Value()
        {
            var part = new TreePartCollection(true);

            var leaf1 = Substitute.For<TreePart>();
            leaf1.IsSatisfiedDeepBy(Arg.Is("ABC")).Returns(-1);
            part.Leafs.Add(leaf1);

            var leaf2 = Substitute.For<TreePart>();
            leaf2.IsSatisfiedDeepBy(Arg.Is("ABC")).Returns(3);
            part.Leafs.Add(leaf2);

            var result = part.IsSatisfiedDeepBy("ABC");

            Assert.AreEqual(3, result);
            leaf1.Received(1).IsSatisfiedDeepBy(Arg.Is("ABC"));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Deep_Check_For_Longest_Solution()
        {
            var part = new TreePartCollection(true);

            var leaf1 = Substitute.For<TreePart>();
            leaf1.IsSatisfiedDeepBy(Arg.Any<string>()).Returns(2);
            part.Leafs.Add(leaf1);

            var leaf2 = Substitute.For<TreePart>();
            leaf2.IsSatisfiedDeepBy(Arg.Any<string>()).Returns(3);
            part.Leafs.Add(leaf2);

            var leaf3 = Substitute.For<TreePart>();
            leaf3.IsSatisfiedDeepBy(Arg.Any<string>()).Returns(1);
            part.Leafs.Add(leaf3);

            var result = part.IsSatisfiedDeepBy("A");

            Assert.AreEqual(3, result);
        }
    }
}
