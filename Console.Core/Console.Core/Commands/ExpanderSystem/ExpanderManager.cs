using System.Collections.Generic;

namespace Console.Core.Commands.ExpanderSystem
{
    public class ExpanderManager
    {
        private readonly List<AExpander> Expanders = new List<AExpander>();

        public void AddExpander(AExpander expander)
        {
            Expanders.Add(expander);
        }

        public string Expand(string input)
        {
            string ret = input;
            for (int i = 0; i < Expanders.Count; i++)
            {
                ret = Expanders[i].Expand(ret);
            }
            return ret;
        }
    }
}