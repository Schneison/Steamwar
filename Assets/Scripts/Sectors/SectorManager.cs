using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Steamwar.Sectors
{
    public class SectorManager : MonoBehaviour
    {
        public Sector selected;

        //GUI
        public void OpenSelection()
        {

        }

        //GUI
        public void Select()
        {
            //GUI: Select a Sector
            selected = new Sector();
        }

        public void StartSector()
        {
            StartSector(selected);
        }

        public void StartSector(Sector sector)
        {
            SessionManager.session.activeSector = new SectorData(sector);
            sector.StartSector(SessionManager.session);
        }

        public void Load(SectorData sector)
        {

        }

        public void End()
        {

        }

        public void Restart()
        {
            SectorData sectorData = SessionManager.session.activeSector;
            Sector sector = sectorData.sector;

        }
    }
}
