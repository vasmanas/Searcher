namespace Searcher.Core
{
    public class TreePartEndOf : TreePart
    {
        private readonly bool _exactMatch;

        public TreePartEndOf(bool exactMatch = false)
        {
            this._exactMatch = exactMatch;
        }

        public override int IsSatisfiedDeepBy(string value)
        {
            if (this._exactMatch && !this.IsSatisfiedTopBy(value))
            {
                return -1;
            }

            return 0;
        }

        public override bool IsSatisfiedTopBy(string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
