using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Units
{
    public class Path : IEnumerable<PathNode>
    {
        public readonly PathNode[] nodes;
        public readonly int length;
        public readonly PathNode destination;
        //public IEnumerable<PathNode> nodes;

        public Path(PathNode[] points)
        {
            this.nodes = points;
            this.destination = points.Last();
            this.length = points.Length;
        }

        public bool Empty()
        {
            return length == 0;
        }

        public IEnumerator<PathNode> GetEnumerator()
        {
            return ((IEnumerable<PathNode>)nodes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return nodes.GetEnumerator();
        }
    }
}
