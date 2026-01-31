using System.Collections.Generic;
using UnityEngine;

public class DecreeManager : MonoBehaviour
{
    public GameObject nextDayButton;
    [Header("Day")]
    public int currentDay = 0;

    [Header("Fixed Decrees Per Day")]
    public List<DecreeSO> day0Fixed;
    public List<DecreeSO> day1Fixed;
    public List<DecreeSO> day2Fixed;
    public List<DecreeSO> day3Fixed; // Added for day 3

    [Header("Random Decrees Per Day")]
    public List<DecreeSO> day0Random;
    public List<DecreeSO> day1Random;
    public List<DecreeSO> day2Random;
    public List<DecreeSO> day3Random; // Added for day 3

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

        // 1️⃣ Fixed
        foreach (var d in GetFixedForDay())
            todayQueue.Enqueue(d);

        // 2️⃣ Random theo ngày (lấy 2)
        List<DecreeSO> randomPool = new List<DecreeSO>(GetRandomPoolForDay());

        for (int i = 0; i < 2 && randomPool.Count > 0; i++)
        {
            int index = Random.Range(0, randomPool.Count);
            todayQueue.Enqueue(randomPool[index]);
            randomPool.RemoveAt(index);
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
            case 3: return day3Fixed; // Added for day 3
            default: return day3Fixed; // Default to day3Fixed for days >= 3
        }
    }

    List<DecreeSO> GetRandomPoolForDay()
    {
        switch (currentDay)
        {
            case 0: return day0Random;
            case 1: return day1Random;
            case 2: return day2Random;
            case 3: return day3Random; // Added for day 3
            default: return day3Random; // Default to day3Random for days >= 3
        }
    }

    // =========================
    // NEXT DAY
    // =========================
    public void NextDay()
    {
        nextDayButton.SetActive(true);
        currentDay++;
        StartDay();
    }
}