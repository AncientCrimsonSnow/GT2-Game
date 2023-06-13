using Features.Input;
using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicInputBehaviour : BaseMagicInput
{
    [SerializeField] private bool breakAutoTicksEntry;
    [SerializeField] private TileManager tileManager;

    private BaseMovementInput _entryMovementInput;
    private bool _breakAutoTick;

    #region figure this out
    private void Awake()
    {
        _breakAutoTick = breakAutoTicksEntry;
    }

    private void Update()
    {
        if (ReplayManager.Instance.IsRecording() || _breakAutoTick) return;
        
        ReplayManager.Instance.Tick();
    }
    #endregion

    public override void OnMagicInput(InputAction.CallbackContext context)
    {
        tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform)).TryCastMagic(gameObject);
    }
    
    public override void OnInterruptMagic(InputAction.CallbackContext context)
    {
        _breakAutoTick = !_breakAutoTick;
    }
}
