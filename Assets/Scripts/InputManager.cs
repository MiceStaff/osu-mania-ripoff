using UnityEngine;

public class InputManager : MonoBehaviour {
    public ButtonController[] buttons = new ButtonController[4]; 

    // Update is called once per frame
    void Update() {
        UpdateButtonVisuals();
    }    
    
    void UpdateButtonVisuals()
    {
        foreach (var btn in buttons)
        {
            if (Input.GetKey(btn.inputKey))
                btn.spriteRenderer.sprite = btn.downSprite;
            else
                btn.spriteRenderer.sprite = btn.upSprite;
        }
    }
}