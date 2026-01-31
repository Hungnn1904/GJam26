using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SokobanController : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 0.1f; 
    public LayerMask wallLayer;
    public LayerMask boxLayer;

    private bool isMoving = false;
    struct GameState { public Vector2 playerPos; public Vector2[] boxesPos; }
    private Stack<GameState> history = new Stack<GameState>();

    void Update()
    {
        if (isMoving) return;

        Vector2 moveDir = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) moveDir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) moveDir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) moveDir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) moveDir = Vector2.right;

        if (moveDir != Vector2.zero) TryMove(moveDir);
        if (Input.GetKeyDown(KeyCode.U)) Undo();
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
                SaveHistory();
                StartCoroutine(SmoothMove(transform.gameObject, targetPos, false));
                StartCoroutine(SmoothMove(boxCollider.gameObject, nextBoxPos, true));
            }
        }
        else
        {
            SaveHistory();
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
        obj.transform.position = target;
        isMoving = false;

        // Nếu là hòm vừa di chuyển, yêu cầu GameManager kiểm tra thắng
        if (isBox) GameManager.instance.CheckWinCondition();
    }

    void SaveHistory()
    {
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        Vector2[] positions = new Vector2[boxes.Length];
        for (int i = 0; i < boxes.Length; i++) positions[i] = boxes[i].transform.position;
        history.Push(new GameState { playerPos = transform.position, boxesPos = positions });
    }

    public void Undo()
    {
        if (isMoving || history.Count == 0) return;
        GameState lastState = history.Pop();
        transform.position = lastState.playerPos;
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        for (int i = 0; i < boxes.Length; i++) boxes[i].transform.position = lastState.boxesPos[i];
        
        // Sau khi Undo cũng cần kiểm tra lại điều kiện thắng
        GameManager.instance.CheckWinCondition();
    }
}