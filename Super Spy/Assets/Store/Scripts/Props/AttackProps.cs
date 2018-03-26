using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
public class AttackProps : Props
{
    public GameObject[] arr;
    // Use this for initialization
    private string []text;
    private string[] introduce = new string[3];
    private int buff;
    private int[] money;
    public GameObject nomal_panel, spy_panel;
    private void Start()
    {
        text = new string[3] { "cursorSword_bronze", "cursorSword_silver", "cursorSword_gold" };
        money = new int[3] { 23, 55, 60 };
        introduce[0] = "木剑\n\t攻击 +5\n\n  ";
        
        introduce[1] = "银剑\n\t攻击 +10\n\n  ";

        introduce[2] = "金剑\n\t攻击 +15\n\n  ";

        for (int i = 0; i < arr.Length; i++)
            arr[i].SetActive(false);
    }
    public void show_weapons()
    {
        nomal_panel.SetActive(true);
        spy_panel.SetActive(false);
        buff = 0;
        GameObject.Find("Canvas/StorePanel/Weapons Panel/weapons").GetComponent<Image>().color = transform.GetComponent<Image>().color;
        for (int i = 0; i < 3; i++)
            arr[i].SetActive(true);
        for (int i = 3; i < arr.Length; i++)
            arr[i].SetActive(false);
        

        for (int i = 0; i < 3; i++)
        {
            buff += 5;
            arr[i].GetComponent<PropsProperty>().scale = 1;
            arr[i].GetComponent<PropsProperty>().attack = buff;
			arr [i].GetComponent<PropsProperty> ().move = 0;
            arr[i].GetComponent<PropsProperty>().cd = 0;
            arr[i].transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(text[i]);
            arr[i].GetComponent<Introduce>().introduce = introduce[i];
            arr[i].GetComponent<PropsProperty>().money = money[i];
            arr[i].GetComponent<Introduce>().symbol = 0;
        }
    }
}





