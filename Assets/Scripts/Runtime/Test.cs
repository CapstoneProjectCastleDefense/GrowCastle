namespace Runtime
{
    using FunctionBase.Extensions;
    using LocalData;
    using UnityEngine;
    using Zenject;

    public class Test : MonoBehaviour
    {
        [Inject] private TestLocalDataController testLocalDataController;
        private void Awake()
        {
            this.GetCurrentContainer().Inject(this);
        }


        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                this.testLocalDataController.Add();
                Debug.Log("Update local data "+ this.testLocalDataController.Get());
            }
        }
    }
}