using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GearDropSlot : MonoBehaviour, IDropHandler
{
    public GearManager gearManager;
    public GameObject item;
    public int slotPos;
    public int type;

    void Start()
    {
        gearManager = FindObjectOfType<GearManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GearDragHandler.itemDragging.GetComponent<Gear>().info.objType == type)
        {
            if (!item)
            {
                Debug.Log("Opcion A");
                item = GearDragHandler.itemDragging;
                item.transform.SetParent(transform);
                item.transform.position = transform.position;
                item.GetComponent<GearDragHandler>().slotParent = transform;
                item.GetComponent<Gear>().info.equiped = true;
                item.GetComponent<Gear>().info.characterId = gearManager.charGO.transform.GetComponent<Character>().info.id;
                for (int i = 0; i < gearManager.gearList.Count; i++)
                {
                    if (gearManager.gearList[i].id == item.GetComponent<Gear>().info.id)
                    {
                        gearManager.gearList[i].equiped = true;
                        gearManager.gearList[i].characterId = gearManager.charGO.transform.GetComponent<Character>().info.id;
                    }
                }
                gearManager.UpdateStatGear(true, item.transform.GetComponent<Gear>().info);
            }
            else
            {
                if(item.GetComponent<GearDragHandler>().slotParent != GearDragHandler.itemDragging.GetComponent<GearDragHandler>().slotParent)
                {
                    Debug.Log("Opcion B");
                    item.transform.SetParent(GameObject.FindGameObjectWithTag("Pool").transform);
                    item.transform.position = GameObject.FindGameObjectWithTag("Pool").transform.position;
                    item.GetComponent<GearDragHandler>().slotParent = GameObject.FindGameObjectWithTag("Pool").transform;
                    item.GetComponent<Gear>().info.equiped = false;
                    item.GetComponent<Gear>().info.characterId = -1;
                    for (int i = 0; i < gearManager.gearList.Count; i++)
                    {
                        if (gearManager.gearList[i].id == item.GetComponent<Gear>().info.id)
                        {
                            gearManager.gearList[i].equiped = false;
                            gearManager.gearList[i].characterId = -1;
                        }
                    }
                    gearManager.UpdateStatGear(false, item.transform.GetComponent<Gear>().info);

                    item = GearDragHandler.itemDragging;
                    item.transform.SetParent(transform);
                    item.transform.position = transform.position;
                    item.GetComponent<GearDragHandler>().slotParent = transform;
                    item.GetComponent<Gear>().info.equiped = true;
                    item.GetComponent<Gear>().info.characterId = gearManager.charGO.transform.GetComponent<Character>().info.id;
                    for (int i = 0; i < gearManager.gearList.Count; i++)
                    {
                        if (gearManager.gearList[i].id == item.GetComponent<Gear>().info.id)
                        {
                            gearManager.gearList[i].equiped = true;
                            gearManager.gearList[i].characterId = gearManager.charGO.transform.GetComponent<Character>().info.id;
                        }
                    }
                    gearManager.UpdateStatGear(true, item.transform.GetComponent<Gear>().info);
                }   
            }
        }
    }
}
