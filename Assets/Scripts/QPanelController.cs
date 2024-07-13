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
    [SerializeField] private Transform qPanel;
    [SerializeField] private Transform buttons;

    private List<Card> cards;

    private DateTime date;
    private Button saveButton;
    private Button archiveButton;


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

        escLayer = 0;

        saveButton = buttons.Find("Save").GetComponent<Button>();
        saveButton.onClick.AddListener(Save);
        archiveButton = buttons.Find("Archive").GetComponent<Button>();
        archiveButton.onClick.AddListener(Archive);
    }

    private void Update()
    {
        print(escLayer);
        if (Input.GetKeyDown(KeyCode.Escape) && escLayer == 0)
        {
            SceneController.instance.LoadScene("Calendar");
        }
    }

    public Card CreateCard(CardInfo cardInfo)
    {
        return CreateCard(cardInfo.posX, cardInfo.posY, cardInfo.qLevel, cardInfo.content, cardInfo.deadline, cardInfo.done);
    }

    public Card CreateCard(float posX, float posY, int qLevel, string content = "", DateTime deadline = default, bool done = false)
    {
        var card = Instantiate(cardPrefab, new(posX, posY), Quaternion.identity);
        card.transform.SetParent(qPanel, false);
        card.Init(posX, posY, content, deadline, qLevel, done);
        cards.Add(card);
        return card;
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);
        Destroy(card.gameObject);
    }

    public void Save()
    {
        string path = GetPath(date);
        var serializer = new XmlSerializer(typeof(List<CardInfo>));
        using var stream = new FileStream(path, FileMode.Create);
        var cardInfos = cards.Select(card => card.cardInfo).ToList();
        serializer.Serialize(stream, cardInfos);

        print("save card infos to " + path);
    }

    /// <summary>
    /// Load card infos from the file and instantiate cards
    /// </summary>
    public void Load()
    {
        cards.Clear();
        string path = GetPath(date);

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

    private string GetPath(DateTime date)
    {
        return Path.Combine(Application.persistentDataPath, date.ToString("yyyy-MM-dd") + ".xml");
    }

    /// <summary>
    /// Not done cards are copied to the next day
    /// </summary>
    private void Archive()
    {
        var nextDate = date.AddDays(1);
        var nextPath = GetPath(nextDate);

        var serializer = new XmlSerializer(typeof(List<CardInfo>));
        using var stream = new FileStream(nextPath, FileMode.Create);
        var cardInfos = cards.Select(card => card.cardInfo).ToList();
        var notDoneCardInfos = cardInfos.Where(cardInfo => !cardInfo.done).ToList();
        serializer.Serialize(stream, notDoneCardInfos);

        print("save card infos to " + nextPath);
    }
}
