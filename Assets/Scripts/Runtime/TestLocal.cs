namespace Runtime
{
    using System;
    using FunctionBase.LocalDataManager;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;

    public class TestLocal : MonoBehaviour
    {
        [Inject] private readonly HandleLocalDataService handleLocalDataService;

        private void Start()
        {
            this.handleLocalDataService.LoadAllData();
            Debug.Log("Load all data");
            SceneManager.LoadScene("Scenes/MainScene");
        }

    }
}