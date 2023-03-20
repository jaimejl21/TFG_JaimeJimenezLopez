using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    int comboType = 0;
    public int timesCombo = 0, nameAtkVar = 1, timesAtkVar = 1;
    public bool startedCombo = false, decidedCombo = false;
    public string comboName = "";

    public Animator anim;
    public GameObject comboGO;
    public FightController fc;
    public AnimationsManager am;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Good")
        {
            comboType = 1;
        }
        if (col.gameObject.tag == "Great")
        {
            comboType = 2;
        }
        if (col.gameObject.tag == "Perfect")
        {
            comboType = 3;
        }
        if (col.gameObject.tag == "Normal")
        {
            comboType = 0;
        }
        //Debug.Log("comboType" + comboType);
    }

    public void StartAnim()
    {
        anim.SetBool("Attack", true);
    }

    public void StopAnim()
    {
        anim.SetBool("Attack", false);
    }

    public void SetActiveComboGO(bool ac)
    {
        comboGO.SetActive(ac);
    }

    public void ResetAnim()
    {
        anim.Play("Anim_ComboBar", -1, 0f);
    }

    public void SetAttackingFalse()
    {
        SetActiveComboGO(false);
        StopAnim();
        startedCombo = false;
        timesCombo = 0;
        fc.comboTxt.text = "";
    }

    public void SetAttackingTrue() {}

    public void SetComboVars()
    {
        switch(comboType)
        {
            case 0:
                comboName = "Normal";
                nameAtkVar = 1;
                break;
            case 1:
                comboName = "Good";
                nameAtkVar = 2;
                break;
            case 2:
                comboName = "Great";
                nameAtkVar = 3;
                break;
            case 3:
                comboName = "Perfect";
                nameAtkVar = 4;
                break;
            default:
                break;
        }

        switch (fc.auxTimesCombo)
        {
            case 1:
                timesAtkVar = 0;
                break;
            case 2:
                timesAtkVar = 10;
                break;
            case 3:
                timesAtkVar = 20;
                break;
            case 4:
                timesAtkVar = 30;
                break;
            default:
                break;
        }
        Debug.Log(comboName + " " + timesAtkVar);
    }

    public void ComboAction()
    {
        SetComboVars();
        am.animComboTxt.SetBool("Combo", true);
    }
}
