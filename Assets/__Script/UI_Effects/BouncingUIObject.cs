using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BouncingUIObject : MonoBehaviour
{
    public float speed = 200f;
    private RectTransform rectTransform;
    private Vector2 movementDirection = Vector2.one.normalized;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        rectTransform.anchoredPosition += movementDirection * speed * Time.deltaTime;

        // Check for canvas boundaries
        if (rectTransform.anchoredPosition.x > Screen.width / 2 - rectTransform.rect.width / 2 || rectTransform.anchoredPosition.x < -(Screen.width / 2 - rectTransform.rect.width / 2))
        {
            movementDirection.x = -movementDirection.x;
        }
        if (rectTransform.anchoredPosition.y > Screen.height / 2 - rectTransform.rect.height / 2 || rectTransform.anchoredPosition.y < -(Screen.height / 2 - rectTransform.rect.height / 2))
        {
            movementDirection.y = -movementDirection.y;
        }
    }
}
