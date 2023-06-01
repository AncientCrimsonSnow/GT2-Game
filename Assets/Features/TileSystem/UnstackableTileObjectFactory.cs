using DataStructures.StateLogic;
using UnityEngine;

namespace Features.TileSystem
{
    [CreateAssetMenu(fileName = "Stackable Resource Tile Context Factory", menuName = "TileContext/Factory/Stackable Resource")]
    public class UnstackableTileObjectFactory : BaseTileObjectFactory
    {
        [SerializeField] private string itemName;
        [SerializeField] private GameObject prefab;

        protected override ITileObjectComponent InternalGenerateAndRegister(TileBase tileBase, Quaternion rotation, Transform parent = null)
        {
            var position = new Vector3(tileBase.WorldPosition.x, 0, tileBase.WorldPosition.x);
            var instantiateObject = Instantiate(prefab, position, rotation);
            if (parent != null) instantiateObject.transform.SetParent(parent);
            return new UnstackableTileObjectComponent(this, instantiateObject);
        }
    }
}