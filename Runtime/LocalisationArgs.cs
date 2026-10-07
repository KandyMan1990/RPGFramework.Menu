using RPGFramework.Localisation;

namespace RPGFramework.Menu
{
    public interface IBeginMenuLocalisationArgs : ILocalisationArgs
    {
        string GameTitle { get; }
        string NewGame   { get; }
        string LoadGame  { get; }
        string QuitGame  { get; }
    }

    public class BeginMenuLocalisationArgs : IBeginMenuLocalisationArgs
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

    public interface ILanguageMenuLocalisationArgs : ILocalisationArgs
    {
        public string ScreenTitle { get; }
        public string Language    { get; }
    }

    public class LanguageMenuLocalisationArgs : ILanguageMenuLocalisationArgs
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

    public interface IConfigMenuLocalisationArgs : ILocalisationArgs
    {
        string ScreenTitle        { get; }
        string LanguageTitle      { get; }
        string Language           { get; }
        string Controls           { get; }
        string MusicVolume        { get; }
        string SfxVolume          { get; }
        string BattleMessageSpeed { get; }
        string FieldMessageSpeed  { get; }
    }

    public class ConfigMenuLocalisationArgs : IConfigMenuLocalisationArgs
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

    public interface IPartyMenuLocalisationArgs : ILocalisationArgs
    {
        string Config { get; }
        string Save   { get; }
        string Time   { get; }
    }

    public class PartyMenuLocalisationArgs : IPartyMenuLocalisationArgs
    {
        private readonly string   m_Config;
        private readonly string   m_Save;
        private readonly string   m_Time;
        private readonly string[] m_DataSheetsToLoad;

        string IPartyMenuLocalisationArgs.Config           => m_Config;
        string IPartyMenuLocalisationArgs.Save             => m_Save;
        string IPartyMenuLocalisationArgs.Time             => m_Time;
        string[] ILocalisationArgs.       DataSheetsToLoad => m_DataSheetsToLoad;

        public PartyMenuLocalisationArgs(string   config,
                                         string   save,
                                         string   time,
                                         string[] dataSheetsToLoad)
        {
            m_Config           = config;
            m_Save             = save;
            m_Time             = time;
            m_DataSheetsToLoad = dataSheetsToLoad;
        }
    }

    public interface ISaveMenuLocalisationArgs : ILocalisationArgs
    {
        string SaveTitle         { get; }
        string LoadTitle         { get; }
        string NewSave           { get; }
        string OverwriteQuestion { get; }
        string DeleteQuestion    { get; }
        string NewerVersion      { get; }
        string Damaged           { get; }
        string Yes               { get; }
        string No                { get; }
    }

    public class SaveMenuLocalisationArgs : ISaveMenuLocalisationArgs
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