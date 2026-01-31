using TMPro;
using UnityEngine;

public class DecreeUIController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI decreeText;
    public TextMeshProUGUI leftChoiceText;
    public TextMeshProUGUI rightChoiceText;

    [Header("Managers")]
    public DecreeManager decreeManager;

    private DecreeSO currentDecree;

    void Start()
    {
        ShowNextDecree();
    }

    void ShowNextDecree()
    {
        currentDecree = decreeManager.GetNextDecree();

        if (currentDecree == null)
        {
            Debug.Log("📅 Hết trát trong ngày");
            return;
        }

        decreeText.text = currentDecree.description;
        leftChoiceText.text = currentDecree.leftChoice.text;
        rightChoiceText.text = currentDecree.rightChoice.text;
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
                Debug.Log("🚨 Quyết định NGUY HIỂM");
                break;
        }

        GameStatsManager.Instance.PrintStats();
        ShowNextDecree();
    }
}
