using System;
using System.Threading.Tasks;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public class BeginMenuUI : MenuUI<IBeginMenuUI>, IBeginMenuUI
    {
        event Action IBeginMenuUI.OnNewGamePressed
        {
            add => m_OnNewGamePressed += value;
            remove => m_OnNewGamePressed -= value;
        }
        event Action IBeginMenuUI.OnLoadGamePressed
        {
            add => m_OnLoadGamePressed += value;
            remove => m_OnLoadGamePressed -= value;
        }
        event Action IBeginMenuUI.OnQuitPressed
        {
            add => m_OnQuitPressed += value;
            remove => m_OnQuitPressed -= value;
        }

        private event Action m_OnNewGamePressed;
        private event Action m_OnLoadGamePressed;
        private event Action m_OnQuitPressed;

        private Label       m_TitleLabel;
        private RPGUIButton m_NewGameBtn;
        private RPGUIButton m_LoadGameBtn;
        private RPGUIButton m_QuitBtn;
        private Label       m_VersionLabel;

        protected override VisualElement GetDefaultFocusedElement() => m_NewGameBtn;

        public BeginMenuUI(IBeginMenuLocalisationArgs localisationArgs,
                           IMenuUIProvider            uiProvider,
                           IAudioIntentPlayer         audioIntentPlayer,
                           ILocalisationService       localisationService) : base(localisationArgs, uiProvider, audioIntentPlayer, localisationService)
        {
        }

        protected override Task OnSuspendAsync(bool hideUi)
        {
            m_LocalisationService.UnloadLocalisationData(m_LocalisationArgs.DataSheetsToLoad);
            return base.OnSuspendAsync(hideUi);
        }

        protected override async Task OnResumeAsync()
        {
            await m_LocalisationService.LoadNewLocalisationDataAsync(m_LocalisationArgs.DataSheetsToLoad);
            await base.OnResumeAsync();
        }

        protected override void HookupUI()
        {
            m_TitleLabel   = m_UIInstance.Q<Label>("TitleLabel");
            m_NewGameBtn   = m_UIInstance.Q<RPGUIButton>("NewGameBtn");
            m_LoadGameBtn  = m_UIInstance.Q<RPGUIButton>("LoadGameBtn");
            m_QuitBtn      = m_UIInstance.Q<RPGUIButton>("QuitBtn");
            m_VersionLabel = m_UIInstance.Q<Label>("VersionLabel");

            m_VersionLabel.text = $"v{Application.version}";
        }

        protected override void LocaliseUI()
        {
            IBeginMenuLocalisationArgs args = (IBeginMenuLocalisationArgs)m_LocalisationArgs;

            m_TitleLabel.text  = m_LocalisationService.Get(args.GameTitle);
            m_NewGameBtn.text  = m_LocalisationService.Get(args.NewGame);
            m_LoadGameBtn.text = m_LocalisationService.Get(args.LoadGame);
            m_QuitBtn.text     = m_LocalisationService.Get(args.QuitGame);
        }

        protected override void RegisterCallbacks()
        {
            UIToolkitInputUtility.RegisterButtonCallbacks(m_NewGameBtn,  OnNewGameBtnNavigate,  OnNewGameBtnSubmitted,  OnNewGameBtnClicked);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_LoadGameBtn, OnLoadGameBtnNavigate, OnLoadGameBtnSubmitted, OnLoadGameBtnClicked);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_QuitBtn,     OnQuitBtnNavigate,     OnQuitBtnSubmitted,     OnQuitBtnClicked);
        }

        protected override void UnregisterCallbacks()
        {
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_QuitBtn,     OnQuitBtnNavigate,     OnQuitBtnSubmitted,     OnQuitBtnClicked);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_LoadGameBtn, OnLoadGameBtnNavigate, OnLoadGameBtnSubmitted, OnLoadGameBtnClicked);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_NewGameBtn,  OnNewGameBtnNavigate,  OnNewGameBtnSubmitted,  OnNewGameBtnClicked);
        }

        private void OnNewGameBtnNavigate(NavigationMoveEvent evt)
        {
            if (UIToolkitInputUtility.Navigate(evt, m_NewGameBtn, m_QuitBtn, m_LoadGameBtn))
            {
                OnBtnNavigate();
            }
        }

        private void OnNewGameBtnSubmitted(NavigationSubmitEvent evt)
        {
            OnNewGameBtnCallback();
        }

        private void OnNewGameBtnClicked(ClickEvent evt)
        {
            OnNewGameBtnCallback();
        }

        private void OnNewGameBtnCallback()
        {
            m_AudioIntentPlayer.Play(AudioIntent.Navigate, AudioContext.Menu);
            m_OnNewGamePressed?.Invoke();
        }

        private void OnLoadGameBtnNavigate(NavigationMoveEvent evt)
        {
            if (UIToolkitInputUtility.Navigate(evt, m_LoadGameBtn, m_NewGameBtn, m_QuitBtn))
            {
                OnBtnNavigate();
            }
        }

        private void OnLoadGameBtnSubmitted(NavigationSubmitEvent evt)
        {
            OnLoadGameBtnCallback();
        }

        private void OnLoadGameBtnClicked(ClickEvent evt)
        {
            OnLoadGameBtnCallback();
        }

        private void OnLoadGameBtnCallback()
        {
            m_AudioIntentPlayer.Play(AudioIntent.Navigate, AudioContext.Menu);
            m_OnLoadGamePressed?.Invoke();
        }

        private void OnQuitBtnNavigate(NavigationMoveEvent evt)
        {
            if (UIToolkitInputUtility.Navigate(evt, m_QuitBtn, m_LoadGameBtn, m_NewGameBtn))
            {
                OnBtnNavigate();
            }
        }

        private void OnQuitBtnSubmitted(NavigationSubmitEvent evt)
        {
            OnQuitBtnCallback();
        }

        private void OnQuitBtnClicked(ClickEvent evt)
        {
            OnQuitBtnCallback();
        }

        private void OnQuitBtnCallback()
        {
            m_AudioIntentPlayer.Play(AudioIntent.Cancel, AudioContext.Menu);
            m_OnQuitPressed?.Invoke();
        }
    }
}