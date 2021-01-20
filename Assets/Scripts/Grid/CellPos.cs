using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Steamwar.Grid
{
    [Serializable]
    public struct CellPos : IEquatable<CellPos>
    {
        public static readonly CellPos s_Zero = new CellPos(0, 0);
        /// <summary>
        /// Maximal value of a position on one axis on the board
        /// </summary>
        public const byte BOARD_MASK = 0b11111111;
        /// <summary>
        /// Maximal value of a position on one axis in one chunk
        /// </summary>
        public const byte REGION_MASK = 0b00000111; // 7 -> Chunk Lenght 8
        public const byte CHUNK_INDEX_MASK = CHUNK_MASK << REGION_BITS;
        public const byte CHUNK_MASK = 0b11111;
        /// <summary>
        /// Amount of bits of the board mask (255 = 8)
        /// </summary>
        public const byte BOARD_BITS = 8; // 
        /// <summary>
        /// Amount of bits of the chunk mask (7 = 3)
        /// </summary>
        public const byte REGION_BITS = 3;
        public const byte CHUNK_BITS = 5;

        [SerializeField]
        private int x;
        [SerializeField]
        private int y;
        private BoardLayerType layer;

        public CellPos(Vector3Int pos, BoardLayerType layer = BoardLayerType.None) : this(pos.x, pos.y, layer)
        {
        }

        public CellPos(Vector2Int pos, BoardLayerType layer = BoardLayerType.None) : this(pos.x, pos.y, layer)
        {
        }

        public CellPos(int _x, int _y, BoardLayerType layer = BoardLayerType.None)
        {
            x = _x;
            y = _y;
            this.layer = layer;
        }

        public CellPos(int index, BoardLayerType layer = BoardLayerType.None)
        {
            x = (sbyte)((byte)(index >> BOARD_BITS) & BOARD_MASK);
            y = (sbyte)((byte)index & BOARD_MASK);
            this.layer = layer;
        }

        public static CellPos FromChunk(int chunkPos, BoardLayerType layer = BoardLayerType.None)
        {
            int x = (sbyte)((byte)(chunkPos >> CHUNK_BITS) & CHUNK_MASK) << REGION_BITS;
            int y = (sbyte)((byte)chunkPos & CHUNK_MASK) << REGION_BITS;
            return new CellPos(x, y, layer);
        }

        public Vector3Int GetTilemapPos()
        {
            return new Vector3Int(X, Y, 0);
        }

        public readonly int X => x;

        public readonly int Y => y;

        public int ChunkIndex
        {
            get
            {
                byte x = unchecked((byte)X);
                byte y = unchecked((byte)Y);
                return (byte)(x & CHUNK_INDEX_MASK) << (CHUNK_BITS - REGION_BITS) | (y & CHUNK_INDEX_MASK) >> REGION_BITS;
            }
        }
        public int RegionIndex
        {
            get
            {
                byte x = unchecked((byte)X);
                byte y = unchecked((byte)Y);
                return (byte)(x & REGION_MASK) << REGION_BITS | y & REGION_MASK;
            }
        }

        public readonly int PosIndex
        {
            get
            {
                byte x = unchecked((byte)X);
                byte y = unchecked((byte)Y);
                return ((int)(x & BOARD_MASK)) << BOARD_BITS | y & BOARD_MASK;
            }
        }

        public readonly int RegionX
        {
            get
            {
                byte x = unchecked((byte)X);
                return x & REGION_MASK;
            }
        }

        public readonly int RegionY
        {
            get
            {
                byte y = unchecked((byte)Y);
                return y & REGION_MASK;
            }
        }

        public readonly int ChunkX
        {
            get
            {
                byte x = unchecked((byte)X);
                return x & CHUNK_INDEX_MASK >> REGION_BITS;
            }
        }

        public readonly int ChunkY
        {
            get
            {
                byte y = unchecked((byte)Y);
                return y & CHUNK_INDEX_MASK >> REGION_BITS;
            }
        }

        public BoardLayerType Layer { get => layer; }

        public readonly CellPos Add(int x, int y)
        {
            return new CellPos(X + x, Y + y, layer);
        }

        public readonly CellPos Sub(int x, int y)
        {
            return new CellPos(X - x, Y - y, layer);
        }

        public readonly CellPos AddChunk(int x, int y)
        {
            return new CellPos(X + x << REGION_BITS, Y + y << REGION_BITS, layer);
        }

        public readonly CellPos SubChunk(int x, int y)
        {
            return new CellPos(X - x << REGION_BITS, Y - y << REGION_BITS, layer);
        }

        public readonly CellPos FromRegion(int regionIndex)
        {
            return new CellPos(X + ((regionIndex >> REGION_BITS) & REGION_MASK), Y + y & REGION_MASK, layer);
        }

        public readonly CellPos Left()
        {
            return new CellPos(X - 1, Y, layer);
        }

        public readonly CellPos Right()
        {
            return new CellPos(X + 1, Y, layer);
        }

        public readonly CellPos Down()
        {
            return new CellPos(X, Y - 1, layer);
        }

        public readonly CellPos Up()
        {
            return new CellPos(X, Y + 1, layer);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CellPos operator +(CellPos a, CellPos b)
        {
            return new CellPos(a.x + b.x, a.y + b.y, a.Layer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CellPos operator -(CellPos a, CellPos b)
        {
            return new CellPos(a.x - b.x, a.y - b.y, a.Layer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CellPos(Vector3Int v)
        {
            return new CellPos(v.x, v.y, BoardLayerType.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CellPos(Vector2Int v)
        {
            return new CellPos(v.x, v.y, BoardLayerType.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3Int(CellPos v)
        {
            return new Vector3Int(v.x, v.y, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2Int(CellPos v)
        {
            return new Vector2Int(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CellPos lhs, CellPos rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(CellPos lhs, CellPos rhs)
        {
            return !(lhs == rhs);
        }

        //
        // Summary:
        //     Returns true if the objects are equal.
        //
        // Parameters:
        //   other:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
        {
            if (!(other is CellPos))
            {
                return false;
            }

            return Equals((CellPos)other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CellPos other)
        {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }


        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format(CultureInfo.InvariantCulture.NumberFormat, "({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }

    }
}
