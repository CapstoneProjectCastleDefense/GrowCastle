namespace LocalData
{
    using System;
    using FunctionBase.LocalDataManager;

    public class TestLocalData : ILocalData<TestLocalDataController>
    {
        public int check = 1;
        public void Init()
        {

        }
    }
}