using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Steamwar.Objects;
using Steamwar.Grid;

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
