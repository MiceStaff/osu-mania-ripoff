using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public ButtonController[] buttons = new ButtonController[4];
    public float flawless = 20f;
    public float perfect = 50f;
    public float great = 100f;
    public float good = 150f;
    public float fail = 200f;
    private void Awake()
    {
        instance = this;
    }
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

        if (abs <= flawless) Judge.instance.Flawless();
        else if (abs <= perfect) Judge.instance.Perfect();
        else if (abs <= great) Judge.instance.Great();
        else if (abs <= good) Judge.instance.Good();
        else if (abs <= fail) Judge.instance.Bad();
        else return;
        UIManager.instance.SetDelta(diff);
        NoteSpawner.laneNotes[note.data.lane].Dequeue();
        Destroy(note.gameObject);
    }

    void ProcessHoldStart(NoteController note, float diff)
    {
        float abs = Mathf.Abs(diff);
        note.onHold();
        Debug.Log("Hold started");
    }

    void ProcessHoldRelease(NoteController note, float diff)
    {
        float abs = Mathf.Abs(diff);
        if (abs <= flawless) Judge.instance.Flawless();
        else if (abs <= perfect) Judge.instance.Perfect();
        else if (abs <= great) Judge.instance.Great();
        else if (abs <= good) Judge.instance.Good();
        else Judge.instance.Bad();

        NoteSpawner.laneNotes[note.data.lane].Dequeue();
        Destroy(note.gameObject);
    }
}