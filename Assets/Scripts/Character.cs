using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public enum Emotion { Default = 0 }

    public Image image;

    [SerializeField]
    private int _id;
    
    [SerializeField]
    private string _name;
    
    public int Id => _id;
    public string Name => _name;

    public void SetColor(Color color)
    {
        image.color = color;
    }
}
