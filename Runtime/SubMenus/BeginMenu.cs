using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Store;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu.SubMenus
{
    internal sealed class BeginMenu : Menu<IBeginMenuUI>, IBeginMenu
    {
        protected override bool m_HidePreviousUIOnSuspend => true;

        private readonly ILocalisationService m_LocalisationService;
        private readonly ISaveDataService     m_SaveDataService;
        private readonly ISettingsStore       m_SettingsStore;
        private readonly IMusicPlayer         m_MusicPlayer;
        private readonly ISfxPlayer           m_SfxPlayer;
        private readonly IPlayTimeStore       m_PlayTimeStore;

        public BeginMenu(ILocalisationService localisationService,
                         ISaveDataService     saveDataService,
                         ISettingsStore       settingsStore,
                         IMusicPlayer         musicPlayer,
                         ISfxPlayer           sfxPlayer,
                         IPlayTimeStore       playTimeStore,
                         IBeginMenuUI         beginMenuUI,
                         IInputRouter         inputRouter,
                         IMenuModule          menuModule,
                         IAudioIntentPlayer   audioIntentPlayer) : base(beginMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_LocalisationService = localisationService;
            m_SaveDataService     = saveDataService;
            m_SettingsStore       = settingsStore;
            m_MusicPlayer         = musicPlayer;
            m_SfxPlayer           = sfxPlayer;
            m_PlayTimeStore       = playTimeStore;

            m_MenuUI.OnNewGamePressed  += OnNewGamePressed;
            m_MenuUI.OnLoadGamePressed += OnLoadGamePressed;
            m_MenuUI.OnConfigPressed   += OnConfigPressed;
            m_MenuUI.OnQuitPressed     += OnQuitPressed;

#if ENABLE_RPG_FRAMEWORK_DEBUG
            // A game's own title UI may not offer the room.
            if (m_MenuUI is IDebugBeginMenuUI debugUI)
            {
                debugUI.OnDebugRoomPressed += OnDebugRoomPressed;
            }
#endif
        }

        protected override async Task OnEnterAsync(Dictionary<string, object> args)
        {
            await ApplySettingsAsync();

            await base.OnEnterAsync(args);
        }

        protected override async Task OnEnterComplete()
        {
            m_PlayTimeStore.StopCounting();

            if (!m_SettingsStore.IsLanguageChosen)
            {
                await m_MenuModule.PushMenu(MenuType.Language);
            }

            await base.OnEnterComplete();
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            return false;
        }

        private void OnNewGamePressed()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.NewGame, AudioContext.Menu).FireAndForget();

            string filename = m_SaveDataService.GetUnusedSaveFileName();

            m_SaveDataService.BeginSave(filename);

            m_PlayTimeStore.StartCounting();

            m_MenuModule.RequestModuleChange(MenuOutcomes.PLAYTHROUGH);
            m_MenuModule.PopMenu().FireAndForget();
        }

        private void OnLoadGamePressed()
        {
            if (m_SaveDataService.GetListOfSaveFiles().Length == 0)
            {
                m_AudioIntentPlayer.PlayAsync(AudioIntent.Error, AudioContext.Menu).FireAndForget();
                return;
            }

            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();
            m_MenuModule.PushMenu(MenuType.Load).FireAndForget();
        }

        private void OnConfigPressed()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();
            m_MenuModule.PushMenu(MenuType.Config).FireAndForget();
        }

#if ENABLE_RPG_FRAMEWORK_DEBUG
        // The room plays a playthrough begun here, as New Game begins one.
        private void OnDebugRoomPressed()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();

            m_SaveDataService.BeginSave(m_SaveDataService.GetUnusedSaveFileName());

            m_MenuModule.RequestModuleChange(DebugMenuOutcomes.DEBUG_ROOM);
            m_MenuModule.PopMenu().FireAndForget();
        }
#endif

        private void OnQuitPressed()
        {
            Task cancelSound = m_AudioIntentPlayer.PlayAsync(AudioIntent.Cancel, AudioContext.Menu);

            QuitAsync(cancelSound).FireAndForget();
        }

        // The cancel sound plays out before the game closes: quitting at once also cut its bundle's load short, which
        // in the editor left the bundle loaded into the next play session. It quits even if the sound failed, which is
        // logged as it passes on.
        private async Task QuitAsync(Task cancelSound)
        {
            SetInputEnabled(false);

            try
            {
                await cancelSound;
            }
            finally
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                UnityEngine.Application.Quit();
#endif
            }
        }

        private async Task ApplySettingsAsync()
        {
            m_MusicPlayer.SetVolume(m_SettingsStore.MusicVolume);
            m_SfxPlayer.SetVolume(m_SettingsStore.SfxVolume);

            if (!m_SettingsStore.IsLanguageChosen)
            {
                return;
            }

            string[] languages = await m_LocalisationService.GetAllLanguages();

            for (int i = 0; i < languages.Length; i++)
            {
                if (m_SettingsStore.IsLanguage(languages[i]))
                {
                    await m_LocalisationService.SetCurrentLanguage(languages[i]);
                    return;
                }
            }
        }
    }
}