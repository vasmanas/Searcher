using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Searcher.Core.Tests
{
    [TestClass]
    public class TreePartConditionTests
    {
        public const string Category = "TreePartCondition";

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Top_With_Null()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedTopBy(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Top_With_Empty()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedTopBy(string.Empty);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Top_With_Matching_Character()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedTopBy("A");

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Top_With_Matching_Character_Longer()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedTopBy("AB");

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Top_With_Non_Matching_Character()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedTopBy("B");

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Top_With_Non_Matching_Character_Longer()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedTopBy("BC");

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Null()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedDeepBy(null);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Empty()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedDeepBy(string.Empty);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Matching_Character()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedDeepBy("A");

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Non_Matching_Character()
        {
            var part = new TreePartCondition('A');

            var result = part.IsSatisfiedDeepBy("B");

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Non_Matching_Character_Longer()
        {
            var part = new TreePartCondition('A');
            var next = Substitute.For<TreePart>();

            part.Next = next;

            var result = part.IsSatisfiedDeepBy("BC");

            Assert.AreEqual(-1, result);
            next.DidNotReceive().IsSatisfiedDeepBy(Arg.Any<string>());
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Matching_Character_Longer_Match()
        {
            var part = new TreePartCondition('A');
            var next = Substitute.For<TreePart>();
            next.IsSatisfiedDeepBy(Arg.Is("B")).Returns(1);

            part.Next = next;

            var result = part.IsSatisfiedDeepBy("AB");

            Assert.AreEqual(2, result);
            next.Received(1).IsSatisfiedDeepBy(Arg.Is("B"));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Check_Deep_With_Matching_Character_Longer_Non_Match()
        {
            var part = new TreePartCondition('A');
            var next = Substitute.For<TreePart>();
            next.IsSatisfiedDeepBy(Arg.Is("B")).Returns(-1);

            part.Next = next;

            var result = part.IsSatisfiedDeepBy("AB");

            Assert.AreEqual(-1, result);
            next.Received(1).IsSatisfiedDeepBy(Arg.Is("B"));
        }
    }
}
