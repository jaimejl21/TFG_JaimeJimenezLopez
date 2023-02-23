using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public TeamManager teamManager;
    public GameObject item;
    public int slotPos;

    void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!item)
        {
            Debug.Log("Opcion A");
            if (teamManager.CanAddToTeam())
            {
                item = DragHandler.itemDragging;
                item.transform.SetParent(transform);
                item.transform.position = transform.position;
                item.GetComponent<DragHandler>().slotParent = transform;
                item.GetComponent<Character>().info.inTeam = true;
                item.GetComponent<Character>().info.pos = transform.GetComponent<DropSlot>().slotPos;
                for (int i = 0; i < teamManager.allCharList.Count; i++)
                {
                    if (teamManager.allCharList[i].id == item.GetComponent<Character>().info.id)
                    {
                        teamManager.allCharList[i].inTeam = true;
                        teamManager.allCharList[i].pos = transform.GetComponent<DropSlot>().slotPos;
                    }
                }
            } 
        }
        else
        {
            if(DragHandler.itemDragging.GetComponent<Character>().info.inTeam)
            {
                Debug.Log("Opcion B");
                item.transform.SetParent(DragHandler.itemDragging.GetComponent<DragHandler>().slotParent);
                item.transform.position = DragHandler.itemDragging.GetComponent<DragHandler>().slotParent.position;
                DragHandler.itemDragging.GetComponent<DragHandler>().slotParent.GetComponent<DropSlot>().item = item;
                item.GetComponent<DragHandler>().slotParent = DragHandler.itemDragging.GetComponent<DragHandler>().slotParent;
                item.GetComponent<Character>().info.pos = DragHandler.itemDragging.GetComponent<DragHandler>().slotParent.GetComponent<DropSlot>().slotPos;
                for (int i = 0; i < teamManager.allCharList.Count; i++)
                {
                    if (teamManager.allCharList[i].id == item.GetComponent<Character>().info.id)
                    {
                        teamManager.allCharList[i].pos = DragHandler.itemDragging.GetComponent<DragHandler>().slotParent.GetComponent<DropSlot>().slotPos;
                    }
                }
                item = DragHandler.itemDragging;
                item.transform.SetParent(transform);
                item.transform.position = transform.position;
                item.GetComponent<DragHandler>().slotParent = transform;
                item.GetComponent<Character>().info.pos = transform.GetComponent<DropSlot>().slotPos;
                for (int i = 0; i < teamManager.allCharList.Count; i++)
                {
                    if (teamManager.allCharList[i].id == item.GetComponent<Character>().info.id)
                    {
                        teamManager.allCharList[i].pos = transform.GetComponent<DropSlot>().slotPos;
                    }
                }
            }
            else
            {
                Debug.Log("Opcion C");
                item.transform.SetParent(GameObject.FindGameObjectWithTag("Pool").transform);
                item.transform.position = GameObject.FindGameObjectWithTag("Pool").transform.position;
                item.GetComponent<DragHandler>().slotParent = GameObject.FindGameObjectWithTag("Pool").transform;
                item.GetComponent<Character>().info.inTeam = false;
                item.GetComponent<Character>().info.pos = -1;
                for (int i = 0; i < teamManager.allCharList.Count; i++)
                {
                    if (teamManager.allCharList[i].id == item.GetComponent<Character>().info.id)
                    {
                        teamManager.allCharList[i].inTeam = false;
                        teamManager.allCharList[i].pos = -1;
                    }
                }
                item = DragHandler.itemDragging;
                item.transform.SetParent(transform);
                item.transform.position = transform.position;
                item.GetComponent<DragHandler>().slotParent = transform;
                item.GetComponent<Character>().info.inTeam = true;
                item.GetComponent<Character>().info.pos = transform.GetComponent<DropSlot>().slotPos;
                for (int i = 0; i < teamManager.allCharList.Count; i++)
                {
                    if (teamManager.allCharList[i].id == item.GetComponent<Character>().info.id)
                    {
                        teamManager.allCharList[i].inTeam = true;
                        teamManager.allCharList[i].pos = transform.GetComponent<DropSlot>().slotPos;
                    }
                }
            } 
        }
    }
}
