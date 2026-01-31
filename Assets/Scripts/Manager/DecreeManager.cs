using System.Collections.Generic;
using UnityEngine;

public class DecreeManager : MonoBehaviour
{
    [Header("Day")]
    public int currentDay = 0;

    [Header("Fixed Decrees Per Day")]
    public List<DecreeSO> day0Fixed;
    public List<DecreeSO> day1Fixed;
    public List<DecreeSO> day2Fixed;

    [Header("Random Decree Pool")]
    public List<DecreeSO> randomPool;

    private Queue<DecreeSO> todayQueue = new Queue<DecreeSO>();

    void Start()
    {
        StartDay();
    }

    // =========================
    // START A NEW DAY
    // =========================
    public void StartDay()
    {
        todayQueue.Clear();

        // 1️⃣ Fixed decrees
        List<DecreeSO> fixedList = GetFixedForDay();
        foreach (var d in fixedList)
        {
            todayQueue.Enqueue(d);
        }

        // 2️⃣ Random decrees (2 cái)
        List<DecreeSO> tempRandom = new List<DecreeSO>(randomPool);

        for (int i = 0; i < 2 && tempRandom.Count > 0; i++)
        {
            int index = Random.Range(0, tempRandom.Count);
            todayQueue.Enqueue(tempRandom[index]);
            tempRandom.RemoveAt(index);
        }
    }

    // =========================
    // GET NEXT DECREE
    // =========================
    public DecreeSO GetNextDecree()
    {
        if (todayQueue.Count == 0)
        {
            Debug.Log("📭 Hết trát trong ngày");
            return null;
        }

        return todayQueue.Dequeue();
    }

    // =========================
    // FIXED BY DAY
    // =========================
    List<DecreeSO> GetFixedForDay()
    {
        switch (currentDay)
        {
            case 0: return day0Fixed;
            case 1: return day1Fixed;
            case 2: return day2Fixed;
            default: return day2Fixed;
        }
    }

    // =========================
    // NEXT DAY
    // =========================
    public void NextDay()
    {
        currentDay++;
        StartDay();
    }
}
