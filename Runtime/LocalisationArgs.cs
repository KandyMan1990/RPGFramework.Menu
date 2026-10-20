using RPGFramework.Localisation;

namespace RPGFramework.Menu
{
    /// <summary>The localisation keys the title menu shows, and the sheets they are in.</summary>
    public interface IBeginMenuLocalisationArgs : ILocalisationArgs
    {
        /// <summary>The key of the game's title.</summary>
        string GameTitle { get; }

        /// <summary>The key of the New Game button.</summary>
        string NewGame { get; }

        /// <summary>The key of the Load Game button.</summary>
        string LoadGame { get; }

        /// <summary>The key of the Quit button.</summary>
        string QuitGame { get; }
    }

    public sealed class BeginMenuLocalisationArgs : IBeginMenuLocalisationArgs
    {
        private readonly string   m_GameTitle;
        private readonly string   m_NewGame;
        private readonly string   m_LoadGame;
        private readonly string   m_QuitGame;
        private readonly string[] m_DataSheetsToLoad;

        string IBeginMenuLocalisationArgs.GameTitle        => m_GameTitle;
        string IBeginMenuLocalisationArgs.NewGame          => m_NewGame;
        string IBeginMenuLocalisationArgs.LoadGame         => m_LoadGame;
        string IBeginMenuLocalisationArgs.QuitGame         => m_QuitGame;
        string[] ILocalisationArgs.       DataSheetsToLoad => m_DataSheetsToLoad;

        public BeginMenuLocalisationArgs(string   gameTitle,
                                         string   newGame,
                                         string   loadGame,
                                         string   quitGame,
                                         string[] dataSheetsToLoad)
        {
            m_GameTitle        = gameTitle;
            m_NewGame          = newGame;
            m_LoadGame         = loadGame;
            m_QuitGame         = quitGame;
            m_DataSheetsToLoad = dataSheetsToLoad;
        }
    }

    /// <summary>The localisation keys the Language menu shows, and the sheets they are in.</summary>
    public interface ILanguageMenuLocalisationArgs : ILocalisationArgs
    {
        /// <summary>The key of the screen's title.</summary>
        string ScreenTitle { get; }

        /// <summary>The key of the language's own name, shown in the language it names.</summary>
        string Language { get; }
    }

    public sealed class LanguageMenuLocalisationArgs : ILanguageMenuLocalisationArgs
    {
        private readonly string   m_ScreenTitle;
        private readonly string   m_Language;
        private readonly string[] m_DataSheetsToLoad;

        string ILanguageMenuLocalisationArgs.ScreenTitle      => m_ScreenTitle;
        string ILanguageMenuLocalisationArgs.Language         => m_Language;
        string[] ILocalisationArgs.          DataSheetsToLoad => m_DataSheetsToLoad;

        public LanguageMenuLocalisationArgs(string   screenTitle,
                                            string   language,
                                            string[] dataSheetsToLoad)
        {
            m_ScreenTitle      = screenTitle;
            m_Language         = language;
            m_DataSheetsToLoad = dataSheetsToLoad;
        }
    }

    /// <summary>The localisation keys the Config menu shows, and the sheets they are in.</summary>
    public interface IConfigMenuLocalisationArgs : ILocalisationArgs
    {
        /// <summary>The key of the screen's title.</summary>
        string ScreenTitle { get; }

        /// <summary>The key of the language setting's label.</summary>
        string LanguageTitle { get; }

        /// <summary>The key of the language's own name, shown in the language it names.</summary>
        string Language { get; }

        /// <summary>The key of the Controls button.</summary>
        string Controls { get; }

        /// <summary>The key of the music volume slider's label.</summary>
        string MusicVolume { get; }

        /// <summary>The key of the sound effects' volume slider's label.</summary>
        string SfxVolume { get; }

        /// <summary>The key of the battle message speed slider's label.</summary>
        string BattleMessageSpeed { get; }

        /// <summary>The key of the field message speed slider's label.</summary>
        string FieldMessageSpeed { get; }
    }

    public sealed class ConfigMenuLocalisationArgs : IConfigMenuLocalisationArgs
    {
        private readonly string   m_ScreenTitle;
        private readonly string   m_LanguageTitle;
        private readonly string   m_Language;
        private readonly string   m_Controls;
        private readonly string   m_MusicVolume;
        private readonly string   m_SfxVolume;
        private readonly string   m_BattleMessageSpeed;
        private readonly string   m_FieldMessageSpeed;
        private readonly string[] m_DataSheetsToLoad;

        string IConfigMenuLocalisationArgs.ScreenTitle        => m_ScreenTitle;
        string IConfigMenuLocalisationArgs.LanguageTitle      => m_LanguageTitle;
        string IConfigMenuLocalisationArgs.Language           => m_Language;
        string IConfigMenuLocalisationArgs.Controls           => m_Controls;
        string IConfigMenuLocalisationArgs.MusicVolume        => m_MusicVolume;
        string IConfigMenuLocalisationArgs.SfxVolume          => m_SfxVolume;
        string IConfigMenuLocalisationArgs.BattleMessageSpeed => m_BattleMessageSpeed;
        string IConfigMenuLocalisationArgs.FieldMessageSpeed  => m_FieldMessageSpeed;
        string[] ILocalisationArgs.        DataSheetsToLoad   => m_DataSheetsToLoad;

        public ConfigMenuLocalisationArgs(string   screenTitle,
                                          string   languageTitle,
                                          string   language,
                                          string   controls,
                                          string   musicVolume,
                                          string   sfxVolume,
                                          string   battleMessageSpeed,
                                          string   fieldMessageSpeed,
                                          string[] dataSheetsToLoad)
        {
            m_ScreenTitle        = screenTitle;
            m_LanguageTitle      = languageTitle;
            m_Language           = language;
            m_Controls           = controls;
            m_MusicVolume        = musicVolume;
            m_SfxVolume          = sfxVolume;
            m_BattleMessageSpeed = battleMessageSpeed;
            m_FieldMessageSpeed  = fieldMessageSpeed;
            m_DataSheetsToLoad   = dataSheetsToLoad;
        }
    }

    /// <summary>The localisation keys the party menu shows, and the sheets they are in.</summary>
    public interface IPartyMenuLocalisationArgs : ILocalisationArgs
    {
        /// <summary>The key of the Config button.</summary>
        string Config { get; }

        /// <summary>The key of the Save button.</summary>
        string Save { get; }

        /// <summary>The key of the Load button.</summary>
        string Load { get; }

        /// <summary>The key of the play time's label.</summary>
        string Time { get; }
    }

    public sealed class PartyMenuLocalisationArgs : IPartyMenuLocalisationArgs
    {
        private readonly string   m_Config;
        private readonly string   m_Save;
        private readonly string   m_Load;
        private readonly string   m_Time;
        private readonly string[] m_DataSheetsToLoad;

        string IPartyMenuLocalisationArgs.Config           => m_Config;
        string IPartyMenuLocalisationArgs.Save             => m_Save;
        string IPartyMenuLocalisationArgs.Load             => m_Load;
        string IPartyMenuLocalisationArgs.Time             => m_Time;
        string[] ILocalisationArgs.       DataSheetsToLoad => m_DataSheetsToLoad;

        public PartyMenuLocalisationArgs(string   config,
                                         string   save,
                                         string   load,
                                         string   time,
                                         string[] dataSheetsToLoad)
        {
            m_Config           = config;
            m_Save             = save;
            m_Load             = load;
            m_Time             = time;
            m_DataSheetsToLoad = dataSheetsToLoad;
        }
    }

    /// <summary>The localisation keys the Save and Load menus show, and the sheets they are in.</summary>
    public interface ISaveMenuLocalisationArgs : ILocalisationArgs
    {
        /// <summary>The key of the Save menu's title.</summary>
        string SaveTitle { get; }

        /// <summary>The key of the Load menu's title.</summary>
        string LoadTitle { get; }

        /// <summary>The key of the new save row.</summary>
        string NewSave { get; }

        /// <summary>The key of the question asked before writing over a save.</summary>
        string OverwriteQuestion { get; }

        /// <summary>The key of the question asked before deleting a save.</summary>
        string DeleteQuestion { get; }

        /// <summary>The key shown on a save written by a newer build of the game, which cannot be loaded.</summary>
        string NewerVersion { get; }

        /// <summary>The key shown on a save that is damaged, which cannot be loaded.</summary>
        string Damaged { get; }

        /// <summary>The key of a question's yes answer.</summary>
        string Yes { get; }

        /// <summary>The key of a question's no answer.</summary>
        string No { get; }
    }

    public sealed class SaveMenuLocalisationArgs : ISaveMenuLocalisationArgs
    {
        private readonly string   m_SaveTitle;
        private readonly string   m_LoadTitle;
        private readonly string   m_NewSave;
        private readonly string   m_OverwriteQuestion;
        private readonly string   m_DeleteQuestion;
        private readonly string   m_NewerVersion;
        private readonly string   m_Damaged;
        private readonly string   m_Yes;
        private readonly string   m_No;
        private readonly string[] m_DataSheetsToLoad;

        string ISaveMenuLocalisationArgs.SaveTitle         => m_SaveTitle;
        string ISaveMenuLocalisationArgs.LoadTitle         => m_LoadTitle;
        string ISaveMenuLocalisationArgs.NewSave           => m_NewSave;
        string ISaveMenuLocalisationArgs.OverwriteQuestion => m_OverwriteQuestion;
        string ISaveMenuLocalisationArgs.DeleteQuestion    => m_DeleteQuestion;
        string ISaveMenuLocalisationArgs.NewerVersion      => m_NewerVersion;
        string ISaveMenuLocalisationArgs.Damaged           => m_Damaged;
        string ISaveMenuLocalisationArgs.Yes               => m_Yes;
        string ISaveMenuLocalisationArgs.No                => m_No;
        string[] ILocalisationArgs.      DataSheetsToLoad  => m_DataSheetsToLoad;

        public SaveMenuLocalisationArgs(string   saveTitle,
                                        string   loadTitle,
                                        string   newSave,
                                        string   overwriteQuestion,
                                        string   deleteQuestion,
                                        string   newerVersion,
                                        string   damaged,
                                        string   yes,
                                        string   no,
                                        string[] dataSheetsToLoad)
        {
            m_SaveTitle         = saveTitle;
            m_LoadTitle         = loadTitle;
            m_NewSave           = newSave;
            m_OverwriteQuestion = overwriteQuestion;
            m_DeleteQuestion    = deleteQuestion;
            m_NewerVersion      = newerVersion;
            m_Damaged           = damaged;
            m_Yes               = yes;
            m_No                = no;
            m_DataSheetsToLoad  = dataSheetsToLoad;
        }
    }
}