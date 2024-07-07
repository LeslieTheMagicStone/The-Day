using System;

[Serializable]
public struct CardInfo 
{
    public float posX;
    public float posY;
    public string content;
    public DateTime deadline;
    public readonly string deadlineString => deadline.ToString("yyyy-MM-dd HH:mm:ss");
    public int qLevel;
}
