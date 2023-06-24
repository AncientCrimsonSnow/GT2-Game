using Features.TileSystem.Scripts.Registrator;
using UnityEngine;

public class SkeletonRegistrator : BaseTileRegistrator
{
    [SerializeField] private GameObject skeletonToInstantiate;

    private GameObject _instantiatedSkeleton;
    
    public override void RegisterOnTile()
    {
        /*
        var internalTransform = transform;
        _instantiatedSkeleton = Instantiate(skeletonToInstantiate, internalTransform.position, Quaternion.identity,
            internalTransform);*/
    }

    public override void UnregisterOnTile()
    {
        /*
        if (_instantiatedSkeleton != null)
        {
            Destroy(_instantiatedSkeleton);
        }
        else
        {
            Debug.Log("Skeleton already got destroyed");
        }*/
    }
}
