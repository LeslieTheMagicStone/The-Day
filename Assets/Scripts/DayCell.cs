using System;
using UnityEngine;
using UnityEngine.UI;

public class DayCell : MonoBehaviour
{
    private DateTime date;
    private int day;
    private bool isToday;
    private bool isSelected;
    private bool isWeekend;

    public void Init(DateTime date,int day, bool isToday, bool isWeekend)
    {
        this.date = date;
        this.day = day;
        this.isToday = isToday;
        this.isWeekend = isWeekend;
        isSelected = false;

        var text = GetComponentInChildren<TMPro.TMP_Text>();
        text.text = day.ToString();
        text.color = isToday ? Color.red : Color.black;
        text.fontStyle = isWeekend ? TMPro.FontStyles.Bold : TMPro.FontStyles.Normal;

        GetComponent<Button>().onClick.AddListener(OnClick); 
    }

    public void Select()
    {
        isSelected = true;
        GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
        GetComponent<Image>().color = Color.blue;
    }

    public void Deselect()
    {
        isSelected = false;
        GetComponentInChildren<TMPro.TMP_Text>().color = isToday ? Color.red : Color.black;
        GetComponent<Image>().color = Color.white;
    }

    public void OnClick()
    {
        GameManager.instance.SelectDate(date);
        SceneController.instance.LoadScene("Tasks");
    }
}
