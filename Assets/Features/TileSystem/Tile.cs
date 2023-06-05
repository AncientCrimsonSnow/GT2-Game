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

        public ItemContainer ItemContainer { get; private set; }
        
        private readonly List<ITileComponent> _tileComponents;

        public Tile(int2 worldPosition, int2 arrayPosition)
        {
            WorldPosition = worldPosition;
            ArrayPosition = arrayPosition;
            _tileComponents = new List<ITileComponent> { new EmptyItemTileComponent(this) };

            ItemContainer = new ItemContainer(this);
        }

        public bool TryGetFirstTileComponentOfType<T>(out T tileComponent) where T : ITileComponent
        {
            tileComponent = default;
            
            foreach (var baseTileComponent in _tileComponents)
            {
                if (baseTileComponent is not T foundTileComponent) continue;
                
                tileComponent = foundTileComponent;
                return true;
            }

            return false;
        }
        
        public void ExchangeFirstTileComponentOfType<T>(T newTileComponent) where T : ITileComponent, IExchangeable<ITileComponent>
        {
            for (var index = 0; index < _tileComponents.Count; index++)
            {
                var baseTileComponent = _tileComponents[index];
                if (baseTileComponent is not T typeTileComponent || !typeTileComponent.IsExchangeable(newTileComponent)) continue;
                _tileComponents[index] = newTileComponent;
                return;
            }
        }
        
        public void RegisterTileComponent(ITileComponent newTileComponent)
        {
            _tileComponents.Add(newTileComponent);
        }

        public void UnregisterTileComponent(ITileComponent newTileComponent)
        {
            _tileComponents.RemoveAll(x => ReferenceEquals(x, newTileComponent));
        }

        //TODO: After each interaction, this must be saved as an element in the current tick list. It must be ordered by interaction call.
        public bool TryInteract(GameObject interactor)
        {
            return _tileComponents.Any(connectedTileContext => connectedTileContext.TryInteract(interactor));
        }

        public bool IsMovable()
        {
            return _tileComponents.All(tileInteractionContext => tileInteractionContext.IsMovable());
        }
    }
}