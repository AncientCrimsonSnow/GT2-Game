using TMPro;
using Uilities.Variables;
using UnityEngine;

public class CurseText : MonoBehaviour
{
    [SerializeField] private FloatVariable magicValue;
    [SerializeField] private TMP_Text text;
    
    private void Update()
    {
        text.text = magicValue.Get().ToString();
    }
}
