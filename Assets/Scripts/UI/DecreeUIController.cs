using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecreeUIController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI decreeText;
    public TextMeshProUGUI leftChoiceText;
    public TextMeshProUGUI rightChoiceText;

    [Header("Managers")]
    public DecreeManager decreeManager;

    private Decree currentDecree;

    void Start()
    {
        ShowNextDecree();
    }

    void ShowNextDecree()
    {
        currentDecree = decreeManager.GetRandomDecree();

        if (currentDecree == null)
        {
            Debug.LogError("❌ No decree to show");
            return;
        }

        decreeText.text = currentDecree.storyText;
        leftChoiceText.text = currentDecree.leftChoice.description;
        rightChoiceText.text = currentDecree.rightChoice.description;
    }

    // =========================
    // BUTTON EVENTS
    // =========================
    public void ChooseLeft()
    {
        ResolveChoice(currentDecree.leftChoice);
    }

    public void ChooseRight()
    {
        ResolveChoice(currentDecree.rightChoice);
    }

    void ResolveChoice(DecreeChoice choice)
    {
        DecisionResult result =
        GameStatsManager.Instance.EvaluateChoice(choice);

        GameStatsManager.Instance.ApplyChoice(choice);

        // =====================
        // GOLD RULE (BUỔI SÁNG)
        // =====================
        switch (result)
        {
            case DecisionResult.Good:
                GameStatsManager.Instance.AddGold(1);
                Debug.Log("✅ Quyết định ỔN → +1 vàng");
                break;

            case DecisionResult.Extreme:
                Debug.Log("⚠️ Quyết định CỰC ĐOAN → +0 vàng");
                break;

            case DecisionResult.Dangerous:
                Debug.Log("🚨 Quyết định NGUY HIỂM → gắn cờ mẹ bệnh");
                break;
        }

        GameStatsManager.Instance.PrintStats();
        ShowNextDecree();
    }
}
