using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    public class TileBase : IInteractable
    {
        public int2 TileManagerArrayPosition { get; }
        
        private ITileContext _tileContext;
        private readonly List<ITileContext> _connectedTileContexts;

        public TileBase(int2 tileManagerArrayPosition)
        {
            TileManagerArrayPosition = tileManagerArrayPosition;
            _connectedTileContexts = new List<ITileContext>();
        }

        public bool HasTileContextOfType(TileContextType tileContextType)
        {
            switch (tileContextType)
            {
                case TileContextType.SelfContext:
                    return _tileContext != null;
                case TileContextType.PointerContext:
                    return _connectedTileContexts.Count != 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileContextType), tileContextType, null);
            }
        }
        
        public bool HasTileContext()
        {
            return _tileContext != null && _connectedTileContexts.Count != 0;
        }

        public void RegisterTileContext(ITileContext tileContext, TileContextType tileContextType)
        {
            switch (tileContextType)
            {
                case TileContextType.SelfContext:
                    _tileContext = tileContext;
                    break;
                case TileContextType.PointerContext:
                    _connectedTileContexts.Add(tileContext);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileContextType), tileContextType, null);
            }
        }
        
        public void UnregisterTileContext(ITileContext tileContext)
        {
            if (ReferenceEquals(_tileContext, tileContext))
            {
                _tileContext = null;
            }
            else
            {
                _connectedTileContexts.RemoveAll(x => ReferenceEquals(x, tileContext));
            }
        }

        public bool OnInteract(GameObject interactor)
        {
            if (_tileContext != null && _tileContext.OnInteract(interactor))
            {
                return true;
            }

            return _connectedTileContexts.Any(connectedTileContext => connectedTileContext.OnInteract(interactor));
        }
    }
}