namespace DataLayerGameOfLife
{
    public class RepositoryFactory
    {
        // 1. Nullable because lazy initialization starts with null
        private static RepositoryFactory? _instance;

        // 2. Singleton accessor
        public static RepositoryFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RepositoryFactory();
                }
                return _instance;
            }
        }

        // 3. Private constructor
        private RepositoryFactory() { }

        // 4. Factory method
        public IInitialStateRepository GetInitialStateRepository()
        {
            return new GridRepository();
        }
    }
}
