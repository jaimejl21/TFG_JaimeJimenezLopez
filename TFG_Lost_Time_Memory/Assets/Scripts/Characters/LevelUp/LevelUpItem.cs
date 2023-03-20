using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI typeTxt, amountTxt;

    public int expAm, amount, type;
    int interval = 20;
    public bool selected;
    bool pointerDown = false;

    LevelUpMananager lum;

    void Start()
    {
        lum = FindObjectOfType<LevelUpMananager>();
        
        SetItemInfo();
    }

    void Update()
    {
        if (pointerDown)
        {
            if (Time.frameCount % interval == 0)
            {
                lum.SelectMaterial(selected, type, expAm);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) 
    {
        pointerDown = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData) { }

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
