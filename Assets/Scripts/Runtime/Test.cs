namespace Runtime
{
    using Blueprints;
    using FunctionBase.Extensions;
    using LocalData;
    using UnityEngine;
    using Zenject;

    public class Test : MonoBehaviour
    {
        [Inject] private TestLocalDataController testLocalDataController;
        [Inject] private TestBlueprint           testBlueprint;
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

            if (Input.GetKey(KeyCode.B))
            {
                foreach (var keyValuePair in this.testBlueprint.Data[1].LevelRecords)
                {
                    Debug.Log("Dm: "+keyValuePair.Value.Damage);
                }
            }
        }
    }
}