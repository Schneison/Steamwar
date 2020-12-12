using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;

namespace Steamwar.UI.Menus
{
    public struct SaveGame
    {
        public string path;
        public string name;

        public static SaveGame[] GetGames()
        {
            List<SaveGame> games = new List<SaveGame>();
            string[] savePaths = SaveManager.GetSavePaths();
            foreach(string path in savePaths){
                games.Add(new SaveGame()
                {
                    path = path,
                    name = Path.GetFileNameWithoutExtension(path)
                });
            }
            return games.ToArray();
        }
    }
}
