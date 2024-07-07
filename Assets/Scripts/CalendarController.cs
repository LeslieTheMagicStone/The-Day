using System;
using System.Collections.Generic;
using UnityEngine;

public class CalendarController : MonoBehaviour
{
    [SerializeField] private DayCell dayCellOrig;
    [SerializeField] private Transform emptyCellOrig;
    [SerializeField] private Transform calendarContent;

    private void Awake()
    {
        var today = DateTime.Today;
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;
        for (int i = 0; i < dayOfWeek; i++)
        {
            var cell = Instantiate(emptyCellOrig, transform);
            cell.SetParent(calendarContent, false);
            cell.gameObject.SetActive(true);
        }
        int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

        for (int i = 1; i <= daysInMonth; i++)
        {
            var cell = Instantiate(dayCellOrig, transform);
            cell.transform.SetParent(calendarContent, false);
            var date = new DateTime(today.Year, today.Month, i);
            cell.Init(date, i, i == today.Day, i % 7 == 0 || i % 7 == 6);
            cell.gameObject.SetActive(true);
        }
    }
}
