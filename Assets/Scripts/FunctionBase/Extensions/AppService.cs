namespace FunctionBase.Extensions
{
    using FunctionBase.LocalDataManager;
    using UnityEngine;
    using Zenject;

    public class AppService : MonoBehaviour
    {
        [Inject] private HandleLocalDataService handleLocalDataService;
        private void OnApplicationPause(bool pauseStatus)
        {
            this.handleLocalDataService.SaveAllData();
            Debug.Log("Save all data");
        }

        private void OnApplicationQuit()
        {
            this.handleLocalDataService.SaveAllData();
            Debug.Log("Save all data");
        }
    }
}