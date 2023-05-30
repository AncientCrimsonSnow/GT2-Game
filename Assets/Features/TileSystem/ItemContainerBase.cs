using System;
using UnityEngine;

namespace Features.TileSystem
{
    public abstract class ItemContainerBase : IItemContainer
    {
        public string ItemName { get; private set; }
        public GameObject InstantiatedObject { get; private set; }

        public ItemContainerBase(string itemName, GameObject instantiatedObject)
        {
            ItemName = itemName;
            InstantiatedObject = instantiatedObject;
        }

        public abstract bool OnActiveInteract(GameObject interactor);

        public bool Equals(IItemContainer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return ItemName == other.ItemName && Equals(InstantiatedObject, other.InstantiatedObject);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemName, InstantiatedObject);
        }
    }
}