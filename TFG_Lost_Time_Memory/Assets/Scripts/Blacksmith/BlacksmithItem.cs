using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlacksmithItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    string typeName;
    public int upMat, awMat, price;

    BlacksmithManager bm;

    public TextMeshProUGUI text;

    [SerializeField]
    Color statTypeColor, rarityColor;

    void Start()
    {
        bm = FindObjectOfType<BlacksmithManager>();
        
        SetName();
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + transform.GetComponent<Gear>().info.id + "\n" + typeName;
        SetGearStatColor();
        SetRarityColor();
        SetMatPrice();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        bm.ChangeItemInfo(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData) { }

    public void SetName()
    {
        int type = transform.GetComponent<Gear>().info.objType;
        switch(type)
        {
            case 0:
                typeName = "Bracer";
                break;
            case 1:
                typeName = "Neck";
                break;
            case 2:
                typeName = "Belt";
                break;
            case 3:
                typeName = "Ring";
                break;
            case 4:
                typeName = "Earring";
                break;
            case 5:
                typeName = "Orb";
                break;
        }
    }

    void SetGearStatColor()
    {
        int statType = gameObject.transform.GetComponent<Gear>().info.statType;
        switch (statType)
        {
            case 0:
                statTypeColor = new Color(.5f, 0f, 0f, 1f);
                break;
            case 1:
                statTypeColor = new Color(0f, 0f, .5f, 1f);
                break;
            case 2:
                statTypeColor = new Color(0f, .5f, 0f, 1f);
                break;
            default:
                break;
        }
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = statTypeColor;
    }

    public void SetRarityColor()
    {
        int rarity = gameObject.GetComponent<Gear>().info.rarity;
        switch (rarity)
        {
            case 0:
                rarityColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);
                break;
            case 1:
                rarityColor = new Color(0.5f, 0f, 1f, 0.6f);
                break;
            case 2:
                rarityColor = new Color(1f, 0.7f, 0f, 0.6f);
                break;
            default:
                break;
        }
        gameObject.transform.GetComponent<Image>().color = rarityColor;
    }

    public void SetMatPrice()
    {
        int rarity = gameObject.GetComponent<Gear>().info.rarity;
        int stars = gameObject.GetComponent<Gear>().info.stars;
        if(stars == 0)
        {
            //switch(rarity)
            //{
            //    case
            //}
        }
        
    }
}
