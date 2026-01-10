using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Data;
using RPGFramework.Core.GlobalConfig;
using RPGFramework.Core.Input;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class BeginMenu : Menu<IBeginMenuUI>, IBeginMenu
    {
        protected override bool m_HidePreviousUiOnSuspend => true;

        private readonly IMenuModule          m_MenuModule;
        private readonly IBeginMenuUI         m_BeginMenuUI;
        private readonly IGlobalConfig        m_GlobalConfig;
        private readonly ILocalisationService m_LocalisationService;

        public BeginMenu(IMenuModule          menuModule,
                         IBeginMenuUI         beginMenuUI,
                         IInputRouter         inputRouter,
                         IGlobalConfig        globalConfig,
                         ILocalisationService localisationService) : base(beginMenuUI, inputRouter)
        {
            m_MenuModule          = menuModule;
            m_BeginMenuUI         = beginMenuUI;
            m_GlobalConfig        = globalConfig;
            m_LocalisationService = localisationService;
        }

        protected override async Task OnEnterAsync(Dictionary<string, object> args)
        {
            await base.OnEnterAsync(args);

            if (!m_GlobalConfig.TryGet(out ConfigData data))
            {
                data = new ConfigData();

                string[] languages = await m_LocalisationService.GetAllLanguages();

                int languageIndex = Array.IndexOf(languages, CultureInfo.CurrentCulture.Name);

                string newLanguage = languageIndex >= 0 ? languages[languageIndex] : "en-GB";

                data.SetLanguage(newLanguage);
                data.MusicVolume        = 1f;
                data.SfxVolume          = 1f;
                data.BattleMessageSpeed = 0.5f;
                data.FieldMessageSpeed  = 0.5f;

                m_GlobalConfig.Set(data);
            }

            string language = data.GetLanguage();

            await m_LocalisationService.SetCurrentLanguage(language);
        }

        protected override void RegisterCallbacks()
        {
            m_BeginMenuUI.OnPlayAudio       += PlaySfx;
            m_BeginMenuUI.OnNewGamePressed  += OnNewGamePressed;
            m_BeginMenuUI.OnSettingsPressed += OnSettingsPressed;
            m_BeginMenuUI.OnQuitPressed     += OnQuitPressed;
        }

        protected override void UnregisterCallbacks()
        {
            m_BeginMenuUI.OnQuitPressed     -= OnQuitPressed;
            m_BeginMenuUI.OnSettingsPressed -= OnSettingsPressed;
            m_BeginMenuUI.OnNewGamePressed  -= OnNewGamePressed;
            m_BeginMenuUI.OnPlayAudio       -= PlaySfx;
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            return false;
        }

        private void PlaySfx(int id)
        {
            m_MenuModule.PlaySfx(id);
        }

        private void OnNewGamePressed()
        {
            //TODO: tell core to init default save map
            m_MenuModule.ReturnToPreviousModuleAsync().FireAndForget();
        }

        private void OnSettingsPressed()
        {
            IMenuModuleArgs args = new MenuModuleArgs<IConfigMenu>();

            m_MenuModule.PushMenu(args).FireAndForget();
        }

        private void OnQuitPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}