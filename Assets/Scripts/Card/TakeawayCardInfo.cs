using System;

[Serializable]
public struct TakeawayCardInfo
{
    public float posX;
    public float posY;
    public string content;

    public TakeawayCardInfo(float posX, float posY, string content)
    {
        this.posX = posX;
        this.posY = posY;
        this.content = content;
    }
}
