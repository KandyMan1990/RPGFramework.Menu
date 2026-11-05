using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// Checks the menu scene's Menu UI Provider as Unity processes the scene: a player build fails on a declared menu with
    /// no layout, or a layout no menu declares, and a scene loaded in play mode logs it, so it is seen the first time the
    /// menus run rather than as one menu opens.
    /// </summary>
    internal sealed class MenuLayoutsSceneCheck : IProcessSceneWithReport
    {
        int IOrderedCallback.callbackOrder => 0;

        void IProcessSceneWithReport.OnProcessScene(Scene scene, BuildReport report)
        {
            if (!MenuLayoutsCheck.HoldsMenuModule(scene))
            {
                return;
            }

            List<string> problems = new List<string>();

            MenuLayoutsCheck.CheckScene(scene, problems);

            if (problems.Count == 0)
            {
                return;
            }

            string message = $"{scene.name}'s menu layouts have {problems.Count} problem(s):\n  {string.Join("\n  ", problems)}";

            if (report == null)
            {
                Debug.LogError(message);
                return;
            }

            throw new BuildFailedException(message);
        }
    }
}
