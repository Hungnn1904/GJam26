using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("Day Settings")]
    public int currentDay = 1;
    public int maxDay = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndDay()
    {
        Debug.Log($"🌙 Kết thúc ngày {currentDay}");

        // 1. Xử lý bệnh của mẹ
        GameStatsManager.Instance.ProcessEndOfDay();

        // 2. Kiểm tra chết ngay (chỉ số = 0)
        if (GameStatsManager.Instance.IsAnyStatDead())
        {
            TriggerBadEnd2();
            return;
        }

        // 3. Sang ngày mới hoặc kết game
        if (currentDay >= maxDay)
        {
            CheckFinalEnding();
        }
        else
        {
            currentDay++;
            Debug.Log($"🌅 Sang ngày {currentDay}");
        }
    }

    void TriggerBadEnd2()
    {
        Debug.Log("☠️ BAD END 2: Bị lật đổ / tử hình");
        // sau này load scene ending
    }

    void CheckFinalEnding()
    {
        GameStatsManager stats = GameStatsManager.Instance;

        bool enoughGold = stats.gold >= stats.motherHealCost;
        float avgStats = stats.GetAverageStats();

        if (!enoughGold)
        {
            Debug.Log("☠️ BAD END 1: Không đủ tiền cứu mẹ");
        }
        else if (avgStats >= 70)
        {
            Debug.Log("🎉 HAPPY END");
        }
        else
        {
            Debug.Log("😐 NEUTRAL END");
        }
    }
}
