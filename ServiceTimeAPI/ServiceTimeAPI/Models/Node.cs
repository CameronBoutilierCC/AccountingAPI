using System.Collections.Generic;

namespace ServiceTimeAPI
{
    public class Node
    {
        public string Key { get; set; }
        public List<Node> Children { get; set; }
    }
}
