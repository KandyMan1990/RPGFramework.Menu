namespace RPGFramework.Menu
{
    /// <summary>
    /// What choosing a menu's entry does, as its menu reads it: the menu it opens, or the command its menu carries out, or
    /// neither, each the hash of its name, and the Bool variables that grey it while any is false, none for none.
    /// </summary>
    public readonly struct MenuChoice
    {
        internal readonly ulong    Opens;
        internal readonly ulong    Command;
        internal readonly string[] When;

        internal MenuChoice(ulong opens, ulong command, string[] when)
        {
            Opens   = opens;
            Command = command;
            When    = when;
        }
    }
}
