using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardInfo cardInfo => _cardInfo;
    private CardInfo _cardInfo;
    private TMP_InputField input;
    private TMP_Text deadlineText;

    private Vector2 startPosition;
    private bool isDragging;
    private Transform canvasTransform;

    public void Init(float posX, float posY, string content, DateTime deadline, int qLevel)
    {
        input = GetComponentInChildren<TMP_InputField>();
        transform.position = new Vector2(posX, posY);
        input.text = content;
        _cardInfo = new CardInfo
        {
            posX = posX,
            posY = posY,
            content = content,
            deadline = deadline,
            qLevel = qLevel
        };
        input.onEndEdit.RemoveListener(UpdateContent);
        input.onEndEdit.AddListener(UpdateContent);

        canvasTransform = GetComponentInParent<Canvas>().transform;

        deadlineText = transform.Find("Deadline").GetComponent<TMP_Text>();
        UpdateDeadline(deadline);
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
        transform.SetAsLastSibling();
        startPosition = transform.position;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvasTransform == null) return;

        Vector2 move = eventData.delta / canvasTransform.localScale.x;
        transform.position += (Vector3)move;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        _cardInfo.posX = transform.position.x;
        _cardInfo.posY = transform.position.y;
        isDragging = false;
    }

    private void UpdateContent(string content)
    {
        _cardInfo.content = content;
    }

    private void UpdateDeadline(DateTime deadline)
    {
        _cardInfo.deadline = deadline;
        deadlineText.text = _cardInfo.deadlineString;
    }
}