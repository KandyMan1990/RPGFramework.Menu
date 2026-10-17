using System;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
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

        event Action<Task> IBeginMenuUI.OnQuitPressed
        {
            add => m_OnQuitPressed += value;
            remove => m_OnQuitPressed -= value;
        }

        private event Action       m_OnNewGamePressed;
        private event Action       m_OnLoadGamePressed;
        private event Action<Task> m_OnQuitPressed;

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

        protected override void HookupUI()
        {
            m_TitleLabel   = m_UIInstance.Q<Label>("TitleLabel");
            m_NewGameBtn   = m_UIInstance.Q<RPGUIButton>("NewGameBtn");
            m_LoadGameBtn  = m_UIInstance.Q<RPGUIButton>("LoadGameBtn");
            m_QuitBtn      = m_UIInstance.Q<RPGUIButton>("QuitBtn");
            m_VersionLabel = m_UIInstance.Q<Label>("VersionLabel");

            m_VersionLabel.text = $"v{Application.version}";

            RPGUIButton.LinkColumn(new RPGUIButton[] { m_NewGameBtn, m_LoadGameBtn, m_QuitBtn }, true);

            m_NewGameBtn.Chosen  += OnNewGameChosen;
            m_LoadGameBtn.Chosen += OnLoadGameChosen;
            m_QuitBtn.Chosen     += OnQuitChosen;
        }

        protected override void LocaliseUI()
        {
            IBeginMenuLocalisationArgs args = (IBeginMenuLocalisationArgs)m_LocalisationArgs;

            m_TitleLabel.text  = m_LocalisationService.Get(args.GameTitle);
            m_NewGameBtn.text  = m_LocalisationService.Get(args.NewGame);
            m_LoadGameBtn.text = m_LocalisationService.Get(args.LoadGame);
            m_QuitBtn.text     = m_LocalisationService.Get(args.QuitGame);
        }

        private void OnNewGameChosen()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();
            m_OnNewGamePressed?.Invoke();
        }

        private void OnLoadGameChosen()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();
            m_OnLoadGamePressed?.Invoke();
        }

        private void OnQuitChosen()
        {
            Task cancelSound = m_AudioIntentPlayer.PlayAsync(AudioIntent.Cancel, AudioContext.Menu);

            m_OnQuitPressed?.Invoke(cancelSound);
        }
    }
}