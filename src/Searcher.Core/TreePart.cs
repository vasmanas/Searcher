namespace Searcher.Core
{
    public abstract class TreePart
    {
        public abstract bool IsSatisfiedTopBy(string value);

        public abstract int IsSatisfiedDeepBy(string value);
    }
}
