using System.Collections.Generic;
using System.Linq;

using Console.Core.CommandSystem.Commands;

namespace Console.Core.CommandSystem
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommandFilter
    {

        /// <summary>
        /// 
        /// </summary>
        private static IComparer<AbstractCommand> Comparer;

        /// <summary>
        /// 
        /// </summary>
        private static readonly List<ICommandFilter> Filters = new List<ICommandFilter>();

        /// <summary>
        /// 
        /// </summary>
        public static void SetComparer(IComparer<AbstractCommand> comparer)
        {
            Comparer = comparer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        public static void Remove(ICommandFilter filter)
        {
            Filters.Remove(filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        public static void Add(ICommandFilter filter)
        {
            Filters.Add(filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterQuery"></param>
        /// <param name="startsWith"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool AllowCommand(string filterQuery, bool startsWith, CommandIdentity command)
        {
            if (Filters.Count == 0) return command.UnfilteredContainsName(filterQuery, startsWith, false) ||
                                           command.UnfilteredContainsName(filterQuery, startsWith, true);
            return Filters.TrueForAll(x => x.Allow(filterQuery, startsWith, command));
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<AbstractCommand> Prioritize(List<AbstractCommand> commands)
        {
            if (Comparer == null)
            {
                return commands;
            }

            List<AbstractCommand> ret = commands.ToList();
            ret.Sort(Comparer);
            return ret;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filterQuery"></param>
        ///// <param name="commands"></param>
        ///// <returns></returns>
        //public static List<AbstractCommand> Filter(string filterQuery, List<AbstractCommand> commands)
        //{
        //    List<AbstractCommand> ret = commands.ToList();
        //    for (int i = ret.Count - 1; i >= 0; i--)
        //    {
        //        if (!Filters.TrueForAll(x => x.Allow(filterQuery, ret[i].Identity))) ret.Remove(ret[i]);
        //    }

        //    return ret;
        //}

    }

    /// <summary>
    /// Used to Filter Available Commands
    /// </summary>
    public interface ICommandFilter
    {

        /// <summary>
        /// Returns true if the Command Passes through the Filter
        /// </summary>
        /// <param name="filterQuery">The Query Filter</param>
        /// <param name="startsWith"></param>
        /// <param name="command">Command</param>
        /// <returns></returns>
        bool Allow(string filterQuery, bool startsWith, CommandIdentity command);

    }
}