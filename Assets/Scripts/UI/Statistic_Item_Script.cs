using UnityEngine;
using UnityEngine.UI;

public class Statistic_Item_Script : MonoBehaviour
{
    private RectTransform fill;
    private Image image; 
    private float startFill;
    private float endFill;

    void Start ()
    {
        image = GetComponent<Image>();
        fill = GetComponent<RectTransform>();
        startFill = fill.anchoredPosition.y;
        endFill = startFill + fill.rect.height;

    }

    public void Set(float current)
    {
        // Tính toán kích thước mới dựa trên tỉ lệ phần trăm
        float percentage = current / 100f;

        // Cập nhật vị trí của fill
        fill.anchoredPosition = new Vector2(fill.anchoredPosition.x, Mathf.Lerp(startFill, endFill, percentage));
        
        if(current <=20)
        {
            //Màu đỏ
            image.color = new Color32(255,0,0,255);

        }
        else
        {
            //màu đen
            image.color = new Color32(0,0,0,255);
        }
}

}
