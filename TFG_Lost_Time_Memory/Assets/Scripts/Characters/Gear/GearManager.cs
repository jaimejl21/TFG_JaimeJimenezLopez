using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GearManager : MonoBehaviour
{
    public GameObject charGO, pool, gear;
    public GameObject[] gearSlots;
    public Transform charPos;
    public TextMeshProUGUI[] statsTxt, bonusTxt, statsBonusTxt;

    public int idToEquip;
    public int atkGears = 0, defGears = 0, hpGears = 0;
    public bool dragging = false;
    bool initialized = false;

    public List<Gear.Info> gearList;
    public List<Character.Info> allCharList;

    private void Start()
    {
        gearList = GameManager.allGear.ToList();
        allCharList = GameManager.allChar;

        pool = GameObject.FindGameObjectWithTag("Pool");

        idToEquip = GameManager.inst.charToEquipGear;
        charGO.transform.GetComponent<Character>().info = GameManager.inst.GetCharInfoById(idToEquip);
        Instantiate(charGO, charPos);

        GearSlots();
        GearInventory();
        GearStats();
        InitBaseStatsTxt();
        UpdateStatsTxt();
        UpdateBonusTxt(false);
        initialized = true;
    }

    void GearSlots()
    {
        for (int i = 0; i < gearList.Count; i++)
        {
            if (gearList[i].equiped && gearList[i].characterId == charGO.transform.GetComponent<Character>().info.id)
            {
                gear.transform.GetComponent<Gear>().info = gearList[i];
                int pos = gearList[i].objType;
                Instantiate(gear, gearSlots[pos].transform);
                //aux.transform.GetComponent<GearItem>().SetGearColor();
                gearSlots[pos].GetComponent<GearDropSlot>().item = gearSlots[pos].transform.GetChild(0).gameObject;
                gearSlots[pos].transform.GetChild(0).GetComponent<GearDragHandler>().slotParent = gearSlots[pos].transform;
                gearSlots[pos].transform.GetChild(0).GetComponent<GearDragHandler>().startParent = pool.transform;
            }
        }
    }

    void GearInventory()
    {
        foreach (Gear.Info g in gearList)
        {
            gear.GetComponent<Gear>().info = g;
            gear.GetComponent<GearDragHandler>().slotParent = pool.transform;
            if (gear.GetComponent<Gear>().info.equiped != true)
            {
                Instantiate(gear, pool.transform);
                //aux.transform.GetComponent<GearItem>().SetGearColor();
            }
        }
    }

    void GearStats()
    {
        for(int i = 0; i < gearSlots.Length; i++)
        {
            if(gearSlots[i].transform.childCount != 0)
            {
                switch(gearSlots[i].transform.GetChild(0).GetComponent<Gear>().info.statType)
                {
                    case 0:
                        atkGears++;
                        break;
                    case 1:
                        defGears++;
                        break;
                    case 2:
                        hpGears++;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void UpdateStatGear(bool add, Gear.Info ginfo)
    {
        if (add)
        {
            switch (ginfo.statType)
            {
                case 0:
                    atkGears++;
                    break;
                case 1:
                    defGears++;
                    break;
                case 2:
                    hpGears++;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (ginfo.statType)
            {
                case 0:
                    atkGears--;
                    break;
                case 1:
                    defGears--;
                    break;
                case 2:
                    hpGears--;
                    break;
                default:
                    break;
            }
        }
        UpdateBonusTxt(add);
        UpdateCharStats(add, ginfo);
    }

    public void UpdateCharStats(bool add, Gear.Info ginfo)
    {
        if(add)
        {
            if (ginfo.objType == 0 || ginfo.objType == 3)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraAtk += ginfo.statAmount;
            }
            else if (ginfo.objType == 1 || ginfo.objType == 4)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraDef += ginfo.statAmount;
            }
            else if (ginfo.objType == 2 || ginfo.objType == 5)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraHp += ginfo.statAmount;
            }
        }
        else
        {
            if (ginfo.objType == 0 || ginfo.objType == 3)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraAtk -= ginfo.statAmount;
            }
            else if (ginfo.objType == 1 || ginfo.objType == 4)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraDef -= ginfo.statAmount;
            }
            else if (ginfo.objType == 2 || ginfo.objType == 5)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraHp -= ginfo.statAmount;
            }
        }
        charGO.transform.GetComponent<Character>().info.stats.UpdateStats();
        UpdateStatsTxt();
    }

    public void UpdateBonusTxt(bool add)
    {
        CheckBonusTxt(add, atkGears, 0);
        CheckBonusTxt(add, defGears, 1);
        CheckBonusTxt(add, hpGears, 2);
    }

    public void CheckBonusTxt(bool add, int statGear, int statType)
    {
        if (statGear < 2)
        {
            if (bonusTxt[statType].gameObject.activeSelf)
            {
                if(statGear == 1 && !add)
                {
                    AddSubtractStats(add, statType, 10);
                }
                bonusTxt[statType].gameObject.SetActive(false);
            }
        }
        else if (statGear < 4)
        {
            if(bonusTxt[statType].gameObject.activeSelf)
            {
                ChangeStatBonusCount(1, statType);
                if (statGear == 3 && !add)
                {
                    AddSubtractStats(add, statType, 10);
                }
            }
            else
            {
                bonusTxt[statType].gameObject.SetActive(true);
                if(initialized)
                {
                    if (statGear == 2)
                    {
                        AddSubtractStats(add, statType, 10);
                    }
                }
            }   
        }
        else if (statGear < 6)
        {
            if (bonusTxt[statType].gameObject.activeSelf)
            {
                ChangeStatBonusCount(2, statType);
                if ((statGear == 5 && !add) || (statGear == 4 && add))
                {
                    AddSubtractStats(add, statType, 10);
                }
            }
            else
            {
                bonusTxt[statType].gameObject.SetActive(true);
                ChangeStatBonusCount(2, statType);
            }       
        }
        else if (statGear > 5)
        {
            if (bonusTxt[statType].gameObject.activeSelf)
            {
                ChangeStatBonusCount(3, statType);
                AddSubtractStats(add, statType, 10);
            }
            else
            {
                bonusTxt[statType].gameObject.SetActive(true);
                ChangeStatBonusCount(3, statType);
            }
        }
    }


    void ChangeStatBonusCount(int num, int statType)
    {
        bonusTxt[statType].text = bonusTxt[statType].text.Remove(bonusTxt[statType].text.Length - 3);
        bonusTxt[statType].text += " x" + num;
    }

    public bool CanDropGear()
    {
        if (atkGears <= 0 && defGears <= 0 && hpGears <= 0)
        {
            return false;
        }
        else return true;
    }

    void InitBaseStatsTxt()
    {
        statsTxt[0].text = "ATK: " + charGO.transform.GetComponent<Character>().info.stats.baseAtk;
        statsTxt[1].text = "DEF: " + charGO.transform.GetComponent<Character>().info.stats.baseDef;
        statsTxt[2].text = "HP: " + charGO.transform.GetComponent<Character>().info.stats.baseHp;
    }

    void UpdateStatsTxt()
    {
        statsBonusTxt[0].text = "+" + charGO.transform.GetComponent<Character>().info.stats.extraAtk;
        statsBonusTxt[1].text = "+" + charGO.transform.GetComponent<Character>().info.stats.extraDef;
        statsBonusTxt[2].text = "+" + charGO.transform.GetComponent<Character>().info.stats.extraHp;
    }

    void AddSubtractStats(bool add, int statType, int amount)
    {
        if (add)
        {
            if (statType == 0)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraAtk += amount;
            }
            else if (statType == 1)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraDef += amount;
            }
            else if (statType == 2)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraHp += amount;
            }
        }
        else
        {
            if (statType == 0)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraAtk -= amount;
            }
            else if (statType == 1)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraDef -= amount;
            }
            else if (statType == 2)
            {
                charGO.transform.GetComponent<Character>().info.stats.extraHp -= amount;
            }
        }
    }

    public void SaveGear()
    {
        for (int i = 0; i < 6; i++)
        {
            if (gearSlots[i].GetComponent<GearDropSlot>().item != null)
            {
                int pos_type = gearSlots[i].GetComponent<GearDropSlot>().slotPos;
                charGO.transform.GetComponent<Character>().info.gear.RemoveAt(pos_type);
                charGO.transform.GetComponent<Character>().info.gear.Insert(pos_type, gearSlots[i].GetComponent<GearDropSlot>().item.transform.GetComponent<Gear>().info);
            }
        }
        for(int i = 0; i < allCharList.Count; i++)
        {
            if(allCharList[i].id == idToEquip)
            {
                allCharList[i].stats = charGO.transform.GetComponent<Character>().info.stats;
            }
        }

        GameManager.allGear = gearList;
        GameManager.inst.SaveListsToJson();
    }
}
