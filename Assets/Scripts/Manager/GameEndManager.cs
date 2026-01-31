using UnityEngine;

public enum GameEndingType
{
    None,
    Happy,
    Neutral,
    BadMoney,
    BadPolitical
}

public class GameEndManager : MonoBehaviour
{
    public static GameEndManager Instance;

    [Header("Game Config")]
    public int lastDay = 3;

    [Header("Ending Result")]
    public GameEndingType currentEnding = GameEndingType.None;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // =========================
    // CALL KHI HẾT NGÀY CUỐI
    // =========================
    public void CheckFinalEnding()
    {
        GameStatsManager stats = GameStatsManager.Instance;

        // ❌ BAD END 2 – CHÍNH TRỊ SỤP ĐỔ
        if (stats.IsAnyStatDead())
        {
            TriggerEnding(GameEndingType.BadPolitical);
            return;
        }

        // ❌ BAD END 1 – THIẾU TIỀN
        if (stats.gold < stats.motherHealCost)
        {
            TriggerEnding(GameEndingType.BadMoney);
            return;
        }

        // ✅ ĐỦ TIỀN → CHECK AVG STATS
        float avg = stats.GetAverageStats();

        if (avg >= 70f)
        {
            TriggerEnding(GameEndingType.Happy);
        }
        else
        {
            TriggerEnding(GameEndingType.Neutral);
        }
    }

    void TriggerEnding(GameEndingType ending)
    {
        currentEnding = ending;

        Debug.Log("🎬 GAME END: " + ending.ToString());

        // Freeze game tạm thời
        Time.timeScale = 0f;

        // Sau này bạn chỉ cần:
        // Load scene ending / show UI / subtitle ở đây
    }
}
