namespace IRG
{
    public abstract class SingletonData<TSingleton> : SingletonData
        where TSingleton : SingletonData, new()
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

    public abstract class SingletonData
    {
        public abstract void Load();
    }
}