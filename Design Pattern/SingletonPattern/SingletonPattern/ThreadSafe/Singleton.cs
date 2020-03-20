namespace SingletonPattern.ThreadSafe
{
    public sealed class Singleton
    {
        private static Singleton _instance;
        private static readonly object LockHelper = new object();

        private Singleton()
        {
        }

        public static Singleton Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (LockHelper)
                {
                    _instance = _instance ?? new Singleton();
                }

                return _instance;
            }
        }
    }
}
