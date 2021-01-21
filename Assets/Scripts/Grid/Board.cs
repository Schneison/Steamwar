using Steamwar.Objects;
using Steamwar.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Steamwar.Grid
{
    /// <summary>
    /// Keeps track of every object that is currently on the board.
    /// </summary>
    public class Board
    {
        public const int CHUNK_AMOUNT = 32 * 32; // 256 / 8
        public const int CHUNK_BYTE_AMOUNT = CHUNK_AMOUNT / 8;
        public const sbyte MAX_POSITION_SIZE = sbyte.MaxValue;
        public const sbyte MIN_POSITION_SIZE = sbyte.MinValue;
        public const int ARRAY_SIZE = ushort.MaxValue + 1;

        /// <summary>
        /// Maximal value of a position on one axis on the board
        /// </summary>
        public const byte BOARD_MASK = byte.MaxValue;
        /// <summary>
        /// Maximal value of a position on one axis in one chunk
        /// </summary>
        public const byte CHUNK_MASK = 7; // 7 -> Chunk Lenght 8
        public const byte CHUNK_POS_MASK = 31;
        /// <summary>
        /// Amount of bits of the board mask (255 = 8)
        /// </summary>
        public const byte BOARD_BITS = 8; // 
        /// <summary>
        /// Amount of bits of the chunk mask (7 = 3)
        /// </summary>
        public const byte CHUNK_BITS = 3;
        public const byte CHUNK_POS_BITS = 5;

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
        private BoardChunk[] chunks = new BoardChunk[0];
        private BitArray occupiedChunks;
        public Board()
        {
            chunks = new BoardChunk[CHUNK_AMOUNT];
            this.occupiedChunks = new BitArray(CHUNK_AMOUNT);
        }

        public void Deserialize(BinaryReader reader, TileNameCallback tileRegistry)
        {
            byte[] chunksOccupied = reader.ReadBytes(32);
            occupiedChunks = new BitArray(chunksOccupied);
            int lenght = reader.ReadInt32();
            byte[] cellBytes = reader.ReadBytes(lenght);
            using MemoryStream memoryStream = new MemoryStream(cellBytes);
            using BinaryReader cellReader = new BinaryReader(memoryStream);
            for (int i = 0; i < occupiedChunks.Length; i++)
            {
                if (!occupiedChunks.Get(i))
                {
                    continue;
                }
                if (chunks[i] == null)
                {
                    chunks[i] = new BoardChunk(CellPos.FromChunk(i));
                }
                chunks[i].Deserialize(cellReader, tileRegistry);
            }
        }

        public void Serialize(BinaryWriter writer, TileIdCallback tileRegistry)
        {
            byte[] cellOccupied = new byte[CHUNK_BYTE_AMOUNT];
            occupiedChunks.CopyTo(cellOccupied, 0);
            writer.Write(cellOccupied);
            int oldPos = (int)writer.BaseStream.Position;
            writer.Write(0);
            using MemoryStream memoryStream = new MemoryStream();
            using (BinaryWriter cellWriter = new BinaryWriter(memoryStream))
            {
                for (int i = 0; i < occupiedChunks.Length; i++)
                {
                    if (!occupiedChunks.Get(i))
                    {
                        continue;
                    }
                    chunks[i].Serialize(cellWriter, tileRegistry);
                }
            }
            writer.Write(memoryStream.ToArray());
            int newPos = (int)writer.BaseStream.Position;
            writer.BaseStream.Position = oldPos;
            writer.Write(newPos - oldPos - 4);
            writer.BaseStream.Position = newPos;
        }

        public bool HasChunk(CellPos pos)
        {
            return GetChunk(pos, false) != null;
        }

        public BoardChunk GetChunk(CellPos pos, bool createEmpty = false)
        {
            int chunkIndex = pos.ChunkIndex;
            if (chunks.Length > chunkIndex)
            {
                if(createEmpty && chunks[chunkIndex] == null)
                {
                    chunks[chunkIndex] = new BoardChunk(chunkIndex);
                    occupiedChunks.Set(chunkIndex, true);
                }
                return chunks[chunkIndex];
            }
            return null;
        }

        public bool SetTile(CellPos pos, TileBase tile)
        {
            BoardChunk chunk = GetChunk(pos, true);
            if(chunk == null)
            {
                return false;
            }
            chunk.SetTile(pos, tile, true, true);
            return true;
        }

        public bool RemoveTile(CellPos pos)
        {
            BoardChunk chunk = GetChunk(pos, true);
            if (chunk == null)
            {
                return false;
            }
            chunk.RemoveTile(pos);
            return true;
        }

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

        public ICellInfo GetCell(CellPos pos, bool createEmpty = false)
        {
            if (pos.X > MAX_POSITION_SIZE || pos.X < MIN_POSITION_SIZE)
            {
                return CellInfo.Empty;
            }
            if (pos.Y > MAX_POSITION_SIZE || pos.Y < MIN_POSITION_SIZE)
            {
                return CellInfo.Empty;
            }
            BoardChunk chunk = GetChunk(pos, createEmpty);
            return chunk != null ? chunk.GetCell(pos, createEmpty) : CellInfo.Empty;
        }

        public bool HasCell(CellPos pos)
        {
            BoardChunk chunk = GetChunk(pos, false);
            return chunk != null && chunk.HasCell(pos);
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
