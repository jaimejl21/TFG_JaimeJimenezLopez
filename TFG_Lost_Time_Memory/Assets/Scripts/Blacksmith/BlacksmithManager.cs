using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithManager : MonoBehaviour
{
    public GameObject pool, blacksmithItem, goSelected;
    public Transform itemPos;
    public List<Gear.Info> gearList;
    public TextMeshProUGUI[] itemInfoTxts;
    public TextMeshProUGUI coinsTxt;
    public Button[] btns;

    public int coins;

    private void Start()
    {
        gearList = GameManager.allGear.ToList();

        coins = GameManager.inst.coins;
        coinsTxt.text = "Coins: " + coins;

        SetInventory(gearList);
    }

    void UpdateCoinsTxt()
    {
        coinsTxt.text = "Coins: " + coins;
        GameManager.inst.coins = coins;
    }

    public void ChangeItemInfo(GameObject go)
    {
        goSelected = go;
        if(itemPos.childCount != 0)
        {
            Destroy(itemPos.GetChild(0).gameObject);
        }
        blacksmithItem.GetComponent<Gear>().info = go.GetComponent<Gear>().info;
        Instantiate(blacksmithItem, itemPos);
        itemInfoTxts[0].text = "Augment: " + blacksmithItem.GetComponent<Gear>().info.augment;
        itemInfoTxts[1].text = "Stars: " + blacksmithItem.GetComponent<Gear>().info.stars;
        itemInfoTxts[2].text = "Stats: " + blacksmithItem.GetComponent<Gear>().info.statAmount;
        itemInfoTxts[3].text = "Materials: ";
        itemInfoTxts[4].text = "Coins: ";
    }

    public void SetInventory(List<Gear.Info> list)
    {
        foreach (Gear.Info g in list)
        {
            blacksmithItem.GetComponent<Gear>().info = g;
            Instantiate(blacksmithItem, pool.transform);
        }
    }
}
