using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public int id, type;
    int nodeSelected;
    TextMeshProUGUI btnTMP;

    public List<MapNode> nextNodes, prevNodes;
    public GameObject parentColumn;
    Button btn; 

    void Start()
    {
        btnTMP = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        parentColumn = gameObject.transform.parent.gameObject;
        btn = gameObject.GetComponent<Button>();

        if (PlayerPrefs.HasKey("nodeSelected" + id))
        {
            nodeSelected = PlayerPrefs.GetInt("nodeSelected" + id);
        }
        else
        {
            nodeSelected = -1;
            SetNodeSelected(-1);
        }
    }

    public void SelectNode()
    {
        SetNodeSelected(1);
        SetTypeVars();
        ColorBlock cb = btn.colors;
        cb.disabledColor = new Color(0, 1, 0, .5f);
        btn.colors = cb;
        for(int i = 0; i < parentColumn.transform.childCount; i++)
        {
            parentColumn.transform.GetChild(i).GetComponent<Button>().interactable = false;
            parentColumn.transform.GetChild(i).GetComponent<MapNode>().SetNodeSelected(0);
        }
    }

    public void SetNodeSelected(int mode)
    {
        nodeSelected = mode;
        PlayerPrefs.SetInt("nodeSelected" + id, nodeSelected);
    }

    void SetTypeVars()
    {
        switch(type)
        {
            case 0:
                btnTMP.text = "Fight";
                break;
            case 1:
                btnTMP.text = "Merchant";
                break;
            case 2:
                btnTMP.text = "Blacksmith";
                break;
            case 3:
                btnTMP.text = "Team";
                break;
            case 4:
                btnTMP.text = "Recruit";
                break;
            default:
                break;
        }
    }
}
