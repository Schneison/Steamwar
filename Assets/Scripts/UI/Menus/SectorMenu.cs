using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Steamwar.Sectors;
using Steamwar.Utils;

namespace Steamwar.UI.Menus
{
    public class SectorMenu : MonoBehaviour
    {
        public Image sectorPreview;
        public Text sectorName;
        public Button nextButton;
        public Button prevButton;

        private Sector[] sectors;
        private int selectedSector = 0;

        public void Start()
        {
            sectors = ScriptableObjectUtility.GetAllInstances<Sector>();
            UpdateSection();
        }

        public void StartSector()
        {
            SessionManager.instance.sectors.StartSector(sectors[selectedSector]);
            MainMenu.instance.mainButtons.gameObject.SetActive(true);
            MainMenu.instance.sectorMenu.gameObject.SetActive(false);
        }

        public void Next()
        {
            selectedSector++;
            UpdateSection();
        }

        public void Previous()
        {
            selectedSector--;
            UpdateSection();
        }

        public void UpdateSection()
        {
            if (selectedSector >= sectors.Length)
            {
                selectedSector = sectors.Length - 1;
            }
            prevButton.gameObject.SetActive((selectedSector - 1) >= 0);
            nextButton.gameObject.SetActive((selectedSector + 1) < sectors.Length);
            Sector sector = sectors[selectedSector];
            sectorName.text = sector.name;
        }
    }
}
