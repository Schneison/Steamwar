using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

using Steamwar.Grid;
using Steamwar.Sectors;
using Steamwar.Buildings;
using Steamwar.Units;
using Steamwar.Utils;

using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Steamwar.InternalEditor
{
    public class SectorEditor : EditorWindow
    {
        string assetPath;
        SectorInfo sector;
        SectorBoard board = null;
        string boardPath = "";
        GameObject boardObj;
        GameObject loadBoard;
        public static int[] tileDataArray;

        [MenuItem("Window/Sector Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(SectorEditor), false, "Sector Editor");
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = Selection.activeObject != null && Selection.activeObject is Sector;
            if (GUILayout.Button("Load Sector"))
            {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out string guiid, out long id))
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guiid);
                    if (assetPath.Length != 0)
                    {
                        LoadAsset(assetPath);
                    }
                }
            }
            GUI.enabled = assetPath != null && assetPath.Length > 0;
            if (GUILayout.Button("Save Sector"))
            {
                SaveAsset();
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load Sector From File"))
            {
                string pathName = EditorUtility.OpenFilePanel("Select Sector File", "Objects/Sectors", "asset");
                if (pathName.Length != 0)
                {
                    string assetPath = pathName.Substring(pathName.LastIndexOf("Assets"));
                    string guiid = AssetDatabase.AssetPathToGUID(assetPath);
                    if (guiid.Length != 0)
                    {
                        LoadAsset(assetPath);
                    }
                }
            }
            if (GUILayout.Button("Save Sector To File"))
            {
                string pathName = EditorUtility.SaveFilePanelInProject("Select Sector File", "New Sector", "asset", "Please select a name and location to save the sector file to", "Objects/Sectors");
                if (pathName.Length != 0)
                {
                    assetPath = pathName;
                    SaveAsset();
                }
            }
            EditorGUILayout.EndHorizontal();
            sector.id = EditorGUILayout.TextField("Id", sector.id);
            sector.diplayName = EditorGUILayout.TextField("Display Name", sector.diplayName);
            sector.center = EditorGUILayout.Vector2Field("Center", sector.center);
            sector.size = EditorGUILayout.Vector2Field("Size", sector.size);
            sector.background = EditorGUILayout.ObjectField("Background", sector.background, typeof(Sprite), false);
            sector.roundsMax = EditorGUILayout.IntSlider("Rounds", sector.roundsMax, 1, 999);
            EditorGUILayout.BeginHorizontal();
            boardObj = EditorGUILayout.ObjectField("Board Object", boardObj, typeof(GameObject), true) as GameObject;
            if (boardObj == null)
            {
                board = null;
            }
            GUI.enabled = boardObj != null;
            if (GUILayout.Button("Load Board Data"))
            {
                //board = BoardManager.CreateBoard(boardObj);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = true;
            if (GUILayout.Button("Load Board From File"))
            {
                string pathName = EditorUtility.OpenFilePanel("Select Board File", "Objects/Boards", "json");
                if (pathName.Length != 0)
                {
                    string assetPath = pathName.Substring(pathName.LastIndexOf("Assets"));
                    string guiid = AssetDatabase.AssetPathToGUID(assetPath);
                    if (guiid.Length != 0)
                    {
                        LoadBoard(assetPath);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = board != null && sector.id != null && sector.id.Length != 0;
            if (GUILayout.Button("Save Board Data"))
            {
                SaveBoard("Assets/Objects/Boards/" + sector.id + ".json");
            }
            GUI.enabled = board != null;
            if (GUILayout.Button("Save Board To File"))
            {
                string pathName = EditorUtility.SaveFilePanelInProject("Select Board File", "New Board", "json", "Please select a name and location to save the board file to", "Objects/Boards");
                if (pathName.Length != 0)
                {
                    SaveBoard(pathName);
                }
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            loadBoard = EditorGUILayout.ObjectField("Load Board Object", loadBoard, typeof(GameObject), true) as GameObject;
            if (GUILayout.Button("Load Board"))
            {
                BoardManager.ApplyBoard(loadBoard, board);
            }
            EditorGUILayout.EndVertical();
            /*if (GUILayout.Button("Convert Tiles"))
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string[] assetGUIDs = Selection.assetGUIDs;
                foreach(string guid in assetGUIDs)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Tile));
                    if(obj != null)
                    {
                        Tile tile = obj as Tile;
                        TileType type = new TileType
                        {
                            colliderType = tile.colliderType,
                            color = tile.color,
                            id = tile.name,
                            displayName = textInfo.ToTitleCase(tile.name).Replace('_', ' '),
                            sprite = tile.sprite,
                            flags = tile.flags,
                            name = tile.name,
                            hideFlags = tile.hideFlags,
                            gameObject = tile.gameObject,
                            transform = tile.transform
                        };
                        AssetDatabase.CreateAsset(type, assetPath);
                    }
                }
            }*/
        }

        public void LoadAsset(string assetPath)
        {
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sector));
            if (obj is Sector)
            {
                Sector data = obj as Sector;
                sector.id = data.id;
                sector.diplayName = data.diplayName;
                sector.center = data.center;
                sector.size = data.size;
                sector.background = data.background;
                sector.roundsMax = data.roundsMax;
                sector.boardPath = data.boardPath;
                this.assetPath = assetPath;
            }
        }

        public void SaveAsset()
        {
            AssetDatabase.CreateAsset(new Sector()
            {
                id = sector.id,
                diplayName = sector.diplayName,
                center = sector.center,
                size = sector.size,
                background = sector.background as Sprite,
                roundsMax = sector.roundsMax,
                boardPath = sector.boardPath,
            }, assetPath);
        }

        public void SaveBoard(string assetPath)
        {
            if (board == null)
            {
                return;
            }
            FileStream jsonGame = File.Create(Application.dataPath + assetPath.Replace("Assets", ""));
            StreamWriter writter = new StreamWriter(jsonGame);
            writter.Write(EditorJsonUtility.ToJson(board, false));
            writter.Close();
            jsonGame.Close();
        }

        public void LoadBoard(string assetPath)
        {
            this.boardPath = assetPath;
        }
    }

    public struct SectorInfo
    {
        public string id;
        public string diplayName;
        public Vector2 center;
        public Vector2 size;
        public UnityEngine.Object background;
        public int roundsMax;
        public string boardPath;
    }
}
