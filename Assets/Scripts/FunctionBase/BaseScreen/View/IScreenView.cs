namespace FunctionBase.BaseScreen.View
{
    using System;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Playables;

    public interface IScreenView
    {
        bool         IsReady      { get; }
        ScreenStatus ScreenStatus { get; }
        UniTask      Open();
        UniTask      Close();
        void         Hide();
        void         Show();
    }

    public class ScreenView : MonoBehaviour, IScreenView
    {
        [field: SerializeField] private PlayableDirector IntroPlayableDirector { get; set; }
        [field: SerializeField] private PlayableDirector OutroPlayableDirector { get; set; }

        public bool         IsReady      { get; private set; }
        public ScreenStatus ScreenStatus { get; private set; }

        private void Awake() { this.IsReady = true; }
        public async UniTask Open()
        {
            this.IsReady      = true;
            this.ScreenStatus = ScreenStatus.Opened;
            await this.PlayIntro();
        }

        public async UniTask Close()
        {
            this.IsReady      = false;
            this.ScreenStatus = ScreenStatus.Closed;
            await this.PlayOutro();
        }

        public void Hide()
        {
            this.ScreenStatus                      = ScreenStatus.Hide;
            this.GetComponent<CanvasGroup>().alpha = 0;
        }

        public void Show()
        {
            this.ScreenStatus                      = ScreenStatus.Opened;
            this.GetComponent<CanvasGroup>().alpha = 1;
        }

        private UniTask PlayIntro() { return this.PlayTransition(this.IntroPlayableDirector); }

        private UniTask PlayOutro() { return this.PlayTransition(this.OutroPlayableDirector); }

        private UniTask PlayTransition(PlayableDirector playableDirector)
        {
            if (playableDirector == null)
            {
                return UniTask.CompletedTask;
            }

            playableDirector.Play();
            return UniTask.WaitUntil(() => playableDirector.state != PlayState.Playing);
        }
    }

    public enum ScreenStatus
    {
        Opened,
        Closed,
        Hide
    }
}