using System;
using Features.Items.Scripts;
using Features.ReplaySystem;
using Features.ReplaySystem.Record;
using UnityEngine;

namespace Features.TileSystem.Scripts.Registrator
{
    public class ConsumerTileRegistrator : BaseTileRegistrator, IReplayOriginator
    {
        public Action<IInputSnapshot> PushNewTick { get; set; }

        private ReplayManager _replayManager;
        private ConsumerSnapshot _consumerSnapshot;

        protected override void InternalRegisterOnTile()
        {
            _replayManager = ReplayManager.Instance;
            _replayManager.InitializeRecording(gameObject, () => { }, () => { }, () => { });
            _replayManager.RegisterOriginator(gameObject, this);
            PushNewTick.Invoke(new ConsumerSnapshot(Tile));
            _replayManager.StartReplay(gameObject, true);
        }

        protected override void UnregisterOnTile()
        {
            if (_replayManager != null)
            {
                _replayManager.StopReplayable(gameObject);
            }
        }
    }

    [Serializable]
    public class ConsumerSnapshot : IInputSnapshot
    {
        private readonly Tile _tile;
        private int _totalConsumedValue;

        public ConsumerSnapshot(Tile tile)
        {
            _tile = tile;
        }
    
        public void Tick(float tickDurationInSeconds)
        {
            if (!_tile.ContainsTileInteractableOfType<UnstackableItemTileInteractable>()) return;

            _totalConsumedValue += _tile.ItemContainer.ContainedBaseItem.itemValue;
            Debug.Log(_totalConsumedValue);
        
            _tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(_tile));
        }
    }
}