using Features;
using Features.TileSystem.CharacterBehaviours;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MagicInputFocus", menuName = "Focus/Input/Magic")]
public class MagicInputFocus : Focus_SO<BaseMagicInput>
{
    public void OnMagicInput(InputAction.CallbackContext context)
    {
        Focus.OnMagicInput(context);
    }
}
