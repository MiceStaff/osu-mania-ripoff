using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float songTime;
    public float startDelay = 2f;
    public AudioSource musicSource;
    public NoteSpawner spawner;
    public string osuFilePath;

    bool songStarted = false;
    float startTime;
    void Start()
    {
        songTime = float.MinValue;
        instance = this;
        spawner.loadChart(OsuParser.Instance.ParseOsuFile(osuFilePath));
        startTime = Time.time * 1000;
    }

    void Update()
    {
        float elapsed = 1000 * Time.time - startTime;
        if (!songStarted && elapsed >= startDelay)
        {
            musicSource.Play();
            songStarted = true;
        }
        if (songStarted)
        {
            songTime = (musicSource.time * 1000f);
        }
        else
        {
            songTime = elapsed - startDelay;
        }
    }
}