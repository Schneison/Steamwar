using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Navigation
{
    public class PathQueue
    {
        private PathNode[] nodes = new PathNode[128];
        private int count;

        public PathNode Enqueue(PathNode point)
        {
            if (point.index >= 0)
            {
                return null;
            }
            else
            {
                if (this.count == this.nodes.Length)
                {
                    PathNode[] apathpoint = new PathNode[this.count << 1];
                    Array.Copy(this.nodes, 0, apathpoint, 0, this.count);
                    this.nodes = apathpoint;
                }

                this.nodes[this.count] = point;
                point.index = this.count;
                this.SortBack(this.count++);
                return point;
            }
        }

        public void Clear()
        {
            this.count = 0;
        }

        public PathNode Dequeue()
        {
            PathNode pathpoint = this.nodes[0];
            this.nodes[0] = this.nodes[--this.count];
            this.nodes[this.count] = null;

            if (this.count > 0)
            {
                this.SortForward(0);
            }

            pathpoint.index = -1;
            return pathpoint;
        }

        public void UpdatePriority(PathNode point, int priority)
        {
            float currentPriority = point.priority;
            point.priority = priority;

            if (priority < currentPriority)
            {
                this.SortBack(point.index);
            }else{
                this.SortForward(point.index);
            }
        }

        private void SortBack(int index)
        {
            PathNode pathpoint = this.nodes[index];
            int i;

            for (float f = pathpoint.priority; index > 0; index = i)
            {
                i = index - 1 >> 1;
                PathNode pathpoint1 = this.nodes[i];

                if (f >= pathpoint1.priority)
                {
                    break;
                }

                this.nodes[index] = pathpoint1;
                pathpoint1.index = index;
            }

            this.nodes[index] = pathpoint;
            pathpoint.index = index;
        }

        private void SortForward(int index)
        {
            PathNode pathpoint = this.nodes[index];
            float priority = pathpoint.priority;

            while (true)
            {
                int i = 1 + (index << 1);
                int j = i + 1;

                if (i >= this.count)
                {
                    break;
                }

                PathNode pathpoint1 = this.nodes[i];
                float f1 = pathpoint1.priority;
                PathNode pathpoint2;
                float f2;

                if (j >= this.count)
                {
                    pathpoint2 = null;
                    f2 = float.PositiveInfinity;
                }
                else
                {
                    pathpoint2 = this.nodes[j];
                    f2 = pathpoint2.priority;
                }

                if (f1 < f2)
                {
                    if (f1 >= priority)
                    {
                        break;
                    }

                    this.nodes[index] = pathpoint1;
                    pathpoint1.index = index;
                    index = i;
                }
                else
                {
                    if (f2 >= priority)
                    {
                        break;
                    }

                    this.nodes[index] = pathpoint2;
                    pathpoint2.index = index;
                    index = j;
                }
            }

            this.nodes[index] = pathpoint;
            pathpoint.index = index;
        }

        public bool Empty()
        {
            return this.count == 0;
        }
    }
}
