using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : MonoBehaviour
{
    public TextMeshProUGUI text;
    string typeName;

    [SerializeField]
    Color rarityColor;

    void Start()
    {
        SetName();
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + transform.GetComponent<Gear>().info.id + "\n" + typeName;
        SetRarityColor();
    }

    public void SetName()
    {
        int type = transform.GetComponent<Gear>().info.objType;
        switch(type)
        {
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

    public void SetRarityColor()
    {
        int rarity = gameObject.GetComponent<Gear>().info.rarity;
        switch (rarity)
        {
            case 0:
                rarityColor = new Color(0.5f, 0.5f, 0.5f, 1f);
                break;
            case 1:
                rarityColor = new Color(0.5f, 0f, 1f, 1f);
                break;
            case 2:
                rarityColor = new Color(1f, 0.7f, 0f, 1f);
                break;
            default:
                break;
        }
        gameObject.transform.GetComponent<Image>().color = rarityColor;
    }
}
