using System.Collections.Generic;
using UnityEngine;

public class DialogEntity : ScriptableObject
{
    public int Id;
    public int BackgroundId;
    public List<Color> CharactersColor;
    public int SpeakerId;
    public string SpeakerName;
    public Character.Emotion SpeakerEmotion;
    public string DialogKey;
    public int NextDialogId;
    public bool HasChoice;
    public List<ChoiceData> Choices;
}
