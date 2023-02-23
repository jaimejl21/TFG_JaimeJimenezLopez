using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    Vector3 startPosition;
    public Transform startParent, dragParent;
    public static GameObject itemDragging;
    public Transform slotParent;
    TeamManager tm;

    void Start()
    {
        tm = FindObjectOfType<TeamManager>();
        dragParent = GameObject.FindGameObjectWithTag("DragParent").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        //Debug.Log("OnBeginDrag");
        tm.dragging = true;
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
        tm.dragging = false;
        itemDragging = null;
        if(transform.parent == dragParent)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }
}
