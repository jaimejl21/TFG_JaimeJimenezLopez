using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GearDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    Vector3 startPosition;
    public Transform startParent, dragParent;
    public static GameObject itemDragging;
    public Transform slotParent;
    GearManager gm;

    void Start()
    {
        gm = FindObjectOfType<GearManager>();
        dragParent = GameObject.FindGameObjectWithTag("DragParent").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        gm.dragging = true;
        itemDragging = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(dragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        gm.dragging = false;
        itemDragging = null;
        if(transform.parent == dragParent)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }
}
