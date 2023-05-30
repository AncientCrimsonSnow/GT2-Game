using System;
using System.Collections;
using Features.TileSystem;
using UnityEngine;

public interface IItemContainer : IActiveInteractable, IEquatable<IItemContainer>
{
    string ItemName { get; }
        
    GameObject InstantiatedObject { get; }
}