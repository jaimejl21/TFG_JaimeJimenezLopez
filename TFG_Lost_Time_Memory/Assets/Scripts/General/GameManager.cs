using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    public int charToEquipGear = 0;
    public bool restartPP, initialized = false;
    public int coins, idGearCount, idCharCount, awMats, upMats;

    [SerializeField]
    string filename;

    public static List<Character.Info> allChar;
    public static List<Gear.Info> allGear;
    public static List<Character.Info> allEnemies;

    int started;

    [System.Serializable]
    public class ListsToJson
    {
        public List<Character.Info> charList;
        public List<Gear.Info> gearList;

        public ListsToJson(List<Character.Info> charList, List<Gear.Info> gearList)
        {
            this.charList = charList;
            this.gearList = gearList;
        }
    }

    ListsToJson lists;

    void Awake()
    {
        if(GameManager.inst == null)
        {
            GameManager.inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        allChar = new List<Character.Info>();
        allGear = new List<Gear.Info>();
        allEnemies = new List<Character.Info>();

        //Debug.Log("Started: " + started);

        if (!restartPP)
        {
            if (PlayerPrefs.HasKey("started"))
            {
                started = PlayerPrefs.GetInt("started");
            }
            else
            {
                started = 0;
            }
        }
        else
        {
            PlayerPrefs.DeleteAll();
            started = 0;
        }

        if (started == 0)
        {
            idGearCount = 0;
            idCharCount = 0;

            Gear.Info gi = new Gear.Info(-1, 10, -1, -1, 0, 0, 0, false, -1);
            for (int i = 0; i < 39; i++)
            {
                if(i < 6)
                {
                    allGear.Add(new Gear.Info(i, 5, i, 0, 0, 0, 0, false, -1));
                    if(i == 0)
                    {
                        allEnemies.Add(new Character.Info(i, 0, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        allChar.Add(new Character.Info(i, 0, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        idCharCount++;
                    }
                    else if(i == 1)
                    {
                        allEnemies.Add(new Character.Info(i, 1, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        allChar.Add(new Character.Info(i, 1, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        idCharCount++;
                    }
                    else if (i == 2)
                    {
                        allEnemies.Add(new Character.Info(i, 2, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        allChar.Add(new Character.Info(i, 2, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        idCharCount++;
                    }
                    else if ((i > 2) && (i < 5))
                    {
                        allEnemies.Add(new Character.Info(i, 3, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        allChar.Add(new Character.Info(i, 3, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        idCharCount++;
                    }
                    else
                    {
                        allEnemies.Add(new Character.Info(i, 4, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        allChar.Add(new Character.Info(i, 4, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        idCharCount++;
                    }
                }
                else if((i > 5) && (i < 12))
                {
                    allGear.Add(new Gear.Info(i, 3, (i-6), 1, 1, 0, 0, false, -1));
                    if(i == 6)
                    {
                        allChar.Add(new Character.Info(i, 4, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        idCharCount++;
                    }
                    if ((i > 6) && (i < 9))
                    {
                        allChar.Add(new Character.Info(i, 5, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        idCharCount++;
                    }
                    else if ((i > 8) && (i < 11))
                    {
                        allChar.Add(new Character.Info(i, 6, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                        idCharCount++;
                    }
                }
                else if ((i > 11) && (i < 18))
                {
                    allGear.Add(new Gear.Info(i, 50, (i - 12), 2, 2, 0, 0, false, -1));
                }
                else if ((i > 17) && (i < 25))
                {
                    allGear.Add(new Gear.Info(i, 10, (i - 12), -1, 0, 0, 0, false, -1));
                }
                else if ((i > 24) && (i < 32))
                {
                    allGear.Add(new Gear.Info(i, 20, (i - 19), -1, 1, 0, 0, false, -1));
                }
                else
                {
                    allGear.Add(new Gear.Info(i, 30, (i - 26), -1, 2, 0, 0, false, -1));
                }
                idGearCount++;
            }

            SaveListsToJson();

            started = 1;
            coins = 50000;
            awMats = 90;
            upMats = 90;

            PlayerPrefs.SetInt("started", started);
            PlayerPrefs.SetInt("idGearCount", idGearCount);
            PlayerPrefs.SetInt("idCharCount", idCharCount);
            PlayerPrefs.SetInt("coins", coins);
            PlayerPrefs.SetInt("awMats", awMats); 
            PlayerPrefs.SetInt("upMats", upMats);
        }
        else
        {
            GetListsFromJson();
            allChar = lists.charList;
            allGear = lists.gearList;

            Gear.Info gi = new Gear.Info(-1, 10, -1, -1, 0, 0, 0, false, -1);
            allEnemies.Add(new Character.Info(0, 0, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
            allEnemies.Add(new Character.Info(1, 1, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
            allEnemies.Add(new Character.Info(2, 2, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
            allEnemies.Add(new Character.Info(3, 3, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
            allEnemies.Add(new Character.Info(4, 4, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
            allEnemies.Add(new Character.Info(5, 5, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));

            GetPlayerPrefs("idGearCount", ref idGearCount, 0);
            GetPlayerPrefs("idCharCount", ref idCharCount, 0);
            GetPlayerPrefs("coins", ref coins, 50000);
            GetPlayerPrefs("upMats", ref upMats, 90);
            GetPlayerPrefs("awMats", ref awMats, 90);
        }
        initialized = true;
    }

    void GetPlayerPrefs(string name, ref int toGet, int num)
    {
        if (PlayerPrefs.HasKey(name))
        {
            toGet = PlayerPrefs.GetInt(name);
        }
        else
        {
            toGet = num;
        }
    }

    public void GetListsFromJson ()
    {
        lists = FileHandler.ReadFromJSON<ListsToJson>(filename);
    }

    public void SaveListsToJson()
    {
        FileHandler.SaveToJson2(new ListsToJson(allChar, allGear), filename);
    }

    public int GetCharPosById(int id)
    {
        int pos = -1;
        for (int i = 0; i < allChar.Count; i++)
        {
            if (allChar[i].id == id)
            {
                pos = i;
            }
        }
        return pos;
    }

    public Character.Info GetCharInfoById(int id)
    {
        Character.Info chi = new Character.Info();
        for (int i = 0; i < allChar.Count; i++)
        {
            if (allChar[i].id == id)
            {
                chi = allChar[i];
            }
        }
        return chi;
    }
}
