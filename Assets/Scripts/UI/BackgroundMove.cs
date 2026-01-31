using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float speed = 10f; // càng nhỏ càng chậm

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
