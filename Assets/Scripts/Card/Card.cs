using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler
{
    public CardInfo info => _info;
    private CardInfo _info;
    private TMP_InputField input;
    private TMP_Text deadlineText;
    private Toggle doneToggle;
    private QLevelVisualizer qLevelVisualizer;
    private GameObject focusFrame;

    public void Init(float posX, float posY, string content, DateTime deadline, int qLevel, bool done = false, bool isFocus = false)
    {
        input = GetComponentInChildren<TMP_InputField>();
        transform.position = new Vector2(posX, posY);
        input.text = content;
        _info = new CardInfo(posX, posY, content, deadline, qLevel, done, isFocus);
        input.onEndEdit.RemoveListener(UpdateContent);
        input.onEndEdit.AddListener(UpdateContent);

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

        // Visualize focus
        focusFrame = transform.Find("Focus").gameObject;
        SetFocus(isFocus);

        // Drag and drop stuff
        var dragAndDrop = GetComponent<DragAndDrop>();
        dragAndDrop.onEndDrag.RemoveListener(UpdatePos);
        dragAndDrop.onEndDrag.AddListener(UpdatePos);
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            QPanelController.instance.RemoveCard(this);
        if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 1)
        {
            if (info.isFocus)
                QPanelController.instance.SetFocusCard(null);
            else
                QPanelController.instance.SetFocusCard(this);
        }
    }

    public void SetFocus(bool value)
    {
        _info.isFocus = value;
        focusFrame.SetActive(value);
        transform.localScale = value ? new Vector3(1.2f, 1.2f, 1.2f) : Vector3.one;
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

    private void UpdateDeadline(DateTime deadline)
    {
        _info.deadline = deadline;
        deadlineText.text = _info.deadlineString;
    }

    private void UpdateDone(bool done)
    {
        _info.done = done;
    }

    private void UpdateQLevel(int qLevel)
    {
        _info.qLevel = qLevel;
    }
}