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
    public TextMeshProUGUI coinsTxt, upMatsTxt, awMatsTxt;
    public Button[] btns;

    public int coins, awMats, upMats;

    private void Start()
    {
        gearList = GameManager.allGear.ToList();

        coins = GameManager.inst.coins;
        awMats = GameManager.inst.awMats;
        upMats = GameManager.inst.upMats;

        coinsTxt.text = "Coins: " + coins;
        upMatsTxt.text = "UpMats: " + upMats;
        awMatsTxt.text = "AwMats: " + awMats;

        SetInventory(gearList);
        ResetItemInfo();
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
        if (goSelected.GetComponent<BlacksmithItem>().upgrade)
        {
            ChangeUpAwBtn(true, false, true, false);
            CanUpAwGear(goSelected.GetComponent<BlacksmithItem>().upPrice, 0, goSelected.GetComponent<BlacksmithItem>().upMat, upMats);
            itemInfoTxts[3].text = "UpgMats: " + goSelected.GetComponent<BlacksmithItem>().upMat;
            itemInfoTxts[4].text = "Coins: " + goSelected.GetComponent<BlacksmithItem>().upPrice;    
        }
        else
        {
            ChangeUpAwBtn(false, true, false, true);
            CanUpAwGear(goSelected.GetComponent<BlacksmithItem>().awPrice, 1, goSelected.GetComponent<BlacksmithItem>().awMat, awMats);
            itemInfoTxts[3].text = "AwaMats: " + goSelected.GetComponent<BlacksmithItem>().awMat;
            itemInfoTxts[4].text = "Coins: " + goSelected.GetComponent<BlacksmithItem>().awPrice;
        }      
    }

    void CanUpAwGear(int upAwPrice, int btn, int upAwMatsNeed, int upAwMats)
    {    
        if (upAwPrice > coins)
        {
            itemInfoTxts[4].color = Color.red;
        }
        else
        {
            itemInfoTxts[4].color = Color.white;
        }
        if (upAwMatsNeed > upAwMats)
        {
            itemInfoTxts[3].color = Color.red;
        }
        else
        {
            itemInfoTxts[3].color = Color.white;
        }
        if(upAwPrice > coins || upAwMatsNeed > upAwMats)
        {
            btns[btn].interactable = false;
        }
        else
        {
            btns[btn].interactable = true;
        }
    }

    void ChangeUpAwBtn(bool upAct, bool awAct, bool upInt, bool awInt)
    {
        btns[0].gameObject.SetActive(upAct);
        btns[1].gameObject.SetActive(awAct);
        btns[0].interactable = upInt;
        btns[1].interactable = awInt;
    }

    void ResetItemInfo()
    {
        if (itemPos.childCount != 0)
        {
            Destroy(itemPos.GetChild(0).gameObject);
        }
        itemInfoTxts[0].text = "Augment: ";
        itemInfoTxts[1].text = "Stars: ";
        itemInfoTxts[2].text = "Stats: ";
        itemInfoTxts[3].text = "Materials: ";
        itemInfoTxts[4].text = "Coins: ";

        ChangeUpAwBtn(true, false, false, false);

        goSelected = null;
    }

    void UpdateHeaderTexts()
    {
        coinsTxt.text = "Coins: " + coins;
        upMatsTxt.text = "UpMats: " + upMats;
        awMatsTxt.text = "AwMats: " + awMats;

        GameManager.inst.coins = coins;
        GameManager.inst.awMats = awMats;
        GameManager.inst.upMats = upMats;

        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.SetInt("awMats", awMats);
        PlayerPrefs.SetInt("upMats", upMats);
    }

    public void SetInventory(List<Gear.Info> list)
    {
        foreach (Gear.Info g in list)
        {
            blacksmithItem.GetComponent<Gear>().info = g;
            Instantiate(blacksmithItem, pool.transform);
        }
    }

    public void UpgradeAwakeGear()
    {      
        if (goSelected.GetComponent<Gear>().info.augment == 5)
        {
            goSelected.GetComponent<Gear>().info.augment = 0;
            goSelected.GetComponent<Gear>().info.stars++;
            coins -= goSelected.GetComponent<BlacksmithItem>().awPrice;
            awMats -= goSelected.GetComponent<BlacksmithItem>().awMat;
            UpdateHeaderTexts();
            goSelected.GetComponent<BlacksmithItem>().SetAwUpValues();

            if (goSelected.GetComponent<Gear>().info.stars == 5)
            {
                goSelected.GetComponent<Gear>().info.augment = 0;
                ChangeUpAwBtn(true, false, false, false);
            }
            else
            {
                goSelected.GetComponent<BlacksmithItem>().upgrade = true;
                ChangeUpAwBtn(true, false, true, false);
                Debug.Log("CanUpgrade : " + goSelected.GetComponent<BlacksmithItem>().upMat + " > " + upMats);
                CanUpAwGear(goSelected.GetComponent<BlacksmithItem>().upPrice, 0, goSelected.GetComponent<BlacksmithItem>().upMat, upMats);
            }           
        }
        else
        {
            goSelected.GetComponent<Gear>().info.augment++;
            UpdateGearStats();
            coins -= goSelected.GetComponent<BlacksmithItem>().upPrice;
            upMats -= goSelected.GetComponent<BlacksmithItem>().upMat;
            UpdateHeaderTexts();
            goSelected.GetComponent<BlacksmithItem>().SetAwUpValues();

            if (goSelected.GetComponent<Gear>().info.augment == 5)
            {
                ChangeUpAwBtn(false, true, false, true);
                goSelected.GetComponent<BlacksmithItem>().upgrade = false;
                Debug.Log("CanAwake");
                CanUpAwGear(goSelected.GetComponent<BlacksmithItem>().awPrice, 1, goSelected.GetComponent<BlacksmithItem>().awMat, awMats);
            }
            else
            {
                Debug.Log("CanUpgradeee : " + goSelected.GetComponent<BlacksmithItem>().upMat + " > " + upMats);
                CanUpAwGear(goSelected.GetComponent<BlacksmithItem>().upPrice, 0, goSelected.GetComponent<BlacksmithItem>().upMat, upMats);
            }
        }
        UpdateUpAwTexts();
        bool found = false;
        int i = 0;
        while (!found)
        {
            if (gearList[i].id == goSelected.GetComponent<Gear>().info.id)
            {
                Gear.Info gi = goSelected.GetComponent<Gear>().info;
                gearList[i] = gi;
                GameManager.allGear = gearList;
                GameManager.inst.SaveListsToJson();
                found = true;
            }
            i++;
        }
    }

    void UpdateGearStats()
    {
        if (goSelected.GetComponent<Gear>().info.objType == 0 || goSelected.GetComponent<Gear>().info.objType == 3)
        {
            goSelected.GetComponent<Gear>().info.statAmount += 5;
        }
        else if (goSelected.GetComponent<Gear>().info.objType == 1 || goSelected.GetComponent<Gear>().info.objType == 4)
        {
            goSelected.GetComponent<Gear>().info.statAmount += 3;
        }
        else
        {
            goSelected.GetComponent<Gear>().info.statAmount += 50;
        }
    }

    void UpdateUpAwTexts()
    {
        itemInfoTxts[0].text = "Augment: " + blacksmithItem.GetComponent<Gear>().info.augment;
        itemInfoTxts[1].text = "Stars: " + blacksmithItem.GetComponent<Gear>().info.stars;
        itemInfoTxts[2].text = "Stats: " + blacksmithItem.GetComponent<Gear>().info.statAmount;
        if (goSelected.GetComponent<BlacksmithItem>().upgrade)
        {
            itemInfoTxts[3].text = "UpgMats: " + goSelected.GetComponent<BlacksmithItem>().upMat;
            itemInfoTxts[4].text = "Coins: " + goSelected.GetComponent<BlacksmithItem>().upPrice;
        }
        else
        {
            itemInfoTxts[3].text = "AwaMats: " + goSelected.GetComponent<BlacksmithItem>().awMat;
            itemInfoTxts[4].text = "Coins: " + goSelected.GetComponent<BlacksmithItem>().awPrice;
        }
    }
}
