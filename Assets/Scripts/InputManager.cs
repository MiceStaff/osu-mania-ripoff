using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public ButtonController[] buttons = new ButtonController[4];
    public float perfect = 50f;
    public float great = 100f;
    public float good = 150f;
    public float miss = 200f;

    void Update()
    {
        for (int i = 0 ; i < 4; i++)
        {
            if (Input.GetKeyDown(buttons[i].inputKey))
                HandlePress(i);

            if (Input.GetKeyUp(buttons[i].inputKey))
                HandleRelease(i);
        }
    }

    void HandlePress(int lane)
    {
        buttons[lane].spriteRenderer.sprite = buttons[lane].downSprite;
        var queue = NoteSpawner.laneNotes[lane];

        if (queue.Count == 0)
            return;

        var note = queue.Peek();
        float diff = GameManager.instance.songTime - note.data.hitTime;

        // Mine hit?
        if (note.data.type == NoteType.Mine)
        {
            Debug.Log("Mine Hit!");
            queue.Dequeue();
            Destroy(note.gameObject);
            return;
        }

        // Tap note
        if (note.data.type == NoteType.Tap)
        {
            ProcessTap(note, diff);
            return;
        }

        // Hold note start
        if (note.data.type == NoteType.Hold)
        {
            ProcessHoldStart(note, diff);
        }
    }

    void HandleRelease(int lane)
    {
        buttons[lane].spriteRenderer.sprite = buttons[lane].upSprite;
        var queue = NoteSpawner.laneNotes[lane];
        if (queue.Count == 0)
            return;

        var note = queue.Peek();

        if (note.data.type != NoteType.Hold)
            return;

        float diff = GameManager.instance.songTime - note.data.releaseTime;
        if (note.isHit)
            ProcessHoldRelease(note, diff);
    }

    void ProcessTap(NoteController note, float diff)
    {
        float abs = Mathf.Abs(diff);

        if (abs <= perfect) Debug.Log("Perfect!");
        else if (abs <= great) Debug.Log("Great!");
        else if (abs <= good) Debug.Log("Good!");
        else if (abs <= miss) Debug.Log("Bad!");
        else return;

        NoteSpawner.laneNotes[note.data.lane].Dequeue();
        Destroy(note.gameObject);
    }

    void ProcessHoldStart(NoteController note, float diff)
    {
        float abs = Mathf.Abs(diff);

        if (abs <= miss)
        {
            note.onHold();
            Debug.Log("Hold started");
        }
        else
        {
            NoteSpawner.laneNotes[note.data.lane].Dequeue();
            Destroy(note.gameObject);
        }
    }

    void ProcessHoldRelease(NoteController note, float diff)
    {
        float abs = Mathf.Abs(diff);

        if (abs <= perfect) Debug.Log("Hold Perfect!");
        else if (abs <= great) Debug.Log("Hold Great!");
        else if (abs <= good) Debug.Log("Hold Good!");
        else Debug.Log("Hold Miss!");

        NoteSpawner.laneNotes[note.data.lane].Dequeue();
        Destroy(note.gameObject);
    }
}