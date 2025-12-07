using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public static NoteSpawner Instance;
    public float[] laneX = new float[4];
    public float spawnY;
    public float hitY;
    public GameObject tapPrefab;
    public GameObject holdPrefab;
    public GameObject minePrefab;
    public AudioSource musicSource;
    public float noteTravelTime = 415f;
    [HideInInspector]
    public int totalNotes;
    List<NoteData> notes = new List<NoteData>();
    public static readonly List<Queue<NoteController>> laneNotes
    = new List<Queue<NoteController>>() { new(), new(), new(), new() };
    public int nextNoteIndex = 0;
    private void Awake()
    {
        Instance = this;                
    }
    void Update()
    {
        if (notes == null) return;
        while (nextNoteIndex < totalNotes && (notes[nextNoteIndex].hitTime - noteTravelTime) <= GameManager.instance.songTime)
        {
            Spawn(notes[nextNoteIndex]);
            nextNoteIndex++;
        }
    }
    public void loadChart(List<NoteData> leNotes)
    {
        nextNoteIndex = 0;
        notes = leNotes;
        totalNotes = notes.Count;
    }
    void Spawn(NoteData n)
    {
        Vector3 spawnPos = new Vector3(laneX[n.lane], spawnY, 0);
        GameObject prefab =
            (n.type == NoteType.Hold) ? holdPrefab :
            (n.type == NoteType.Mine) ? minePrefab :
            tapPrefab;

        var go = Instantiate(prefab, spawnPos, Quaternion.identity);
        var nc = go.GetComponent<NoteController>();
        
        nc.data = n;
        nc.speed = (spawnPos.y - hitY) / noteTravelTime;
        if(nc.data.type == NoteType.Hold)
        {
            nc.setupHold();
        }

        laneNotes[n.lane].Enqueue(nc);
    }
}
