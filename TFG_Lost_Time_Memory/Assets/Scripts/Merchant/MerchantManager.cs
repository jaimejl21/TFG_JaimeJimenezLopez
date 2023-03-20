using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantManager : MonoBehaviour
{
    public GameObject pool, merchantItem, buyTabPanel, sellTabPanel, goSelected;
    public Transform itemPos;
    public List<Gear.Info> sellGearList, buyGearList;
    public TextMeshProUGUI[] itemInfoTxts;
    public TextMeshProUGUI coinsTxt;
    public Button[] btns;

    public int coins, idGearCount;

    private void Start()
    {
        sellGearList = GameManager.allGear.ToList();
        buyGearList = new List<Gear.Info>();

        coins = GameManager.inst.coins;
        coinsTxt.text = "Coins: " + coins;

        idGearCount = GameManager.inst.idGearCount;

        for (int i = 0; i < 39; i++)
        {
            if (i < 6)
            {
                buyGearList.Add(new Gear.Info(i, 5, i, 0, 0, 0, 0, false, -1));
            }
            else if ((i > 5) && (i < 12))
            {
                buyGearList.Add(new Gear.Info(i, 3, (i - 6), 1, 1, 0, 0, false, -1));
            }
            else if ((i > 11) && (i < 18))
            {
                buyGearList.Add(new Gear.Info(i, 50, (i - 12), 2, 2, 0, 0, false, -1));
            }
            else if ((i > 17) && (i < 25))
            {
                buyGearList.Add(new Gear.Info(i, 10, (i - 12), -1, 0, 0, 0, false, -1));
            }
            else if ((i > 24) && (i < 32))
            {
                buyGearList.Add(new Gear.Info(i, 20, (i - 19), -1, 1, 0, 0, false, -1));
            }
            else
            {
                buyGearList.Add(new Gear.Info(i, 30, (i - 26), -1, 2, 0, 0, false, -1));
            }
            BuyTabBtn();
        }
    }

    void UpdateCoinsTxt()
    {
        coinsTxt.text = "Coins: " + coins;
        GameManager.inst.coins = coins;
    }

    public void BuyBtn()
    {
        bool found = false;
        int i = 0;
        while(!found)
        {
            if (buyGearList[i].id == itemPos.GetChild(0).gameObject.GetComponent<Gear>().info.id)
            {
                coins -= goSelected.GetComponent<MerchantItem>().price;
                UpdateCoinsTxt();
                Gear.Info gi = goSelected.GetComponent<Gear>().info;
                gi.id = idGearCount;
                idGearCount++;
                PlayerPrefs.SetInt("idGearCount", idGearCount);
                GameManager.inst.idGearCount = idGearCount;
                sellGearList.Add(gi);
                buyGearList.RemoveAt(i);
                Destroy(goSelected);
                ResetItemInfo();
                GameManager.allGear = sellGearList;
                GameManager.inst.SaveListsToJson();
                found = true;
            }
            i++;
        }
    }

    public void SellBtn()
    {
        bool found = false;
        int i = 0;
        while(!found)
        {
            if (sellGearList[i].id == goSelected.GetComponent<Gear>().info.id)
            {
                coins += goSelected.GetComponent<MerchantItem>().price;
                UpdateCoinsTxt();
                sellGearList.RemoveAt(i);
                Destroy(goSelected);
                ResetItemInfo();
                GameManager.allGear = sellGearList;
                GameManager.inst.SaveListsToJson();
                found = true;
            }
            i++;
        }
    }

    public void ChangeItemInfo(GameObject go)
    {
        goSelected = go;
        if(itemPos.childCount != 0)
        {
            Destroy(itemPos.GetChild(0).gameObject);
        }
        merchantItem.GetComponent<Gear>().info = go.GetComponent<Gear>().info;
        merchantItem.GetComponent<MerchantItem>().price = go.GetComponent<MerchantItem>().price;
        Instantiate(merchantItem, itemPos);
        itemInfoTxts[0].text = "Rarity: " + merchantItem.GetComponent<Gear>().info.rarity;
        itemInfoTxts[1].text = "Stats: " + merchantItem.GetComponent<Gear>().info.statAmount;
        itemInfoTxts[2].text = "Coins: " + merchantItem.GetComponent<MerchantItem>().price;
        if (btns[0].interactable == false)
        {
            if (goSelected.GetComponent<MerchantItem>().price > coins)
            {
                btns[2].interactable = false;
                itemInfoTxts[2].color = Color.red;
            }
            else
            {
                btns[2].interactable = true;
                itemInfoTxts[2].color = Color.white;
            }
        }else if(btns[1].interactable == false)
        {
            btns[3].interactable = true;
        }

    }

    public void ChangeInventory()
    {
        ResetItemInfo();

        foreach (Transform child in pool.transform)
        {
            Destroy(child.gameObject);
        }

        if (btns[0].interactable)
        {
            SetInventory(sellGearList);
        }
        else
        {
            SetInventory(buyGearList);
        }
    }

    void ResetItemInfo()
    {
        if (itemPos.childCount != 0)
        {
            Destroy(itemPos.GetChild(0).gameObject);
        }

        itemInfoTxts[0].text = "Rarity: ";
        itemInfoTxts[1].text = "Stats: ";
        itemInfoTxts[2].text = "Coins: ";

        btns[2].interactable = false;
        btns[3].interactable = false;

        goSelected = null;
    }

    public void SetInventory(List<Gear.Info> list)
    {
        foreach (Gear.Info g in list)
        {
            merchantItem.GetComponent<Gear>().info = g;
            if (merchantItem.GetComponent<Gear>().info.equiped != true)
            {
                Instantiate(merchantItem, pool.transform);
            }
        }
    }

    public void SellTabBtn()
    {
        btns[1].interactable = false;
        btns[0].interactable = true;
        sellTabPanel.SetActive(true);       
        buyTabPanel.SetActive(false);
        ChangeInventory();
        btns[2].gameObject.SetActive(false);
        btns[3].gameObject.SetActive(true);
    }

    public void BuyTabBtn()
    {
        btns[0].interactable = false;
        btns[1].interactable = true;
        buyTabPanel.SetActive(true);
        sellTabPanel.SetActive(false);
        ChangeInventory();
        btns[3].gameObject.SetActive(false);
        btns[2].gameObject.SetActive(true);
    }
}
