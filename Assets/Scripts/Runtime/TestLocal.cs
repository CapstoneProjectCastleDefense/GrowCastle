namespace Runtime
{
    using System;
    using FunctionBase.BlueprintManager;
    using FunctionBase.LocalDataManager;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;

    public class TestLocal : MonoBehaviour
    {
        [Inject] private readonly HandleLocalDataService handleLocalDataService;
        [Inject] private readonly BlueprintDataManager   blueprintDataManager;

        private void Start()
        {
            this.handleLocalDataService.LoadAllData();
            this.blueprintDataManager.LoadAllData();
            Debug.Log("Load all data");
            SceneManager.LoadScene("Scenes/MainScene");
        }
    }
}