using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.StateLogic;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    public class TileBase : IActiveInteractable, IPassiveInteractable, ITileContextRegistration
    {
        public int2 WorldPosition { get; }
        public int2 ArrayPosition { get; }

        private ITileObjectComponent _tileObjectComponent;
        
        //Tile Context: what to do on the tile / tile behaviour
        private readonly List<ITileInteractionContext> _stackedTileContexts;
        
        
        //displayed placed object -> baum, amboss (needs boolean, if item is exchangable/pickupable)
        
        //used tile interaction context -> can only be one -> buildings can only be stacked, if this interaction context is the same
        
        //connected interaction
        
        //abbauen auf lager
        //lager auf lager -> same behaviour, use first added object for all of them
        //abbauen of abbauen -> same behaviour, different tree
        //abbauen auf ambos
        
        
        //TODO:
        //same types can stack on top of each other (lager/lager, holz abbauen/holz abbauen) => ist im grunde das stackableTileObject nur mit actions
        //types are ordered by the time, they got added. the last index will be executed
        
        //abbauen sind immer pointer auf input. items droppen immer unter character wie bei baum.

        public TileBase(int2 worldPosition, int2 arrayPosition)
        {
            WorldPosition = worldPosition;
            ArrayPosition = arrayPosition;
            _stackedTileContexts = new List<ITileInteractionContext>();

            _tileObjectComponent = new EmptyTileObjectComponent(this);
        }
        
        public bool HasTileContext()
        {
            return _stackedTileContexts.Count != 0;
        }

        public void SetTileObjectComponent(ITileObjectComponent objectInteractionContext)
        {
            if (_tileObjectComponent is TileObjectDecorator oldTileObjectDecorator && objectInteractionContext is TileObjectDecorator newTileObjectDecorator)
            {
                oldTileObjectDecorator.TileObjectComponent = newTileObjectDecorator.TileObjectComponent;
            }
            else
            {
                _tileObjectComponent = objectInteractionContext;
            }
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
            return _tileObjectComponent.OnActiveInteract(interactor) || 
                   _stackedTileContexts.Any(connectedTileContext => connectedTileContext.OnActiveInteract(interactor));
        }

        //TODO: call this, at the end of each tick. addition: solve this differently
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