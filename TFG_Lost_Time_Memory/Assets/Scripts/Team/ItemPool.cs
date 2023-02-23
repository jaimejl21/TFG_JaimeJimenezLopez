using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPool : MonoBehaviour, IDropHandler
{
    public TeamManager teamManager;
    public GameObject item;

    void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(teamManager.dragging == true)
        {
            DragHandler.itemDragging.GetComponent<DragHandler>().slotParent.GetComponent<DropSlot>().item = null;

            item = DragHandler.itemDragging;

            if (item.GetComponent<Character>().info.inTeam)
            {
                item.GetComponent<Character>().info.inTeam = false;
                for (int i = 0; i < teamManager.allCharList.Count; i++)
                {
                    if (teamManager.allCharList[i].id == item.GetComponent<Character>().info.id)
                    {
                        teamManager.allCharList[i].inTeam = false;
                        teamManager.allCharList[i].pos = -1;
                    }
                }

            }
            DragHandler.itemDragging.transform.SetParent(transform);
            item.GetComponent<DragHandler>().slotParent = transform;
        }   
    }
}
