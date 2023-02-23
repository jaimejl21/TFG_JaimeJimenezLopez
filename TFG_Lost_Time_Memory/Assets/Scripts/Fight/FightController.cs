using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FightController : MonoBehaviour
{
    public int enemiesN, playersN, deadPlayers = 0, deadEnemies = 0, auxTimesCombo;
    public int enemySelect, playerSelect, pointerPlayer, pointerEnemy;
    int timesAttacked = 0;
    bool turn = true;
    public bool fightResult;

    public List<Character.Info> auxCharList;
    public List<Character.Info> teamList;
    public List<Character.Info> ordTeamList;
    public List<Character.Info> allEnemiesList;
    public List<GameObject> listAttackButtons;

    [SerializeField]
    List<GameObject> playerSlots, enemieSlots;

    public List<int> atkBtnsIds;
    public List<int> playersPositions, enemiesPositions;

    [SerializeField]
    GameObject player, enemy, attackButtons;

    public TextMeshProUGUI resultText, comboTxt;
    public GameObject enemies, players, buttonsDown;
    public ComboController comboCntrl;

    private void Start()
    {
        auxCharList = GameManager.allChar.ToList();
        teamList = new List<Character.Info>() { null, null, null, null, null, null };
        allEnemiesList = GameManager.allEnemies.ToList();

        for (int i = 0; i < auxCharList.Count; i++)
        {
            if(auxCharList[i].inTeam)
            {
                teamList.RemoveAt(auxCharList[i].pos);
                teamList.Insert(auxCharList[i].pos, auxCharList[i]);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (teamList[i] == null)
            {
                teamList.RemoveAt(i);
                teamList.Insert(i, new Character.Info(-1, -1, false, new List<Gear.Info>(), 1, 0, 320, new Character.Stats()));
            }
        }
        ordTeamList = teamList.OrderBy(character => character.pos).ToList();
        teamList = ordTeamList;

        playersPositions = new List<int>();
        enemiesPositions = new List<int>();

        SetFight();

        playersN = playersPositions.Count - 1;

        enemiesN = enemiesPositions.Count - 1;
        enemySelect = enemiesPositions[0]; ;
        pointerEnemy = 0;

        enemies.transform.GetChild(enemySelect).GetChild(0).GetComponent<FightCharacter>().Select(true);
    }

    public void SetFight()
    {
        for (int i = 0; i < 6; i++)
        {
            if (teamList[i].id != -1)
            {
                player.GetComponent<Character>().info = teamList[i];
                player.GetComponent<FightCharacter>().position = player.GetComponent<Character>().info.pos;
                playersPositions.Add(player.GetComponent<FightCharacter>().position);
                if ((i % 2) == 0)
                {
                    player.GetComponent<FightCharacter>().abilityType = "attackRow";
                }
                else
                {
                    player.GetComponent<FightCharacter>().abilityType = "attackColumn";
                }
                GameObject p = Instantiate(player, playerSlots[player.GetComponent<FightCharacter>().position].transform);
                GameObject ab = Instantiate(attackButtons, buttonsDown.transform);
                ab.GetComponent<AttackButton>().id = p.GetComponent<Character>().info.id;
                ab.GetComponent<AttackButton>().charPos = p.GetComponent<FightCharacter>().position;
                listAttackButtons.Add(ab);
            }
            enemy.GetComponent<Character>().info = allEnemiesList[i];
            enemy.GetComponent<FightCharacter>().position = i;
            enemiesPositions.Add(enemy.GetComponent<FightCharacter>().position);
            if ((i % 2) == 0)
            {
                enemy.GetComponent<FightCharacter>().abilityType = "attackRow";
            }
            else
            {
                enemy.GetComponent<FightCharacter>().abilityType = "attackColumn";
            }
            Instantiate(enemy, enemieSlots[enemy.GetComponent<FightCharacter>().position].transform);
        }

        for (int i=0; i<listAttackButtons.Count; i++)
        {
            atkBtnsIds.Add(listAttackButtons[i].GetComponent<AttackButton>().id);
        }
    }

    public void Attack()
    {
        DesactivateAllSpBtns();
        timesAttacked++;
        if(playersPositions.Count > 1)
        {
            comboCntrl.timesCombo++;
            if (timesAttacked < playersPositions.Count)
            {
                comboCntrl.SetActiveComboGO(true);
                if (!comboCntrl.startedCombo)
                {
                    comboCntrl.StartAnim();
                    comboCntrl.startedCombo = true;
                }
                else
                {
                    comboCntrl.ResetAnim();
                }
                auxTimesCombo = comboCntrl.timesCombo;
            }
            else
            {
                auxTimesCombo = comboCntrl.timesCombo;
                comboCntrl.SetAttackingFalse();
            }
        }
        comboCntrl.ComboAction();
        comboTxt.text = "" + comboCntrl.comboName + "  x" + auxTimesCombo;
        CharacterAction(true);
        DesactivateBtn();
    }

    public void SpecialAbility()
    {
        CharacterAction(false);
    }

    public void CharacterAction(bool normal)
    {
        if (turn && playersN >= 0)
        {
            pointerPlayer = playersPositions.IndexOf(playerSelect);
            FightCharacter fch = players.transform.GetChild(playerSelect).GetChild(0).GetComponent<FightCharacter>();

            //fch.Select(true);
            if (normal)
            {
                fch.Attack();
                if (timesAttacked < playersPositions.Count)
                {
                    //ActivateBtn();
                }
                else
                {
                    turn = false;
                    timesAttacked = 0;
                    if (enemiesN >= 0)
                    {
                        StartCoroutine(AttackEn());
                    }
                }
                //if (playerSelect == playersPositions[playersPositions.Count - 1])
                //{
                //    playerSelect = playersPositions[0];
                //    pointerPlayer = 0;
                //    turn = false;
                //    timesAttacked = 0;
                //    if (enemiesN >= 0)
                //    {
                //        StartCoroutine(AttackEn());
                //    }
                //}
                //else
                //{
                //    pointerPlayer++;
                //    playerSelect = playersPositions[pointerPlayer];
                //    ActivateBtn();
                //}

                //fch = players.transform.GetChild(playerSelect).GetChild(0).GetComponent<FightCharacter>();
            }
            else
            {
                fch.SpecialAbility();
            }
            //fch.Select(false);
        }
    }

    public IEnumerator AttackEn()
    {
        if (enemiesN >= 0)
        {
            playerSelect = playersPositions[0];
            pointerPlayer = 0;
            DesactivateAllBtns();
            DesactivateAllSpBtns();
            yield return new WaitForSecondsRealtime(1f);
            comboTxt.text = "";
            for (int i = 0; i < enemiesPositions.Count; i++)
            {
                if(enemies.transform.GetChild(enemiesPositions[i]).GetChild(0).GetComponent<FightCharacter>().specialActivated)
                {
                    enemies.transform.GetChild(enemiesPositions[i]).GetChild(0).GetComponent<FightCharacter>().SpecialAbility();
                }
                else
                {
                    enemies.transform.GetChild(enemiesPositions[i]).GetChild(0).GetComponent<FightCharacter>().Attack();
                }
                if (playersN < 0)
                {
                    StopAllCoroutines();
                }
                yield return new WaitForSecondsRealtime(1f);
            }
            if (playersN >= 0 && enemiesN >= 0)
            {
                turn = true;
                ActivateBtns();
                ActivateSpBtns();
            }
        }
    }

    //public void ActivateBtn()
    //{
    //    int index = atkBtnsIds.IndexOf(players.transform.GetChild(playerSelect).GetChild(0).GetComponent<Character>().info.id);
    //    DesactivateBtns();
    //    listAttackButtons[index].GetComponent<AttackButton>().attackButton.interactable = true;
    //}

    public void ActivateBtns()
    {
        for (int i = 0; i < listAttackButtons.Count; i++)
        {
            if(listAttackButtons[i].GetComponent<AttackButton>().isAlive)
            {
                listAttackButtons[i].GetComponent<AttackButton>().attackButton.interactable = true;
            }
        }
    }

    public void DesactivateBtn()
    {
        int index = atkBtnsIds.IndexOf(players.transform.GetChild(playerSelect).GetChild(0).GetComponent<Character>().info.id);
        listAttackButtons[index].GetComponent<AttackButton>().attackButton.interactable = false;
    }

    public void DesactivateAllBtns()
    {
        for (int i = 0; i < listAttackButtons.Count; i++)
        {
            listAttackButtons[i].GetComponent<AttackButton>().attackButton.interactable = false;
        }
    }

    public void ActivateSpBtns()
    {
        for (int i = 0; i < listAttackButtons.Count; i++)
        {
            if (listAttackButtons[i].GetComponent<AttackButton>().specialActivated && listAttackButtons[i].GetComponent<AttackButton>().isAlive)
            {
                listAttackButtons[i].GetComponent<AttackButton>().specialButton.interactable = true;
            }
        }
            
    }

    public void DesactivateAllSpBtns()
    {
        for (int i = 0; i < listAttackButtons.Count; i++)
        {
            listAttackButtons[i].GetComponent<AttackButton>().specialButton.interactable = false;
        }
    }


    public void  SetResult()
    {
        DesactivateAllBtns();
        DesactivateAllSpBtns();
        if (enemiesN < 0 && playersN > -1)
        {
            fightResult = true;
            resultText.text = "WIN";
        }
        else if (enemiesN > -1 && playersN < 0)
        {
            fightResult = false;
            resultText.text = "LOSE";
        }
    }

    //public void SelectCharacter()
    //{
    //    if (enemiesN > -1)
    //    {
    //        if (Input.GetKeyDown(KeyCode.UpArrow))
    //        {
    //            FightCharacter e = enemies.transform.GetChild(enemySelect).GetChild(0).GetComponent<FightCharacter>();
    //            if (enemySelect == enemiesPositions[0])
    //            {
    //                enemySelect = enemiesPositions[enemiesPositions.Count - 1];
    //                pointerEnemy = enemiesPositions.Count - 1;
    //            }
    //            else
    //            {
    //                pointerEnemy--;
    //                enemySelect = enemiesPositions[pointerEnemy];
    //            }
    //            //Debug.Log("enemySelect: " + enemySelect);
    //            //Debug.Log("pointerEnemy: " + pointerEnemy);

    //            e.Select(false);
    //            e = enemies.transform.GetChild(enemySelect).GetChild(0).GetComponent<FightCharacter>();
    //            e.Select(true);
    //        }
    //        if (Input.GetKeyDown(KeyCode.DownArrow))
    //        {
    //            FightCharacter e = enemies.transform.GetChild(enemySelect).GetChild(0).GetComponent<FightCharacter>();
    //            if (enemySelect == enemiesPositions[enemiesPositions.Count - 1])
    //            {
    //                enemySelect = enemiesPositions[0];
    //                pointerEnemy = 0;
    //            }
    //            else
    //            {
    //                pointerEnemy++;
    //                enemySelect = enemiesPositions[pointerEnemy];
    //            }
    //            //Debug.Log("enemySelect: " + enemySelect);
    //            //Debug.Log("pointerEnemy: " + pointerEnemy);

    //            e.Select(false);
    //            e = enemies.transform.GetChild(enemySelect).GetChild(0).GetComponent<FightCharacter>();
    //            e.Select(true);
    //        }
    //    }
    //}
}
