using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MerchantItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    string typeName;
    public int price;

    MerchantManager mm;

    public TextMeshProUGUI text;

    [SerializeField]
    Color statTypeColor, rarityColor;

    void Start()
    {
        mm = FindObjectOfType<MerchantManager>();
        
        SetName();
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + transform.GetComponent<Gear>().info.id + "\n" + typeName;
        SetGearStatColor();
        SetRarityColor();
        SetPrice();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        mm.ChangeItemInfo(gameObject);
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
            case 6:
                typeName = "Sword";
                break;
            case 7:
                typeName = "Spear";
                break;
            case 8:
                typeName = "Scythe";
                break;
            case 9:
                typeName = "Dagger";
                break;
            case 10:
                typeName = "Staff";
                break;
            case 11:
                typeName = "Bow";
                break;
            case 12:
                typeName = "Axe";
                break;
            default:
                break;
        }
    }

    void SetGearStatColor()
    {
        int statType = gameObject.transform.GetComponent<Gear>().info.statType;
        switch (statType)
        {
            case -1:
                statTypeColor = new Color(1f, 1f, 1f, 1f);
                break;
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

    public void SetPrice()
    {
        int rarity = gameObject.GetComponent<Gear>().info.rarity;
        switch(rarity)
        {
            case 0:
                price = 60;
                break;
            case 1:
                price = 120;
                break;
            case 2:
                price = 180;
                break;
            default:
                break;
        }
    }
}
