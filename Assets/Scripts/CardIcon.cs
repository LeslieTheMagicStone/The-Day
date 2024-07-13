using UnityEngine;
using UnityEngine.EventSystems;

public class CardIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private DragAndDrop currentDragAndDrop;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var pos = transform.position;
        currentDragAndDrop = QPanelController.instance.CreateCard(pos.x, pos.y, 1).GetComponent<DragAndDrop>();
        currentDragAndDrop.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentDragAndDrop.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentDragAndDrop.OnEndDrag(eventData);
        currentDragAndDrop = null;
    }
}
