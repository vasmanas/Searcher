using System;

namespace Searcher.Core
{
    public class TreePartCondition : TreePart
    {
        private readonly Func<char, bool> _comparer;

        public TreePartCondition(char c)
        {
            this.Character = c;

            var lower = char.ToLower(this.Character);
            var upper = char.ToUpper(this.Character);

            if (lower == upper)
            {
                this._comparer = ch => ch == lower;
            }
            else
            {
                this._comparer = ch => ch == lower || ch == upper;
            }
        }

        public char Character { get; private set; }

        public TreePart Next { get; set; }

        public override int IsSatisfiedDeepBy(string value)
        {
            if (!this.IsSatisfiedTopBy(value))
            {
                return -1;
            }

            if (this.Next == null)
            {
                return 1;
            }

            var result = this.Next.IsSatisfiedDeepBy(value.Substring(1));

            if (result == -1)
            {
                return result;
            }

            return result + 1;
        }

        public override bool IsSatisfiedTopBy(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return this._comparer(value[0]);
        }
    }
}
