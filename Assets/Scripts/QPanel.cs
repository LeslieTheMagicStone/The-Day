using UnityEngine;
using UnityEngine.EventSystems;

public class QPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int qLevel;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        QPanelController.instance.CreateCard(eventData.position.x, eventData.position.y, qLevel);
    }
}
