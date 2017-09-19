using System.Linq;

namespace Searcher.Core
{
    public class TreeBuilder
    {
        public TreePart Create(string value)
        {
            return this.Create((TreePart)null, value);
        }

        public TreePart Create(TreePart t, string value)
        {
            if (t is null)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return new TreePartEndOf();
                }
                else
                {
                    return this.Create(new TreePartCondition(value[0]), value);
                }
            }

            return this.Create((dynamic)t, value);
        }

        protected TreePart Create(TreePartCollection t, string value)
        {
            if (t.IsSatisfiedDeepBy(value) >= 0)
            {
                return t;
            }

            var satisfiedLeaf = t.Leafs.FirstOrDefault(l => l.IsSatisfiedTopBy(value));

            TreePart newLeaf;
            if (satisfiedLeaf == null)
            {
                newLeaf = this.Create((TreePart)null, value);
            }
            else
            {
                newLeaf = this.Create((dynamic)satisfiedLeaf, value);

                t.Leafs.Remove(satisfiedLeaf);
            }

            t.Leafs.Add(newLeaf);

            return t;
        }

        protected TreePart Create(TreePartCondition t, string value)
        {
            if (t.IsSatisfiedTopBy(value))
            {
                t.Next = this.Create(t.Next, value.Substring(1));

                return t;
            }

            var part = new TreePartCollection();
            part.Leafs.Add(this.Create((TreePart)null, value));
            part.Leafs.Add(t);

            return part;
        }

        protected TreePart Create(TreePartEndOf t, string value)
        {
            if (t.IsSatisfiedTopBy(value))
            {
                return t;
            }

            var collection = new TreePartCollection();
            collection.Leafs.Add(this.Create(new TreePartCondition(value[0]), value));
            collection.Leafs.Add(t);

            return collection;
        }
    }
}
