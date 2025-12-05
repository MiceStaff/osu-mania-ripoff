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
    public int hitTime;
    public int releaseTime;
    public NoteType type;
}
public class OsuParser
{
    public static OsuParser Instance = new OsuParser();
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
            lane = xPos / 128,
            hitTime = time
        };
        /*
        if ((type & 128) > 0)
        {
            note.type = NoteType.Tap; note.type = NoteType.Hold;
            note.releaseTime = int.Parse(values[5].Split(':')[0]);
        }
        else
        {
            note.type = NoteType.Tap;
            note.releaseTime = note.hitTime;
        }
        */
        
        note.type = NoteType.Tap;
        note.releaseTime = note.hitTime;
        
        return note;
    }
}
