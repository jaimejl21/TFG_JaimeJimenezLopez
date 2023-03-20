using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectChar : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    ScenesManager sm;

    Color typeColor;
    
    void Start()
    {
        sm = GameObject.Find("CharactersManager").GetComponent<ScenesManager>();
        
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + transform.GetComponent<Character>().info.id;
        SetTypeColor();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("Selected character: " + transform.GetComponent<Character>().info.id);
        GameManager.inst.charToEquipGear = transform.GetComponent<Character>().info.id;
        sm.ChangeScene("CharInfo");
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData) { }

    void SetTypeColor()
    {
        switch(transform.GetComponent<Character>().info.type)
        {
            case 0:
                typeColor = Color.white;
                break;
            case 1:
                typeColor = new Color(.5f, .2f, .6f, 1f);
                break;
            case 2:
                typeColor = new Color(.5f, .3f, 0f, 1f);
                break;
            case 3:
                typeColor = Color.green;
                break;
            case 4:
                typeColor = Color.yellow;
                break;
            case 5:
                typeColor = Color.blue;
                break;
            case 6:
                typeColor = Color.red;
                break;
            default:
                break;
        }
        gameObject.transform.GetComponent<Image>().color = typeColor;
    }
}
