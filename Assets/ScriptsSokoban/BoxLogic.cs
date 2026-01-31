using UnityEngine;

public class BoxLogic : MonoBehaviour
{
    [Header("Định danh hòm")]
    public int boxID; 
    public Sprite hinhXongViec; // Kéo Sprite hòm hoàn thành vào đây

    public bool IsInCorrectGoal(LayerMask goalLayer)
    {
        // Quét lỗ tại vị trí tâm hòm
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.1f, goalLayer);
        
        if (hit != null)
        {
            // Kiểm tra tên Object Goal có chứa ID không
            if (hit.gameObject.name.Contains(boxID.ToString()))
            {
                // Nếu đúng, đổi hình rồi trả về true
                if(hinhXongViec != null) GetComponent<SpriteRenderer>().sprite = hinhXongViec;
                return true;
            }
        }
        return false;
    }
}