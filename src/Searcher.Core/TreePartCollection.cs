using System;
using System.Collections.Generic;
using System.Linq;

namespace Searcher.Core
{
    public class TreePartCollection : TreePart
    {
        private readonly bool _findLongest;

        public TreePartCollection(bool findLongest = false)
        {
            this.Leafs = new List<TreePart>();
            this._findLongest = findLongest;
        }

        public List<TreePart> Leafs { get; private set; }

        public override int IsSatisfiedDeepBy(string value)
        {
            TreePartEndOf eof = null;

            var longestResult = -1;

            foreach (var leaf in this.Leafs)
            {
                if (leaf is TreePartEndOf)
                {
                    if (eof != null)
                    {
                        throw new Exception("Only one end part can be in collection");
                    }

                    eof = leaf as TreePartEndOf;

                    continue;
                }

                var result = leaf.IsSatisfiedDeepBy(value);
                
                if (this._findLongest)
                {
                    if (result >= 0 && longestResult < result)
                    {
                        longestResult = result;
                    }
                }
                else
                {
                    return result;
                }
            }

            if (longestResult > -1)
            {
                return longestResult;
            }

            if (eof is null)
            {
                return -1;
            }

            return eof.IsSatisfiedDeepBy(value);
        }

        public override bool IsSatisfiedTopBy(string value)
        {
            return this.Leafs.Any(l => l.IsSatisfiedTopBy(value));
        }
    }
}
