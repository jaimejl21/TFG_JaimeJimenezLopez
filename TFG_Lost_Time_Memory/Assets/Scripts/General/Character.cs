using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{
    public Character(Info info)
    {
        this.info = info;
    }

    [System.Serializable]
    public class Info
    {
        public int id;
        public int type;
        public int pos;
        public bool inTeam;
        public List<Gear.Info> gear;
        public int level;
        public int exp;
        public int expNextLv;
        public Stats stats;

        public Info() { }

        public Info(int id, int type, int pos, bool inTeam, List<Gear.Info> gear, int level, int exp, int expNextLv, Stats stats)
        {
            this.id = id;
            this.type = type;
            this.pos = pos;
            this.inTeam = inTeam;
            this.gear = gear;
            this.level = level;
            this.exp = exp;
            this.expNextLv = expNextLv;
            this.stats = stats;
        }
    }

    [System.Serializable]
    public class Stats
    {
        public float baseAtk;
        public float baseDef;
        public float baseHp;
        public float extraAtk;
        public float extraDef;
        public float extraHp;
        public float atk;
        public float def;
        public float hp;

        public Stats(float baseAtk, float baseDef, float baseHp, float extraAtk, float extraDef, float extraHp)
        {
            this.baseAtk = baseAtk;
            this.baseDef = baseDef;
            this.baseHp = baseHp;
            this.extraAtk = extraAtk;
            this.extraDef = extraDef;
            this.extraHp = extraHp;
            this.atk = baseAtk + extraAtk;
            this.def = baseDef + extraDef;
            this.hp = baseHp + extraHp;
        }

        public Stats()
        {
            this.baseAtk = 10;
            this.baseDef = 5;
            this.baseHp = 100;
            this.extraAtk = 0;
            this.extraDef = 0;
            this.extraHp = 0;
            this.atk = baseAtk + extraAtk;
            this.def = baseDef + extraDef;
            this.hp = baseHp + extraHp;  
        }

        public void UpdateStats()
        {
            this.atk = baseAtk + extraAtk;
            this.def = baseDef + extraDef;
            this.hp = baseHp + extraHp;
        }
    }

    public Info info;
}
