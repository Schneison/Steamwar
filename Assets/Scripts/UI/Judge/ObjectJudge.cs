using Steamwar.Buildings;
using Steamwar.Core;
using System.Collections;
using UnityEngine;

namespace Steamwar.Judge
{
    public class ObjectJudge : SteamBehaviour
    {
        public ObjectEvent judgeChange;
        public ObjectEvent judgeClear;

        protected override void OnInit()
        {
            base.OnInit();
        }
    }
}