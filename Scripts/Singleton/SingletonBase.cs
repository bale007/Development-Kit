namespace Bale007
{
    public abstract class SingletonBase<T> where T : SingletonBase<T>, new()
    {
        private static readonly T instance = new T();

        public static T Instance
        {
            get { return instance; }
        }
    }
}