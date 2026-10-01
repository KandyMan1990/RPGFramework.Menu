using RPGFramework.DI;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SubMenus;
using RPGFramework.Menu.SubMenus.UI;
using UnityEngine;

namespace RPGFramework.Menu.Menu_Sample
{
    /// <summary>
    /// The keys name the sheets this installer expects the game to ship. A game passes the constants its localisation
    /// generates instead of these literals.
    /// </summary>
    public class MenuModuleSceneInstaller : SceneInstallerBase
    {
        private const string GENERIC_SHEET     = "Generic";
        private const string BEGIN_MENU_SHEET  = "BeginMenu";
        private const string CONFIG_MENU_SHEET = "ConfigMenu";
        private const string PARTY_MENU_SHEET  = "PartyMenu";
        private const string SAVE_MENU_SHEET   = "SaveMenu";
        private const string LOCATIONS_SHEET   = "Locations";

        [SerializeField]
        private MenuUIProvider m_MenuUIProvider;

        public override void InstallBindings(IDIContainer container)
        {
            BindLocalisationArgs(container);

            container.BindSingletonFromInstance<IMenuUIProvider>(m_MenuUIProvider);
            container.BindTransient<IBeginMenu, BeginMenu>();
            container.BindTransient<IBeginMenuUI, BeginMenuUI>();
            container.BindTransient<IConfigMenu, ConfigMenu>();
            container.BindTransient<IConfigMenuUI, ConfigMenuUI>();
            container.BindTransient<ILanguageMenu, LanguageMenu>();
            container.BindTransient<ILanguageMenuUI, LanguageMenuUI>();
            container.BindTransient<IPartyMenu, PartyMenu>();
            container.BindTransient<IPartyMenuUI, PartyMenuUI>();
            container.BindTransient<ISaveMenu, SaveMenu>();
            container.BindTransient<ILoadMenu, LoadMenu>();
            container.BindTransient<ISaveSlotMenuUI, SaveSlotMenuUI>();

            container.BindSingleton<IMenuTypeProvider, MenuTypeProvider>();
            container.BindSingleton<IMenuModule, MenuModule>();
        }

        private static void BindLocalisationArgs(IDIContainer container)
        {
            IBeginMenuLocalisationArgs beginMenuLocalisationArgs = new BeginMenuLocalisationArgs("Generic/Game_Title",
                                                                                                 "BeginMenu/New_Game",
                                                                                                 "BeginMenu/Load_Game",
                                                                                                 "BeginMenu/Quit_Game",
                                                                                                 new[] { GENERIC_SHEET, BEGIN_MENU_SHEET });

            IConfigMenuLocalisationArgs configMenuLocalisationArgs = new ConfigMenuLocalisationArgs("Generic/Settings",
                                                                                                    "ConfigMenu/Language_Title",
                                                                                                    "ConfigMenu/Language",
                                                                                                    "ConfigMenu/Controls",
                                                                                                    "ConfigMenu/Music_Volume",
                                                                                                    "ConfigMenu/Sfx_Volume",
                                                                                                    "ConfigMenu/Battle_Message_Speed",
                                                                                                    "ConfigMenu/Field_Message_Speed",
                                                                                                    new[] { GENERIC_SHEET, CONFIG_MENU_SHEET });

            ILanguageMenuLocalisationArgs languageMenuLocalisationArgs = new LanguageMenuLocalisationArgs("ConfigMenu/Language_Title",
                                                                                                          "ConfigMenu/Language",
                                                                                                          new[] { GENERIC_SHEET, CONFIG_MENU_SHEET });

            IPartyMenuLocalisationArgs partyMenuLocalisationArgs = new PartyMenuLocalisationArgs("Generic/Settings",
                                                                                                 "PartyMenu/Save",
                                                                                                 "PartyMenu/Time",
                                                                                                 new[] { GENERIC_SHEET, PARTY_MENU_SHEET, LOCATIONS_SHEET });

            ISaveMenuLocalisationArgs saveMenuLocalisationArgs = new SaveMenuLocalisationArgs("SaveMenu/Save_Title",
                                                                                              "SaveMenu/Load_Title",
                                                                                              "SaveMenu/New_Save",
                                                                                              "SaveMenu/Overwrite_Question",
                                                                                              "SaveMenu/Delete_Question",
                                                                                              "SaveMenu/Newer_Version",
                                                                                              "SaveMenu/Damaged",
                                                                                              "Generic/Yes",
                                                                                              "Generic/No",
                                                                                              new[] { GENERIC_SHEET, SAVE_MENU_SHEET, LOCATIONS_SHEET });

            container.BindSingletonFromInstance(beginMenuLocalisationArgs);
            container.BindSingletonFromInstance(configMenuLocalisationArgs);
            container.BindSingletonFromInstance(languageMenuLocalisationArgs);
            container.BindSingletonFromInstance(partyMenuLocalisationArgs);
            container.BindSingletonFromInstance(saveMenuLocalisationArgs);
        }
    }
}
