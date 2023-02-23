using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FightCharacter : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject select;
    GameObject targets;
    FightController fightCntrl;
    public SpriteRenderer sr;
    public GameObject lifeBar;
    public GameObject specialBar;
    Character.Info charInfo;
    ComboController cc;

    public int position;
    int target;
    public bool type, specialActivated = false;
    public string abilityType;
    public float life, special;
    float maxLife, maxSpecial, attack, defense, scaleI;

    private  void Start()
    {
        fightCntrl = FindObjectOfType<FightController>();
        cc = fightCntrl.comboCntrl;

        charInfo = transform.GetComponent<Character>().info;
        gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshPro>().text = "" + charInfo.id;

        scaleI = lifeBar.transform.localScale.x;
        maxLife = charInfo.stats.hp;

        maxSpecial = maxLife;
        special = 0;

        attack = charInfo.stats.atk;
        defense = charInfo.stats.def;

        if (type)
        {
            targets = GameObject.Find("Enemies");
        }
        else
        {
            targets = GameObject.Find("Players");
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!type)
        {
            fightCntrl.enemySelect = position;
            fightCntrl.pointerEnemy = fightCntrl.enemiesPositions.IndexOf(position);

            for (int i=0; i<fightCntrl.enemiesPositions.Count; i++)
            {
                fightCntrl.enemies.transform.GetChild(fightCntrl.enemiesPositions[i]).GetChild(0).GetComponent<FightCharacter>().Select(false);
            }
            fightCntrl.enemies.transform.GetChild(fightCntrl.enemySelect).GetChild(0).GetComponent<FightCharacter>().Select(true);
            Debug.Log("enemySelect: " + fightCntrl.enemySelect);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {}

    public void OnPointerUp(PointerEventData eventData){}

    public void Attack()
    {
        StartCoroutine(AnimAttack());
        if(type)
        {
            target = fightCntrl.enemySelect;
        }
        else
        {
            target = fightCntrl.playerSelect;
        }
        if(fightCntrl.enemiesN >= 0 && fightCntrl.playersN >= 0)
        {
            if(type)
            {
                targets.transform.GetChild(target).GetChild(0).GetComponent<FightCharacter>().Damage(attack * cc.nameAtkVar + cc.timesAtkVar);
            }
            else
            {
                targets.transform.GetChild(target).GetChild(0).GetComponent<FightCharacter>().Damage(attack);
            }
        }
        AddSpecial();
    }

    public void SpecialAbility()
    {
        switch (abilityType)
        {
            case "healAll":
                HealCharacters();
                break;
            case "attackAll":
                StartCoroutine(AnimAttack());
                for(int i=0; i<6; i++)
                {
                    DamageCharacters(i);
                }
                break;
            case "attackRow":
                StartCoroutine(AnimAttack());
                if(type)
                {
                    if (fightCntrl.enemySelect == 0 || fightCntrl.enemySelect == 3)
                    {
                        DamageCharacters(0);
                        DamageCharacters(3);
                    }
                    else if (fightCntrl.enemySelect == 1 || fightCntrl.enemySelect == 4)
                    {
                        DamageCharacters(1);
                        DamageCharacters(4);
                    }
                    else
                    {
                        DamageCharacters(2);
                        DamageCharacters(5);
                    }
                }
                else
                {
                    if (fightCntrl.playerSelect == 0 || fightCntrl.playerSelect == 3)
                    {
                        DamageCharacters(0);
                        DamageCharacters(3);
                    }
                    else if (fightCntrl.playerSelect == 1 || fightCntrl.playerSelect == 4)
                    {
                        DamageCharacters(1);
                        DamageCharacters(4);
                    }
                    else
                    {
                        DamageCharacters(2);
                        DamageCharacters(5);
                    }
                }
                
                break;
            case "attackColumn":
                StartCoroutine(AnimAttack());
                if(type)
                {
                    if (fightCntrl.enemySelect == 0 || fightCntrl.enemySelect == 1 || fightCntrl.enemySelect == 2)
                    {
                        DamageCharacters(0);
                        DamageCharacters(1);
                        DamageCharacters(2);
                    }
                    else
                    {
                        DamageCharacters(3);
                        DamageCharacters(4);
                        DamageCharacters(5);
                    }
                }
                else
                {
                    if (fightCntrl.playerSelect == 0 || fightCntrl.playerSelect == 1 || fightCntrl.playerSelect == 2)
                    {
                        DamageCharacters(0);
                        DamageCharacters(1);
                        DamageCharacters(2);
                    }
                    else
                    {
                        DamageCharacters(3);
                        DamageCharacters(4);
                        DamageCharacters(5);
                    }
                }
                break;
            default:
                break;
        }
        ResetSpecial();
    }

    public void ResetSpecial()
    {
        special = 0;
        specialBar.transform.localScale = new Vector3(0f, specialBar.transform.localScale.y, specialBar.transform.localScale.z);
        if(type)
        {
            int index = fightCntrl.atkBtnsIds.IndexOf(gameObject.GetComponent<Character>().info.id);
            fightCntrl.listAttackButtons[index].GetComponent<AttackButton>().specialActivated = false;
            fightCntrl.listAttackButtons[index].GetComponent<AttackButton>().specialButton.interactable = false;
        }
        specialActivated = false;
    }

    public void DamageCharacters(int position)
    {
        if(type)
        {
            if (fightCntrl.enemiesPositions.Contains(position))
            {
                if(type)
                {
                    GameObject.Find("Enemies").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Damage(attack * cc.nameAtkVar + cc.timesAtkVar);
                }
                else
                {
                    GameObject.Find("Enemies").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Damage(attack);
                }
            }
        }
        else
        {
            if (fightCntrl.playersPositions.Contains(position))
            {
                if(type)
                {
                    GameObject.Find("Players").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Damage(attack * cc.nameAtkVar + cc.timesAtkVar);
                }
                else
                {
                    GameObject.Find("Players").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Damage(attack);
                }
            }
        } 
    }

    public void HealCharacters()
    {
        if (type)
        {
            for (int i = 0; i < fightCntrl.playersPositions.Count; i++)
            {
                GameObject.Find("Players").transform.GetChild(fightCntrl.playersPositions[i]).GetChild(0).GetComponent<FightCharacter>().Heal();
            }
        }
        else
        {
            for (int i = 0; i < fightCntrl.enemiesPositions.Count; i++)
            {
                GameObject.Find("Enemies").transform.GetChild(fightCntrl.enemiesPositions[i]).GetChild(0).GetComponent<FightCharacter>().Heal();
            }
        }
    }

    public void Heal()
    {
        int amount = 10;
        if (life < maxLife)
        {
            StartCoroutine(AnimHeal(amount));
            if ((life + amount) > maxLife)
            {
                life = maxLife;
            }
            else
            {
                life += amount;
            }
        }
    }

    public void AddSpecial()
    {
        int amount = 50;
        if (special < maxSpecial)
        {
            StartCoroutine(AnimChargeSpecial(amount));
            if ((special + amount) > maxSpecial)
            {
                special = maxSpecial;
            }
            else
            {
                special += amount;
            }
        }
        if(special >= maxSpecial)
        {
            if(type)
            {
                int index = fightCntrl.atkBtnsIds.IndexOf(gameObject.GetComponent<Character>().info.id);
                fightCntrl.listAttackButtons[index].GetComponent<AttackButton>().specialActivated = true;
            }
            specialActivated = true;
        }
    }

    public void Damage(float amount)
    {
        amount -= defense;
        life -= amount;
        StartCoroutine(AnimDamage(amount));
        if(life <= 0)
        {
            if(type)
            {
                if (fightCntrl.playersN == 0)
                {
                    fightCntrl.playersN--;
                    cc.SetAttackingFalse();
                    fightCntrl.SetResult();
                }
                else
                {
                    fightCntrl.playersPositions.RemoveAt(fightCntrl.playersPositions.IndexOf(position));
                    fightCntrl.playerSelect = fightCntrl.playersPositions[0];
                    fightCntrl.pointerPlayer = 0;

                    int index = fightCntrl.atkBtnsIds.IndexOf(gameObject.GetComponent<Character>().info.id);
                    fightCntrl.listAttackButtons[index].GetComponent<AttackButton>().isAlive = false;

                    fightCntrl.playersN--;
                }
            }
            else
            {
                if (fightCntrl.enemiesN == 0)
                {
                    fightCntrl.enemiesN--;
                    cc.SetAttackingFalse();
                    fightCntrl.SetResult();
                }
                else
                {
                    fightCntrl.enemiesPositions.RemoveAt(fightCntrl.enemiesPositions.IndexOf(position));
                    fightCntrl.enemySelect = fightCntrl.enemiesPositions[0];
                    fightCntrl.pointerEnemy = 0;
                    for (int i = 0; i < fightCntrl.enemiesPositions.Count; i++)
                    {
                        fightCntrl.enemies.transform.GetChild(fightCntrl.enemiesPositions[i]).GetChild(0).GetComponent<FightCharacter>().Select(false);
                    }
                    fightCntrl.enemies.transform.GetChild(fightCntrl.enemySelect).GetChild(0).GetComponent<FightCharacter>().Select(true);
                    fightCntrl.enemiesN--;
                }
            }
            Destroy(gameObject);
        }
    }

    //IEnumerator WaitTime(float time)
    //{
    //    yield return new WaitForSecondsRealtime(time);
    //}

    IEnumerator AnimAttack()
    {
        float mov = 0.3f;
        if(!type)
        {
            mov *= -1;
        }
        transform.position = new Vector3(transform.position.x + mov, transform.position.y, transform.position.z);
        yield return new WaitForSecondsRealtime(0.2f);
        transform.position = new Vector3(transform.position.x - mov, transform.position.y, transform.position.z);
    }

    IEnumerator AnimDamage(float damage)
    {
        lifeBar.transform.localScale = new Vector3(lifeBar.transform.localScale.x - (damage / maxLife) * scaleI, lifeBar.transform.localScale.y, lifeBar.transform.localScale.z);
        for(int i=0; i<10; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    IEnumerator AnimChargeSpecial(float amount)
    {
        if ((special + amount) > maxSpecial)
        {
            amount = maxSpecial - special;
        }
        yield return new WaitForSecondsRealtime(0.2f);
        specialBar.transform.localScale = new Vector3(specialBar.transform.localScale.x + (amount / maxSpecial) * scaleI, specialBar.transform.localScale.y, specialBar.transform.localScale.z);
    }

    IEnumerator AnimHeal(float heal)
    {
        if ((life + heal) > maxLife)
        {
            heal = maxLife - life;
        }
        lifeBar.transform.localScale = new Vector3(lifeBar.transform.localScale.x + (heal / maxLife) * scaleI, lifeBar.transform.localScale.y, lifeBar.transform.localScale.z);
        for (int i = 0; i < 10; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    public void Select(bool select)
    {
        this.select.SetActive(select);
        //Debug.Log("Select: " + select);
    }
}
