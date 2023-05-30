using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    public class TileBase : IActiveInteractable, IPassiveInteractable, ITileContextRegistration
    {
        public int2 TileManagerArrayPosition { get; }
        
        
        
        private readonly List<ITileInteractionContext> _stackedTileContexts;

        public TileBase(int2 tileManagerArrayPosition)
        {
            TileManagerArrayPosition = tileManagerArrayPosition;
            _stackedTileContexts = new List<ITileInteractionContext>();
        }
        
        public bool HasTileContext()
        {
            return _stackedTileContexts.Count != 0;
        }

        public void RegisterTileContext(ITileInteractionContext tileInteractionContext)
        {
            _stackedTileContexts.Add(tileInteractionContext);
        }
        
        public void UnregisterTileContext(ITileInteractionContext tileInteractionContext)
        {
            _stackedTileContexts.RemoveAll(x => ReferenceEquals(x, tileInteractionContext));
        }

        //TODO: After each interaction, this must be saved as an element in the current tick list. It must be ordered by interaction call.
        public bool OnActiveInteract(GameObject interactor)
        {
            return _stackedTileContexts.Any(connectedTileContext => connectedTileContext.OnActiveInteract(interactor));
        }

        //TODO: call this, at the end of each tick
        public void OnPassiveInteract()
        {
            foreach (var stackedTileContext in _stackedTileContexts)
            {
                stackedTileContext.OnPassiveInteract();
            }
        }

        public bool IsMovable()
        {
            return _stackedTileContexts.All(tileInteractionContext => tileInteractionContext.IsMovable());
        }

        public bool CanContainResource()
        {
            return _stackedTileContexts.All(tileInteractionContext => tileInteractionContext.CanContainResource());
        }
    }
}