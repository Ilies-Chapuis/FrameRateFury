using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    private Collider2D col;
    private bool isDragging;
    private Vector3 offset;

    void Start()
    {
        cam = Camera.main;
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        Vector3 touchPos = cam.ScreenToWorldPoint(touch.position);
        touchPos.z = 0f;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (col.OverlapPoint(touchPos))
                {
                    isDragging = true;
                    offset = transform.position - touchPos;
                }
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (isDragging)
                    transform.position = ClampToScreen(touchPos + offset);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                isDragging = false;
                break;
        }
    }

    private Vector3 ClampToScreen(Vector3 pos)
    {
        Vector3 min = cam.ViewportToWorldPoint(Vector3.zero);
        Vector3 max = cam.ViewportToWorldPoint(Vector3.one);
        Vector3 half = col.bounds.extents;

        pos.x = Mathf.Clamp(pos.x, min.x + half.x, max.x - half.x);
        pos.y = Mathf.Clamp(pos.y, min.y + half.y, max.y - half.y);
        pos.z = 0f;
        return pos;
    }
}