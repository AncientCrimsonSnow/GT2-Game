using System;
using Features.TileSystem;
using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileSystem;
using UnityEngine;
using Zenject;

namespace Features.Testing
{
    public class Testing : CarriedItemBaseBehaviour
    {
        [SerializeField] private Item heldItem;
        [SerializeField] private bool takeItemAtAwake;
        
        private ITileManager _tileManager;

        private void Awake()
        {
            if (takeItemAtAwake)
            {
                CarriedItem = heldItem;
            }
        }

        [Inject]
        public void Initialize(ITileManager tileManager)
        {
            _tileManager = tileManager;
        }
        
        private void OnMouseDown()
        {
            var registeredPosition = TileHelper.TransformPositionToInt2(transform);
            var tile = _tileManager.GetTileTypeAt(registeredPosition);
            tile.TryInteract(gameObject);
        }

        protected override void OnDropItem()
        {
        }

        protected override void OnPickupItem()
        {
            
        }
    }
}