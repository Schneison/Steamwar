using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Steamwar.Utils;

namespace Steamwar.Units
{
    public class PathNode : IComparable<PathNode>
    {
        public NodeType type;
        public PathNode previous;
        public Vector2 position;
        public int index = -1;
        public int hash;
        public bool visited;
        public int distanceToDestination;//h
        public float distanceToOrigin;
        public int malusFromDistance;//g
        public int priority;//f

        public PathNode(Vector2 position)
        {
            this.type = NodeType.BLOCKED;
            this.position = position;
            this.hash = CreateHash(position);
        }

        public static int CreateHash(Vector2 position)
        {
            Vector2Int pos2 = (position - new Vector2(0.5F, 0.5F)).Floor();
            return pos2.y & 32767 | (pos2.x & 32767) << 16 | (pos2.x < 0 ? int.MinValue : 0);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PathNode))
            {
                return false;
            }

            var point = (PathNode)obj;
            return position.Equals(point.position);
        }

        public override int GetHashCode()
        {
            return hash;
        }

        public bool Assigned()
        {
            return index >= 0;
        }

        public override string ToString()
        {
            return position.ToString() + ";" + index;
        }

        public int CompareTo(PathNode other)
        {
            return priority.CompareTo(other.priority);
        }
    }

    public enum NodeType
    {
        ROAD = 1, WALKABLE = 2, BLOCKED = 20, 
    }
}
