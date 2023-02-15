using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    public int charToEquipGear = 0;
    public bool restartPP;
    public int coins, idGearCount, idCharCount;

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

            Gear.Info gi = new Gear.Info(-1, 10, -1, -1, 0, false, -1);
            for (int i = 0; i < 18; i++)
            {
                if(i<6)
                {
                    allGear.Add(new Gear.Info(i, 10, i, 0, 0,false, -1));
                    idGearCount++;
                    allEnemies.Add(new Character.Info(i, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                }
                else if((i > 5) && (i < 12))
                {
                    allGear.Add(new Gear.Info(i, 10, (i-6), 1, 1, false, -1));
                    idGearCount++;
                }
                else
                {
                    allGear.Add(new Gear.Info(i, 10, (i - 12), 2, 2, false, -1));
                    idGearCount++;
                }
                allChar.Add(new Character.Info(i, -1, false, new List<Gear.Info>() { gi, gi, gi, gi, gi, gi }, 1, 0, 320, new Character.Stats()));
                idCharCount++;
            }

            SaveListsToJson();

            started = 1;
            coins = 100;

            PlayerPrefs.SetInt("started", started);
            PlayerPrefs.SetInt("idGearCount", idGearCount);
            PlayerPrefs.SetInt("idCharCount", idCharCount);
            PlayerPrefs.SetInt("coins", coins);
        }
        else
        {
            GetListsFromJson();
            allChar = lists.charList;
            allGear = lists.gearList;

            GetPlayerPrefs(ref idGearCount, 0);
            GetPlayerPrefs(ref idCharCount, 0);
            GetPlayerPrefs(ref coins, 100);
        }
    }

    void GetPlayerPrefs(ref int var, int num)
    {
        if (PlayerPrefs.HasKey(nameof(var)))
        {
            var = PlayerPrefs.GetInt(nameof(var));
        }
        else
        {
            var = num;
        }
    }

    public void GetListsFromJson ()
    {
        lists = FileHandler.ReadFromJSON<ListsToJson>(filename);
        //Debug.Log(lists.charList[0].id);
        //Debug.Log(lists.teamList[0].id);
    }

    public void SaveListsToJson()
    {
        FileHandler.SaveToJson2(new ListsToJson(allChar, allGear), filename);
        //Debug.Log("SaveJson");
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
