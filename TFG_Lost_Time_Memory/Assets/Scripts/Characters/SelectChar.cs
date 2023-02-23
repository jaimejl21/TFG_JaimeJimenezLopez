using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectChar : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    ScenesManager sm;
    
    void Start()
    {
        sm = GameObject.Find("CharactersManager").GetComponent<ScenesManager>();
        
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + transform.GetComponent<Character>().info.id;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("Selected character: " + transform.GetComponent<Character>().info.id);
        GameManager.inst.charToEquipGear = transform.GetComponent<Character>().info.id;
        sm.ChangeScene("CharInfo");
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData) { }
}
