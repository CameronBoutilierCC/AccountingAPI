using System.Collections.Generic;
using System.Linq;

namespace ServiceTimeAPI
{
    static class NodeExtensions
    {
        public static IEnumerable<Node> Descendants(this Node root)
        {
            var nodes = new Stack<Node>(new[] { root });
            while (nodes.Any())
            {
                Node node = nodes.Pop();
                yield return node;
                if (node.Children != null)
                    foreach (var n in node.Children) nodes.Push(n);
            }
        }
    }
}
