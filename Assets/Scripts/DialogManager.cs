using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public List<Image> backgrounds;
    public List<Character> characters;
    public List<NameUI> names;
    public TextMeshProUGUI dialogText;
    public GameObject buttonParent;
    public List<ChoiceButton> choiceButtons;

    [SerializeField] private TextAsset dialogData;
}
