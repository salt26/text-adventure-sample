using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    /// 캐릭터가 가질 수 있는 감정 상태의 종류
    public enum Emotion { Default = 0 }
    
    /// 캐릭터의 이미지 컴포넌트
    public Image image;
    
    /// 캐릭터의 이름을 표시할 NameUI 컴포넌트
    public NameUI nameUI;
    
    /// 캐릭터의 각 감정 상태마다 어떤 모습으로 그려낼지 정하는 목록.
    /// Unity Inspector에서 설정하세요. 
    [SerializeField]
    private List<EmotionalSprite> emotionalSprites;
    
    [SerializeField]
    private int _id;
    
    [SerializeField]
    private string _name;
    
    /// 코드에서는 emotionalSprites 대신 이 EmotionToSprite를 사용하세요.
    public readonly Dictionary<Emotion, Sprite> EmotionToSprite = new();
    
    /// 캐릭터 번호
    public int Id => _id;
    
    /// 캐릭터 고유 이름 (표시되는 이름과는 다를 수 있습니다.) 
    public string Name => _name;

    private void Awake()
    {
        foreach (EmotionalSprite es in emotionalSprites)
        {
            EmotionToSprite.TryAdd(es.emotion, es.sprite);
        }
    }

    [Serializable]
    public struct EmotionalSprite
    {
        public Emotion emotion;
        public Sprite sprite;
    }
}
