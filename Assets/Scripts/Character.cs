using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public enum Emotion { Default = 0 }

    public Image image;
    
    public NameUI nameUI;
    
    [SerializeField]
    private List<EmotionalSprite> emotionalSprites;

    [SerializeField]
    private int _id;
    
    [SerializeField]
    private string _name;
    
    public readonly Dictionary<Emotion, Sprite> EmotionToSprite = new();
    
    public int Id => _id;
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
