namespace FunctionBase.ScreenManager
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using FunctionBase.AssetsManager;
    using FunctionBase.Attribute;
    using FunctionBase.BaseScreen.Presenter;
    using FunctionBase.BaseScreen.View;
    using FunctionBase.Extensions;
    using R3;
    using Unity.VisualScripting;
    using UnityEngine;
    using Zenject;

    public interface IScreenManager
    {
        UniTask<T> OpenScreen<T>() where T : IScreenPresenter;
        UniTask<T> CloseScreen<T>() where T : IScreenPresenter;
        UniTask<T> OpenScreenOverlay<T>();

        UniTask CloseAllScreens();
        UniTask CloseCurrentScreen();

        public RootUICanvas RootUICanvas { get; set; }
    }

    public class ScreenManager : MonoBehaviour, IScreenManager
    {
        private List<IScreenPresenter>                      activeScreenPresenters;
        private Dictionary<Type, IScreenPresenter>          typeToLoadedScreenPresenters;
        private Dictionary<Type, UniTask<IScreenPresenter>> typeToPendingScreenPresenters;

        public Transform    CurrentRootScreen  { get; set; }
        public Transform    CurrentHiddenRoot  { get; set; }
        public Transform    CurrentOverlayRoot { get; set; }
        public RootUICanvas RootUICanvas       { get; set; }

        private GameAssetsManager gameAssets;

        [Inject]
        public void Init(GameAssetsManager gameAssetsManager) { this.gameAssets = gameAssetsManager; }

        public async UniTask<T> OpenScreen<T>() where T : IScreenPresenter
        {
            var nextScreen = await this.GetScreen<T>();

            if (nextScreen != null)
            {
                await nextScreen.OpenViewAsync();

                return nextScreen;
            }
            else
            {
                Debug.LogError($"The {typeof(T).Name} screen does not exist");
                return default;
            }
        }

        private async UniTask<T> GetScreen<T>() where T : IScreenPresenter
        {
            var screenType = typeof(T);

            if (this.typeToLoadedScreenPresenters.TryGetValue(screenType, out var screenPresenter)) return (T)screenPresenter;

            if (!this.typeToPendingScreenPresenters.TryGetValue(screenType, out var loadingTask))
            {
                loadingTask = InstantiateScreen();
                this.typeToPendingScreenPresenters.Add(screenType, loadingTask);
            }

            var result = await loadingTask;
            this.typeToPendingScreenPresenters.Remove(screenType);

            return (T)result;

            async UniTask<IScreenPresenter> InstantiateScreen()
            {
                screenPresenter = this.GetCurrentContainer().Instantiate<T>();
                var screenInfo = screenPresenter.GetCustomAttribute<ScreenInfoAttribute>();

                var viewObject = Instantiate(await this.gameAssets.LoadAssetAsync<GameObject>(screenInfo.AddressableScreenPath, ""),
                    this.CheckPopupIsOverlay(screenPresenter) ? this.CurrentOverlayRoot : this.CurrentRootScreen).GetComponent<IScreenView>();

                screenPresenter.SetView(viewObject);
                this.typeToLoadedScreenPresenters.Add(screenType, screenPresenter);

                return (T)screenPresenter;
            }
        }

        private bool CheckScreenIsPopup(IScreenPresenter screenPresenter)  { return screenPresenter.GetType().IsSubclassOfRawGeneric(typeof(PopupPresenter<>)); }
        private bool CheckPopupIsOverlay(IScreenPresenter screenPresenter) { return this.CheckScreenIsPopup(screenPresenter) && screenPresenter.GetCustomAttribute<PopupInfoAttribute>().IsOverlay; }

        public UniTask<T> CloseScreen<T>() where T : IScreenPresenter { throw new System.NotImplementedException(); }
        public UniTask<T> OpenScreenOverlay<T>()                      { throw new System.NotImplementedException(); }
        public UniTask    CloseAllScreens()                           { throw new System.NotImplementedException(); }
        public UniTask    CloseCurrentScreen()                        { throw new System.NotImplementedException(); }
    }
}