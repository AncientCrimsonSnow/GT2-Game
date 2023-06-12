﻿using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.ItemSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Testing
{
    public class CarriedItemTesting : CarriedItemBaseBehaviour
    {
        [FormerlySerializedAs("heldItemAtAwake")] [SerializeField] private BaseItem heldBaseItemAtAwake;
        [SerializeField] private bool takeItemAtAwake;
        
        private void Awake()
        {
            if (takeItemAtAwake)
            {
                CarriedBaseItem = heldBaseItemAtAwake;
            }
        }
        
        protected override void OnDropItem()
        {
        }

        protected override void OnPickupItem()
        {
        }
    }
}