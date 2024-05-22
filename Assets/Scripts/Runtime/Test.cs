namespace Runtime
{
    using FunctionBase.Extensions;
    using LocalData;
    using UnityEngine;
    using Zenject;

    public class Test : MonoBehaviour
    {
        [Inject] private TestLocalData testLocalData;
        private void Awake()
        {
            this.GetCurrentContainer().Inject(this);
        }


        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                var a = this.GetCurrentContainer().Resolve<TestLocalData>();
                this.testLocalData.check++;
                Debug.Log("Update local data "+ this.testLocalData.check);
            }
        }
    }
}