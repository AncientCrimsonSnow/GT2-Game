using System;
using System.Collections;
using Features.TileSystem.Scripts;
using Features.TileSystem.Scripts.Registrator;
using Uilities.Variables;
using UnityEngine;
using Object = UnityEngine.Object;

public class CurseInteractable : ITileInteractable
{
    public static Action OnCurseTriggered;
    
    private readonly FloatVariable _currentMagicValue;
    private readonly GameObject _curseVFX;
    private readonly float _neededMagicValue;
    private readonly BaseTileRegistrator _tileRegistrator;
    private readonly float _time;

    public CurseInteractable(FloatVariable currentMagicValue, GameObject curseVFX, float neededMagicValue, BaseTileRegistrator tileRegistrator, float time)
    {
        _currentMagicValue = currentMagicValue;
        _curseVFX = curseVFX;
        _neededMagicValue = neededMagicValue;
        _tileRegistrator = tileRegistrator;
        _time = time;
    }
    
    public bool CanInteract(GameObject interactor, out string interactionText)
    {
        interactionText = "";
        return false;
    }

    public bool TryInteract(GameObject interactor)
    {
        return false;
    }
    
    public bool CanCast(GameObject caster, out string interactionText)
    {
        interactionText = "";
        if (_currentMagicValue.Get() < _neededMagicValue) return false;

        interactionText = "Curse";
        
        return true;
    }

    public bool TryCast(GameObject caster)
    {
        if (_currentMagicValue.Get() < _neededMagicValue) return false;

        _tileRegistrator.StartCoroutine(EnableCurse());
        
        return true;
    }

    private IEnumerator EnableCurse()
    {
        var instantiatedCurse= Object.Instantiate(_curseVFX, _tileRegistrator.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(_time / 2f);
        OnCurseTriggered?.Invoke();
        yield return new WaitForSeconds(_time / 2f);
        
        Object.Destroy(instantiatedCurse);
    }

    public bool IsMovable()
    {
        return false;
    }
}
