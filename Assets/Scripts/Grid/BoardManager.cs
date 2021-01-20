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
using UnityEngine.Events;
using System.Collections;
using System.IO;

namespace Steamwar.Grid
{
    public class BoardManager : Singleton<BoardManager>, IService
    {
        public UnityEngine.Grid world;
        [Header("World Layers")]
        public Tilemap ground;
        public Tilemap objects;
        [Header("World Overlay Layers")]
        public Tilemap chessOverlay;
        public Tilemap areaOverlay;

        [Header("Containers")]
        public GameObject unitContainer;
        public GameObject buildingContainer;

        [Header("Events")]
        public TileEvent tileAdded;
        public TileEvent tileRemoved;
        public TileChangeEvent tileChanged;

        // IBoardListener
        public ObjectEvent objectConstrcuted;
        // IBoardListener
        public ObjectEvent objectDeconstructed;

        public const byte MAX_SIZE = 64;
        public const byte MAX_HALF = MAX_SIZE / 2;
        public const int SHIFT_BITS = 7;//Max index for the tiles currently is 64 << 7 | 64. "7" is the number of bitysued by 64
        public CellWalker walker;

        public static ICellInfo GetInfo(Vector3Int pos)
        {
            return Board.GetCell(pos);
        }

        public static ICellInfo GetInfoWorld(Vector2 pos)
        {
            return GetInfo(WorldToCell(pos));
        }

        public static int GetHeight(Vector3Int pos)
        {
            ICellInfo info = GetInfo(pos);
            return info.Height;
        }

        public static Board Board => SessionManager.session.board;

        public static Tilemap GetMapForLayer(BoardLayerType layer) => layer switch
        {
            BoardLayerType.Ground => Instance.ground,
            BoardLayerType.ChessOverlay => Instance.chessOverlay,
            BoardLayerType.AreaOverlay => Instance.areaOverlay,
            _ => Instance.ground
        };

        public static Vector3Int WorldToCell(Vector3 pos)
        {
            return Instance.world.WorldToCell(pos);
        }

        public static Vector3Int UnitToCell(Vector3 pos)
        {
            return WorldToCell(pos - new Vector3(0.5F, 0.5F, 0F));
        }

        public static Vector3 CellToWorld(Vector3Int pos)
        {
            return Instance.world.CellToWorld(pos);
        }

        public static Vector3Int ScreenToCell(Vector3 pos)
        {
            return ScreenToCell(pos, SessionManager.Instance.mainCamera);
        }

        public static Vector3Int ScreenToCell(Vector3 pos, Camera camera)
        {
            return WorldToCell(camera.ScreenToWorldPoint(pos));
        }

        public static Vector2 ScreenToUnit(Vector3 pos)
        {
            return ScreenToUnit(pos, SessionManager.Instance.mainCamera);
        }

        public static Vector2 ScreenToUnit(Vector3 pos, Camera camera)
        {
            return WorldToUnit(camera.ScreenToWorldPoint(pos));
        }

        public static Vector2 WorldToUnit(Vector3 pos)
        {
            Vector3Int cellPos = WorldToCell(pos);
            return new Vector2(cellPos.x, cellPos.y) + new Vector2(0.5F, 0.5F);
        }

        public static TileBase GetTileWorld(Vector2 pos, BoardLayerType layer = BoardLayerType.Ground)
        {
            return GetTile(WorldToCell(pos), layer);
        }

        public static void SetTile(Vector2Int pos, TileBase tile, BoardLayerType type)
        {
            GetMapForLayer(type).SetTile(new Vector3Int(pos.x, pos.y, 0), tile);
        }

        public static TileBase GetTileWorld(Vector3 pos, BoardLayerType layer = BoardLayerType.Ground)
        {
            return GetTile(WorldToCell(pos), layer);
        }

        public static bool HasTile(Vector3Int position, BoardLayerType layer = BoardLayerType.Ground)
        {
            return GetMapForLayer(layer).HasTile(position);
        }

        public static TileBase GetTile(Vector3Int pos, BoardLayerType layer = BoardLayerType.Ground)
        {
            return GetMapForLayer(layer).GetTile(pos);
        }

        public static bool AnyTile(CellPos pos, out (TileBase, BoardLayerType) tile)
        {
            TileBase baseTile;
            foreach (BoardLayerType type in Enum.GetValues(typeof(BoardLayerType))) {
                if(type == BoardLayerType.None)
                {
                    continue;
                }
                baseTile = GetTile(pos, type);
                if (baseTile != null)
                {
                    tile = (baseTile, type);
                    return true;
                }
            }
            tile = (null, BoardLayerType.None);
            return false;
         }
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

        protected override void OnInit()
        {
            Services.board.Create<BoardManager>((state) => state == LifecycleState.SESSION, ()=>new ServiceContainer[] { Services.registry });
        }

        public IEnumerator Initialize()
        {
            walker = new CellWalker(Board, Vector3Int.zero, 30);
            yield return walker.Walk();
        }

        public IEnumerator CleanUp()
        {
            yield return null;
        }

        private void AddObjectsData(List<ObjectData> objects, params GameObject[] containers)
        {
            foreach (GameObject  container in containers) {
                foreach (Transform obj in container.transform)
                {
                    AddObjectData(objects, obj);
                }
            }
        }

        private void AddObjectData(List<ObjectData> objects, Transform obj)
        {
            ObjectContainer unit = obj.GetComponent<ObjectContainer>();
            if(unit == null)
            {
                return;
            }
            objects.Add(unit.Data.Copy());
        }

      /* public SectorBoard CreateBoard(GameObject boardObj)
       {
            Dictionary<string, Sectors.BoardCellData> tileDatas = new Dictionary<string, Sectors.BoardCellData>
            {
                { "dummy", new Sectors.BoardCellData() { index = 0 } }
            };
            List<ObjectData> objects = new List<ObjectData>();
            AddObjectsData(objects, unitContainer, buildingContainer);
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
                            tileDatas.Add(tile.id, new Sectors.BoardCellData()
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
            UnityEngine.Grid grid = world.GetComponent<UnityEngine.Grid>();
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

            Sectors.BoardCellData[] datas = board.tileDatas;
            BoardStorage tileStrorage = new BoardStorage(8, board.tiles);
            for (int xIndex = 0; xIndex < 64; xIndex++)
            {
                for (int yIndex = 0; yIndex < 64; yIndex++)
                {
                    int xPos = -32 + xIndex;
                    int yPos = -32 + yIndex;
                    Vector3Int cellPos = new Vector3Int(xPos, yPos, 0);
                    int tileIndex = tileStrorage[xIndex, yIndex];
                    Sectors.BoardCellData tileData = datas[tileIndex];
                    if (tileIndex > 0)
                    {
                        TileType tile = tileByName[tileData.id];
                        groundTiles.SetTile(cellPos, tile);
                    }
                }
            }
        }
    }

    [Serializable]
    public class ObjectEvent : UnityEvent<ObjectContainer>
    {
    }

    [Serializable]
    public class TileEvent : UnityEvent<TileBase, BoardLayerType, ICellInfo, Board>
    {

    }

    [Serializable]
    public class TileChangeEvent : UnityEvent<(TileBase, TileBase), BoardLayerType, ICellInfo, Board>
    {

    }
}
