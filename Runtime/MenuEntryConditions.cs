using System;
using RPGFramework.Core;
using RPGFramework.Core.Memory;
using RPGFramework.Core.SharedTypes;

namespace RPGFramework.Menu
{
    /// <summary>Whether an entry's variable holds: a Bool the game's scripts keep, read as the menu opens and resumes.</summary>
    internal static class MenuEntryConditions
    {
        internal static bool Holds(string when, IVariableMap variableMap, IMemoryService memoryService)
        {
            if (when == null)
            {
                return true;
            }

            variableMap.TryGetVariable(when, out VariableDefinition variable);
#if UNITY_EDITOR
            string problem = ProblemWith(when, variableMap);

            if (problem != null)
            {
                throw new InvalidOperationException($"A menu entry {problem}");
            }
#endif
            return memoryService.ReadBool(variable.Bank, (ushort)variable.Offset);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Authoring only. What is wrong with greying an entry by <paramref name="when" />, said of the entry — "is greyed
        /// by …" — or null: asked as the menu opens in play and as its layout is checked.
        /// </summary>
        internal static string ProblemWith(string when, IVariableMap variableMap)
        {
            if (!variableMap.TryGetVariable(when, out VariableDefinition variable))
            {
                return $"is greyed by {when}, which the variable map does not hold";
            }

            if (variable.Width != VariableWidth.Bool || variable.Count != 1 || variable.IsRecord || variable.Bank == MemoryBank.Temp)
            {
                return $"is greyed by {when}, which is not a single Bool outside Temp";
            }

            return null;
        }
#endif
    }
}
