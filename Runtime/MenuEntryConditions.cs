using System;
using RPGFramework.Core;
using RPGFramework.Core.Memory;
using RPGFramework.Core.SharedTypes;

namespace RPGFramework.Menu
{
    /// <summary>Whether an entry's variables hold: Bools the game's scripts keep, read as the menu opens and resumes.</summary>
    internal static class MenuEntryConditions
    {
        /// <summary>Whether every variable <paramref name="when" /> names is true; true for none.</summary>
        internal static bool Holds(string[] when, IVariableMap variableMap, IMemoryService memoryService)
        {
            for (int i = 0; i < when.Length; i++)
            {
                if (!IsTrue(when[i], variableMap, memoryService))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsTrue(string name, IVariableMap variableMap, IMemoryService memoryService)
        {
            variableMap.TryGetVariable(name, out VariableDefinition variable);
#if UNITY_EDITOR
            string problem = ProblemWith(name, variableMap);

            if (problem != null)
            {
                throw new InvalidOperationException($"A menu entry {problem}");
            }
#endif
            return memoryService.ReadBool(variable.Bank, (ushort)variable.Offset);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Authoring only. What is wrong with greying an entry by the variable <paramref name="name" />, one of its
        /// <c>when</c>, said of the entry — "is greyed by …" — or null: asked as the menu opens in play and as its layout
        /// is checked.
        /// </summary>
        internal static string ProblemWith(string name, IVariableMap variableMap)
        {
            // Among several names Unity keeps an empty one, which a row of the UI Builder's list left at None writes.
            if (string.IsNullOrEmpty(name))
            {
                return "is greyed by an empty name, a row of its When left at None";
            }

            if (!variableMap.TryGetVariable(name, out VariableDefinition variable))
            {
                return $"is greyed by {name}, which the variable map does not hold";
            }

            if (variable.Width != VariableWidth.Bool || variable.Count != 1 || variable.IsRecord || variable.Bank == MemoryBank.Temp)
            {
                return $"is greyed by {name}, which is not a single Bool outside Temp";
            }

            return null;
        }
#endif
    }
}
