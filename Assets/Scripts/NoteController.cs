using UnityEngine;

[System.Serializable]
public class NoteController : MonoBehaviour
{
    public NoteData data;
    public float radius = 0.84f;
    public float speed; //unit/ms
    public Transform tail;
    public Transform cap;
    [HideInInspector]
    public bool isHit = false;
    [HideInInspector]
    public bool isReleased = false;
    float tailLength;
    void Start()
    {
        if (data.type == NoteType.Hold)
        {
            tailLength = speed * (data.releaseTime - data.hitTime);
            tail.localScale = new Vector3(1f, tailLength, 1f);
            cap.Translate(new Vector3(0, radius * 2 * tailLength, 0));
        }

    }
    void Update()
    {
        if (isHit && data.type != NoteType.Hold) return;
        // move downward at speed
        float dy = (float)(speed * Time.deltaTime * 1000);
        transform.position += new Vector3(0f, -dy, 0f);
    }
}
