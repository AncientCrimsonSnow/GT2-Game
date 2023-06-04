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

        //TODO: solve this by decorator
        private ExchangeableBaseTileComponent _exchangeableItemTileComponent;
        
        private readonly List<BaseTileComponent> _tileComponents;

        public Tile(int2 worldPosition, int2 arrayPosition)
        {
            WorldPosition = worldPosition;
            ArrayPosition = arrayPosition;
            _tileComponents = new List<BaseTileComponent>();

            _exchangeableItemTileComponent = new EmptyItemTileComponent(this);
        }
        
        public bool TryRegisterTileComponent(BaseTileComponent newExchangeable)
        {
            if (newExchangeable is ExchangeableBaseTileComponent exchangeableBaseTileComponent)
            {
                if (!_exchangeableItemTileComponent.IsExchangeable(newExchangeable)) return false;

                _exchangeableItemTileComponent.OnExchange(exchangeableBaseTileComponent);
                _exchangeableItemTileComponent = exchangeableBaseTileComponent;
                return true;
            }

            _tileComponents.Add(newExchangeable);
            return true;
        }

        public bool TryUnregisterTileComponent(BaseTileComponent newExchangeable)
        {
            if (_exchangeableItemTileComponent == newExchangeable)
            {
                var emptyItemTileComponent = new EmptyItemTileComponent(this);
                _exchangeableItemTileComponent.IsExchangeable(emptyItemTileComponent);
                _exchangeableItemTileComponent = emptyItemTileComponent;
                return true;
            }
            
            _tileComponents.RemoveAll(x => ReferenceEquals(x, newExchangeable));
            return true;
        }

        //TODO: put this into the storage instantiation -> TODO: destroy on building destroy
        public void AddStackableTileObjectComponent(Item item)
        {
            var instantiatedObject = TileHelper.InstantiateOnTile(this, item.prefab, Quaternion.identity);
            var tileObjectComponent = new StackableItemTileComponent(this, item, instantiatedObject);
            TryRegisterTileComponent(tileObjectComponent);
        }

        //TODO: After each interaction, this must be saved as an element in the current tick list. It must be ordered by interaction call.
        public bool TryInteract(GameObject interactor)
        {
            return _exchangeableItemTileComponent.TryInteract(interactor) || 
                   _tileComponents.Any(connectedTileContext => connectedTileContext.TryInteract(interactor));
        }

        public bool IsMovable()
        {
            return _exchangeableItemTileComponent.IsMovable() && _tileComponents.All(tileInteractionContext => tileInteractionContext.IsMovable());
        }
    }
}