using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    
    public TextMeshProUGUI text;

    [HideInInspector] public int nextDialog;

    public void Initialize(ChoiceData data)
    {
        nextDialog = data.NextDialogId;
        text.text = data.NameKey;
    }
}
