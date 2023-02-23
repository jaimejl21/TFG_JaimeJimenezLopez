using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    public GameObject pool, charSlotGO;

    public List<Character.Info> auxCharList;

    private void Start()
    {
        auxCharList = GameManager.allChar;

        CharInventory();
    }

    private void CharInventory()
    {
        foreach (Character.Info c in auxCharList)
        {
            charSlotGO.GetComponent<Character>().info = c;
            Instantiate(charSlotGO, pool.transform);
        }
    }
}
