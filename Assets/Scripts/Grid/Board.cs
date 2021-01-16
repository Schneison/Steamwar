using Steamwar.Core;
using Steamwar.Objects;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Tilemaps;

namespace Steamwar.Grid
{
    /// <summary>
    /// Keeps track of every object that is currently on the board.
    /// </summary>
    public class Board
    {
        public const sbyte MAX_POSITION_SIZE = sbyte.MaxValue;
        public const sbyte MIN_POSITION_SIZE = sbyte.MinValue;
        public const byte MAX_SIZE = byte.MaxValue;
        public const int ARRAY_SIZE = ushort.MaxValue + 1;

        /// <summary>
        /// All objects on the board by there kind
        /// </summary>
        public IDictionary<ObjectKind, HashSet<int>> objectsByKind = new Dictionary<ObjectKind, HashSet<int>>();
        /// <summary>
        /// All objects on the board by there own unique id
        /// </summary>
        public IDictionary<int, ObjectContainer> objectById = new Dictionary<int, ObjectContainer>();
        /// <summary>
        /// All objects on the board by there type
        /// </summary>
        public IDictionary<ObjectType, HashSet<int>> objectByType = new Dictionary<ObjectType, HashSet<int>>();
        /// <summary>
        /// All objects on the board by there name. Can later by used for "scenarios" ?
        /// </summary>
        //public static IDictionary<string, ObjectBehaviour> objectByName = new Dictionary<string, ObjectBehaviour>();

        public IDictionary<ObjectTag, HashSet<int>> objectsByTag = new Dictionary<ObjectTag, HashSet<int>>();
        /// <summary>
        /// All objects on the boartd by there faction index.
        /// </summary>
        public IDictionary<int, HashSet<int>> objectsByFaction = new Dictionary<int, HashSet<int>>();
        private CellInfo[] cells = new CellInfo[0];
        public int width, height;
        public Board()
        {
            this.width = MAX_SIZE;
            this.height = MAX_SIZE;
            cells = new CellInfo[ARRAY_SIZE];
            for (int x = MIN_POSITION_SIZE; x <= MAX_POSITION_SIZE; x++)
            {
                for (int y = MIN_POSITION_SIZE; y <= MAX_POSITION_SIZE; y++)
                {
                    int index = GetCellIndex(new Vector2Int(x, y));
                    cells[index] = new CellInfo(index);
                }
            }
        }

        public void Clear()
        {
            cells = new CellInfo[0];
        }

        public bool SetTile(Vector2Int pos, TileBase tile, BoardLayerType type)
        {
            ICellInfo info = GetCell(pos);
            if (!info.Exists)
            {
                return false;
            }
            BoardManager.SetTile(pos, tile, type);
            info.SetTile(Registry.GetName(tile), type);
            return true;
        }

        public bool SetTile(Vector3Int pos, TileBase tile, BoardLayerType type)
        {
            return SetTile(new Vector2Int(pos.x, pos.y), tile, type);
        }

        public bool RemoveTile(Vector2Int pos, BoardLayerType type)
        {
            ICellInfo info = GetCell(pos);
            if (!info.Exists)
            {
                return false;
            }
            info.RemoveTile(type);
            return true;
        }

        public bool RemoveTile(Vector3Int pos, BoardLayerType type)
        {
            return RemoveTile(new Vector2Int(pos.x, pos.y), type);
        }

        /*public CellVendor AddCellVendor(Vector3Int pos)
        {
            ICellInfo info = GetCell(pos);
            info.
        }

        public bool AddCellCustomer(Vector3Int pos)
        {

        }*/

        public void Add(GameObject gameObject)
        {
            ObjectContainer obj = gameObject.GetComponent<ObjectContainer>();
            if (obj != null)
            {
                Add(obj);
            }
        }

        public void Add(ObjectContainer obj)
        {
            if (GameManager.ShuttDown())
            {
                return;
            }
            BoardManager.Instance.objectConstrcuted.Invoke(obj);
            int id = obj.GetInstanceID();
            ICellInfo cell = GetCell(BoardManager.WorldToCell(obj.transform.position));
            if (cell.Exists)
            {
                cell.OnObjectAdded(id);
            }
            ObjectData data = obj.Data;
            objectsByKind.AddToSub(obj.Kind, id);
            if (data == null)
            {
                return;
            }
            ObjectType type = data.Type;
            objectByType.AddToSub(type, id);
            objectsByFaction.AddToSub(obj.Data.FactionIndex, id);
            ObjectTag tag = type.Tag;
            if (tag == ObjectTag.None)
            {
                return;
            }
            objectById[id] = obj;
            foreach (ObjectTag objTag in Enum.GetValues(typeof(ObjectTag)))
            {
                if ((tag & objTag) == objTag)
                {
                    objectsByTag.AddToSub(objTag, id);
                }
            }
        }

        public void Remove(GameObject gameObject)
        {
            ObjectContainer obj = gameObject.GetComponent<ObjectContainer>();
            if (obj != null)
            {
                Remove(obj);
            }
        }

        public void Remove(ObjectContainer obj)
        {
            if (GameManager.ShuttDown())
            {
                return;
            }
            BoardManager.Instance.objectDeconstructed.Invoke(obj);
            int id = obj.GetInstanceID();
            ICellInfo cell = GetCell(BoardManager.WorldToCell(obj.transform.position));
            if (cell.Exists)
            {
                cell.OnObjectRemoved(id);
            }
            ObjectData data = obj.Data;
            objectsByKind.RemoveFromSub(obj.Kind, id);
            if (data == null)
            {
                return;
            }
            ObjectType type = obj.Data.Type;
            objectByType.RemoveFromSub(type, id);
            objectsByFaction.RemoveFromSub(obj.Data.FactionIndex, id);
            objectById.Remove(id);
            ObjectTag tag = type.Tag;
            if (tag == ObjectTag.None)
            {
                return;
            }
            foreach (ObjectTag objTag in Enum.GetValues(typeof(ObjectTag)))
            {
                if ((tag & objTag) == objTag)
                {
                    objectsByTag.RemoveFromSub(objTag, id);
                }
            }
        }

        public IEnumerable<ObjectContainer> GetObjects(ObjectTag tag)
        {
            return from id in objectsByTag[tag]
                   select GetObject(id);
        }

        public ObjectContainer GetObject(int index)
        {
            return objectById[index];
        }

        public IEnumerable<ObjectContainer> GetObjectsFromFaction(int faction)
        {
            return from id in objectsByFaction.AddIfAbsent(faction)
                   select GetObject(id);
        }

        public IEnumerable<(int, IEnumerable<ObjectContainer>)> GetObjectsByFactions()
        {
            return from pair in objectsByFaction
                   select (pair.Key, (from id in pair.Value select GetObject(id)));
        }

        public ICellInfo GetCell(Vector2Int pos)
        {
            if (pos.x > MAX_POSITION_SIZE || pos.x < MIN_POSITION_SIZE)
            {
                return CellInfo.Empty;
            }
            if (pos.y > MAX_POSITION_SIZE || pos.y < MIN_POSITION_SIZE)
            {
                return CellInfo.Empty;
            }
            return GetCell(GetCellIndex(pos));
        }

        public ICellInfo GetCell(Vector3Int pos)
        {
            return GetCell(new Vector2Int(pos.x, pos.y));
        }

        public ICellInfo GetCell(int index)
        {
            return cells[index];
        }

        public static int GetCellIndex(Vector3Int pos)
        {
            return GetCellIndex((sbyte)pos.x, (sbyte)pos.y);
        }

        public static int GetCellIndex(Vector2Int pos)
        {
            return GetCellIndex((sbyte)pos.x, (sbyte)pos.y);
        }

        public static int GetCellIndex(sbyte posX, sbyte posY)
        {
            byte x = unchecked((byte)posX);
            byte y = unchecked((byte)posY);
            return ((int)(x & MAX_SIZE)) << 8 | y & MAX_SIZE;
        }

        public static int GetOffsetIndex(int pos, sbyte offsetX, sbyte offsetY)
        {
            sbyte x = (sbyte)((byte)(pos >> 8) & MAX_SIZE);
            sbyte y = (sbyte) ((byte)pos & MAX_SIZE);
            x += offsetX;
            y += offsetY;
            return GetCellIndex(x, y);
        }

        public static Vector2Int GetPosFromIndex(int pos)
        {
            sbyte x = (sbyte)((byte)(pos >> 8) & MAX_SIZE);
            sbyte y = (sbyte)((byte)pos & MAX_SIZE);
            return new Vector2Int(x, y);
        }

        public static int CellLeft(int pos)
        {
            return GetOffsetIndex(pos, -1, 0);
        }

        public static int CellRight(int pos)
        {
            return GetOffsetIndex(pos, 1, 0);
        }

        public static int CellUp(int pos)
        {
            return GetOffsetIndex(pos, 0, 1);
        }

        public static int CellDown(int pos)
        {
            return GetOffsetIndex(pos, 0, -1);
        }

    }

    [Flags]
    public enum CellFlag
    {
        None = 0,
        Chessed,
        ConstructionBuilding = 2,
        ConstructionUnit = 4
    }
}
