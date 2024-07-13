using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine.UI;

public class QPanelController : MonoBehaviour
{
    public static QPanelController instance { get; private set; }
    public int escLayer;

    [SerializeField] private Card cardPrefab;
    [SerializeField] private TakeawayCard takeawayCardPrefab;
    [SerializeField] private Transform qPanel;
    [SerializeField] private Transform buttons;

    private List<Card> cards;
    private List<TakeawayCard> takeawayCards;

    private DateTime date;
    private Button saveButton;
    private Button archiveButton;

    private void Awake()
    {
        instance = this;

        date = GameManager.instance.selectedDate;
        cards = new();
        takeawayCards = new();
        Load();

        escLayer = 0;

        saveButton = buttons.Find("Save").GetComponent<Button>();
        saveButton.onClick.AddListener(Save);
        archiveButton = buttons.Find("Archive").GetComponent<Button>();
        archiveButton.onClick.AddListener(Archive);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && escLayer == 0)
        {
            SceneController.instance.LoadScene("Calendar");
        }
    }

    public Card CreateCard(CardInfo cardInfo)
    {
        return CreateCard(cardInfo.posX, cardInfo.posY, cardInfo.qLevel, cardInfo.content, cardInfo.deadline, cardInfo.done);
    }

    public TakeawayCard CreateTakeawayCard(TakeawayCardInfo cardInfo)
    {
        return CreateTakeawayCard(cardInfo.posX, cardInfo.posY, cardInfo.content);
    }

    public Card CreateCard(float posX, float posY, int qLevel, string content = "", DateTime deadline = default, bool done = false)
    {
        var card = Instantiate(cardPrefab, new(posX, posY), Quaternion.identity);
        card.transform.SetParent(qPanel, false);
        card.Init(posX, posY, content, deadline, qLevel, done);
        cards.Add(card);
        return card;
    }

    public TakeawayCard CreateTakeawayCard(float posX, float posY, string content = "")
    {
        var card = Instantiate(takeawayCardPrefab, new(posX, posY), Quaternion.identity);
        card.transform.SetParent(qPanel, false);
        card.Init(posX, posY, content);
        takeawayCards.Add(card);
        return card;
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);
        Destroy(card.gameObject);
    }

    public void RemoveTakeawayCard(TakeawayCard takeawayCard)
    {
        takeawayCards.Remove(takeawayCard);
        Destroy(takeawayCard.gameObject);
    }

    public void Save()
    {
        {
            string path = GetPath(date);
            var serializer = new XmlSerializer(typeof(List<CardInfo>));
            using var stream = new FileStream(path, FileMode.Create);
            var cardInfos = cards.Select(card => card.info).Distinct().ToList();
            serializer.Serialize(stream, cardInfos);

            print("save card infos to " + path);
        }

        {
            string path = GetPath(date, "takeaway");
            var serializer = new XmlSerializer(typeof(List<TakeawayCardInfo>));
            using var stream = new FileStream(path, FileMode.Create);
            var cardInfos = takeawayCards.Select(card => card.info).Distinct().ToList();
            serializer.Serialize(stream, cardInfos);

            print("save takeaway card infos to " + path);
        }
    }

    /// <summary>
    /// Load card infos from the file and instantiate cards
    /// </summary>
    public void Load()
    {
        {
            cards.Clear();
            string path = GetPath(date);
            print("load card infos from " + path);
            if (File.Exists(path))
            {
                var serializer = new XmlSerializer(typeof(List<CardInfo>));
                using var stream = new FileStream(path, FileMode.Open);
                var cardInfos = (List<CardInfo>)serializer.Deserialize(stream);
                cardInfos.ForEach(cardInfo => CreateCard(cardInfo));
            }
        }

        {
            takeawayCards.Clear();
            string path = GetPath(date, "takeaway");
            print("load takeaway card infos from " + path);
            if (File.Exists(path))
            {
                var serializer = new XmlSerializer(typeof(List<TakeawayCardInfo>));
                using var stream = new FileStream(path, FileMode.Open);
                var cardInfos = (List<TakeawayCardInfo>)serializer.Deserialize(stream);
                cardInfos.ForEach(cardInfo => CreateTakeawayCard(cardInfo));
            }
        }
    }

    private string GetPath(DateTime date, string type = "")
    {
        string typeSuffix = type == "" ? "" : "_" + type;
        return Path.Combine(Application.persistentDataPath, date.ToString("yyyy-MM-dd") + typeSuffix + ".xml");
    }

    /// <summary>
    /// Not done cards are copied to the next day
    /// </summary>
    private void Archive()
    {
        var nextDate = date.AddDays(1);
        var nextPath = GetPath(nextDate);

        var nextDateInfos = new List<CardInfo>();
        if (File.Exists(nextPath))
        {
            var sr = new XmlSerializer(typeof(List<CardInfo>));
            using var str = new FileStream(nextPath, FileMode.Open);
            nextDateInfos = (List<CardInfo>)sr.Deserialize(str);
        }

        var serializer = new XmlSerializer(typeof(List<CardInfo>));
        using var stream = new FileStream(nextPath, FileMode.Create);
        var cardInfos = cards.Select(card => card.info).ToList();
        var notDoneCardInfos = cardInfos.Where(cardInfo => !cardInfo.done).ToList();
        nextDateInfos = nextDateInfos.Concat(notDoneCardInfos).Distinct().ToList();
        serializer.Serialize(stream, nextDateInfos);

        print(notDoneCardInfos.Count + " cards are copied to " + nextDate.ToString("yyyy-MM-dd"));

    }
}
