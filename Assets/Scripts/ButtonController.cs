using UnityEngine;

[System.Serializable]
public class ButtonController : MonoBehaviour
{
    public KeyCode inputKey;
    public Sprite upSprite;
    public Sprite downSprite;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}