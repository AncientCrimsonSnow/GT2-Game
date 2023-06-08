using System;
using System.Collections;
using System.Collections.Generic;
using NewReplaySystem;
using UnityEngine;

//TODO: how to restore destroyed gameObjects?
public class PositionOriginator : MonoBehaviour
{
    
}

public class PositionSnapshot : ISnapshot
{
    private readonly Transform _transform;
    private readonly Vector3 _position;
    private readonly Quaternion _rotation;
    
    public PositionSnapshot(Transform transform)
    {
        _transform = transform;
        _position = transform.position;
        _rotation = transform.rotation;
    }
    
    public void Restore()
    {
        _transform.position = _position;
        _transform.rotation = _rotation;
    }
}

public interface ISnapshot
{
    void Restore();
}

public interface IOriginator
{
    ISnapshot CreateSnapshot();
}
