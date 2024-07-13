using UnityEngine;
using UnityEngine.EventSystems;

public class TakeawayCardIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private DragAndDrop currentDragAndDrop;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var pos = transform.position;
        currentDragAndDrop = QPanelController.instance.CreateTakeawayCard(pos.x, pos.y).GetComponent<DragAndDrop>();
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
