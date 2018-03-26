using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
public class SpyProps : Props
{
    public GameObject[] arr;
    // Use this for initialization
    private string[] text;
    private string[] introduce = new string[6];
    private int buff;
    private int[] money;
    public GameObject nomal_panel,spy_panel;

    private void Start()
    {
        
        text = new string[6] { "cursorSword_bronze", "arrowSilver_right", "cursorSword_gold", "cursorHand_beige", "cursorHand_grey", "cursorHand_blue" };
        money = new int[6] { 101, 102, 103, 104, 105, 106 };
        introduce[0] = "免疫\n\t 防御塔和小兵将无视你\n\n  ";

        introduce[1] = "治疗者\n\t增加100最大生命并且加快血量恢复速度 \n\n  ";
        introduce[2] = "吸收(唯一主动)\n\t 3秒内吸收你受到的一切伤害\n\n  ";
        introduce[3] = "一击必杀(唯一主动)\n\t 2秒内你的攻击力将变得非常高\n\n  ";
        introduce[4] = "缩小之剑(唯一主动)\n\t 使你缩小并且加快移动速度持续1.4秒";
        introduce[5] = "转换\n\t 只可购买一次并且不可出售\n\t改变你的队别\n\n"; 



    }
    public void show_weapons()
    {
        nomal_panel.SetActive(false);
        spy_panel.SetActive(true);
        GameObject.Find("Canvas/StorePanel/Weapons Panel/weapons").GetComponent<Image>().color = transform.GetComponent<Image>().color;
        for (int i = 0; i < arr.Length; i++)
            arr[i].SetActive(true);


        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].GetComponent<PropsProperty>().scale = 1;
            arr[i].GetComponent<PropsProperty>().attack = 0;
			arr [i].GetComponent<PropsProperty> ().move = 0;
            arr[i].GetComponent<PropsProperty>().cd = 0;
            arr[i].transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(text[i]);
            arr[i].GetComponent<Introduce>().introduce = introduce[i];
            arr[i].GetComponent<PropsProperty>().money = money[i];

            arr[i].GetComponent<Introduce>().symbol = i + 1;
            
              
        }
    }
}





