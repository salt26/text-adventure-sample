using System.Collections.Generic;
using UnityEngine;

public class DialogEntity : ScriptableObject
{
    public int Id;
    public int BackgroundId;
    public int CharacterId;
    public string CharacterName;
    public List<Color> CharacterColor;
    public Character.Emotion EmotionType;
    public string DialogKey;
    public int NextDialogId;
    public bool HasChoice;
    public List<ChoiceData> Choices;
}
