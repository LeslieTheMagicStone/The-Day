using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class ExampleClass : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        //Grab the number of consecutive clicks and assign it to an integer varible.
        int i = eventData.clickCount;
        //Display the click count.
        Debug.Log(i);
    }
}