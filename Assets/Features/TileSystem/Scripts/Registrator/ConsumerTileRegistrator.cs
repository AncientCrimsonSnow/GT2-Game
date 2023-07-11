using System;
using Features.Items.Scripts;
using Features.ReplaySystem;
using Features.ReplaySystem.Record;
using Uilities.Variables;
using UnityEngine;

namespace Features.TileSystem.Scripts.Registrator
{
    public class ConsumerTileRegistrator : BaseTileRegistrator, IReplayOriginator
    {
        [SerializeField] private FloatVariable consumedValue;
        
        public Action<IInputSnapshot> PushNewTick { get; set; }

        private bool _consumeItems;
        
        protected override void InternalRegisterOnTile()
        {
            _consumeItems = true;
        }

        private void Update()
        {
            if (!_consumeItems || !Tile.ContainsTileInteractableOfType<UnstackableItemTileInteractable>()) return;
            
            consumedValue.Add(Tile.ItemContainer.ContainedBaseItem.itemValue);
        
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }

        protected override void InternalUnregisterOnTile()
        {
            _consumeItems = false;
        }
    }
}