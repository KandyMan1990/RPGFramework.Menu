using RPGFramework.Localisation;

namespace RPGFramework.Menu
{
    public interface IBeginMenuLocalisationArgs : ILocalisationArgs
    {
        string   GameTitle        { get; }
        string   NewGame          { get; }
        string   LoadGame         { get; }
        string   Settings         { get; }
        string   QuitGame         { get; }
    }

    public class BeginMenuLocalisationArgs : IBeginMenuLocalisationArgs
    {
        public string   GameTitle        { get; }
        public string   NewGame          { get; }
        public string   LoadGame         { get; }
        public string   Settings         { get; }
        public string   QuitGame         { get; }
        public string[] DataSheetsToLoad { get; }

        public BeginMenuLocalisationArgs(string   gameTitle,
                                         string   newGame,
                                         string   loadGame,
                                         string   settings,
                                         string   quitGame,
                                         string[] dataSheetsToLoad)
        {
            GameTitle        = gameTitle;
            NewGame          = newGame;
            LoadGame         = loadGame;
            Settings         = settings;
            QuitGame         = quitGame;
            DataSheetsToLoad = dataSheetsToLoad;
        }
    }
}