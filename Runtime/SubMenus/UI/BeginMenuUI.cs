using System;
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
        event Action IBeginMenuUI.OnNewGame
        {
            add => m_OnNewGame += value;
            remove => m_OnNewGame -= value;
        }
        event Action IBeginMenuUI.OnLoadGame
        {
            add => m_OnLoadGame += value;
            remove => m_OnLoadGame -= value;
        }
        event Action IBeginMenuUI.OnSettings
        {
            add => m_OnSettings += value;
            remove => m_OnSettings -= value;
        }
        event Action IBeginMenuUI.OnQuit
        {
            add => m_OnQuit += value;
            remove => m_OnQuit -= value;
        }

        private event Action m_OnNewGame;
        private event Action m_OnLoadGame;
        private event Action m_OnSettings;
        private event Action m_OnQuit;

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
        { }

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
            UIToolkitInputUtility.RegisterButtonCallbacks(m_NewGameBtn,  OnNewGameBtnNavigate,  OnNewGameBtnSubmitted,  OnNewGameBtnClicked,  OnNewGameBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_LoadGameBtn, OnLoadGameBtnNavigate, OnLoadGameBtnSubmitted, OnLoadGameBtnClicked, OnLoadGameBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_SettingsBtn, OnSettingsBtnNavigate, OnSettingsBtnSubmitted, OnSettingsBtnClicked, OnSettingsBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_QuitBtn,     OnQuitBtnNavigate,     OnQuitBtnSubmitted,     OnQuitBtnClicked,     OnQuitBtnFocus);
        }

        protected override void UnregisterCallbacks()
        {
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_QuitBtn,     OnQuitBtnNavigate,     OnQuitBtnSubmitted,     OnQuitBtnClicked,     OnQuitBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_SettingsBtn, OnSettingsBtnNavigate, OnSettingsBtnSubmitted, OnSettingsBtnClicked, OnSettingsBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_LoadGameBtn, OnLoadGameBtnNavigate, OnLoadGameBtnSubmitted, OnLoadGameBtnClicked, OnLoadGameBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_NewGameBtn,  OnNewGameBtnNavigate,  OnNewGameBtnSubmitted,  OnNewGameBtnClicked,  OnNewGameBtnFocus);
        }

        private void OnNewGameBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_NewGameBtn, m_QuitBtn, m_LoadGameBtn);
        }

        private void OnNewGameBtnFocus(FocusInEvent evt)
        {
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonNavigate);
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
            m_OnNewGame?.Invoke();
        }

        private void OnLoadGameBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_LoadGameBtn, m_NewGameBtn, m_SettingsBtn);
        }

        private void OnLoadGameBtnFocus(FocusInEvent evt)
        {
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonNavigate);
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
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonPositive);
            m_OnLoadGame?.Invoke();
        }

        private void OnSettingsBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_SettingsBtn, m_LoadGameBtn, m_QuitBtn);
        }

        private void OnSettingsBtnFocus(FocusInEvent evt)
        {
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonNavigate);
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
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonPositive);
            m_OnSettings?.Invoke();
        }

        private void OnQuitBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_QuitBtn, m_SettingsBtn, m_NewGameBtn);
        }

        private void OnQuitBtnFocus(FocusInEvent evt)
        {
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonNavigate);
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
            m_OnQuit?.Invoke();
        }
    }
}