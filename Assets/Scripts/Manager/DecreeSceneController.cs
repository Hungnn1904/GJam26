using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecreeSceneController : MonoBehaviour
{
    [Header("UI - Decree")]
    public TextMeshProUGUI decreeText;
    public Button leftButton;
    public Button rightButton;
    public TextMeshProUGUI leftButtonText;
    public TextMeshProUGUI rightButtonText;

    [Header("Stats Bars Manager")]
    public Statistic_Item_Script statsVisualManager;

    [Header("Managers")]
    public DecreeManager decreeManager;

    private DecreeSO currentDecree;
    private GameStatsManager statsManager;
    private DayManager dayManager;

    void Start()
    {
        // Tìm managers
        if (decreeManager == null)
            decreeManager = FindObjectOfType<DecreeManager>();

        statsManager = GameStatsManager.Instance;
        dayManager = DayManager.Instance;

        if (statsVisualManager == null)
            statsVisualManager = FindObjectOfType<Statistic_Item_Script>();

        // Đồng bộ ngày
        if (dayManager != null && decreeManager != null)
        {
            decreeManager.currentDay = dayManager.currentDay;
            decreeManager.StartDay();
        }

        // Hiển thị decree đầu tiên
        ShowNextDecree();
        UpdateStatsBars();
    }

    void ShowNextDecree()
    {
        currentDecree = decreeManager.GetNextDecree();

        if (currentDecree == null)
        {
            // Hết decree trong ngày
            EndDay();
            return;
        }

        // Hiển thị decree
        decreeText.text = currentDecree.description;
        leftButtonText.text = currentDecree.leftChoice.text;
        rightButtonText.text = currentDecree.rightChoice.text;
    }

    // Gọi từ Button Left trong Inspector
    public void OnLeftButtonClick()
    {
        if (currentDecree == null) return;
        ProcessChoice(currentDecree.leftChoice);
    }

    // Gọi từ Button Right trong Inspector
    public void OnRightButtonClick()
    {
        if (currentDecree == null) return;
        ProcessChoice(currentDecree.rightChoice);
    }

    void ProcessChoice(DecreeChoice choice)
    {
        // Tắt buttons tạm thời
        leftButton.interactable = false;
        rightButton.interactable = false;

        // Đánh giá và áp dụng
        DecisionResult result = statsManager.EvaluateChoice(choice);
        statsManager.ApplyChoice(choice);

        // Cộng gold nếu quyết định tốt
        if (result == DecisionResult.Good)
        {
            statsManager.AddGold(1);
            Debug.Log("✅ +1 vàng");
        }

        // Cập nhật bars
        UpdateStatsBars();

        // Chuyển decree tiếp theo sau 1 giây
        Invoke(nameof(NextDecree), 1f);
    }

    void NextDecree()
    {
        leftButton.interactable = true;
        rightButton.interactable = true;
        ShowNextDecree();
    }

    void UpdateStatsBars()
    {
        if (statsVisualManager != null && statsManager != null)
        {
            statsVisualManager.Set(StatType.Popularity, statsManager.popularity);
            statsVisualManager.Set(StatType.Military, statsManager.military);
            statsVisualManager.Set(StatType.Finance, statsManager.finance);
            statsVisualManager.Set(StatType.Religion, statsManager.religion);
        }
    }

    void EndDay()
    {
        Debug.Log("Kết thúc ngày");

        if (dayManager != null)
        {
            dayManager.EndDay();

            // Nếu chưa game over, chuyển ngày mới
            if (!statsManager.IsAnyStatDead() && dayManager.currentDay <= dayManager.maxDay)
            {
                decreeManager.currentDay = dayManager.currentDay;
                decreeManager.StartDay();
                ShowNextDecree();
            }
        }
    }
}
