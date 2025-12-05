using UnityEngine;

[System.Serializable]
public enum TimingWindow { 
    Perfect = 50,
    Good = 100,
    Bad = 150,
    Miss = 200
}
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
       
    }
    void Update()
    {
        float dt = Time.deltaTime * 1000f;
        if (isHit)
            transform.position = new Vector3(NoteSpawner.Instance.laneX[data.lane],NoteSpawner.Instance.hitY,0);
        else
            transform.position += Vector3.down * speed * dt;
        if (!isHit && GameManager.instance.songTime > data.hitTime + (int)TimingWindow.Miss)
        {
            NoteSpawner.laneNotes[data.lane].Dequeue();
            Destroy(gameObject);
        }
    }
    public void setupHold()
    {
        tailLength = speed * (data.releaseTime - data.hitTime);
        tail.localScale = new Vector3(1f, tailLength, 1f);
        cap.Translate(new Vector3(0, radius * 2 * tailLength, 0));
    }
    public void onHold()
    {
        isHit = true;
    }
}
