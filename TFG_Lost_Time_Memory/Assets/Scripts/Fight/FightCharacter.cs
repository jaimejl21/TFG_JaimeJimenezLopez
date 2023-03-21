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
    Color typeColor;

    public int position, atkBuffTurns = 0, atkDebuffTurns = 0, defBuffTurns = 0, defDebuffTurns = 0;
    int target, effectiveType, weakType, charType;
    public bool type, specialActivated = false;
    public string abilityType;
    public float life, special;
    float maxLife, maxSpecial, attack, defense, scaleI, atkBuff, atkDebuff, defBuff, defDebuff;

    private  void Start()
    {
        fightCntrl = FindObjectOfType<FightController>();
        cc = fightCntrl.comboCntrl;

        charInfo = transform.GetComponent<Character>().info;
        gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshPro>().text = "" + charInfo.id;
        charType = transform.GetComponent<Character>().info.type;
        SetTypeStats();

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
            float typeBonus = GetTypeBonus(targets.transform.GetChild(target).GetChild(0).GetComponent<Character>().info.type);
            if(type)
            {
                targets.transform.GetChild(target).GetChild(0).GetComponent<FightCharacter>().Damage(typeBonus * attack * cc.nameAtkVar + cc.timesAtkVar);
                fightCntrl.typeBonusTxt.text = (typeBonus * attack * cc.nameAtkVar + cc.timesAtkVar).ToString();
            }
            else
            {
                targets.transform.GetChild(target).GetChild(0).GetComponent<FightCharacter>().Damage(typeBonus * attack);
                fightCntrl.typeBonusTxt.text = (typeBonus * attack).ToString();
            }
        }
        AddSpecial();
    }

    public void SpecialAbility()
    {
        switch (abilityType)
        {
            case "healAll":
                for (int i = 0; i < 6; i++)
                {
                    HealCharacters(i, 10f);
                }
                break;
            case "heal":
                if(type)
                {
                    HealCharacters(fightCntrl.playerSelect, 10f);
                }
                else
                {
                    HealCharacters(fightCntrl.enemySelect, 10f);
                }
                break;
            case "attackAll":
                StartCoroutine(AnimAttack());
                for(int i = 0; i < 6; i++)
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
            case "buffAtkAll":
                for (int i = 0; i < 6; i++)
                {
                    DeBuffStatChars(i, true, ref attack, ref atkBuff, 10f);
                }                   
                break;
            case "debuffAtkAll":
                for (int i = 0; i < 6; i++)
                {
                    DeBuffStatChars(i, false, ref attack, ref atkDebuff, 10f);
                }                   
                break;
            case "buffDefAll":
                for (int i = 0; i < 6; i++)
                {
                    DeBuffStatChars(i, true, ref defense, ref defBuff, 10f);
                }                   
                break;
            case "debuffDefAll":
                for (int i = 0; i < 6; i++)
                {
                    DeBuffStatChars(i, false, ref defense, ref defDebuff, 10f);
                }                
                break;
            case "buffAtk":
                if(type)
                {
                    DeBuffStatChars(fightCntrl.playerSelect, true, ref attack, ref atkBuff, 10f);
                }
                else
                {
                    DeBuffStatChars(fightCntrl.enemySelect, true, ref attack, ref atkBuff, 10f);
                }               
                break;
            case "debuffAtk":
                if (!type)
                {
                    DeBuffStatChars(fightCntrl.playerSelect, false, ref attack, ref atkDebuff, 10f);
                }
                else
                {
                    DeBuffStatChars(fightCntrl.enemySelect, false, ref attack, ref atkDebuff, 10f);
                }
                break;
            case "buffDef":
                if (type)
                {
                    DeBuffStatChars(fightCntrl.playerSelect, true, ref defense, ref defBuff, 10f);
                }
                else
                {
                    DeBuffStatChars(fightCntrl.enemySelect, true, ref defense, ref defBuff, 10f);
                }
                break;
            case "debuffDef":
                if (!type)
                {
                    DeBuffStatChars(fightCntrl.playerSelect, false, ref defense, ref defDebuff, 10f);
                }
                else
                {
                    DeBuffStatChars(fightCntrl.enemySelect, false, ref defense, ref defDebuff, 10f);
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
                float typeBonus = GetTypeBonus(GameObject.Find("Enemies").transform.GetChild(position).GetChild(0).GetComponent<Character>().info.type);
                if (type)
                {
                    GameObject.Find("Enemies").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Damage(typeBonus * attack * cc.nameAtkVar + cc.timesAtkVar);
                    fightCntrl.typeBonusTxt.text = (typeBonus * attack * cc.nameAtkVar + cc.timesAtkVar).ToString();
                }
                else
                {
                    GameObject.Find("Enemies").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Damage(typeBonus * attack);
                    fightCntrl.typeBonusTxt.text = (typeBonus * attack).ToString();
                }
            }
        }
        else
        {
            if (fightCntrl.playersPositions.Contains(position))
            {
                float typeBonus = GetTypeBonus(GameObject.Find("Players").transform.GetChild(position).GetChild(0).GetComponent<Character>().info.type);
                if (type)
                {
                    GameObject.Find("Players").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Damage(typeBonus * attack * cc.nameAtkVar + cc.timesAtkVar);
                    fightCntrl.typeBonusTxt.text = (typeBonus * attack * cc.nameAtkVar + cc.timesAtkVar).ToString();
                }
                else
                {
                    GameObject.Find("Players").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Damage(typeBonus * attack);
                    fightCntrl.typeBonusTxt.text = (typeBonus * attack).ToString();
                }
            }
        } 
    }

    public void HealCharacters(int position, float amount)
    {
        if (type)
        {
            if (fightCntrl.playersPositions.Contains(position))
            {
                GameObject.Find("Players").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Heal(amount);
            }
        }
        else
        {
            if (fightCntrl.enemiesPositions.Contains(position))
            {
                GameObject.Find("Enemies").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().Heal(amount);
            }
        }
    }

    public void Heal(float amount)
    {
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

    public void DeBuffStat(bool buff, ref float stat, ref float statInc, float amount)
    {
        if(buff)
        {
            stat += amount;
            statInc = amount;
        }
        else
        {
            if((stat - amount) >= 0)
            {
                stat -= amount;
                statInc = amount;
            }
            else
            {
                statInc = stat;
                stat = 0;
            }
        }       
    }

    void DeBuffStatChars(int position, bool buff, ref float stat, ref float statInc, float amount)
    {
        if (type)
        {
            if(buff)
            {
                if (fightCntrl.playersPositions.Contains(position))
                {
                    GameObject.Find("Players").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().DeBuffStat(buff, ref stat, ref statInc, amount);
                }
            }
            else
            {
                if (fightCntrl.enemiesPositions.Contains(position))
                {
                    GameObject.Find("Enemies").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().DeBuffStat(buff, ref stat, ref statInc, amount);
                }                  
            }           
        }
        else
        {
            if (buff)
            {
                if (fightCntrl.enemiesPositions.Contains(position))
                {
                    GameObject.Find("Enemies").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().DeBuffStat(buff, ref stat, ref statInc, amount);
                }
            }
            else
            {
                if (fightCntrl.playersPositions.Contains(position))
                {
                    GameObject.Find("Players").transform.GetChild(position).GetChild(0).GetComponent<FightCharacter>().DeBuffStat(buff, ref stat, ref statInc, amount);
                }
            }
        }
    }

    public void CheckDeBuffsTurns()
    {
        CheckDeBuffsTurnsAux(true, ref atkBuffTurns, ref atkBuff);
        CheckDeBuffsTurnsAux(false, ref atkDebuffTurns, ref atkDebuff);
        CheckDeBuffsTurnsAux(true, ref defBuffTurns, ref defBuff);
        CheckDeBuffsTurnsAux(false, ref defDebuffTurns, ref defDebuff);
    }

    public void CheckDeBuffsTurnsAux(bool buff, ref int deBuffTurns, ref float statInc)
    {       
        if(deBuffTurns > 1)
        {
            deBuffTurns--;
        }
        else if(deBuffTurns == 1)
        {
            deBuffTurns--;

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
        fightCntrl.typeBonusTxt.text = "";
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

    void SetTypeStats()
    {
        switch (charType)
        {
            case 0:
                typeColor = Color.white;
                effectiveType = 1;
                weakType = 2;
                break;
            case 1:
                typeColor = new Color(.5f, .2f, .6f, 1f);
                effectiveType = 2;
                weakType = 0;
                break;
            case 2:
                typeColor = new Color(.5f, .3f, 0f, 1f);
                effectiveType = 0;
                weakType = 1;
                break;
            case 3:
                typeColor = Color.green;
                effectiveType = 4;
                weakType = 6;
                break;
            case 4:
                typeColor = Color.yellow;
                effectiveType = 5;
                weakType = 3;
                break;
            case 5:
                typeColor = Color.blue;
                effectiveType = 6;
                weakType = 4;
                break;
            case 6:
                typeColor = Color.red;
                effectiveType = 3;
                weakType = 5;
                break;
            default:
                break;
        }
        gameObject.transform.GetComponent<SpriteRenderer>().color = typeColor;
    }

    float GetTypeBonus(int objType)
    {
        if (objType == effectiveType)
        {
            fightCntrl.typeBonusTxt.color = Color.green;
            return 2f;
        }
        else if (objType == weakType)
        {
            fightCntrl.typeBonusTxt.color = Color.red;
            return 0.5f;
        }
        else
        {
            fightCntrl.typeBonusTxt.color = Color.white;
            return 1f;
        }
    }
}
