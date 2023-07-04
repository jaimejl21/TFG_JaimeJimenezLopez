using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class CharInfoManager : MonoBehaviour
{
    public GameObject charGO;
    public Transform charPos;
    public TextMeshProUGUI[] txts;

    public int idToEquip;
    
    string typeName, weaponName, specialName, charInfo;

    public List<Character.Info> allCharList;

    private void Start()
    {
        allCharList = GameManager.allChar.ToList();

        idToEquip = GameManager.inst.charToEquipGear;
        charGO.transform.GetComponent<Character>().info = GameManager.inst.GetCharInfoById(idToEquip);
        Instantiate(charGO, charPos);

        SetSpecial(charGO.transform.GetComponent<Character>().info.special);
        SetType();
        SetWeapon();
        SetCharInfo();
        InitTxts();
    }

    void InitTxts()
    {
        txts[0].text = charGO.transform.GetComponent<Character>().info.name;
        txts[1].text = "Nivel: " + charGO.transform.GetComponent<Character>().info.level;
        txts[2].text = "Tipo: " + typeName;
        txts[3].text = "Arma: " + weaponName;
        txts[4].text = "Atq especial: " + specialName;
        txts[5].text = "Atq: " + charGO.transform.GetComponent<Character>().info.stats.baseAtk;
        txts[6].text = "Def: " + charGO.transform.GetComponent<Character>().info.stats.baseDef;
        txts[7].text = "Vida: " + charGO.transform.GetComponent<Character>().info.stats.baseHp;
        txts[8].text = charInfo;
    }

    void SetType()
    {
        switch (charGO.transform.GetComponent<Character>().info.type)
        {
            case 0:
                typeName = "Magia ancestral";
                //typeColor = Color.white;
                break;
            case 1:
                typeName = "Magia oscura";
                //typeColor = new Color(.5f, .2f, .6f, 1f);
                break;
            case 2:
                typeName = "Magia de naturaleza";
                //typeColor = new Color(.5f, .3f, 0f, 1f);
                break;
            case 3:
                typeName = "Astucia";
                //typeColor = Color.green;
                break;
            case 4:
                typeName = "Vitalidad";
                //typeColor = Color.yellow;
                break;
            case 5:
                typeName = "Velocidad";
                //typeColor = Color.blue;
                break;
            case 6:
                typeName = "Fuerza";
                //typeColor = Color.red;
                break;
            default:
                break;
        }
    }

    void SetWeapon()
    {
        switch (charGO.transform.GetComponent<Character>().info.weapon)
        {
            case 6:
                weaponName = "Espada";
                break;
            case 7:
                weaponName = "Lanza";
                break;
            case 8:
                weaponName = "Guada�a";
                break;
            case 9:
                weaponName = "Daga";
                break;
            case 10:
                weaponName = "Bast�n";
                break;
            case 11:
                weaponName = "Arco";
                break;
            case 12:
                weaponName = "Hacha";
                break;
            default:
                break;
        }
    }

    void SetSpecial(int sp)
    {
        switch (sp)
        {
            case -1:
                int rand = new Random().Next(0, 13);
                charGO.transform.GetComponent<Character>().info.special = rand;
                for (int i = 0; i < allCharList.Count; i++)
                {
                    if (allCharList[i].id == idToEquip)
                    {
                        allCharList[i] = charGO.transform.GetComponent<Character>().info;
                    }
                }
                GameManager.allChar = allCharList;
                GameManager.inst.SaveListsToJson();
                SetSpecial(rand);
                break;
            case 0:
                specialName = "Curaci�n todos";
                break;
            case 1:
                specialName = "Curaci�n personal";
                break;
            case 2:
                specialName = "Atq todos";
                break;
            case 3:
                specialName = "Atq fila";
                break;
            case 4:
                specialName = "Atq columna";
                break;
            case 5:
                specialName = "Aumenta atq todos";
                break;
            case 6:
                specialName = "Disminuye atq todos";
                break;
            case 7:
                specialName = "Aumenta def todos";
                break;
            case 8:
                specialName = "Disminuye def todos";
                break;
            case 9:
                specialName = "Aumenta atq personal";
                break;
            case 10:
                specialName = "Disminuye atq personal";
                break;
            case 11:
                specialName = "Aumenta def personal";
                break;
            case 12:
                specialName = "Disminuye def personal";
                break;
            default:
                break;
        }
    }

    void SetCharInfo()
    {
        switch(charGO.transform.GetComponent<Character>().info.name)
        {
            case "Zindrael":
                charInfo = "Joven caballero que perdi� a sus padres en la guerra de los " +
                    "soles. Tras la muerte de sus padres decidi� convertirse en caballero para as� proteger a " +
                    "los indefensos y a sus seres m�s queridos. Luch� en diferentes batallas al servicio del " +
                    "rey. Tras ganarse la popularidad del pueblo y otros se�ores, su estatus se vio " +
                    "aumentado en poco tiempo lo que hizo que conociese a Dira y m�s adelante se " +
                    "convirtiese en su esposa. Su gran popularidad hizo que el mism�simo rey le envidiase y " +
                    "temiese que llegase a destronarle";
                break;
            case "Velkra":
                charInfo = "Es una joven maga que se licencio en la academia de magia y brujer�a con altas " +
                    "habilidades llegando a destacar entre los de su promoci�n. Debido a sus altos " +
                    "resultados, fue propuesta para convertirse en la maga protectora de uno de los reinos " +
                    "de las regiones exaltadas, pero ella deneg� la oferta pues no quer�a servir a ning�n rey " +
                    "importante o poderoso. Su reticencia por no relacionarse con la nobleza le acarreo " +
                    "varios problemas, pero su reputaci�n como maga se manten�a intacta debido a sus " +
                    "capacidades m�gicas";
                break;
            case "Freydam":
                charInfo = "Joven guerrera que vaga por los bosques del reino en busca de monstruos " +
                    "que asesinar por dinero. De car�cter sombr�o y taciturno, esta joven guerrera viaja por " +
                    "los diferentes reinos en busca de monstruos que matar. A pesar de su edad es " +
                    "reconocida como una grandiosa caza monstruos y su reputaci�n es alta en varios " +
                    "reinos. Es hu�rfana desde muy joven debido a que un grupo de monstruos " +
                    "asaltaron su casa y asesinaran a toda su familia de forma despiadada";
                break;
            case "Karris":
                charInfo = "Guerrero fuerte y experimentado que elude las leyes debido a su gran historial " +
                    "de soldado. Tiene el favor de los reyes y se�ores y hace lo que quiere sin importar las " +
                    "consecuencias. Puede que tenga el favor de los reyes, pero no les tiene el m�nimo de " +
                    "respeto por ellos y se mueve entre el que mejor le trate. En su interior " +
                    "sabe que �nicamente fue un pe�n usado por reyes y aunque se convirti� en un h�roe y " +
                    "gan� fama, perdi� mucho m�s e intenta compensar ese vac�o con bienes materiales y " +
                    "cumpliendo encargos de se�ores";
                break;
            case "Belaran":
                charInfo = "Se trata de una elfa de joven aspecto, aunque tiene m�s de 100 a�os. Vive en " +
                    "las tierras imperecederas junto al resto de los elfos. Su padre form� parte de la " +
                    "guardia real �lfica, pero muri� en la batalla del estrecho contra los orcos. Esa " +
                    "batalla perteneci� a una guerra en la que lucharon elfos, humanos y enanos para " +
                    "expulsar a los orcos y otras criaturas de los reinos del este. " +
                    "Es muy h�bil con el arco y diestra con la espada curva";
                break;
            case "Oriel":
                charInfo = "Se trata de un joven bardo que vaga por los reinos en busca de h�roes con " +
                    "haza�as que pueda transformar en mel�dicas canciones que toca con su inseparable " +
                    "la�d por las diferentes ciudades y pueblos. Se licenci� en la universidad, pero decidi� " +
                    "embarcar su vida en la m�sica y descubrir experiencias inolvidables. Con muchos " +
                    "conocidos y pocos amigos este bardo posee un car�cter afable y en�rgico que podr�a " +
                    "llegar a irritar a algunas personas. Sufri� por amor y busca la soledad en la " +
                    "m�sica";
                break;
            case "Glokku":
                charInfo = "Brujo ermita�o de aspecto anciano, pero muy poderoso. Vive aislado " +
                    "de la sociedad y se relaciona con pocas personas. Se dice que es uno de los brujos m�s " +
                    "poderosos de todos los reinos que no soporta a los reyes y nobles y prefiere vivir " +
                    "alejado de esos asuntos. Anta�o fue un brujo al servicio de un poderoso rey y le " +
                    "proporciono consejo y poder. Un poder que acab� consumi�ndole y convirti�ndole en " +
                    "una persona insensible y cruel, llegando a realizar actos horribles por los que todav�a " +
                    "se culpa";
                break;
            case "Yonlud":
                charInfo = "Cazarrecompensas de las tierras altas. Se trata de un experto y habilidoso " +
                    "cazarrecompensas muy conocido y temido en muchos reinos. Se gan� la fama tras " +
                    "capturar a un temible asesino que acabo con las vidas de muchas personas incluida la " +
                    "de su hermana . Ella andaba con personas influyentes y poderosas lo cual la puso en el " +
                    "punto de mira. Fue asesinada por ese criminal por orden de alg�n noble que no quer�a " +
                    "que se entrometiese en sus asuntos. Tras conseguir fama y respeto fue contratado por " +
                    "varios reyes y nobles que le pagaban cuantiosas recompensas";
                break;
            case "Dramor":
                charInfo = "Joven mago que se inscribi� en la academia de magia donde consigui� llamar " +
                    "la atenci�n debido a sus grandes habilidades a pesar de su corta edad.Destaco en " +
                    "muchas y diferentes aptitudes, pero quiso indagar en poderes restringidos por la " +
                    "academia como la necromancia y otras artes oscuras. Este tipo de relaciones con " +
                    "magias oscuras le ocasion� la expulsi�n de la academia. El triste pasado que guarda le " +
                    "condujo al inter�s de la magia oscura ya que toda su familia fue masacrada en una " +
                    "disputa entre dos reinos";
                break;
            case "Godrick":
                charInfo = "Se trata de un cl�rigo ambulante que vaga por las tierras del este ayudando a " +
                    "la gente que se pierda o se encuentra en peligro. En su pasado fue un general " + 
                    "que luch� en las guerras contra los orcos del este. La guerra acab� sin un claro " +
                    "vencedor, haciendo que todas las muertes fuesen en vano. Tras esas batallas y la " +
                    "�derrota� en la guerra, decidi� alejarse de la guerra y no volver a entrometerse en " +
                    "temas b�licos si no fuese por una buena raz�n";
                break;
            default:
                break;
        }
    }
}
