using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Jobs;
using Unity.Collections;
using Steamwar.Objects;
using Steamwar.Buildings;
using Steamwar.Units;

namespace Steamwar
{
    public class SaveManager
    {

        public static readonly string SAVE_NAME = "default";
        public static readonly string LEVEL_FILE = "level.dat";
        public static readonly string FILE_EXTENSION = ".swsave";

        public static void Save(string fileName)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream saveGame = File.Create(Application.persistentDataPath + "/saves/" + fileName + FILE_EXTENSION);
            binaryFormatter.Serialize(saveGame, SessionManager.session.CreateData());
            saveGame.Close();
            FileStream jsonGame = File.Create(Application.persistentDataPath + "/saves/" + fileName + ".json");
            StreamWriter writter = new StreamWriter(jsonGame);
            writter.Write(JsonUtility.ToJson(SessionManager.session.CreateData()));
            writter.Close();
            jsonGame.Close();
            PlayerPrefs.SetString("lastSave", fileName);
        }

        public static void Save(Session game)
        {
            //string jsonGame = JsonUtility.ToJson(game);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (!Directory.Exists(Application.persistentDataPath + "/saves/" + SAVE_NAME))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + SAVE_NAME);
            }
            FileStream saveGame = File.Create(Application.persistentDataPath + "/saves/" + SAVE_NAME + "/" + LEVEL_FILE);
            binaryFormatter.Serialize(saveGame, game.CreateData());
            saveGame.Close();
        }

        public static void Load(string path)
        {
            Session game;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (File.Exists(path))
            {
                FileStream saveGame = null;
                try
                {
                    saveGame = File.Open(path, FileMode.Open);
                    game = ((SessionData)binaryFormatter.Deserialize(saveGame)).CreateGame();
                }
                catch (Exception e)
                {
                    game = new Session();
                    Debug.Log("Failed to load game: " + e);
                }
                finally
                {
                    if (saveGame != null)
                    {
                        saveGame.Close();
                    }
                }
            }

            else
            {
                game = new Session();

            }
            SessionManager.Instance.Setup(game);
        }

        public static void Load(out Session session)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if(File.Exists(Application.persistentDataPath + "/saves/" + SAVE_NAME + "/" + LEVEL_FILE))
            {
                FileStream saveGame = null;
                try
                {
                    saveGame = File.Open(Application.persistentDataPath + "/saves/" + SAVE_NAME + "/" + LEVEL_FILE, FileMode.Open);
                    session = ((SessionData)binaryFormatter.Deserialize(saveGame)).CreateGame();
                }
                catch (Exception e)
                {
                    session = new Session();
                    Debug.Log("Failed to load game: " + e);
                    //throw e;
                }
                finally
                {
                    if(saveGame != null)
                    {
                        saveGame.Close();
                    }
                }
            }

            else
            {
                session = new Session();

            }
            SessionManager.Instance.rounds.OnLoad(session);
        }

        public static string[] GetSavePaths()
        {
            string dir = Application.persistentDataPath + "/saves/";
            return Directory.EnumerateFiles(dir, "*" + FILE_EXTENSION, SearchOption.TopDirectoryOnly).ToArray();
        }
    }
}
