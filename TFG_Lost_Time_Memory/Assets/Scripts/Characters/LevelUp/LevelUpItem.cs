using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI typeTxt, amountTxt;

    public int expAm, amount, type;
    public bool selected;

    LevelUpMananager lum;

    void Start()
    {
        lum = FindObjectOfType<LevelUpMananager>();
        
        SetItemInfo();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        lum.SelectMaterial(selected, type, expAm);
        //Debug.Log("Click type " + type + " position " + position);
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData) { }

    void SetItemInfo()
    {
        switch(type)
        {
            case 0:
                typeTxt.text = "C";
                break;
            case 1:
                typeTxt.text = "R";
                break;
            case 2:
                typeTxt.text = "SR";
                break;
            default:
                break;
        }

        switch (typeTxt.text)
        {
            case "C":
                expAm =  50;
                break;
            case "R":
                expAm = 150;
                break;
            case "SR":
                expAm = 350;
                break;
            default:
                break;
        }

        amountTxt.text = "" + amount;
    }

    public void SetAmount(int am)
    {
        amount = am;
        amountTxt.text = "" + amount;
    }
}
