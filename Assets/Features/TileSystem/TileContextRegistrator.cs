using Features.TileSystem;
using Unity.Mathematics;
using UnityEngine;
using Zenject;


//TODO: this must be able to represent a building -> have a list af all registratable things and register at one spot
/// <summary>
/// This script automatically registers itself to the TileManager dependant on the position at Start.
/// When changing it's position later, it will still be inside the same position inside the TileManager!
/// Tiles must currently be static on a position!
///
/// Current suitable concept for pooling: setting out-of-screenspace objects inactive. It got registered inside the TileManager.
/// Thus, things will be able to interact with it, even though it is set inactive & out of screenspace (useful for the tick system).
/// </summary>
public class TileContextRegistrator : MonoBehaviour
{
    [SerializeField] private TileContextRegistration tileContextRegistration;
    [SerializeField] private TileContextFactory tileContextFactory;

    private ITileManager _tileManager;
    private ITileInteractionContext _ownedTileInteractionContext;
    
    private int2 _registeredPosition;

    private void Start()
    {
        ApplyRoundedPosition();
        _registeredPosition = TileHelper.TransformPositionToInt2(transform);
        _ownedTileInteractionContext = tileContextFactory.Generate(this);
        tileContextRegistration.OnRegister(_ownedTileInteractionContext, _tileManager, _registeredPosition);
    }

    private void ApplyRoundedPosition()
    {
        var position = transform.position;
        if (position.x % 1 != 0 || position.z % 1 != 0)
        {
            Debug.LogWarning("Position of this Tile got mathematically rounded, because it isn't on a whole number!");
            
            position = new Vector3(Mathf.RoundToInt(position.x), position.y, Mathf.RoundToInt(position.z));
            transform.position = position;
        }
    }

    [Inject]
    public void Initialize(ITileManager tileManager)
    {
        _tileManager = tileManager;
    }

    private void OnDestroy()
    {
        var position = transform.position;

        if (!_registeredPosition.Equals(new int2((int)position.x, (int)position.z)))
        {
            Debug.LogWarning("You changed the position of this tile during Runtime!");
        }
        
        tileContextRegistration.OnUnregister(_ownedTileInteractionContext, _tileManager, _registeredPosition);
    }
}
