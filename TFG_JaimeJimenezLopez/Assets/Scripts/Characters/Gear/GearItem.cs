using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearItem : MonoBehaviour
{
    public TextMeshProUGUI text;
    string typeName;

    [SerializeField]
    Color statTypeColor, rarityColor;

    void Start()
    {
        SetName();
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = 
            "" + transform.GetComponent<Gear>().info.id + "\n" + typeName;
        SetGearStatColor();
        SetRarityColor();
    }

    public void SetName()
    {
        int type = transform.GetComponent<Gear>().info.objType;
        switch(type)
        {
            case 0:
                typeName = "Brazalete";
                break;
            case 1:
                typeName = "Collar";
                break;
            case 2:
                typeName = "Cintur�n";
                break;
            case 3:
                typeName = "Anillo";
                break;
            case 4:
                typeName = "Pendientes";
                break;
            case 5:
                typeName = "Orbe";
                break;
        }
    }

    void SetGearStatColor()
    {
        int statType = gameObject.transform.GetComponent<Gear>().info.statType;
        switch(statType)
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
