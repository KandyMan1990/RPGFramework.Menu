using System;
using System.Threading.Tasks;
using RPGFramework.Audio;
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
        event Action IBeginMenuUI.OnSettingsPressed
        {
            add => m_OnSettingsPressed += value;
            remove => m_OnSettingsPressed -= value;
        }
        event Action IBeginMenuUI.OnQuitPressed
        {
            add => m_OnQuitPressed += value;
            remove => m_OnQuitPressed -= value;
        }

        private event Action m_OnNewGamePressed;
        private event Action m_OnLoadGamePressed;
        private event Action m_OnSettingsPressed;
        private event Action m_OnQuitPressed;

        private Label       m_TitleLabel;
        private RPGUIButton m_NewGameBtn;
        private RPGUIButton m_LoadGameBtn;
        private RPGUIButton m_SettingsBtn;
        private RPGUIButton m_QuitBtn;
        private Label       m_VersionLabel;

        protected override VisualElement GetDefaultFocusedElement() => m_NewGameBtn;

        public BeginMenuUI(IBeginMenuLocalisationArgs localisationArgs,
                           IMenuUIProvider            uiProvider,
                           IGenericAudioIdProvider    audioIdProvider,
                           ILocalisationService       localisationService) : base(localisationArgs,
                                                                                  uiProvider,
                                                                                  audioIdProvider,
                                                                                  localisationService)
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
            m_SettingsBtn  = m_UIInstance.Q<RPGUIButton>("SettingsBtn");
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
            m_SettingsBtn.text = m_LocalisationService.Get(args.Settings);
            m_QuitBtn.text     = m_LocalisationService.Get(args.QuitGame);
        }

        protected override void RegisterCallbacks()
        {
            UIToolkitInputUtility.RegisterButtonCallbacks(m_NewGameBtn,  OnNewGameBtnNavigate,  OnNewGameBtnSubmitted,  OnNewGameBtnClicked,  OnBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_LoadGameBtn, OnLoadGameBtnNavigate, OnLoadGameBtnSubmitted, OnLoadGameBtnClicked, OnBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_SettingsBtn, OnSettingsBtnNavigate, OnSettingsBtnSubmitted, OnSettingsBtnClicked, OnBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_QuitBtn,     OnQuitBtnNavigate,     OnQuitBtnSubmitted,     OnQuitBtnClicked,     OnBtnFocus);
        }

        protected override void UnregisterCallbacks()
        {
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_QuitBtn,     OnQuitBtnNavigate,     OnQuitBtnSubmitted,     OnQuitBtnClicked,     OnBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_SettingsBtn, OnSettingsBtnNavigate, OnSettingsBtnSubmitted, OnSettingsBtnClicked, OnBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_LoadGameBtn, OnLoadGameBtnNavigate, OnLoadGameBtnSubmitted, OnLoadGameBtnClicked, OnBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_NewGameBtn,  OnNewGameBtnNavigate,  OnNewGameBtnSubmitted,  OnNewGameBtnClicked,  OnBtnFocus);
        }

        private void OnNewGameBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_NewGameBtn, m_QuitBtn, m_LoadGameBtn);
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
            //TODO: temporarily use item consumed until we have new game sfx
            RaiseOnPlayAudio(m_AudioIdProvider.ItemConsumed);
            m_OnNewGamePressed?.Invoke();
        }

        private void OnLoadGameBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_LoadGameBtn, m_NewGameBtn, m_SettingsBtn);
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
            m_LastFocusedElement = m_LoadGameBtn;
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonPositive);
            m_OnLoadGamePressed?.Invoke();
        }

        private void OnSettingsBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_SettingsBtn, m_LoadGameBtn, m_QuitBtn);
        }

        private void OnSettingsBtnSubmitted(NavigationSubmitEvent evt)
        {
            OnSettingsBtnCallback();
        }

        private void OnSettingsBtnClicked(ClickEvent evt)
        {
            OnSettingsBtnCallback();
        }

        private void OnSettingsBtnCallback()
        {
            m_LastFocusedElement = m_SettingsBtn;
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonPositive);
            m_OnSettingsPressed?.Invoke();
        }

        private void OnQuitBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_QuitBtn, m_SettingsBtn, m_NewGameBtn);
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
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonNegative);
            m_OnQuitPressed?.Invoke();
        }
    }
}