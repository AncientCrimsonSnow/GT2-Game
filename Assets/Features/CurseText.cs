using TMPro;
using Uilities.Variables;
using UnityEngine;

public class CurseText : MonoBehaviour
{
    [SerializeField] private FloatVariable magicValue;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float requiredMagic;
    
    private void Update()
    {
        string canCurseText = "";
        if (magicValue.Get() >= requiredMagic)
        {
            canCurseText = "\n" + "Interact for Curse";
        }

        text.text = magicValue.Get() + canCurseText;
    }
}
