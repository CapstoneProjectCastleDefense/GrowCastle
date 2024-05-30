namespace FunctionBase.ScreenManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using FunctionBase.AssetsManager;
    using FunctionBase.Attribute;
    using FunctionBase.BaseScreen.Presenter;
    using FunctionBase.BaseScreen.View;
    using FunctionBase.Extensions;
    using R3;
    using UnityEngine;
    using Zenject;

    public interface IScreenManager
    {
        UniTask<T> OpenScreen<T>() where T : IScreenPresenter;
        UniTask<T> CloseScreen<T>() where T : IScreenPresenter;

        UniTask CloseAllScreens();
        UniTask CloseCurrentScreen();

        public RootUICanvas RootUICanvas { get; set; }
    }

    public class ScreenManager : MonoBehaviour, IScreenManager
    {
        private Dictionary<Type, IScreenPresenter>          typeToLoadedScreenPresenters;
        private Dictionary<Type, UniTask<IScreenPresenter>> typeToPendingScreenPresenters;
        private List<IScreenPresenter>                      activeScreenPresenters;

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
                this.activeScreenPresenters.Add(nextScreen);

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

        public async UniTask<T> CloseScreen<T>() where T : IScreenPresenter
        {
            var screenType      = typeof(T);
            var screenPresenter = this.typeToLoadedScreenPresenters.GetValueOrDefault(screenType);
            if (screenPresenter != null) await screenPresenter.CloseViewAsync();

            return default;
        }
        public UniTask CloseAllScreens()
        {
            var cacheActiveScreens = this.activeScreenPresenters.ToList();
            this.activeScreenPresenters.Clear();

            foreach (var screen in cacheActiveScreens)
            {
                screen.CloseViewAsync();
            }

            return UniTask.CompletedTask;
        }
        public async UniTask CloseCurrentScreen()
        {
            if (this.activeScreenPresenters.Count == 0) return;

            var currentScreen = this.activeScreenPresenters.Last();
            await currentScreen.CloseViewAsync();
            this.activeScreenPresenters.Remove(currentScreen);
            await this.activeScreenPresenters.Last().OpenViewAsync();
        }
    }
}