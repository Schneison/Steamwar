using Steamwar;
using Steamwar.Interaction;
using System.Collections;
using UnityEngine;

namespace Steamwar.Objects
{
    public class DestroyableSelection : SelectionBehaviour
    {
        private HealthIndicator healthIndicator;
        private ProgressIndicator progressIndicator;

        protected override void OnInit()
        {
            base.OnInit();
            healthIndicator = GetComponentInChildren<HealthIndicator>(true);
            progressIndicator = GetComponentInChildren<ProgressIndicator>(true);
        }

        public override void OnDeselection()
        {
            if (healthIndicator != null)
            {
                healthIndicator.gameObject.SetActive(false);
            }
            if (progressIndicator != null)
            {
                progressIndicator.gameObject.SetActive(false);
            }
        }

        public override void OnSelection()
        {
            if (healthIndicator != null)
            {
                healthIndicator.gameObject.SetActive(true);
            }
            if (progressIndicator != null && progressIndicator.IsVisible())
            {
                progressIndicator.gameObject.SetActive(true);
            }
        }
    }
}