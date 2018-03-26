using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SpySkillCD : MonoBehaviour
{
    //冷却周期  
    private float coolingTimer = 10;
    private HeroProperty character_p;
    private float currentTime = 0;
    private GameObject character;
    private Text show_cooltime;
    //冷却图片  
    public Image coolingImage;
    private float last_time=0;
    private float total_time=0;
    private bool init = true;
    public int symbol;
    private string[] arr;
    // Use this for initialization  
    void Start()
    {
        arr = new string[3] { "absorb", "kill", "run" };
		character = GameSceneManager.localHero;
        currentTime = coolingTimer;
        show_cooltime = transform.Find("Text").GetComponent<Text>();
        character_p =character.GetComponent<HeroProperty>();
    }

    // Update is called once per frame  
    void Update()
    {
        if(last_time < total_time)
        {
            last_time += Time.deltaTime;
        }
        else if(init==false)
        {
            if(symbol==5)
            {
                character_p.speed -= 6;
                character_p.speed -= 6;
                character.transform.localScale *= 3;
            }
            else if(symbol==3)
            {
				character.GetComponent<HeroProperty>().symbol = 0;
            }
            else if (symbol == 4)
            {
                character_p.attackPower -= 100;
            }

            init = true;
        }
        if (currentTime < coolingTimer)
        {
            currentTime += Time.deltaTime;
            show_cooltime.text = (coolingTimer - currentTime).ToString("f0");
            //按时间比例计算出Fill Amount值  
            coolingImage.fillAmount = 1 - currentTime / coolingTimer;
        }
        else
            show_cooltime.text = arr[symbol - 3];
  
            
    }

    public void OnBtnClickSkill()
    {
        if (currentTime >= coolingTimer)//冷却完成了
        {
            if(symbol==5)//缩小之剑
            {
                character_p.speed += 6;
                character_p.speed += 6;
                character.transform.localScale /= 3;
                total_time = 1.4f;
            }
            else if(symbol==3)//吸收
            {
				character.GetComponent<HeroProperty>().symbol = 3;
                total_time = 3f;
            }
            else if(symbol==4)
            {
                character_p.attackPower += 100;
                total_time = 2f;
            }
            
            last_time = 0;
            init = false;
            currentTime = 0.0f;
            coolingImage.fillAmount = 1.0f;
        }
        
    }
}