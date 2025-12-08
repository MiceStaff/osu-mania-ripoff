using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float songTime;
    public float startDelay = 2f;
    public AudioSource musicSource;
    public VideoPlayer videoPlayer;
    public NoteSpawner spawner;
    public string osuFilePath;

    bool songStarted = false;
    float startTime;
    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        songTime = float.MinValue;
        spawner.loadChart(OsuParser.Instance.ParseOsuFile(osuFilePath));
        startTime = Time.time * 1000;
    }

    void Update()
    {
        float elapsed = 1000 * Time.time - startTime;
        if (!songStarted && elapsed >= startDelay)
        {
            musicSource.Play();
            videoPlayer.Play();
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