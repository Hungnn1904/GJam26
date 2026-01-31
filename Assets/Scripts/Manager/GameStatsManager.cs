using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    [Header("Core Stats (0 - 100)")]
    public int popularity = 50;
    public int military = 50;
    public int finance = 50;
    public int religion = 50;

    [Header("Gold")]
    public int gold = 0;

    [Header("Mother Health")]
    public int motherHealCost = 15;

    [Header("Danger Thresholds")]
    public int warningThreshold = 20;
    public int criticalThreshold = 10;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // =========================
    // APPLY DECREE EFFECT
    // =========================
    public void ApplyChoice(DecreeChoice choice)
    {
        foreach (StatEffect effect in choice.effects)
        {
            ApplyStat(effect.stat, effect.value);
        }

        CheckDeath();
        CheckDangerZones();
    }

    void ApplyStat(StatType stat, int value)
    {
        switch (stat)
        {
            case StatType.Popularity:
                popularity = Mathf.Clamp(popularity + value, 0, 100);
                break;
            case StatType.Military:
                military = Mathf.Clamp(military + value, 0, 100);
                break;
            case StatType.Finance:
                finance = Mathf.Clamp(finance + value, 0, 100);
                break;
            case StatType.Religion:
                religion = Mathf.Clamp(religion + value, 0, 100);
                break;
        }
    }

    // =========================
    // GOLD
    // =========================
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("💰 Gold: " + gold);
    }

    // =========================
    // DECISION EVALUATION
    // =========================
    public DecisionResult EvaluateChoice(DecreeChoice choice)
    {
        bool hasHeavyDamage = false;
        bool willEnterDangerZone = false;

        foreach (StatEffect effect in choice.effects)
        {
            if (effect.value < -10)
                hasHeavyDamage = true;

            int futureValue = GetStatValue(effect.stat) + effect.value;

            if ((effect.stat == StatType.Popularity || effect.stat == StatType.Religion)
                && futureValue < warningThreshold)
            {
                willEnterDangerZone = true;
            }
        }

        if (willEnterDangerZone)
            return DecisionResult.Dangerous;

        if (hasHeavyDamage)
            return DecisionResult.Extreme;

        return DecisionResult.Good;
    }

    int GetStatValue(StatType stat)
    {
        switch (stat)
        {
            case StatType.Popularity: return popularity;
            case StatType.Military: return military;
            case StatType.Finance: return finance;
            case StatType.Religion: return religion;
        }
        return 0;
    }

    // =========================
    // END OF DAY
    // =========================
    public void ProcessEndOfDay()
    {
        int addedCost = 0;

        if (popularity < criticalThreshold || religion < criticalThreshold)
            addedCost = 2;
        else if (popularity < warningThreshold || religion < warningThreshold)
            addedCost = 1;

        if (addedCost > 0)
        {
            motherHealCost += addedCost;
            Debug.Log($"💔 Mẹ yếu đi, cần thêm {addedCost} vàng");
        }
        else
        {
            Debug.Log("❤️ Mẹ tạm ổn hôm nay");
        }

        Debug.Log($"💊 Tổng chi phí chữa: {motherHealCost} vàng");
    }

    // =========================
    // CHECK GAME OVER
    // =========================
    void CheckDeath()
    {
        if (IsAnyStatDead())
        {
            Debug.Log("☠️ BAD END 2 – Bị lật đổ / xử tử");
            Time.timeScale = 0f;
        }
    }

    void CheckDangerZones()
    {
        if (popularity <= criticalThreshold || religion <= criticalThreshold)
            Debug.Log("🔴 NGUY KỊCH");
        else if (popularity <= warningThreshold || religion <= warningThreshold)
            Debug.Log("🟡 CẢNH BÁO");
    }

    public bool IsAnyStatDead()
    {
        return popularity <= 0 || military <= 0 || finance <= 0 || religion <= 0;
    }

    public float GetAverageStats()
    {
        return (popularity + military + finance + religion) / 4f;
    }

    public void PrintStats()
    {
        Debug.Log($"📊 Stats | Dân:{popularity} Quân:{military} Tài:{finance} Tôn:{religion} | 💰 Gold:{gold}");
    }
}
