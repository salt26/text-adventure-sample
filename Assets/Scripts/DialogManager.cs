using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public List<Image> backgrounds;
    public List<Character> characters;
    public TextMeshProUGUI dialogText;
    public GameObject choiceButtonParent;
    public List<ChoiceButton> choiceButtons;

    [SerializeField] private TextAsset dialogData;
    
    [SerializeField]
    private Dictionary<int, DialogEntity> dialogEntities;

    private int currentDialogId;

    private void Start()
    {
        if (!dialogData) return;

        var dialogs = dialogData.text.ToDialogs();
        dialogEntities = new Dictionary<int, DialogEntity>();
        foreach (var dialog in dialogs)
        {
            dialogEntities.Add(dialog.Id, dialog);
        }
        SetDialog(0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return) ||
            Input.GetKey(KeyCode.DownArrow) ||  Input.GetKey(KeyCode.RightArrow))
        {
            NextDialog();
        }
    }

    public void SetDialog(int dialogId)
    {
        if (dialogEntities == null || dialogEntities.Count == 0 || dialogId < 0) return;
        
        currentDialogId = dialogId;
        if (dialogEntities.TryGetValue(dialogId, out DialogEntity d))
        {
            // 대화를 찾았으면
            SetBackground(d.BackgroundId);
            SetCharacters(d.CharactersColor, d.SpeakerId, d.SpeakerName, d.SpeakerEmotion);
            SetDialogText(d.DialogKey);
            if (d.HasChoice)
            {
                SetChoices();
                choiceButtonParent.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError(ZString.Concat("Dialog Error: ", dialogId, "번 대화를 찾을 수 없습니다."));
            SetDialog(0);
        }
    }

    public void NextDialog()
    {
        if (dialogEntities == null) return;

        int nextDialogId = 0;
        if (dialogEntities.TryGetValue(currentDialogId, out DialogEntity d))
        {
            nextDialogId = d.NextDialogId;
        }
        SetDialog(nextDialogId);
    }

    private void SetBackground(int backgroundId)
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            if (i == backgroundId) backgrounds[i].gameObject.SetActive(true);
            else backgrounds[i].gameObject.SetActive(false);
        }
    }

    private void SetCharacters(List<Color> characterColors, int speakerId, string speakerName, Character.Emotion speakerEmotion)
    {
        if (characters == null) return;
        for (int i = 0; i < characters.Count; i++)
        {
            bool isNameUIActive = false;
            characters[i].image.color = characterColors[i];
            if (speakerId == characters[i].Id)
            {
                if (characters[i].EmotionToSprite != null && characters[i].EmotionToSprite.TryGetValue(speakerEmotion, out Sprite sprite))
                {
                    characters[i].image.sprite = sprite;
                }

                if (characters[i].nameUI)
                {
                    characters[i].nameUI.nameText.text = speakerName;
                    characters[i].nameUI.gameObject.SetActive(true);
                    isNameUIActive = true;
                }
            }

            if (!isNameUIActive)
            {
                characters[i].nameUI.gameObject.SetActive(false);
            }
        }
    }

    private void SetDialogText(string text)
    {
        dialogText.text = text;
    }

    private void SetChoices()
    {
        throw new NotImplementedException();
    }
}
