using UnityEngine;
using System.Collections;

namespace Steamwar.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}