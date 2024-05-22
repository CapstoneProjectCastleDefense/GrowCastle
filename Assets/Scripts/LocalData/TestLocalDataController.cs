namespace LocalData
{
    using FunctionBase.LocalDataManager;

    public class TestLocalDataController : ILocalDataController
    {
        private readonly TestLocalData testLocalData;

        public TestLocalDataController(TestLocalData testLocalData)
        {
            this.testLocalData = testLocalData;
        }

        public void Add()
        {
            this.testLocalData.check++;
        }

        public int Get() => this.testLocalData.check;

    }
}