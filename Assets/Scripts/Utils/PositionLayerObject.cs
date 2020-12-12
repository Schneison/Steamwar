using UnityEngine;
using System.Collections;

namespace Steamwar.Utils
{
    public class PositionLayerObject : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (transform.hasChanged)
            {
                spriteRenderer.sortingOrder = (int)(transform.position.y * -1);
                transform.hasChanged = false;
            }
        }
    }
}
