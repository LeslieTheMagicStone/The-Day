using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class QLevelVisualizer : MonoBehaviour, IScrollHandler
{
    public int qLevel { get; private set; }
    public UnityEvent<int> onQLevelChange;
    private GameObject cellOrig;

    public void UpdateQLevel(int qLevel)
    {
        if (qLevel == this.qLevel) return;
        if (qLevel < 0) return;

        if (cellOrig == null)
            cellOrig = transform.GetChild(0).gameObject;

        for (int i = 1; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        for (int i = 0; i < qLevel; i++)
        {
            var cell = Instantiate(cellOrig, transform);
            cell.SetActive(true);
        }

        this.qLevel = qLevel;
        onQLevelChange.Invoke(qLevel);
    }

    public void Increase()
    {
        UpdateQLevel(qLevel + 1);
    }

    public void Decrease()
    {
        UpdateQLevel(qLevel - 1);
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (eventData.scrollDelta.y > 0)
            Increase();
        else
            Decrease();
    }
}
