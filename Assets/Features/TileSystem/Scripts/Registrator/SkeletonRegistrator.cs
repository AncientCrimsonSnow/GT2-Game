using UnityEngine;

namespace Features.TileSystem.Scripts.Registrator
{
    public class SkeletonRegistrator : BaseTileRegistrator
    {
        [SerializeField] private GameObject skeletonToInstantiate;

        protected override void InternalRegisterOnTile()
        {
            Instantiate(skeletonToInstantiate, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        protected override void UnregisterOnTile() { }
    }
}
