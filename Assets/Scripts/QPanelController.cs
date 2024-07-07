using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;
using System.Linq;

public class QPanelController : MonoBehaviour
{
    public static QPanelController instance { get; private set; }

    [SerializeField] private Card cardPrefab;
    [SerializeField] private List<QPanel> qPanels;

    private List<Card> cards;

    private DateTime date;


    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "Save"))
        {
            Save();
        }
        if (GUI.Button(new Rect(10, 70, 100, 50), "Load"))
        {
            cards.ForEach(card => Destroy(card.gameObject));
            Load();
        }
    }

    private void Awake()
    {
        instance = this;

        date = GameManager.instance.selectedDate;
        cards = new();
        Load();
    }

    public Card CreateCard(CardInfo cardInfo)
    {
        return CreateCard(cardInfo.posX, cardInfo.posY, cardInfo.qLevel, cardInfo.content, cardInfo.deadline);
    }

    public Card CreateCard(float posX, float posY, int qLevel, string content = "", DateTime deadline = default)
    {
        var card = Instantiate(cardPrefab, new(posX, posY), Quaternion.identity);
        card.transform.SetParent(qPanels[qLevel - 1].transform, false);
        card.Init(posX, posY, content, deadline, qLevel);
        cards.Add(card);
        return card;
    }

    public void Save()
    {
        string path = GetPath();
        var serializer = new XmlSerializer(typeof(List<CardInfo>));
        using var stream = new FileStream(path, FileMode.Create);
        var cardInfos = cards.Select(card => card.cardInfo).ToList();
        serializer.Serialize(stream, cardInfos);

        print("save card infos to " + path);
    }

    public void Load()
    {
        cards.Clear();
        string path = GetPath();

        print("load card infos from " + path);
        if (!File.Exists(path))
        {
            return;
        }

        var serializer = new XmlSerializer(typeof(List<CardInfo>));
        using var stream = new FileStream(path, FileMode.Open);
        var cardInfos = (List<CardInfo>)serializer.Deserialize(stream);
        cardInfos.ForEach(cardInfo => CreateCard(cardInfo));
        return;
    }

    private string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, date.ToString("yyyy-MM-dd") + ".xml");
    }
}
