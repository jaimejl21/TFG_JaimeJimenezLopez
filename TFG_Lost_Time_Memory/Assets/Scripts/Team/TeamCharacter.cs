using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamCharacter : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + transform.GetComponent<Character>().info.id;
    }
}
