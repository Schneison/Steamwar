using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Steamwar.Objects;
using Steamwar.Grid;
using System.IO;

public class BoardTest
{
    private Board board;

    public Board Board
    {
        get
        {
            if (board == null)
            {
                board = new Board();
            }
            return board;
        }
    }

    [Test]
    public void BoardInitialisation()
    {
        InvalidCell(new Vector2Int(128, 127));
        ValidCell(new Vector2Int(127, 127));
        InvalidCell(new Vector2Int(-129, -128));
        ValidCell(new Vector2Int(-128, 127));
    }

    [Test]
    public void CellPositionConvertion()
    {
        for (int x = -5; x <= 5; x++)
        {
            for (int y = -5; y <= 5; y++)
            {
                CellPos startPos = new CellPos(x, y);
                CellPos endPos = new CellPos(startPos.PosIndex);
                Debug.Log($"start={startPos}, end={endPos}, index={startPos.PosIndex}");
                Assert.True(startPos == endPos, $"Board convertion failed. start={startPos}, end={endPos}, index={startPos.PosIndex}");
            }
        }
    }

    [Test]
    public void SaveAndLoad()
    {
        static byte idCallback(string name) => name == "a" ? (byte)0 : (byte)1;
        static string nameCallback(byte id) => id == 1 ? "a" : "b";
        BoardChunk chunk = new BoardChunk(new CellPos(48, -16));
        chunk.SetTile(new CellPos(48, -16, BoardLayerType.Ground).Add(5, 7), "a", true);
        chunk.SetTile(new CellPos(48, -16, BoardLayerType.Ground).Add(2, 1), "a", true);
        chunk.SetTile(new CellPos(48, -16, BoardLayerType.Ground).Add(6, 2), "b", true);
        chunk.SetTile(new CellPos(48, -16, BoardLayerType.Ground).Add(0, 7), "b", true);
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter cellWriter = new BinaryWriter(memoryStream);
        chunk.Serialize(cellWriter, idCallback);
        memoryStream.Position = 0;
        using BinaryReader cellReader = new BinaryReader(memoryStream);
        BoardChunk chunkNew = new BoardChunk(new CellPos(48, -16));
        chunkNew.Deserialize(cellReader, nameCallback);
        chunkNew.ToString();
    }

    private void InvalidCell(Vector2Int pos)
    {
        ICellInfo cell = Board.GetCell(pos);
        Assert.False(cell.Exists, $"{cell} should not exist.");
    }

    private void ValidCell(Vector2Int pos)
    {
        ICellInfo cell = Board.GetCell(pos);
        Assert.True(cell.Exists, $"{cell} should exist.");
    }
}
