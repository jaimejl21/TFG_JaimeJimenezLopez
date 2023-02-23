using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour
{
    public GameObject pool;

    [SerializeField]
    GameObject character;

    [SerializeField]
    GameObject[] teamSlots;

    public List<Character.Info> allCharList;

    public bool dragging = false;

    void Start()
    {
        allCharList = GameManager.allChar.ToList();

        pool = GameObject.FindGameObjectWithTag("Pool");

        CharInventory();
        TeamSlots();
    }

    void TeamSlots()
    {
        for (int i = 0; i < allCharList.Count; i++)
        {
            if(allCharList[i].inTeam)
            {
                character.GetComponent<Character>().info = allCharList[i];
                int pos = 0;
                GameObject aux = new GameObject();
                for (int j=0; j < teamSlots.Length; j++)
                {
                    if(character.GetComponent<Character>().info.pos == teamSlots[j].transform.GetComponent<DropSlot>().slotPos)
                    {
                        aux = Instantiate(character, teamSlots[j].transform);
                        pos = j;

                    }
                }
                aux.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                aux.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                teamSlots[pos].GetComponent<DropSlot>().item = teamSlots[pos].transform.GetChild(0).gameObject;
                teamSlots[pos].transform.GetChild(0).GetComponent<DragHandler>().slotParent = teamSlots[pos].transform;
                teamSlots[pos].transform.GetChild(0).GetComponent<DragHandler>().startParent = pool.transform;
            }
        }
    }

    void CharInventory()
    {
        foreach (Character.Info c in allCharList)
        {
            character.GetComponent<Character>().info = c;
            if(character.GetComponent<Character>().info.inTeam != true)
            {
                Instantiate(character, pool.transform);
            } 
        } 
    }

    public bool CanAddToTeam()
    {
        int cont = 0;
        for(int i=0; i<6; i++)
        {
            if(teamSlots[i].GetComponent<DropSlot>().item != null)
            {
                cont++;
            }
        }
        if(cont < 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SaveTeam()
    {
        GameManager.allChar = allCharList;
        GameManager.inst.SaveListsToJson();
    }
}
