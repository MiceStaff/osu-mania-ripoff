using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public float[] laneX = new float[4];
    public float spawnY;
    public float hitY;
    public GameObject tapPrefab;
    public GameObject holdPrefab;
    public GameObject minePrefab;
    public AudioSource musicSource;
    public float noteTravelTime = 415f;
    List<NoteData> notes = new List<NoteData>();
    int nextNoteIndex = 0;

    void Update()
    {
        if (musicSource == null || notes == null) return;
        float songTime = musicSource.time * 1000f;
        // spawn any notes whose (hitTime - noteTravelTime) <= songTime
        while (nextNoteIndex < notes.Count && notes[nextNoteIndex].hitTime <= songTime)
        {
            Spawn(notes[nextNoteIndex]);
            nextNoteIndex++;
        }
    }
    public void loadChart(List<NoteData> leNotes)
    {
        nextNoteIndex = 0;
        notes = leNotes;
    }
    void Spawn(NoteData n)
    {
        Vector3 spawnPos = new Vector3(laneX[n.lane], spawnY, 0);
        GameObject prefab = tapPrefab;
        if (n.type == NoteType.Hold) prefab = holdPrefab;
        if (n.type == NoteType.Mine) prefab = minePrefab;

        var go = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
        var nc = go.GetComponent<NoteController>();
        nc.data.hitTime = n.hitTime;
        nc.data.releaseTime = n.releaseTime;
        nc.speed = (spawnPos.y - hitY) / noteTravelTime;
        nc.data.lane = n.lane;
    }
}
