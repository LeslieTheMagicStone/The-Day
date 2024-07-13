using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TakeawayCard : MonoBehaviour, IPointerDownHandler
{
    public TakeawayCardInfo info => _info;
    private TakeawayCardInfo _info;
    private TMP_InputField input;

    public void Init(float posX, float posY, string content)
    {
        input = GetComponentInChildren<TMP_InputField>();
        transform.position = new Vector2(posX, posY);
        input.text = content;
        _info = new TakeawayCardInfo(posX, posY, content);
        input.onEndEdit.RemoveListener(UpdateContent);
        input.onEndEdit.AddListener(UpdateContent);

        // Drag and drop stuff
        var dragAndDrop = GetComponent<DragAndDrop>();
        dragAndDrop.onEndDrag.RemoveListener(UpdatePos);
        dragAndDrop.onEndDrag.AddListener(UpdatePos);
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            QPanelController.instance.RemoveTakeawayCard(this);
    }

    private void UpdatePos(float x, float y)
    {
        _info.posX = x;
        _info.posY = y;
    }

    private void UpdateContent(string content)
    {
        _info.content = content;
    }
}