using System;
using System.Collections.Generic;
using RPGFramework.Core.Editor;
using RPGFramework.Core.Memory;
using RPGFramework.Localisation.Editor;
using UnityEditor;
using Object = UnityEngine.Object;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// What a menu's layouts may name, as the project declares it: the menus, the UI each is shown by and the commands
    /// each carries out, the variable map's variables, and the keys the sheets recorded at their last pull.
    /// </summary>
    internal sealed class LayoutNames
    {
        private readonly DeclaredMenu[]        m_Menus;
        private readonly Type[]                m_UIs;
        private readonly DeclaredMenuCommand[] m_Commands;
        private readonly VariableMapAsset[]    m_Maps;
        private readonly IPersistentValues[]   m_PersistentValues;
        private readonly string[]              m_Keys;

        /// <param name="menus">The declared menus.</param>
        /// <param name="uis">The class of each menu's UI, in the same order; null for one that cannot be told.</param>
        /// <param name="commands">The commands the menus declare.</param>
        /// <param name="maps">The project's variable maps.</param>
        /// <param name="sheets">The project's localisation sheets.</param>
        internal LayoutNames(DeclaredMenu[] menus, Type[] uis, DeclaredMenuCommand[] commands, VariableMapAsset[] maps, LocalisationSheetAsset[] sheets)
        {
            m_Menus            = menus;
            m_UIs              = uis;
            m_Commands         = commands;
            m_Maps             = maps;
            m_PersistentValues = new IPersistentValues[maps.Length];
            m_Keys             = KeysOf(sheets);

            for (int i = 0; i < maps.Length; i++)
            {
                m_PersistentValues[i] = new PersistentValues(maps[i], maps[i]);
            }
        }

        /// <summary>The project's, the menus being those already found.</summary>
        internal static LayoutNames FindInProject(DeclaredMenu[] menus)
        {
            Type[] uis = new Type[menus.Length];

            for (int i = 0; i < menus.Length; i++)
            {
                uis[i] = MenuUITypes.Find(menus[i].Menu);
            }

            return new LayoutNames(menus, uis, DeclaredMenuCommands.FindAll(), Load<VariableMapAsset>(), Load<LocalisationSheetAsset>());
        }

        /// <summary>Whether the project has a variable map, without which none can be checked.</summary>
        internal bool HasVariableMap => m_Maps.Length > 0;

        internal bool IsMenu(string name)
        {
            for (int i = 0; i < m_Menus.Length; i++)
            {
                if (m_Menus[i].Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>The class of the UI <paramref name="menu" /> is shown by, or null when it cannot be told.</summary>
        internal Type UIOf(string menu)
        {
            for (int i = 0; i < m_Menus.Length; i++)
            {
                if (m_Menus[i].Name == menu)
                {
                    return m_UIs[i];
                }
            }

            return null;
        }

        internal bool Declares(string menu, string command)
        {
            for (int i = 0; i < m_Commands.Length; i++)
            {
                if (m_Commands[i].Menu == menu && m_Commands[i].Name == command)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// What is wrong with greying an entry by <paramref name="name" />, one of the variables its <c>when</c> names, as
        /// the menu would say it in play, or null when a map holds it as it should.
        /// </summary>
        internal string ProblemWithWhen(string name)
        {
            string problem = null;

            for (int i = 0; i < m_Maps.Length; i++)
            {
                problem = MenuEntryConditions.ProblemWith(name, m_Maps[i]);

                if (problem == null)
                {
                    return null;
                }
            }

            return problem;
        }

        /// <summary>
        /// Whether <paramref name="path" /> names a persistent value as the map lists it — <c>Money</c>,
        /// <c>Items[3]</c>, <c>Party[0].Level</c> — and its width.
        /// </summary>
        internal bool TryGetPersistentWidth(string path, out VariableWidth width)
        {
            for (int i = 0; i < m_PersistentValues.Length; i++)
            {
                IReadOnlyList<PersistentValue> values = m_PersistentValues[i].Values;

                for (int j = 0; j < values.Count; j++)
                {
                    if (values[j].Label == path)
                    {
                        width = values[j].Width;
                        return true;
                    }
                }
            }

            width = default;
            return false;
        }

        internal bool HasKey(string key) => Array.IndexOf(m_Keys, key) >= 0;

        private static string[] KeysOf(LocalisationSheetAsset[] sheets)
        {
            int count = 0;

            for (int i = 0; i < sheets.Length; i++)
            {
                count += sheets[i].Keys.Count;
            }

            string[] keys = new string[count];
            int      next = 0;

            for (int i = 0; i < sheets.Length; i++)
            {
                for (int j = 0; j < sheets[i].Keys.Count; j++)
                {
                    keys[next++] = sheets[i].Keys[j];
                }
            }

            return keys;
        }

        private static T[] Load<T>() where T : Object
        {
            string[] guids  = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[]      assets = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[i]));
            }

            return assets;
        }
    }
}
