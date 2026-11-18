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

            bool found = variableMap.TryGetVariable(when, out VariableDefinition variable);
#if UNITY_EDITOR
            Check(when, found, variable);
#endif
            return memoryService.ReadBool(variable.Bank, (ushort)variable.Offset);
        }

#if UNITY_EDITOR
        private static void Check(string when, bool found, VariableDefinition variable)
        {
            if (!found)
            {
                throw new InvalidOperationException($"A menu entry is greyed by {when}, which the variable map does not hold");
            }

            if (variable.Width != VariableWidth.Bool || variable.Count != 1 || variable.IsRecord || variable.Bank == MemoryBank.Temp)
            {
                throw new InvalidOperationException($"A menu entry is greyed by {when}, which is not a single Bool outside Temp");
            }
        }
#endif
    }
}
