namespace DataLayerGameOfLife
{
    public class RepositoryFactory
    {
        // 1. The private static field that holds the single instance
        private static RepositoryFactory _instance;

        // 2. The public property used to access the factory (Singleton)
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

        // 3. A private constructor so no one else can use 'new RepositoryFactory()'
        private RepositoryFactory() { }

        // 4. The method that creates and returns your repository
        // It returns the Interface (IInitialStateRepository) to satisfy UML requirements
        public IInitialStateRepository GetInitialStateRepository()
        {
            return new GridRepository();
        }
    }
}