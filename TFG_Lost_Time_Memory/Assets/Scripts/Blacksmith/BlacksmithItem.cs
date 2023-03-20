using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlacksmithItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    string typeName;
    int upMatInc, awMatInc, awPriceInc, upPriceInc, upInitPrice, cumuPrice;
    public int upMat, awMat, awPrice, upPrice;
    public bool upgrade = true;

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
        SetAwUpValues();
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

    public void SetAwUpValues()
    {
        int rarity = gameObject.GetComponent<Gear>().info.rarity;
        int augment = gameObject.GetComponent<Gear>().info.augment;
        int stars = gameObject.GetComponent<Gear>().info.stars;
        if(augment == 5 && stars != 5)
        {
            upgrade = false;
        }
        else
        {
            upgrade = true;
        }
        switch(rarity)
        {
            case 0:
                upMatInc = 6;
                awMatInc = 1;
                awPriceInc = 2000;
                upPriceInc = 10;
                upInitPrice = 60;
                break;
            case 1:
                upMatInc = 8;
                awMatInc = 2;
                awPriceInc = 4000;
                upPriceInc = 20;
                upInitPrice = 100;
                break;
            case 2:
                upMatInc = 10;
                awMatInc = 3;
                awPriceInc = 8000;
                upPriceInc = 30;
                upInitPrice = 150;
                break;
        }
        for (int i = 0; i <= stars; i++)
        {
            if (i == 0)
            {
                cumuPrice = 0;
            }
            else
            {
                cumuPrice += ((stars * upPriceInc) + upInitPrice) * 5;
            }
        }
        upMat = upMatInc * (stars + 1);
        awMat = awMatInc * (stars + 1);
        awPrice = awPriceInc * (stars + 1);
        upPrice = (((stars * upPriceInc) + upInitPrice) * (augment + 1) + cumuPrice) ;
    }
}
