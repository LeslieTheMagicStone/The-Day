using System;

[Serializable]
public struct CardInfo
{
    public float posX;
    public float posY;
    public string content;
    public DateTime deadline;
    public readonly string deadlineString => deadline == default ? "" : deadline.ToString("yyyy-MM-dd HH:mm:ss");
    public int qLevel;
    public bool done;
    public bool isFocus;

    public CardInfo(float posX, float posY, string content, DateTime deadline, int qLevel, bool done = false, bool isFocus = false)
    {
        this.posX = posX;
        this.posY = posY;
        this.content = content;
        this.deadline = deadline;
        this.qLevel = qLevel;
        this.done = done;
        this.isFocus = isFocus;
    }
}
