using System;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;

[Serializable]
public class DialogEntity
{
    /// 대사 번호
    [ReadOnly]
    public int Id;
    
    /// 배경 번호
    [ReadOnly]
    public int BackgroundId;
    
    /// 캐릭터들에게 덧입힐 색상 목록
    [ReadOnly]
    public List<Color> CharactersColor;
    
    /// 화자 캐릭터 번호
    [ReadOnly]
    public int SpeakerId;
    
    /// 화자 이름 (표시될 이름입니다. 캐릭터 고유 이름과 다를 수 있습니다.)
    [ReadOnly]
    public string SpeakerName;
    
    /// 화자의 감정 상태 (화자 모습에 영향을 줍니다.)
    [ReadOnly]
    public Character.Emotion SpeakerEmotion;
    
    /// 대사 텍스트
    [ReadOnly]
    public string DialogKey;
    
    /// 다음 대사 번호 (선택지가 없는 경우에만 사용, 선택지가 있으면 -1)
    [ReadOnly]
    public int NextDialogId;
    
    /// 선택지 유무
    [ReadOnly]
    public bool HasChoice;
    
    /// 선택지 목록
    [ReadOnly]
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
            //Debug.Log(ToString());
            return;
        }
        DialogKey = DialogKey.Trim('"').Replace('/', '\n');
        if (!HasChoice)
        {
            //Debug.Log(ToString());
            return;
        }
        Choices = new List<ChoiceData>();
        if (string.IsNullOrEmpty(Button0NameKey)) return;
        Choices.Add(new ChoiceData(Button0NameKey, Button0NextDialogId));
        if (string.IsNullOrEmpty(Button1NameKey)) return;
        Choices.Add(new ChoiceData(Button1NameKey, Button1NextDialogId));
        if (string.IsNullOrEmpty(Button2NameKey)) return;
        Choices.Add(new ChoiceData(Button2NameKey, Button2NextDialogId));
        //Debug.Log(ToString());
    }

    public override string ToString()
    {
        using (Utf16ValueStringBuilder sb = ZString.CreateStringBuilder(true))
        {
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
                foreach (ChoiceData choice in Choices)
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
}
