using Steamwar.Objects;
using System.Collections;
using UnityEngine;

namespace Steamwar.UI
{
    public class JudgeHealthIndicator : AbstractIndicator, IJudgeListener
    {
        public override int MaxBarCount => 7;
        public ObjectContainer objectContainer;

        public void OnJudgeChange(ObjectContainer container)
        {
            this.objectContainer = container;
            InitIndicator((int)container.Data.Health, (int)container.Type.Health);
        }

        public void FixedUpdate()
        {
            if(objectContainer != null)
            {
                UpdateIndicator((int)objectContainer.Data.Health);
            }
        }

        public void OnJudgeClear(ObjectContainer container)
        {
            this.objectContainer = null;
            ClearIndicator();
        }
    }
}