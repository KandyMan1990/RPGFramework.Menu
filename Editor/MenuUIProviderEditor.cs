using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// The Menu UI Provider's inspector: a layout for each declared menu, under the name its <see cref="MenuNameAttribute" />
    /// gives it, so no name is typed; what the check finds wrong, and a button removing layouts no menu declares.
    /// </summary>
    [CustomEditor(typeof(MenuUIProvider))]
    internal sealed class MenuUIProviderEditor : UnityEditor.Editor
    {
        private DeclaredMenu[] m_Declared;
        private ObjectField[]  m_Layouts;
        private HelpBox        m_Problems;
        private Button         m_RemoveUndeclared;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            m_Declared         = DeclaredMenus.FindAll();
            m_Layouts          = new ObjectField[m_Declared.Length];
            m_Problems         = new HelpBox(string.Empty, HelpBoxMessageType.Error);
            m_RemoveUndeclared = new Button(RemoveUndeclared) { text = "Remove Layouts No Menu Declares" };

            for (int i = 0; i < m_Declared.Length; i++)
            {
                string      name  = m_Declared[i].Name;
                ObjectField field = new ObjectField(name) { objectType = typeof(VisualTreeAsset), allowSceneObjects = false, tooltip = $"The layout of {m_Declared[i].Menu.Name}" };

                field.RegisterValueChangedCallback(e => SetLayout(name, (VisualTreeAsset)e.newValue));

                m_Layouts[i] = field;
                root.Add(field);
            }

            root.Add(new PropertyField(serializedObject.FindProperty("m_SaveSlot")));
            root.Add(m_Problems);
            root.Add(m_RemoveUndeclared);

            root.TrackSerializedObjectValue(serializedObject, _ => Refresh());

            Refresh();

            return root;
        }

        private SerializedProperty Layouts => serializedObject.FindProperty("m_Layouts");

        // A menu's entry is made as it is given a layout and removed as its layout is cleared, so the list holds only the
        // menus that have one.
        private void SetLayout(string name, VisualTreeAsset layout)
        {
            serializedObject.Update();

            SerializedProperty layouts = Layouts;
            int                index   = MenuLayoutsCheck.IndexOf(layouts, name);

            if (layout == null)
            {
                if (index >= 0)
                {
                    layouts.DeleteArrayElementAtIndex(index);
                }
            }
            else
            {
                if (index < 0)
                {
                    index = layouts.arraySize;
                    layouts.arraySize++;
                    layouts.GetArrayElementAtIndex(index).FindPropertyRelative("m_Menu").stringValue = name;
                }

                layouts.GetArrayElementAtIndex(index).FindPropertyRelative("m_Layout").objectReferenceValue = layout;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void RemoveUndeclared()
        {
            serializedObject.Update();

            SerializedProperty layouts = Layouts;

            for (int i = layouts.arraySize - 1; i >= 0; i--)
            {
                if (!MenuLayoutsCheck.IsDeclared(MenuLayoutsCheck.Name(layouts, i), m_Declared))
                {
                    layouts.DeleteArrayElementAtIndex(i);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void Refresh()
        {
            serializedObject.Update();

            SerializedProperty layouts    = Layouts;
            bool               undeclared = false;

            for (int i = 0; i < m_Declared.Length; i++)
            {
                int index = MenuLayoutsCheck.IndexOf(layouts, m_Declared[i].Name);

                m_Layouts[i].SetValueWithoutNotify(index >= 0 ? MenuLayoutsCheck.Layout(layouts, index) : null);
            }

            for (int i = 0; i < layouts.arraySize; i++)
            {
                undeclared |= !MenuLayoutsCheck.IsDeclared(MenuLayoutsCheck.Name(layouts, i), m_Declared);
            }

            List<string> problems = new List<string>();

            MenuLayoutsCheck.Check((MenuUIProvider)target, m_Declared, problems);

            m_Problems.text                  = string.Join("\n", problems);
            m_Problems.style.display         = problems.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            m_RemoveUndeclared.style.display = undeclared ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
