using UnityEngine;

public class LogicGame : MonoBehaviour {
    // Kéo mảnh tileset_8 (hòm viền xanh) vào đây ở Inspector
    public Sprite hinhXongViec; 
    private Sprite hinhBanDau;
    private SpriteRenderer render;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        hinhBanDau = render.sprite;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Target")) {
            render.sprite = hinhXongViec;
            Debug.Log("Làm tốt lắm! Bạn nhận được 2 xu vàng.");
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Target")) {
            render.sprite = hinhBanDau;
        }
    }
}