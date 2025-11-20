using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Input;
using RPGFramework.Core.UI;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public class BeginMenuUI : MenuUI<IBeginMenu>, IBeginMenuUI
    {
        private RPGUIButton m_NewGameBtn;
        private RPGUIButton m_LoadGameBtn;
        private RPGUIButton m_SettingsBtn;
        private RPGUIButton m_QuitBtn;

        protected override VisualElement ElementToFocusOnEnter => m_NewGameBtn;

        public BeginMenuUI(IMenuUIProvider uiProvider) : base(uiProvider)
        {

        }

        protected override Task OnEnterAsync(VisualElement rootContainer)
        {
            m_RootUI      = rootContainer.Q<VisualElement>("BeginMenu");
            m_NewGameBtn  = m_RootUI.Q<RPGUIButton>("NewGameBtn");
            m_LoadGameBtn = m_RootUI.Q<RPGUIButton>("LoadGameBtn");
            m_SettingsBtn = m_RootUI.Q<RPGUIButton>("SettingsBtn");
            m_QuitBtn     = m_RootUI.Q<RPGUIButton>("QuitBtn");

            return Task.CompletedTask;
        }

        protected override Task OnSuspendAsync()
        {
            return Task.CompletedTask;
        }

        protected override Task OnResumeAsync()
        {
            return Task.CompletedTask;
        }

        protected override Task OnExitAsync()
        {
            return Task.CompletedTask;
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
            // TODO: pass in specific id
            RaiseOnPlayAudio(0);
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
            UnityEngine.Debug.Log("OnNewGameBtnCallback");
            RaiseOnPlayAudio(0);
        }

        private void OnLoadGameBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_LoadGameBtn, m_NewGameBtn, m_SettingsBtn);
        }

        private void OnLoadGameBtnFocus(FocusInEvent evt)
        {
            // TODO: pass in specific id
            RaiseOnPlayAudio(0);
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
            UnityEngine.Debug.Log("OnLoadGameBtnCallback");
            RaiseOnPlayAudio(0);
        }

        private void OnSettingsBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_SettingsBtn, m_LoadGameBtn, m_QuitBtn);
        }

        private void OnSettingsBtnFocus(FocusInEvent evt)
        {
            // TODO: pass in specific id
            RaiseOnPlayAudio(0);
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
            UnityEngine.Debug.Log("OnSettingsBtnCallback");
            RaiseOnPlayAudio(0);
        }

        private void OnQuitBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_QuitBtn, m_SettingsBtn, m_NewGameBtn);
        }

        private void OnQuitBtnFocus(FocusInEvent evt)
        {
            // TODO: pass in specific id
            RaiseOnPlayAudio(0);
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
            UnityEngine.Debug.Log("OnQuitBtnCallback");
            RaiseOnPlayAudio(0);
        }
    }
}