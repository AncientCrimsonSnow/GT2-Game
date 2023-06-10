using System.Collections.Generic;
using System.Linq;
using Features.TileSystem.ItemSystem;
using Features.TileSystem.Registrator;
using Features.TileSystem.TileComponents;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem.TileSystem
{
    // TODO: implement a way, that buildings are able to do stuff by themself: e.g. a consumer building wants to consume items on tiles after all the other TileComponents interactions
    //TODO: implement blocked tile
    public class Tile : IInteractable, ITileComponentRegistration, IMovable, IGridPosition
    {
        public int2 WorldPosition { get; }
        public int2 ArrayPosition { get; }

        public ItemContainer ItemContainer { get; private set; }
        
        private readonly List<ITileInteractable> _tileInteractables;

        public Tile(int2 worldPosition, int2 arrayPosition)
        {
            WorldPosition = worldPosition;
            ArrayPosition = arrayPosition;
            ItemContainer = new ItemContainer(this);
            
            _tileInteractables = new List<ITileInteractable> { new EmptyItemTileInteractable(this) };
        }
        
        public bool ContainsTileInteractableOfType<T>() where T : ITileInteractable
        {
            return _tileInteractables.Any(baseTileComponent => baseTileComponent is T);
        }

        public bool TryGetFirstTileInteractableOfType<T>(out T tileComponent) where T : ITileInteractable
        {
            tileComponent = default;
            
            foreach (var baseTileComponent in _tileInteractables)
            {
                if (baseTileComponent is not T foundTileComponent) continue;
                
                tileComponent = foundTileComponent;
                return true;
            }

            return false;
        }
        
        public void ExchangeFirstTileInteractableOfType<T>(T newTileComponent) where T : ITileInteractable, IExchangeable<ITileInteractable>
        {
            for (var index = 0; index < _tileInteractables.Count; index++)
            {
                var baseTileComponent = _tileInteractables[index];
                if (baseTileComponent is not T typeTileComponent || !typeTileComponent.IsExchangeable(newTileComponent)) continue;
                _tileInteractables[index] = newTileComponent;
                return;
            }
        }
        
        public void RegisterTileInteractable(ITileInteractable newTileInteractable)
        {
            _tileInteractables.Add(newTileInteractable);
        }

        public void UnregisterTileInteractable(ITileInteractable newTileInteractable)
        {
            _tileInteractables.RemoveAll(x => ReferenceEquals(x, newTileInteractable));
        }

        public bool TryInteract(GameObject interactor)
        {
            return _tileInteractables.Any(connectedTileContext => connectedTileContext.TryInteract(interactor));
        }

        public bool IsMovable()
        {
            return _tileInteractables.All(tileInteractionContext => tileInteractionContext.IsMovable());
        }
    }
}