using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    /// 배경 이미지 컴포넌트 목록.
    /// 목록의 인덱스(0부터 시작하는 순서)가 배경 번호(BackgroundId)와 일치해야 합니다.
    public List<Image> backgrounds;
    
    /// 캐릭터 컴포넌트 목록.
    /// 목록의 인덱스가 캐릭터 번호(SpeakerId)와 일치할 필요는 없지만, 색상 목록(CharactersColor)의 인덱스와는 일치해야 합니다. 
    public List<Character> characters;
    
    /// 대사 텍스트 컴포넌트
    public TextMeshProUGUI dialogText;
    
    /// 선택지 버튼들을 자식으로 둔 부모 게임오브젝트
    public GameObject choiceButtonParent;
    
    /// 선택지 버튼 컴포넌트 목록.
    /// 최대로 등장할 수 있는 선택지 개수보다 이 목록에 있는 것이 많거나 같아야 합니다.
    public List<ChoiceButton> choiceButtons;

    /// 대사 데이터가 담겨 있는 Dialog.csv를 여기에 넣습니다.
    [SerializeField] private TextAsset dialogData;

    /// 플레이 중에만 보입니다.
    /// 대사 데이터를 잘 불러왔는지 Inspector에서 확인할 수 있습니다.
    [SerializeField, ReadOnly] private List<DialogEntity> dialogs;
    
    /// 불러온 대사 데이터를 사용하기 쉽게 가공하여 보관합니다.
    private Dictionary<int, DialogEntity> _dialogEntities;

    /// 현재 재생 중인 대사의 번호
    private int _currentDialogId;

    /// 빠른 클릭을 방지하기 위해 마지막으로 대사를 넘긴 후 몇 초가 지났는지 기억합니다.
    private float _clickCooldown;

    private void Start()
    {
        if (!dialogData) return;

        // Dialog.csv 파일을 읽어서 대사를 불러온다.
        dialogs = dialogData.text.ToDialogs();
        _dialogEntities = new Dictionary<int, DialogEntity>();
        foreach (DialogEntity dialog in dialogs)
        {
            _dialogEntities.Add(dialog.Id, dialog);
        }
        
        // 첫 대사 실행
        SetDialog(0);
    }

    private void Update()
    {
        _clickCooldown += Time.deltaTime;
        // 마우스 왼쪽 클릭을 하거나 키보드의 Space, Enter, 아래 화살표 또는 오른쪽 화살표를 누른 경우
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return) ||
            Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            // 0.2초 이내에는 따닥 클릭 불가
            if (_clickCooldown < 0.2f) return;
            
            // 선택지가 없는 대사인 경우에 한해 다음 대사로 넘어간다.
            NextDialog();
        }
    }

    /// <summary>
    /// 대사, 배경, 캐릭터, 선택지를 주어진 대사 번호의 것으로 설정합니다.
    /// </summary>
    /// <param name="dialogId">대사 번호</param>
    public void SetDialog(int dialogId)
    {
        // 대사를 불러오지 않았거나 다음 대사가 없으면(dialogId == -1) 아무 것도 하지 않습니다.
        if (_dialogEntities == null || _dialogEntities.Count == 0 || dialogId < 0) return;
        
        _currentDialogId = dialogId;
        if (_dialogEntities.TryGetValue(dialogId, out DialogEntity d))
        {
            // 대화를 찾았으면 배경, 캐릭터, 대사, 선택지를 적절히 설정해줍니다.
            SetBackground(d.BackgroundId);
            SetCharacters(d.CharactersColor, d.SpeakerId, d.SpeakerName, d.SpeakerEmotion);
            SetDialogText(d.DialogKey);
            if (d.HasChoice)
            {
                SetChoices(d.Choices);
            }
            else
            {
                choiceButtonParent.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError(ZString.Concat("Dialog Error: ", dialogId, "번 대화를 찾을 수 없습니다."));
            SetDialog(0);
        }

        _clickCooldown = 0;
    }

    /// <summary>
    /// 선택지가 있는 대사에서는 다음으로 넘어가지지 않습니다.
    /// </summary>
    public void NextDialog()
    {
        if (_dialogEntities == null) return;

        int nextDialogId = 0;
        if (_dialogEntities.TryGetValue(_currentDialogId, out DialogEntity d))
        {
            nextDialogId = d.NextDialogId;
        }
        SetDialog(nextDialogId);
    }

    /// <summary>
    /// 배경을 설정합니다.
    /// </summary>
    /// <param name="backgroundId">배경 번호 (0부터 시작)</param>
    private void SetBackground(int backgroundId)
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            if (i == backgroundId) backgrounds[i].gameObject.SetActive(true);
            else backgrounds[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 캐릭터를 설정합니다.
    /// </summary>
    /// <param name="characterColors">캐릭터들에게 덧입힐 색상 목록</param>
    /// <param name="speakerId">화자 번호</param>
    /// <param name="speakerName">화자 이름</param>
    /// <param name="speakerEmotion">화자의 감정 표현 상태</param>
    private void SetCharacters(List<Color> characterColors, int speakerId, string speakerName, Character.Emotion speakerEmotion)
    {
        if (characters == null) return;
        for (int i = 0; i < characters.Count; i++)
        {
            bool isNameUIActive = false;
            characters[i].image.color = characterColors[i];
            if (Mathf.Approximately(characters[i].image.color.a, 0))
            {
                characters[i].gameObject.SetActive(false);
            }
            else
            {
                characters[i].gameObject.SetActive(true);
            }
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

    /// <summary>
    /// 대사 내용을 설정합니다.
    /// </summary>
    /// <param name="text">대사</param>
    private void SetDialogText(string text)
    {
        dialogText.text = text;
    }

    /// <summary>
    /// 선택지를 설정합니다.
    /// </summary>
    /// <param name="choices">선택지 목록</param>
    private void SetChoices(List<ChoiceData> choices)
    {
        if (choices == null)
        {
            foreach (ChoiceButton t in choiceButtons)
            {
                t.gameObject.SetActive(false);
            }
            choiceButtonParent.gameObject.SetActive(false);

            return;
        }
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            if (choices.Count > i)
            {
                choiceButtons[i].Initialize(choices[i], SetDialog);
                choiceButtons[i].gameObject.SetActive(true);
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
        choiceButtonParent.gameObject.SetActive(true);
    }
}
