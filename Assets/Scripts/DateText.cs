using TMPro;
using UnityEngine;

public class DateText : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();

        text.text = GameManager.instance.selectedDate.ToString("yyyy-MM-dd");
    }
}
