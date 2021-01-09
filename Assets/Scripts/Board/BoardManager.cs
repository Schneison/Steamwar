using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Steamwar.Sectors;
using Steamwar.Buildings;
using Steamwar.Units;
using Steamwar.Utils;
using Steamwar.Objects;

using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Steamwar.Board
{
    public class BoardManager
    {
        public const byte MAX_SIZE = 64;
        public const byte MAX_HALF = MAX_SIZE / 2;
        public const int SHIFT_BITS = 7;//Max index for the tiles currently is 64 << 7 | 64. "7" is the number of bitysued by 64

        public static void ApplyObjects(BoardObjects objects)
        {
           // ApplyObjects(objects.buildings);
           // ApplyObjects(objects.units);
        }

        private static void ApplyObjects<D>(List<D> objects) where D : ObjectData, new()
        {
            foreach(D obj in objects)
            {
                ConstructionManager.CreateObjectFromData(obj);
            }
        }

       /* public static SectorBoard CreateBoard(GameObject boardObj)
        {
            Transform world = boardObj.transform.Find("World");
            if (world == null)
            {
                Debug.Log("Missing World Object in the selected board object");
                return null;
            }
            Grid grid = world.GetComponent<Grid>();
            if (grid == null)
            {
                Debug.Log("Missing Grid Object in the selected board object");
                return null;
            }
            Transform ground = world.Find("Ground");
            if (ground == null)
            {
                Debug.Log("Missing Ground Object in the selected board object");
                return null;
            }
            Tilemap groundTiles = ground.GetComponent<Tilemap>();
            if (grid == null)
            {
                Debug.Log("Missing Tilemap Object in the selected board object");
                return null;
            }
            Transform buildingContainer = boardObj.transform.Find("Buildings");
            if (buildingContainer == null)
            {
                Debug.Log("Missing Buildings Object in the selected board object");
                return null;
            }
            Transform unitContainer = boardObj.transform.Find("UnitController");
            if (unitContainer == null)
            {
                Debug.Log("Missing UnitController Object in the selected board object");
                return null;
            }
            Dictionary<string, Sectors.TileData> tileDatas = new Dictionary<string, Sectors.TileData>
            {
                { "dummy", new Sectors.TileData() { index = 0 } }
            };
            List<UnitData> units = new List<UnitData>();
            List<BuildingData> buildings = new List<BuildingData>();
            foreach (Transform unitObj in unitContainer)
            {
                UnitContainer unit = unitObj.GetComponent<UnitContainer>();
                if (unit != null)
                {
                    units.Add(unit.Data.Copy());
                }
            }
            foreach (Transform buildingObj in buildingContainer)
            {
                BuildingContainer building = buildingObj.GetComponent<BuildingContainer>();
                if (building != null)
                {
                   buildings.Add(building.Data.Copy());
                }
            }
            BoardStorage tileStorage = new BoardStorage(MAX_SIZE, 8);
            byte tileId = 1;
            for (byte xIndex = 0; xIndex < MAX_SIZE; xIndex++)
            {
                for (byte yIndex = 0; yIndex < MAX_SIZE; yIndex++)
                {
                    int xPos = -MAX_HALF + xIndex;
                    int yPos = -MAX_HALF + yIndex;
                    Vector3Int position = new Vector3Int(xPos, yPos, 0);
                    TileType tile = groundTiles.GetTile<TileType>(position);
                    if (tile != null)
                    {
                        byte index;
                        if (!tileDatas.ContainsKey(tile.id))
                        {
                            index = tileId++;
                            tileDatas.Add(tile.id, new Sectors.TileData()
                            {
                                index = index,
                                id = tile.id
                            });
                        }
                        else
                        {
                            index = tileDatas[tile.id].index;
                        }
                        tileStorage[xIndex, yIndex] = index;
                    }
                }
            }
            return new SectorBoard()
            {
                tileDatas = (from data in tileDatas.Values
                             orderby data.index
                             select data).ToArray(),
                objects = new BoardObjects
                {
                    units = units,
                    buildings = buildings,
                },
                tiles = tileStorage.ToArray(),
                version = "1.0"
            };
        }*/

        public static void ApplyBoard(GameObject boardObj, SectorBoard board)
        {
            Transform world = boardObj.transform.Find("World");
            if (world == null)
            {
                Debug.Log("Missing World Object in the selected board object");
                return;
            }
            Grid grid = world.GetComponent<Grid>();
            if (grid == null)
            {
                Debug.Log("Missing Grid Object in the selected board object");
                return;
            }
            Transform ground = world.Find("Ground");
            if (ground == null)
            {
                Debug.Log("Missing Ground Object in the selected board object");
                return;
            }
            Tilemap groundTiles = ground.GetComponent<Tilemap>();
            if (grid == null)
            {
                Debug.Log("Missing Tilemap Object in the selected board object");
                return;
            }
            Transform buildings = boardObj.transform.Find("Buildings");
            if (buildings == null)
            {
                Debug.Log("Missing Buildings Object in the selected board object");
                return;
            }
            Transform units = boardObj.transform.Find("UnitController");
            if (units == null)
            {
                Debug.Log("Missing UnitController Object in the selected board object");
                return;
            }
            Dictionary<string, TileType> tileByName = new Dictionary<string, TileType>();
            foreach (TileType type in ScriptableObjectUtility.GetAllInstances<TileType>())
            {
                tileByName.Add(type.id, type);
            }
            groundTiles.ClearAllTiles();

            Sectors.TileData[] datas = board.tileDatas;
            BoardStorage tileStrorage = new BoardStorage(8, board.tiles);
            for (int xIndex = 0; xIndex < 64; xIndex++)
            {
                for (int yIndex = 0; yIndex < 64; yIndex++)
                {
                    int xPos = -32 + xIndex;
                    int yPos = -32 + yIndex;
                    Vector3Int cellPos = new Vector3Int(xPos, yPos, 0);
                    int tileIndex = tileStrorage[xIndex, yIndex];
                    Sectors.TileData tileData = datas[tileIndex];
                    if (tileIndex > 0)
                    {
                        TileType tile = tileByName[tileData.id];
                        groundTiles.SetTile(cellPos, tile);
                    }
                }
            }
        }
    }
}
