using System;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class Judge : MonoBehaviour
{
    public static Judge instance;
    [HideInInspector] public int combo = 0;
    [HideInInspector] public float score = 0;
    [HideInInspector] public float accuracy = 100;
    [HideInInspector] public float[] typeCount = new float[6];
    [HideInInspector] public float[] typeMult = new float[6] {1 , 1 , 2 / 3 , 1 / 3 , 1 / 6 , 0};
    float comboBonus = 0;
    float mainScore = 0;
    float hitCount = 0;
    void Awake()
    {
        instance = this;
    }
    public void Flawless() {
        typeCount[0]++;
        mainScore += 900000 / NoteSpawner.Instance.totalNotes;
        comboAdd();
        accUpdate();
        scoreUpdate();
    }

    public void Perfect() {
        typeCount[1]++;
        mainScore += 890000 / NoteSpawner.Instance.totalNotes;
        comboAdd();
        accUpdate();
        scoreUpdate();
    }

    public void Great() {
        typeCount[2]++;
        mainScore += 600000 / NoteSpawner.Instance.totalNotes;
        comboAdd();
        accUpdate();
        scoreUpdate();
    }

    public void Good() {
        typeCount[3]++;
        mainScore += 300000 / NoteSpawner.Instance.totalNotes;
        accUpdate();
        scoreUpdate();
    }
    public void Bad() {
        typeCount[4]++;
        mainScore += 150000 / NoteSpawner.Instance.totalNotes;
        comboBreak();
        accUpdate();
        scoreUpdate();
    }
    public void Miss()
    {
        typeCount[5]++;
        comboBreak();
        accUpdate();
    }

    void comboBreak()
    {
        combo = 0;
        UIManager.instance.SetCombo(combo);
    }
    void comboAdd()
    {
        comboBonus += (1 + 2 * combo) * 100000 / (NoteSpawner.Instance.totalNotes * NoteSpawner.Instance.totalNotes);
        combo++;
        UIManager.instance.SetCombo(combo);
    }
    void accUpdate() {
        hitCount++;
        float sum = 0;
        for (int i = 0; i < typeCount.Length; i++)
            sum += typeCount[i] * typeMult[i];
        accuracy = 100 * sum / Mathf.Max(hitCount , 1);
        UIManager.instance.SetAccuracy(accuracy);
    }

    void scoreUpdate()
    {
        score = mainScore + comboBonus;
        UIManager.instance.SetScore((int)score);
    }

}
