using UnityEngine;

namespace Features.TileSystem
{
    public class ResourceBase : IResource
    {
        public string ResourceName { get; private set; }
        public GameObject GameObject { get; private set; }

        public ResourceBase(string resourceName, GameObject gameObject)
        {
            ResourceName = resourceName;
            GameObject = gameObject;
        }
    }

    public interface IResource
    {
        string ResourceName { get; }
        
        GameObject GameObject { get; }
    }
}