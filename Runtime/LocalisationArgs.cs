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
        public string   GameTitle        { get; }
        public string   NewGame          { get; }
        public string   LoadGame         { get; }
        public string   QuitGame         { get; }
        public string[] DataSheetsToLoad { get; }

        public BeginMenuLocalisationArgs(string   gameTitle,
                                         string   newGame,
                                         string   loadGame,
                                         string   quitGame,
                                         string[] dataSheetsToLoad)
        {
            GameTitle        = gameTitle;
            NewGame          = newGame;
            LoadGame         = loadGame;
            QuitGame         = quitGame;
            DataSheetsToLoad = dataSheetsToLoad;
        }
    }

    public interface ILanguageMenuLocalisationArgs : ILocalisationArgs
    {
        public string ScreenTitle { get; }
        public string Language    { get; }
    }

    public class LanguageMenuLocalisationArgs : ILanguageMenuLocalisationArgs
    {
        public string   ScreenTitle      { get; }
        public string   Language         { get; }
        public string[] DataSheetsToLoad { get; }

        public LanguageMenuLocalisationArgs(string   screenTitle,
                                            string   language,
                                            string[] dataSheetsToLoad)
        {
            ScreenTitle      = screenTitle;
            Language         = language;
            DataSheetsToLoad = dataSheetsToLoad;
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
        public string   ScreenTitle        { get; }
        public string   LanguageTitle      { get; }
        public string   Language           { get; }
        public string   Controls           { get; }
        public string   MusicVolume        { get; }
        public string   SfxVolume          { get; }
        public string   BattleMessageSpeed { get; }
        public string   FieldMessageSpeed  { get; }
        public string[] DataSheetsToLoad   { get; }

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
            ScreenTitle        = screenTitle;
            LanguageTitle      = languageTitle;
            Language           = language;
            Controls           = controls;
            MusicVolume        = musicVolume;
            SfxVolume          = sfxVolume;
            BattleMessageSpeed = battleMessageSpeed;
            FieldMessageSpeed  = fieldMessageSpeed;
            DataSheetsToLoad   = dataSheetsToLoad;
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
        public string   Config           { get; }
        public string   Save             { get; }
        public string   Time             { get; }
        public string[] DataSheetsToLoad { get; }

        public PartyMenuLocalisationArgs(string   config,
                                         string   save,
                                         string   time,
                                         string[] dataSheetsToLoad)
        {
            Config           = config;
            Save             = save;
            Time             = time;
            DataSheetsToLoad = dataSheetsToLoad;
        }
    }

    public interface ISaveMenuLocalisationArgs : ILocalisationArgs
    {
        string SaveTitle         { get; }
        string LoadTitle         { get; }
        string NewSave           { get; }
        string OverwriteQuestion { get; }
        string DeleteQuestion    { get; }
        string Yes               { get; }
        string No                { get; }
    }

    public class SaveMenuLocalisationArgs : ISaveMenuLocalisationArgs
    {
        public string   SaveTitle         { get; }
        public string   LoadTitle         { get; }
        public string   NewSave           { get; }
        public string   OverwriteQuestion { get; }
        public string   DeleteQuestion    { get; }
        public string   Yes               { get; }
        public string   No                { get; }
        public string[] DataSheetsToLoad  { get; }

        public SaveMenuLocalisationArgs(string   saveTitle,
                                        string   loadTitle,
                                        string   newSave,
                                        string   overwriteQuestion,
                                        string   deleteQuestion,
                                        string   yes,
                                        string   no,
                                        string[] dataSheetsToLoad)
        {
            SaveTitle         = saveTitle;
            LoadTitle         = loadTitle;
            NewSave           = newSave;
            OverwriteQuestion = overwriteQuestion;
            DeleteQuestion    = deleteQuestion;
            Yes               = yes;
            No                = no;
            DataSheetsToLoad  = dataSheetsToLoad;
        }
    }
}