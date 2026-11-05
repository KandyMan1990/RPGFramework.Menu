using RPGFramework.Core.Input;
using RPGFramework.Hashing;
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
#endif

namespace RPGFramework.Menu.SubMenus
{
    /// <summary>
    /// The actions the menus read beside Core's <c>Cancel</c>, each the hash of its name in the input actions' UI map,
    /// which UI Toolkit reads too: its confirm is that map's <c>Submit</c>. Every action the menus read is declared for
    /// the editor's check of the menu scene's map.
    /// </summary>
    internal sealed class MenuActions
#if UNITY_EDITOR
        : IRequiredActions
#endif
    {
        /// <summary>The name of UI Toolkit's confirm.</summary>
        internal const string SUBMIT = "Submit";

        /// <summary>The name of the action that deletes the save slot the cursor is on.</summary>
        internal const string DELETE = "Delete";

        internal static readonly ulong Submit = Fnv1a64.Hash(SUBMIT);

        internal static readonly ulong Delete = Fnv1a64.Hash(DELETE);

#if UNITY_EDITOR
        private static readonly RequiredAction[] m_Required =
        {
            new RequiredAction(SUBMIT, RequiredActionKind.Button, "chooses the button the cursor is on, as UI Toolkit reads it"),
            new RequiredAction(CoreActions.CANCEL, RequiredActionKind.Button, "backs out of a menu"),
            new RequiredAction(DELETE, RequiredActionKind.Button, "deletes the save slot the cursor is on"),
            new RequiredAction("Navigate", RequiredActionKind.Direction, "moves the cursor, as UI Toolkit reads it")
        };

        Type IRequiredActions.Module => typeof(MenuModuleMonoBehaviour);

        string IRequiredActions.Map => "UI";

        bool IRequiredActions.Moves => false;

        IReadOnlyList<RequiredAction> IRequiredActions.Actions => m_Required;
#endif
    }
}
