﻿using UnityEngine;

namespace Features.Items.Scripts
{
    [CreateAssetMenu]
    public class BaseItem_SO : ScriptableObject
    {
        public string itemName;
        public GameObject prefab;

        public virtual bool TryCastMagic(GameObject caster)
        {
            return false;
        }
    }
}