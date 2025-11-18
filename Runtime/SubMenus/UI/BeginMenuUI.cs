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
            UIToolkitInputUtility.RegisterButtonCallbacks(m_NewGameBtn,  OnNewGameBtnNavigate,  OnNewGameBtnSubmitted,  OnNewGameBtnClicked);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_LoadGameBtn, OnLoadGameBtnNavigate, OnLoadGameBtnSubmitted, OnLoadGameBtnClicked);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_SettingsBtn, OnSettingsBtnNavigate, OnSettingsBtnSubmitted, OnSettingsBtnClicked);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_QuitBtn,     OnQuitBtnNavigate,     OnQuitBtnSubmitted,     OnQuitBtnClicked);
        }

        protected override void UnregisterCallbacks()
        {
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_QuitBtn,     OnQuitBtnNavigate,     OnQuitBtnSubmitted,     OnQuitBtnClicked);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_SettingsBtn, OnSettingsBtnNavigate, OnSettingsBtnSubmitted, OnSettingsBtnClicked);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_LoadGameBtn, OnLoadGameBtnNavigate, OnLoadGameBtnSubmitted, OnLoadGameBtnClicked);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_NewGameBtn,  OnNewGameBtnNavigate,  OnNewGameBtnSubmitted,  OnNewGameBtnClicked);
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
            UnityEngine.Debug.Log("OnNewGameBtnCallback");
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
            UnityEngine.Debug.Log("OnLoadGameBtnCallback");
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
            UnityEngine.Debug.Log("OnSettingsBtnCallback");
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
            UnityEngine.Debug.Log("OnQuitBtnCallback");
        }
    }
}