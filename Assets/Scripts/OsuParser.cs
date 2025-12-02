using System.IO;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum NoteType
{
    Tap,
    Hold,
    Mine
}
[System.Serializable]
public class NoteData
{
    public int lane;
    public float hitTime;
    public float releaseTime;
    public NoteType type;
}
[System.Serializable]
public class OsuParser
{
    [HideInInspector]
    public List<NoteData> ParseOsuFile(string osuFilePath)
    {
        List<NoteData> notes = new List<NoteData>();
        bool hitObjectSection = false;

        foreach (var line in File.ReadAllLines(osuFilePath))
        {
            if (line.StartsWith("[HitObjects]"))
            {
                hitObjectSection = true;
                continue;
            }
            if (hitObjectSection)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                notes.Add(ParseHitObjectLine(line));
            }
        }
        return notes;
    }

    NoteData ParseHitObjectLine(string line)
    {
        var values = line.Split(',');

        int xPos = int.Parse(values[0]);
        int time = int.Parse(values[2]);
        int type = int.Parse(values[3]);

        NoteData note = new NoteData
        {
            lane = (int)Mathf.Floor(xPos / 128f),
            hitTime = int.Parse(values[2])
        };

        if ((type & 128) > 0)
        {
            note.type = NoteType.Hold;
            note.releaseTime = int.Parse(values[5]);
        }
        else if ((type & 64) > 0)
        {
            note.type = NoteType.Mine;
            note.releaseTime = note.hitTime;
        }
        else
        {
            note.type = NoteType.Tap;
            note.releaseTime = note.hitTime;
        }

        return note;
    }
}
