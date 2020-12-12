using UnityEngine;
using System.Collections;

namespace Steamwar.Renderer
{
    public class SessionRenderer : MonoBehaviour
    {
        public static SessionRenderer instance;
        internal SelectionManager selection;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(gameObject);
                return;
            }
            selection = GetComponent<SelectionManager>();
        }

        void Update()
        {

        }
    }
}
