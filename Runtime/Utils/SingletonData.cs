namespace IRG
{
    public abstract class SingletonData<TSingleton>
        where TSingleton : SingletonData<TSingleton>, new()
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
        
        public abstract void Load();
    }
}