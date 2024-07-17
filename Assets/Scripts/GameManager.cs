using System;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public DateTime selectedDate { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SelectDate(DateTime day)
    {
        selectedDate = day;
    }

    public static string GetPath(DateTime date, string type = "")
    {
        string typeSuffix = type == "" ? "" : "_" + type;
        return Path.Combine(Application.persistentDataPath, date.ToString("yyyy-MM-dd") + typeSuffix + ".xml");
    }

    public static DayInfo GetDayInfo(DateTime date)
    {
        var path = GetPath(date, "day_info");
        if (File.Exists(path))
        {
            var sr = new XmlSerializer(typeof(DayInfo));
            using var str = new FileStream(path, FileMode.Open);
            return (DayInfo)sr.Deserialize(str);
        }
        return new DayInfo();
    }

    public static void SetDayInfo(DateTime date, DayInfo info)
    {
        var path = GetPath(date, "day_info");
        var sr = new XmlSerializer(typeof(DayInfo));
        using var str = new FileStream(path, FileMode.Create);
        sr.Serialize(str, info);
    }
}
