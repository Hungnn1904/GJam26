using System.Collections.Generic;
using UnityEngine;

public class DecreeManager : MonoBehaviour
{
    [Header("Day Config")]
    public int currentDay = 0;

    [Header("Decree Pools")]
    public List<Decree> day0Decrees;
    public List<Decree> day1Decrees;
    public List<Decree> day2Decrees;
    public List<Decree> day3Decrees;

    private List<Decree> currentPool;

    void Start()
    {
        LoadPoolForDay();
    }

    void LoadPoolForDay()
    {
        switch (currentDay)
        {
            case 0:
                currentPool = day0Decrees;
                break;
            case 1:
                currentPool = day1Decrees;
                break;
            case 2:
                currentPool = day2Decrees;
                break;
            case 3:
                currentPool = day3Decrees;
                break;
            default:
                currentPool = day3Decrees;
                break;
        }
    }

    public Decree GetRandomDecree()
    {
        if (currentPool == null || currentPool.Count == 0)
        {
            Debug.LogError("❌ No decrees in current pool!");
            return null;
        }

        int index = Random.Range(0, currentPool.Count);
        return currentPool[index];
    }

    public void NextDay()
    {
        currentDay++;
        LoadPoolForDay();
    }
}
