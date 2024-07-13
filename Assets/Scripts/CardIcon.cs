using UnityEngine;
using UnityEngine.EventSystems;

public class CardIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 startPosition;
    private bool isDragging;
    private Transform canvasTransform;

    private Card currentCard;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var pos = transform.position;
        currentCard = QPanelController.instance.CreateCard(pos.x, pos.y, 1);
        currentCard.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentCard.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentCard.OnEndDrag(eventData);
        currentCard = null;
    }
}
