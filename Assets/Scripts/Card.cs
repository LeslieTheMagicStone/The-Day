using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public CardInfo cardInfo => _cardInfo;
    private CardInfo _cardInfo;
    private TMP_InputField input;
    private TMP_Text deadlineText;
    private Toggle doneToggle;

    private Vector2 startPosition;
    private bool isDragging;
    private Transform canvasTransform;

    private QLevelVisualizer qLevelVisualizer;

    public void Init(float posX, float posY, string content, DateTime deadline, int qLevel, bool done = false)
    {
        input = GetComponentInChildren<TMP_InputField>();
        transform.position = new Vector2(posX, posY);
        input.text = content;
        _cardInfo = new CardInfo(posX, posY, content, deadline, qLevel, done);
        input.onEndEdit.RemoveListener(UpdateContent);
        input.onEndEdit.AddListener(UpdateContent);

        canvasTransform = GetComponentInParent<Canvas>().transform;

        // Visualize Deadline
        deadlineText = transform.Find("Deadline").GetComponent<TMP_Text>();
        UpdateDeadline(deadline);

        // Visualize Done
        doneToggle = transform.Find("Done").GetComponent<Toggle>();
        doneToggle.isOn = done;
        doneToggle.onValueChanged.RemoveListener(UpdateDone);
        doneToggle.onValueChanged.AddListener(UpdateDone);

        // Visualize Q Level
        qLevelVisualizer = GetComponentInChildren<QLevelVisualizer>();
        qLevelVisualizer.UpdateQLevel(qLevel);
        qLevelVisualizer.onQLevelChange.RemoveListener(UpdateQLevel);
        qLevelVisualizer.onQLevelChange.AddListener(UpdateQLevel);
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
        _cardInfo.posX = transform.position.x;
        _cardInfo.posY = transform.position.y;
        isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            QPanelController.instance.RemoveCard(this);
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

    private void UpdateDone(bool done)
    {
        _cardInfo.done = done;
    }

    private void UpdateQLevel(int qLevel)
    {
        _cardInfo.qLevel = qLevel;
    }
}