using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// Makes the menus' layouts in the project from the Menu Sample's, as the game's own files, with the menu
    /// stylesheet they share: found by its first line wherever it is, or made beside them when the project has none.
    /// </summary>
    internal static class MenuLayoutCreator
    {
        private const string MENU_PATH       = "Assets/Create/RPG Framework/Menu/Layouts/";
        private const string TEMPLATES       = "Samples~/Menu Sample/Layouts";
        private const string STYLESHEET      = "Menus.uss";
        private const string STYLESHEET_LINE = "/* RPG Framework menus */";
        private const string TEMPLATE_STYLE  = "<Style src=\"" + STYLESHEET + "\"/>";

        private const string BEGIN    = "BeginMenu";
        private const string CONFIG   = "ConfigMenu";
        private const string LANGUAGE = "LanguageMenu";
        private const string CONTROLS = "ControlsMenu";
        private const string PARTY    = "PartyMenu";
        private const string SAVE     = "SaveMenu";
        private const string SLOT     = "SaveSlot";

        [MenuItem(MENU_PATH + "Title Menu")]
        private static void CreateBeginMenu() => CreateOne(BEGIN);

        [MenuItem(MENU_PATH + "Settings Menu")]
        private static void CreateConfigMenu() => CreateOne(CONFIG);

        [MenuItem(MENU_PATH + "Language Menu")]
        private static void CreateLanguageMenu() => CreateOne(LANGUAGE);

        [MenuItem(MENU_PATH + "Controls Menu")]
        private static void CreateControlsMenu() => CreateOne(CONTROLS);

        [MenuItem(MENU_PATH + "Party Menu")]
        private static void CreatePartyMenu() => CreateOne(PARTY);

        [MenuItem(MENU_PATH + "Save Menu")]
        private static void CreateSaveMenu() => CreateOne(SAVE);

        [MenuItem(MENU_PATH + "Save Slot")]
        private static void CreateSaveSlot() => CreateOne(SLOT);

        /// <summary>Every layout, and a Menu UI Provider holding them.</summary>
        [MenuItem(MENU_PATH + "All Menus")]
        private static void CreateAll()
        {
            string         folder     = SelectedFolder();
            string         stylesheet = FindOrCreateStylesheet(folder);
            MenuUIProvider provider   = ScriptableObject.CreateInstance<MenuUIProvider>();
            string         path       = AssetDatabase.GenerateUniqueAssetPath($"{folder}/Menu UI Provider.asset");

            AssetDatabase.CreateAsset(provider, path);

            SerializedObject serialized = new SerializedObject(provider);

            serialized.FindProperty("m_BeginMenu").objectReferenceValue    = Create(BEGIN, folder, stylesheet);
            serialized.FindProperty("m_ConfigMenu").objectReferenceValue   = Create(CONFIG, folder, stylesheet);
            serialized.FindProperty("m_LanguageMenu").objectReferenceValue = Create(LANGUAGE, folder, stylesheet);
            serialized.FindProperty("m_ControlsMenu").objectReferenceValue = Create(CONTROLS, folder, stylesheet);
            serialized.FindProperty("m_PartyMenu").objectReferenceValue    = Create(PARTY, folder, stylesheet);
            serialized.FindProperty("m_SaveMenu").objectReferenceValue     = Create(SAVE, folder, stylesheet);
            serialized.FindProperty("m_SaveSlot").objectReferenceValue     = Create(SLOT, folder, stylesheet);
            serialized.ApplyModifiedPropertiesWithoutUndo();

            AssetDatabase.SaveAssets();
            Show(provider);
        }

        private static void CreateOne(string template)
        {
            string folder = SelectedFolder();

            Show(Create(template, folder, FindOrCreateStylesheet(folder)));
        }

        private static VisualTreeAsset Create(string template, string folder, string stylesheet)
        {
            string path   = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{template}.uxml");
            string layout = File.ReadAllText(TemplatePath(template + ".uxml"));

            layout = layout.Replace(TEMPLATE_STYLE, $"<Style src=\"{RelativePath(folder, stylesheet)}\"/>");

            File.WriteAllText(path, layout);
            AssetDatabase.ImportAsset(path);

            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        }

        private static string FindOrCreateStylesheet(string folder)
        {
            string[] guids = AssetDatabase.FindAssets("t:" + nameof(StyleSheet), new[] { "Assets" });

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);

                using StreamReader reader = new StreamReader(path);

                if (reader.ReadLine() == STYLESHEET_LINE)
                {
                    return path;
                }
            }

            string created = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{STYLESHEET}");

            File.Copy(TemplatePath(STYLESHEET), created);
            AssetDatabase.ImportAsset(created);

            return created;
        }

        private static string TemplatePath(string file)
        {
            PackageInfo package = PackageInfo.FindForAssembly(typeof(MenuUIProvider).Assembly);

            return Path.Combine(package.resolvedPath, TEMPLATES, file);
        }

        private static string SelectedFolder()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path) || !path.StartsWith("Assets", StringComparison.Ordinal))
            {
                return "Assets";
            }

            return AssetDatabase.IsValidFolder(path) ? path : Path.GetDirectoryName(path).Replace('\\', '/');
        }

        // A UXML style sheet's src may be relative to the layout, which keeps working as the two move together.
        private static string RelativePath(string fromFolder, string toFile)
        {
            Uri from = new Uri(Path.GetFullPath(fromFolder) + Path.DirectorySeparatorChar);
            Uri to   = new Uri(Path.GetFullPath(toFile));

            return Uri.UnescapeDataString(from.MakeRelativeUri(to).ToString());
        }

        private static void Show(UnityEngine.Object asset)
        {
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }
    }
}
