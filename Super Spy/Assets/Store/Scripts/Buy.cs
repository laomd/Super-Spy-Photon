using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
public class Buy : MonoBehaviour {
    public Sprite nullpicture;
    public int count;
    public GameObject []arr;
    public Image image;
    private Text text;
    private Text introduce;
    private GameObject character;
	private HeroProperty character_p;
	private PropsProperty weapon_p;
    private HeroProperty character_compoment;
    public GameObject skillbtn;
    private float truecd;
    public int symbol=0;
    public GameObject last_weapon;
	// Use this for initialization
	void Start () {
        count = 0;
        text = transform.Find("Text").GetComponent<Text>();
		character = GameSceneManager.localHero;
		character_p = character.GetComponent<HeroProperty>();
		weapon_p = transform.GetComponent<PropsProperty>();
        
        character_compoment = character.GetComponent<HeroProperty>();
        truecd = character_compoment.attackCd;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void buysth()
    {
        
        introduce = GameObject.Find("Canvas/StorePanel/Weapons Panel/Introduce/Text").GetComponent<Text>();
        if (text.text=="Buy")
        {
            if(count<=3)
            {
                if (character_p.money >= weapon_p.money)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (arr[i].transform.Find("Image").GetComponent<Image>().sprite == nullpicture)
                        {
                            arr[i].transform.Find("Image").GetComponent<Image>().sprite = image.sprite;
                            arr[i].GetComponent<Introduce>().introduce = introduce.text;
                            arr[i].GetComponent<Introduce>().symbol = symbol;
                            arr[i].GetComponent<PropsProperty>().attack = weapon_p.attack;
                            arr[i].GetComponent<PropsProperty>().cd = weapon_p.cd;
                            arr[i].GetComponent<PropsProperty>().move = weapon_p.move;
                            arr[i].GetComponent<PropsProperty>().money = (int)(weapon_p.money * 0.8);
                            arr[i].GetComponent<PropsProperty>().scale = weapon_p.scale;
                            
                            break;
                        }
                    }
                    count++;
                    character_p.money -= weapon_p.money;
                    character.transform.localScale /= weapon_p.scale;
                    character_compoment.speed += (weapon_p.move / 50);
                    character_compoment.attackPower += (int)(weapon_p.attack);
                    truecd = character_compoment.attackCd - (float)weapon_p.cd;
                    if (character_compoment.attackCd - (float)weapon_p.cd >= 0.1)
                        character_compoment.attackCd -= (float)weapon_p.cd;
                    else
                        character_compoment.attackCd = 0.1f;
                    
                    
                    if (symbol == 1)
                        character_compoment.OnEnableCheck(false);
                    if (symbol==2)
                    {
                        character_compoment.maxBlood += 100;
                        character.GetComponent<HeroAttack>().recover_speed += 2;
                    }


                    
                    if (symbol == 3||symbol ==4|| symbol == 5)//吸收，未解锁，缩小之剑
                    {
                        skillbtn.SetActive(true);
						skillbtn.GetComponentInChildren<SpySkillCD>().symbol = symbol;
                    }
                        
                     
                        
                    if (symbol == 6)
                    {
                        changeToReverseTeam();
                        last_weapon.GetComponent<Introduce>().ischangeteam = true;
                    }
                        
                    disappearance(introduce);
                    gameObject.SetActive(false);
                    
                }
                else
                    text.text = "不够钱";

            }
            
        }
        else if(text.text == "Sell")
        {
            if (count > 0)
            {
                character_p.money += weapon_p.money;
                character_compoment.attackPower -= (int)(weapon_p.attack);
                truecd += (float)weapon_p.cd;
                character_compoment.attackCd = truecd;
                character.transform.localScale *= weapon_p.scale;
                character_compoment.speed -= (weapon_p.move / 50);

                if (symbol == 1)
                    character_compoment.OnEnableCheck(true);
                if (symbol == 2)
                {
                    character_compoment.maxBlood -= 100;
                    character.GetComponent<HeroAttack>().recover_speed -= 2;
                }
                if (symbol==3||symbol == 4||symbol == 5)//吸收，未解锁，缩小之剑
                    skillbtn.SetActive(false);
                
                
                character.transform.localScale *= weapon_p.scale;
				int index = image.GetComponent<IndexRecord>().index;
                arr[index].transform.Find("Image").GetComponent<Image>().sprite = nullpicture;
                arr[index].GetComponent<Introduce>().introduce = "";
                disappearance(introduce);

                arr[index].GetComponent<Introduce>().symbol = 0;
				character.GetComponent<HeroProperty>().symbol = 0;
				skillbtn.GetComponentInChildren<SpySkillCD>().symbol = 0;
                bool first = false, second = false;
                for (int i=arr.Length-1;i>=0;i--)
                {
                    int tmp = arr[i].GetComponent<Introduce>().symbol;
                    if (!first&&tmp==3 )//如果有两把吸收，则卖掉一把后仍可以起效
                    {
						character.GetComponentInChildren<HeroProperty>().symbol = tmp;
                        first = true;
                    }
                    if(!second&&(tmp == 3||tmp == 4 || tmp == 5 ))//还有主动技能的道具
                    {
                        skillbtn.SetActive(true);
						skillbtn.GetComponentInChildren<SpySkillCD>().symbol = tmp;
                        second = true;
                    }
                }
                count--;
            }
        }
    
        
    }

    public void disappearance(Text introduce)
    {
        image.sprite = nullpicture;
        introduce.text = "";
        introduce.transform.Find("Text").GetComponent<Text>().text = "";
        gameObject.SetActive(false);
    }
    public void changeToReverseTeam()
    {
        PunTeams.Team whichteam = character.GetComponent<HeroAttack>().team;
        whichteam = (whichteam == PunTeams.Team.blue) ? PunTeams.Team.red : PunTeams.Team.blue;
        character.GetComponent<HeroAttack>().OnTeamChanged(whichteam);
    }
}



