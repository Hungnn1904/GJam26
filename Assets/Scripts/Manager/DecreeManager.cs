using System.Collections.Generic;
using UnityEngine;

public class DecreeManager : MonoBehaviour
{
    [Header("Day Config")]
    public int currentDay = 0;

    [Header("Decree Pools")]
    public List<DecreeSO> day0Decrees;
    public List<DecreeSO> day1Decrees;
    public List<DecreeSO> day2Decrees;
    public List<DecreeSO> day3Decrees;

    private List<DecreeSO> currentPool;

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

    public DecreeSO GetRandomDecree()
    {
        if (currentPool == null || currentPool.Count == 0)
        {
            Debug.LogError("❌ No decrees in pool");
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
