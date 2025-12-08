using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI songInfoText;
    public TextMeshProUGUI deltaText;

    void Awake()
    {
        instance = this;
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString("N0");
    }

    public void SetAccuracy(float accuracy)
    {
        accuracyText.text = accuracy.ToString("F2") + "%";
    }

    public void SetCombo(int combo , Color color)
    {
        comboText.text = combo > 5 ? combo.ToString("N0") : "";
        comboText.color = color;
    }

    public void SetSongInfo(string title, string artist)
    {
        songInfoText.text = $"{title} {artist}";
    }

    public void SetDelta(float delta)
    {
        deltaText.text = delta.ToString("F2") + "ms";
    }
}