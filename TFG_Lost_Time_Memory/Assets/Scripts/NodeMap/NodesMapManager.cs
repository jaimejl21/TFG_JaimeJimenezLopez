using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;

public class NodesMapManager : MonoBehaviour
{
    public List<GameObject> columnsList;

    public GameObject objAlertPn;
    //Random rnd;

    int actualCol, idGearCount;

    void Start()
    {
        GameManager.inst.GetPlayerPrefs("actualCol", ref actualCol, 0);
        GameManager.inst.GetPlayerPrefs("idGearCount", ref idGearCount, 0);

        //rnd = new Random();

        //if (PlayerPrefs.HasKey("actualCol"))
        //{
        //    actualCol = PlayerPrefs.GetInt("actualCol");
        //    Debug.Log("key " + actualCol);
        //}
        //else
        //{
        //    actualCol = 0;
        //    Debug.Log("No key");
        //}

        for (int i = 0; i < (actualCol + 1); i++)
        {
            columnsList[i].SetActive(true);
            Debug.Log("column " + i + " active");
        }        
    }

    public void ManageColumns()
    {
        actualCol++;
        PlayerPrefs.SetInt("actualCol", actualCol);
        if (actualCol < columnsList.Count)
        {
            columnsList[actualCol].SetActive(true);
            int nodesCount = columnsList[actualCol].transform.childCount;
            for (int i = 0; i < nodesCount; i++)
            {
                if (columnsList[actualCol].transform.GetChild(i).GetComponent<MapNode>().prevNodes.Count > 0)
                {
                    int j = 0;
                    bool aux = false;
                    bool isActive = false;
                    while(!aux)
                    {
                        if (columnsList[actualCol].transform.GetChild(i).GetComponent<MapNode>().prevNodes[j].GetComponent<MapNode>().nodeSelected == 1)
                        {

                            isActive = true;
                            aux = true;
                        }
                        j++;
                        if (j >= columnsList[actualCol].transform.GetChild(i).GetComponent<MapNode>().prevNodes.Count)
                        {
                            aux = true;
                        }                        
                    }
                    if (!isActive)
                    {
                        columnsList[actualCol].transform.GetChild(i).GetComponent<MapNode>().SetNodeSelected(0);
                        columnsList[actualCol].transform.GetChild(i).GetComponent<Button>().interactable = false;
                        if(nodesCount == 1)
                        {
                            ManageColumns();
                        }
                    }
                }
            }
        }
    }

    public void ObjectAlert()
    {
        objAlertPn.SetActive(true);
        TextMeshProUGUI objNameTMP = objAlertPn.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        int obj = new Random().Next(0, 4);
        int rarity = new Random().Next(0, 3);

        switch (obj)
        {
            case 0:
                // int id  int statAmount  int objType  int statType  int rarity  int augment  int stars  bool equiped  int characterId
                int objType = new Random().Next(0, 13);
                int statType;
                if (objType > 5)
                {
                    statType = -1;
                }
                else
                {
                    statType = new Random().Next(0, 3);
                }
                
                Gear.Info gi = new Gear.Info(idGearCount, GameManager.inst.AuxSetStatAmount(objType, rarity), objType, statType, rarity, 0, 0, false, -1);

                idGearCount++;
                PlayerPrefs.SetInt("idGearCount", idGearCount);
                GameManager.inst.idGearCount = idGearCount;
                GameManager.allGear.Add(gi);
                GameManager.inst.SaveListsToJson();

                objNameTMP.color = SetGearStatColor(statType);
                objNameTMP.text = "" + SetRarityName(rarity) + " " + SetName(objType);
                break;
            case 1:
                int upMats = new Random().Next(5, 11);
                GameManager.inst.upMats += upMats;
                PlayerPrefs.SetInt("upMats", GameManager.inst.upMats);

                objNameTMP.color = SetGearStatColor(-1);
                objNameTMP.text = "" + upMats + " Upgrade materials";
                break;
            case 2:
                int awMats = new Random().Next(1, 4);
                GameManager.inst.awMats += awMats;
                PlayerPrefs.SetInt("awMats", GameManager.inst.awMats);

                objNameTMP.color = SetGearStatColor(-1);
                objNameTMP.text = "" + awMats + " Awake materials";
                break;
            case 3:
                int lvlUpMats = new Random().Next(5, 16);
                if(rarity == 0)
                {
                    GameManager.inst.lvlUpMatC += lvlUpMats;
                    PlayerPrefs.SetInt("amountC", GameManager.inst.lvlUpMatC);
                }
                else if(rarity == 1)
                {
                    GameManager.inst.lvlUpMatR += lvlUpMats;
                    PlayerPrefs.SetInt("amountR", GameManager.inst.lvlUpMatR);
                }
                else
                {
                    GameManager.inst.lvlUpMatSR += lvlUpMats;
                    PlayerPrefs.SetInt("amountSR", GameManager.inst.lvlUpMatSR);
                }

                objNameTMP.color = SetGearStatColor(-1);
                objNameTMP.text = "" + lvlUpMats + " " + SetRarityName(rarity) + " Level Up materials";
                break;
        }
    }

    string SetName(int objType)
    {
        switch (objType)
        {
            case 0:
                return "Bracer";
            case 1:
                return "Neck";
            case 2:
                return "Belt";
            case 3:
                return "Ring";
            case 4:
                return "Earring";
            case 5:
                return "Orb";
            case 6:
                return "Sword";
            case 7:
                return "Spear";
            case 8:
                return "Scythe";
            case 9:
                return "Dagger";
            case 10:
                return "Staff";
            case 11:
                return "Bow";
            case 12:
                return "Axe";
            default:
                return "";
        }
    }

    Color SetGearStatColor(int statType)
    {
        switch (statType)
        {
            case -1:
                return Color.black;
            case 0:
                return new Color(.5f, 0f, 0f, 1f);
            case 1:
                return new Color(0f, 0f, .5f, 1f);
            case 2:
                return new Color(0f, .5f, 0f, 1f);
            default:
                return Color.black;
        }
    }

    string SetRarityName(int rarity)
    {
        switch (rarity)
        {
            case 0:
                return "C";
            case 1:
                return "R";
            case 2:
                return "SR";
            default:
                return "";
        }
    }
}
