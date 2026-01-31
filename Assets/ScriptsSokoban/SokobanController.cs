using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SokobanController : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 0.1f;
    public LayerMask wallLayer;
    public LayerMask boxLayer;

    private bool isMoving = false;

    // Lưu trữ dữ liệu để lùi bước (Undo)
    struct StepData { 
        public Vector2 playerPos; 
        public Vector3[] boxesPos; 
        public bool[] boxesActive; 
    }
    private Stack<StepData> history = new Stack<StepData>();

    void Update()
    {
        // Phím R: Chơi lại màn
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (isMoving) return;

        // Phím E: Reset 1 bước (Undo)
        if (Input.GetKeyDown(KeyCode.E)) Undo();

        Vector2 moveDir = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) moveDir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) moveDir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) moveDir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) moveDir = Vector2.right;

        if (moveDir != Vector2.zero) TryMove(moveDir);
    }

    void TryMove(Vector2 direction)
    {
        Vector2 targetPos = (Vector2)transform.position + direction * moveDistance;
        if (Physics2D.OverlapCircle(targetPos, 0.1f, wallLayer)) return;

        Collider2D boxCollider = Physics2D.OverlapCircle(targetPos, 0.1f, boxLayer);
        if (boxCollider != null)
        {
            Vector2 nextBoxPos = (Vector2)boxCollider.transform.position + direction * moveDistance;
            if (!Physics2D.OverlapCircle(nextBoxPos, 0.1f, wallLayer | boxLayer))
            {
                SaveStep(); // Lưu trước khi đẩy hòm
                StartCoroutine(SmoothMove(transform.gameObject, targetPos, false));
                StartCoroutine(SmoothMove(boxCollider.gameObject, nextBoxPos, true));
            }
        }
        else
        {
            SaveStep(); // Lưu trước khi bước đi
            StartCoroutine(SmoothMove(transform.gameObject, targetPos, false));
        }
    }

    IEnumerator SmoothMove(GameObject obj, Vector2 target, bool isBox)
    {
        isMoving = true;
        Vector2 startPos = obj.transform.position;
        float elapsed = 0;
        while (elapsed < moveSpeed)
        {
            obj.transform.position = Vector2.Lerp(startPos, target, elapsed / moveSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = new Vector3(target.x, target.y, 0);
        isMoving = false;
        if (isBox) GameManager.instance.CheckWinCondition();
    }

    void SaveStep()
    {
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        Vector3[] bPos = new Vector3[boxes.Length];
        bool[] bActive = new bool[boxes.Length];
        for (int i = 0; i < boxes.Length; i++) {
            bPos[i] = boxes[i].transform.position;
            bActive[i] = boxes[i].activeSelf;
        }
        history.Push(new StepData { playerPos = transform.position, boxesPos = bPos, boxesActive = bActive });
    }

    void Undo()
    {
        if (history.Count == 0) return;
        StepData last = history.Pop();
        transform.position = last.playerPos;
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        for (int i = 0; i < boxes.Length; i++) {
            boxes[i].transform.position = last.boxesPos[i];
            boxes[i].SetActive(last.boxesActive[i]);
        }
        GameManager.instance.CheckWinCondition();
    }
}