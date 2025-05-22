using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    
    public TextMeshProUGUI text;

    private int _nextDialogId;
    private Action<int> _setDialog;

    public void Initialize(ChoiceData data, Action<int> setDialog)
    {
        _nextDialogId = data.NextDialogId;
        text.text = data.NameKey;
        _setDialog = setDialog;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _setDialog?.Invoke(_nextDialogId);
    }
}
