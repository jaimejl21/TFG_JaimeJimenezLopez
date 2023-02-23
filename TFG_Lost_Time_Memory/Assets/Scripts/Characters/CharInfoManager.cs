using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharInfoManager : MonoBehaviour
{
    public GameObject charGO;
    public Transform charPos;
    public TextMeshProUGUI[] statsTxt;

    public int idToEquip;

    public List<Character.Info> allCharList;

    private void Start()
    {
        allCharList = GameManager.allChar;

        idToEquip = GameManager.inst.charToEquipGear;
        charGO.transform.GetComponent<Character>().info = GameManager.inst.GetCharInfoById(idToEquip);
        Instantiate(charGO, charPos);

        InitBaseStatsTxt();
    }

    void InitBaseStatsTxt()
    {
        statsTxt[0].text = "ATK: " + charGO.transform.GetComponent<Character>().info.stats.baseAtk;
        statsTxt[1].text = "DEF: " + charGO.transform.GetComponent<Character>().info.stats.baseDef;
        statsTxt[2].text = "HP: " + charGO.transform.GetComponent<Character>().info.stats.baseHp;
    }
}
