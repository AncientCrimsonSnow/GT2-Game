using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    // TODO: implement a way, that buildings are able to do stuff by themself: e.g. a consumer building wants to consume items on tiles after all the other TileComponents interactions
    public class Tile : IInteractable, ITileComponentRegistration, IMovable, IGridPosition
    {
        public int2 WorldPosition { get; }
        public int2 ArrayPosition { get; }

        private BaseTileComponent _itemBaseTileComponent;
        
        private readonly List<BaseTileComponent> _baseTileComponents;

        public Tile(int2 worldPosition, int2 arrayPosition)
        {
            WorldPosition = worldPosition;
            ArrayPosition = arrayPosition;
            _baseTileComponents = new List<BaseTileComponent>();

            _itemBaseTileComponent = new EmptyBaseTileComponent(this);
        }
        
        public bool TryRegisterTileComponent(BaseTileComponent newExchangeable)
        {
            if (newExchangeable is EmptyBaseTileComponent or StackableBaseTileComponent or UnstackableBaseTileComponent)
            {
                if (!_itemBaseTileComponent.IsExchangeable(newExchangeable)) return false;
                
                _itemBaseTileComponent = newExchangeable;
                return true;
            }

            _baseTileComponents.Add(newExchangeable);
            return true;
        }

        public bool TryUnregisterTileComponent(BaseTileComponent newExchangeable)
        {
            if (newExchangeable is EmptyBaseTileComponent or StackableBaseTileComponent or UnstackableBaseTileComponent)
            {
                if (!_itemBaseTileComponent.IsExchangeable(newExchangeable)) return false;
                
                _itemBaseTileComponent = newExchangeable;
                return true;
            }
            
            _baseTileComponents.RemoveAll(x => ReferenceEquals(x, newExchangeable));
            return true;
        }

        //TODO: put this into the storage instantiation -> TODO: destroy on building destroy
        public void AddStackableTileObjectComponent(Item item)
        {
            var instantiatedObject = TileHelper.InstantiateOnTile(this, item.prefab, Quaternion.identity);
            var tileObjectComponent = new StackableBaseTileComponent(this, item, instantiatedObject);
            TryRegisterTileComponent(tileObjectComponent);
        }

        //TODO: After each interaction, this must be saved as an element in the current tick list. It must be ordered by interaction call.
        public bool TryInteract(GameObject interactor)
        {
            return _itemBaseTileComponent.TryInteract(interactor) || 
                   _baseTileComponents.Any(connectedTileContext => connectedTileContext.TryInteract(interactor));
        }

        public bool IsMovable()
        {
            return _itemBaseTileComponent.IsMovable() && _baseTileComponents.All(tileInteractionContext => tileInteractionContext.IsMovable());
        }
    }
}