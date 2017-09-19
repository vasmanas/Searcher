using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Searcher.Core.Tests
{
    [TestClass]
    public class TreeBuilderTests
    {
        public const string Category = "TreeBuilder";

        [TestMethod]
        [TestCategory(Category)]
        public void Build_Null_Tree()
        {
            var builder = new TreeBuilder();

            string value = null;

            var tree = builder.Create(value);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOfType(tree, typeof(TreePartEndOf));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Build_Empty_Tree()
        {
            var builder = new TreeBuilder();

            var value = string.Empty;

            var tree = builder.Create(value);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOfType(tree, typeof(TreePartEndOf));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Build_One_Character_Tree()
        {
            var builder = new TreeBuilder();

            var value = "A";

            var tree = builder.Create(value);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOfType(tree, typeof(TreePartCondition));
            Assert.IsInstanceOfType(((TreePartCondition)tree).Next, typeof(TreePartEndOf));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Build_Multiple_Characters_Tree()
        {
            var builder = new TreeBuilder();

            var value = "ABC";

            var tree = builder.Create(value);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOfType(tree, typeof(TreePartCondition));

            var next = ((TreePartCondition)tree).Next;
            Assert.IsInstanceOfType(next, typeof(TreePartCondition));

            next = ((TreePartCondition)next).Next;
            Assert.IsInstanceOfType(next, typeof(TreePartCondition));

            next = ((TreePartCondition)next).Next;
            Assert.IsInstanceOfType(next, typeof(TreePartEndOf));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Add_Same_Character_To_Tree()
        {
            var builder = new TreeBuilder();

            var value = "A";

            var tree = builder.Create(value);
            var tree2 = builder.Create(tree, value);

            Assert.IsNotNull(tree);
            Assert.IsNotNull(tree2);
            Assert.ReferenceEquals(tree, tree2);
            Assert.IsInstanceOfType(tree2, typeof(TreePartCondition));
            Assert.IsInstanceOfType(((TreePartCondition)tree2).Next, typeof(TreePartEndOf));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Build_Tree_Add_A_Then_B_Then_C()
        {
            var builder = new TreeBuilder();

            var value1 = "A";
            var value2 = "B";
            var value3 = "C";

            var tree = builder.Create(value1);
            tree = builder.Create(tree, value2);
            tree = builder.Create(tree, value3);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOfType(tree, typeof(TreePartCollection));
            Assert.AreEqual(3, ((TreePartCollection)tree).Leafs.Count);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Build_Tree_Add_1_Then_19()
        {
            var builder = new TreeBuilder();

            var value1 = "1";
            var value2 = "19";

            var tree = builder.Create(value1);
            tree = builder.Create(tree, value2);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOfType(tree, typeof(TreePartCondition));

            var next = ((TreePartCondition)tree).Next;

            Assert.IsNotNull(next);
            Assert.IsInstanceOfType(next, typeof(TreePartCollection));
            Assert.AreEqual(2, ((TreePartCollection)next).Leafs.Count);

            var leafs = ((TreePartCollection)next).Leafs;

            Assert.IsTrue(leafs.Any(l => l is TreePartCondition));
            Assert.IsTrue(leafs.Any(l => l is TreePartEndOf));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Build_Tree_Add_19_Then_1()
        {
            var builder = new TreeBuilder();

            var value1 = "19";
            var value2 = "1";

            var tree = builder.Create(value1);
            tree = builder.Create(tree, value2);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOfType(tree, typeof(TreePartCondition));

            var next = ((TreePartCondition)tree).Next;

            Assert.IsNotNull(next);
            Assert.IsInstanceOfType(next, typeof(TreePartCollection));
            Assert.AreEqual(2, ((TreePartCollection)next).Leafs.Count);

            var leafs = ((TreePartCollection)next).Leafs;

            Assert.IsTrue(leafs.Any(l => l is TreePartCondition));
            Assert.IsTrue(leafs.Any(l => l is TreePartEndOf));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Build_Tree_Add_AB_Then_AC_Then_AD()
        {
            var builder = new TreeBuilder();

            var value1 = "AB";
            var value2 = "AC";
            var value3 = "AD";

            var tree = builder.Create(value1);
            tree = builder.Create(tree, value2);
            tree = builder.Create(tree, value3);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOfType(tree, typeof(TreePartCondition));

            var next = ((TreePartCondition)tree).Next;

            Assert.IsNotNull(next);
            Assert.IsInstanceOfType(next, typeof(TreePartCollection));
            Assert.AreEqual(3, ((TreePartCollection)next).Leafs.Count);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Build_Tree_Add_ABC_Then_ABD_Then_AE()
        {
            var builder = new TreeBuilder();

            var value1 = "ABC";
            var value2 = "ABD";
            var value3 = "AE";

            var tree = builder.Create(value1);
            tree = builder.Create(tree, value2);
            tree = builder.Create(tree, value3);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOfType(tree, typeof(TreePartCondition));

            var next = ((TreePartCondition)tree).Next;

            Assert.IsNotNull(next);
            Assert.IsInstanceOfType(next, typeof(TreePartCollection));
            Assert.AreEqual(2, ((TreePartCollection)next).Leafs.Count);
        }
    }
}
