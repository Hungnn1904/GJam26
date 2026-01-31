using UnityEngine;
using UnityEngine.UI;


public class Statistic_Item_Script : MonoBehaviour
{
    public RectTransform humanBar;
    public RectTransform militaryBar;
    public RectTransform economyBar;
    public RectTransform religionBar;

    private float originalWidth;

    void Start()
    {
        // Lấy width gốc từ 1 thanh duy nhất
        if (humanBar != null)
        {
            originalWidth = humanBar.rect.width;

            // fallback nếu rect.width = 0
            if (originalWidth <= 0f)
                originalWidth = humanBar.sizeDelta.x;
        }
    }

    public void SetSize(StatType stat, float current)
    {
        // Chuẩn hóa current về 0–1
        float fraction = current > 1f ? current / 100f : current;
        fraction = Mathf.Clamp01(fraction);

        float newWidth = originalWidth * fraction;

        switch (stat)
        {
            case StatType.Popularity:
                SetBarWidth(humanBar, newWidth);
                break;

            case StatType.Military:
                SetBarWidth(militaryBar, newWidth);
                break;

            case StatType.Finance:
                SetBarWidth(economyBar, newWidth);
                break;

            case StatType.Religion:
                SetBarWidth(religionBar, newWidth);
                break;
        }
    }

    private void SetBarWidth(RectTransform bar, float width)
    {
        if (bar == null) return;

        bar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            width
        );
    }

}
