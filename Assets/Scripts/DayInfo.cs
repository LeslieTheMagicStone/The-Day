using System;

[Serializable]
public struct DayInfo
{
    public bool checkmarked;

    public DayInfo(bool checkmarked = false)
    {
        this.checkmarked = checkmarked;
    }
}