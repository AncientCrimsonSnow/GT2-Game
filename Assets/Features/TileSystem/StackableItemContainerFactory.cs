using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    [CreateAssetMenu(fileName = "Stackable Resource Tile Context Factory", menuName = "TileContext/Factory/Stackable Resource")]
    public class StackableItemContainerFactory : BaseItemContainerFactory
    {
        [SerializeField] private string itemName;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int maxStack;

        public override IItemContainer GenerateAt(TileBase tileBase, Quaternion rotation, Transform parent)
        {
            var position = new Vector3(tileBase.WorldPosition.x, 0, tileBase.WorldPosition.x);
            var instantiateObject = Instantiate(prefab, position, rotation, parent);
            return new StackableItemContainer(itemName, instantiateObject, maxStack);
        }
    }
}