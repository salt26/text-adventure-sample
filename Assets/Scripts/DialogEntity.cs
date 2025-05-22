using System.Collections.Generic;
using Cysharp.Text;
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

    [HideInInspector] public string Button0NameKey;
    [HideInInspector] public int Button0NextDialogId;
    [HideInInspector] public string Button1NameKey;
    [HideInInspector] public int Button1NextDialogId;
    [HideInInspector] public string Button2NameKey;
    [HideInInspector] public int Button2NextDialogId;

    public void Initialize()
    {
        if (DialogKey == null)
        {
            Debug.Log(ToString());
            return;
        }
        DialogKey = DialogKey.Trim('"').Replace('/', '\n');
        if (!HasChoice)
        {
            Debug.Log(ToString());
            return;
        }
        Choices = new List<ChoiceData>();
        if (string.IsNullOrEmpty(Button0NameKey)) return;
        Choices.Add(new ChoiceData(Button0NameKey, Button0NextDialogId));
        if (string.IsNullOrEmpty(Button1NameKey)) return;
        Choices.Add(new ChoiceData(Button1NameKey, Button1NextDialogId));
        if (string.IsNullOrEmpty(Button2NameKey)) return;
        Choices.Add(new ChoiceData(Button2NameKey, Button2NextDialogId));
        Debug.Log(ToString());
    }

    public override string ToString()
    {
        using var sb = ZString.CreateStringBuilder(true);
        sb.Append("ID: ");
        sb.AppendLine(Id);
        sb.Append("Background ID: ");
        sb.AppendLine(BackgroundId);
        sb.Append("Characters Color: [");
        foreach (Color color in CharactersColor)
        {
            sb.Append(color.ToString());
        }

        sb.AppendLine(']');
        sb.Append("Speaker ID: ");
        sb.AppendLine(SpeakerId);
        sb.Append("Speaker Name: ");
        sb.AppendLine(SpeakerName);
        sb.Append("Speaker Emotion: ");
        sb.AppendLine((int)SpeakerEmotion);
        sb.AppendLine("Dialog Key: ");
        sb.AppendLine(DialogKey);
        if (HasChoice)
        {
            sb.AppendLine("Choices: ");
            foreach (var choice in Choices)
            {
                sb.AppendLine(choice.ToString());
            }
        }
        else
        {
            sb.Append("NextDialog ID: ");
            sb.AppendLine(NextDialogId);
        }

        return sb.ToString();
    }
}
