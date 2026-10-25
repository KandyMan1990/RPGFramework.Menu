using UnityEditor;
using UnityEngine;

namespace RPGFramework.Menu.Editor
{
    internal static class MenuModuleCreator
    {
        [MenuItem("GameObject/RPG Framework/Menu/Menu Module", false, 10)]
        private static void Create(MenuCommand command)
        {
            GameObject module = new GameObject("Menu Module", typeof(MenuModuleMonoBehaviour));

            GameObjectUtility.SetParentAndAlign(module, command.context as GameObject);
            Undo.RegisterCreatedObjectUndo(module, "Create Menu Module");
            Selection.activeObject = module;
        }
    }
}
