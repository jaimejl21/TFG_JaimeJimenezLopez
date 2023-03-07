using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMananager : MonoBehaviour
{
    public GameObject charGO, pool, selectedPool, materialGO;
    public Button lvlUpBtn;
    public Transform charPos;
    public TextMeshProUGUI[] statsTxt, statsBonusTxt;
    public TextMeshProUGUI lvTxt, coinsTxt, priceTxt;

    public int idToEquip, amountC, amountR, amountSR, exp, maxExp, level;
    int selectedC, selectedR, selectedSR, extraAtk = 0, extraDef = 0, extraHp = 0, coins, price = 0;

    public List<Character.Info> allCharList;
    public ExpSlider expSlider;

    private void Start()
    {
        allCharList = GameManager.allChar;

        pool = GameObject.FindGameObjectWithTag("Pool");
        selectedPool = GameObject.Find("SelectedPool");

        coins = GameManager.inst.coins;
        coinsTxt.text = "Coins: " + coins;
        lvlUpBtn.interactable = false;

        idToEquip = GameManager.inst.charToEquipGear;
        charGO.transform.GetComponent<Character>().info = GameManager.inst.GetCharInfoById(idToEquip);
        Instantiate(charGO, charPos);

        level = charGO.transform.GetComponent<Character>().info.level;
        exp = charGO.transform.GetComponent<Character>().info.exp;
        maxExp = charGO.transform.GetComponent<Character>().info.expNextLv;
        expSlider.UpdateValues(exp, maxExp);

        lvTxt.text = "Lv. " + level;

        SetAmounts();
        MaterialsInventory();
        UpdateBaseStatsTxt();
    }

    void SetAmounts()
    {
        GetPlayerPrefs("amountC", ref amountC, 100);
        GetPlayerPrefs("amountR", ref amountR, 100);
        GetPlayerPrefs("amountSR", ref amountSR, 100);
    }

    void GetPlayerPrefs(string name, ref int var, int num)
    {
        if (PlayerPrefs.HasKey(name))
        {
            var = PlayerPrefs.GetInt(name);
        }
        else
        {
            var = num;
        }
    }

    void MaterialsInventory()
    {
        SetMaterials(0, amountC);
        SetMaterials(1, amountR);
        SetMaterials(2, amountSR);
    }

    void SetMaterials(int type, int amount)
    {
        if (amount != 0)
        {
            materialGO.GetComponent<LevelUpItem>().type = type;
            materialGO.GetComponent<LevelUpItem>().amount = amount;
            materialGO.GetComponent<LevelUpItem>().selected = false;
            Instantiate(materialGO, pool.transform);
        }  
    }

    public void SelectMaterial(bool selecType, int type, int expAm)
    {
        switch(type)
        {
            case 0:
                SelectionManager(selecType, type, expAm, ref selectedC, ref amountC);
                break;
            case 1:
                SelectionManager(selecType, type, expAm, ref selectedR, ref amountR);
                break;
            case 2:
                SelectionManager(selecType, type, expAm, ref selectedSR, ref amountSR);
                break;
            default:
                break;
        }
    }

    void SelectionManager(bool selecType, int type, int expAm, ref int selectedAm, ref int amount)
    {
        if(!selecType)
        {
            if (selectedAm == 0)
            {
                selectedAm++;
                materialGO.GetComponent<LevelUpItem>().type = type;
                materialGO.GetComponent<LevelUpItem>().amount = selectedAm;
                materialGO.GetComponent<LevelUpItem>().selected = true;
                Instantiate(materialGO, selectedPool.transform);
                amount--;
                ChangeAmounts(pool, type, amount);
            }
            else if(amount == 1)
            {
               amount--;
                for (int i = 0; i < pool.transform.childCount; i++)
                {
                    if (pool.transform.GetChild(i).transform.GetComponent<LevelUpItem>().type == type)
                    {
                        Destroy(pool.transform.GetChild(i).gameObject);
                    }
                }
                selectedAm++;
                ChangeAmounts(selectedPool, type, selectedAm);
            }
            else
            {
                selectedAm++;
                ChangeAmounts(selectedPool, type, selectedAm);
                amount--;
                ChangeAmounts(pool, type, amount);
            }
            UpdateExp(selecType, expAm);
            SetPrice(selecType);
        }
        else
        {
            if (selectedAm == 1)
            {
                selectedAm--;
                for (int i = 0; i < selectedPool.transform.childCount; i++)
                {
                    if (selectedPool.transform.GetChild(i).transform.GetComponent<LevelUpItem>().type == type)
                    {
                        Destroy(selectedPool.transform.GetChild(i).gameObject);
                    }
                }
                amount++;
                ChangeAmounts(pool, type, amount);
            }
            else if (amount == 0)
            {
                amount++;
                materialGO.GetComponent<LevelUpItem>().type = type;
                materialGO.GetComponent<LevelUpItem>().amount = amount;
                materialGO.GetComponent<LevelUpItem>().selected = false;
                Instantiate(materialGO, pool.transform);
                selectedAm--;
                ChangeAmounts(selectedPool, type, selectedAm);
            }
            else
            {
                selectedAm--;
                ChangeAmounts(selectedPool, type, selectedAm);
                amount++;
                ChangeAmounts(pool, type, amount);
            }
            UpdateExp(selecType, expAm);
            SetPrice(selecType);
        }
        
    }

    void ChangeAmounts(GameObject poolType, int type, int amount)
    {
        for (int i = 0; i < poolType.transform.childCount; i++)
        {
            if (poolType.transform.GetChild(i).transform.GetComponent<LevelUpItem>().type == type)
            {
                poolType.transform.GetChild(i).transform.GetComponent<LevelUpItem>().SetAmount(amount);
            }
        }
    }

    void UpdateExp(bool selectType, int expAm)
    {
        if (selectType)
        {
            exp -= expAm;
            if (exp < 0)
            {
                level--;
                lvTxt.text = "Lv. " + level;
                maxExp -= 320;
                exp = maxExp + exp;
                extraAtk -= 5;
                extraDef -= 2;
                extraHp -= 30;
            }
        }
        else
        {
            exp += expAm;
            if (exp >= maxExp)
            {
                level++;
                lvTxt.text = "Lv. " + level;
                exp -= maxExp;
                maxExp += 320;
                extraAtk += 5;
                extraDef += 2;
                extraHp += 30;
            }
        }
        expSlider.UpdateValues(exp, maxExp);
        UpdateStatsTxt();
    }

    void UpdateBaseStatsTxt()
    {
        statsTxt[0].text = "ATK: " + charGO.transform.GetComponent<Character>().info.stats.baseAtk;
        statsTxt[1].text = "DEF: " + charGO.transform.GetComponent<Character>().info.stats.baseDef;
        statsTxt[2].text = "HP: " + charGO.transform.GetComponent<Character>().info.stats.baseHp;
    }

    void UpdateStatsTxt()
    {
        statsBonusTxt[0].text = "+" + extraAtk;
        statsBonusTxt[1].text = "+" + extraDef;
        statsBonusTxt[2].text = "+" + extraHp;
    }

    public void LvlUp()
    {
        foreach (Transform child in selectedPool.transform)
        {
            Destroy(child.gameObject);
        }

        charGO.transform.GetComponent<Character>().info.stats.baseAtk += extraAtk;
        charGO.transform.GetComponent<Character>().info.stats.baseDef += extraDef;
        charGO.transform.GetComponent<Character>().info.stats.baseHp += extraHp;

        extraAtk = 0;
        extraDef = 0;
        extraHp = 0;

        coins -= price;
        coinsTxt.text = "Coins: " + coins;
        GameManager.inst.coins = coins;
        PlayerPrefs.SetInt("coins", coins);
        price = 0;
        priceTxt.text = "Coins: " + price;
        lvlUpBtn.interactable = false;

        UpdateBaseStatsTxt();
        UpdateStatsTxt();
    }

    void SetPrice(bool selectType)
    {
        int incPrice;
        if(level <= 35) { incPrice = 60; }
            else if(level <= 70) { incPrice = 80; }
                else { incPrice = 100; }

        if(!selectType) {price += incPrice;}
            else {price -= incPrice;}

        priceTxt.text = "Coins: " + price;

        if(price > coins)
        {
            priceTxt.color = Color.red;
            lvlUpBtn.interactable = false;
        }
        else
        {
            priceTxt.color = Color.white;
            lvlUpBtn.interactable = true;
        }
    }

    public void SaveAmounts()
    {
        PlayerPrefs.SetInt("amountC", amountC);
        PlayerPrefs.SetInt("amountR", amountR);
        PlayerPrefs.SetInt("amountSR", amountSR);

        charGO.transform.GetComponent<Character>().info.level = level;
        charGO.transform.GetComponent<Character>().info.exp = exp;
        charGO.transform.GetComponent<Character>().info.expNextLv = maxExp;

        for (int i = 0; i < allCharList.Count; i++)
        {
            if (allCharList[i].id == idToEquip)
            {
                allCharList[i].level = charGO.transform.GetComponent<Character>().info.level;
                allCharList[i].exp = charGO.transform.GetComponent<Character>().info.exp;
                allCharList[i].expNextLv = charGO.transform.GetComponent<Character>().info.expNextLv;
                allCharList[i].stats.baseAtk = charGO.transform.GetComponent<Character>().info.stats.baseAtk;
                allCharList[i].stats.baseDef = charGO.transform.GetComponent<Character>().info.stats.baseDef;
                allCharList[i].stats.baseHp = charGO.transform.GetComponent<Character>().info.stats.baseHp;
                Debug.Log("id: " + allCharList[i].id + " level: " + allCharList[i].level + " exp: " + allCharList[i].exp + " expNextLvl: " + allCharList[i].expNextLv);
            }
        }
        GameManager.allChar = allCharList;
        GameManager.inst.SaveListsToJson();
    }
}
