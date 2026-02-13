namespace IRG
{
    public abstract class Singleton<TSingleton> : Singleton
        where TSingleton : Singleton, new()
    {
        private static TSingleton _instance;

        public static TSingleton Instance
        {
            get
            {
                _instance ??= new TSingleton();
                _instance.Load();
                return _instance;
            }
        }
        
        public static TSingleton Data
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TSingleton();
                    _instance.Load();
                }
                return _instance;
            }
        }
    }

    public abstract class Singleton
    {
        public abstract void Load();
    }
}