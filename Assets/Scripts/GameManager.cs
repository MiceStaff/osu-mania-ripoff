using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NoteSpawner spawner;
    public string osuFilePath;
    public AudioSource musicSource;
    OsuParser osuParser = new OsuParser();
    void Start()
    {
        spawner.loadChart(osuParser.ParseOsuFile(osuFilePath));
        musicSource.PlayDelayed(spawner.noteTravelTime / 1000f);
    }
}