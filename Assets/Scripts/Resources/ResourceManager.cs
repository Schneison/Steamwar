using System.Collections;
using UnityEngine;
using Steamwar.Utils;

namespace Steamwar.Resources
{
    public class ResourceManager : Singleton<ResourceManager>
    {

        public delegate void ResourceUpdateEvent(int factionId, Resource resource, int amount, int maxAmount);


        void Update()
        {

        }
    }
}