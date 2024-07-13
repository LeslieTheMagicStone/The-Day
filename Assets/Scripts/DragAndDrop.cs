using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityEvent<float, float> onEndDrag;
    private Vector2 startPosition;
    private bool isDragging;
    private Transform canvasTransform;

    private void Start()
    {
        canvasTransform = GetComponentInParent<Canvas>().transform;
    }

    private void Update()
    {
        if (isDragging && Input.GetKeyDown(KeyCode.Escape))
        {
            transform.position = startPosition;
            OnEndDrag(null);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        QPanelController.instance.escLayer++;
        transform.SetAsLastSibling();
        startPosition = transform.position;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvasTransform == null) return;
        if (!isDragging) return;

        Vector2 move = eventData.delta / canvasTransform.localScale.x;
        transform.position += (Vector3)move;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        QPanelController.instance.escLayer--;
        onEndDrag.Invoke(transform.position.x, transform.position.y);
        isDragging = false;
    }
}
