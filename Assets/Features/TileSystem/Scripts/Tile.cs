using System.Collections.Generic;
using System.Linq;
using Features.Items.Scripts;
using Features.TileSystem.Scripts.Registrator;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem.Scripts
{
    // TODO: implement a way, that buildings are able to do stuff by themself: e.g. a consumer building wants to consume items on tiles after all the other TileComponents interactions
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
                if (baseTileComponent is T typeTileComponent && typeTileComponent.IsExchangeable(newTileComponent))
                {
                    _tileInteractables[index] = newTileComponent;
                    return;
                }
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
            //PrintInteractable();

            return _tileInteractables.Any(connectedTileContext => connectedTileContext.TryInteract(interactor));
        }

        public bool TryCast(GameObject caster)
        {
            //PrintInteractable();
            
            return _tileInteractables.Any(connectedTileContext => connectedTileContext.TryCast(caster));
        }

        public bool IsMovable()
        {
            return _tileInteractables.All(tileInteractionContext => tileInteractionContext.IsMovable());
        }

        public void PrintInteractable()
        {
            foreach (var tileInteractable in _tileInteractables)
            {
                Debug.Log("Try Cast: " + tileInteractable + " | " + ItemContainer.PooledGameObject + " | " + ItemContainer.ContainedBaseItem + " | " + WorldPosition.x + " " + WorldPosition.y);
            }
        }
    }
}