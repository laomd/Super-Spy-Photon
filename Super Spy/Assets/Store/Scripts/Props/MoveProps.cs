using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
public class MoveProps : Props
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
        text = new string[3] { "arrowBeige_right", "arrowSilver_right", "arrowBlue_right" };
        money = new int[3] { 20, 50, 60 };
        introduce[0] = "普通鞋\n\t速度 +5\n\n  ";
        introduce[1] = "中等鞋\n\t速度 +10\n\n  ";
        introduce[2] = "快速鞋\n\t速度 +15\n\n  " ;

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
            buff += 60;
            arr[i].GetComponent<PropsProperty>().scale = 1;
            arr[i].GetComponent<PropsProperty>().move = buff;
            arr[i].GetComponent<PropsProperty>().cd = arr[i].GetComponent<PropsProperty>().attack = 0;
            arr[i].transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(text[i]);
            arr[i].GetComponent<Introduce>().introduce = introduce[i];
            arr[i].GetComponent<PropsProperty>().money = money[i];
            arr[i].GetComponent<Introduce>().symbol = 0;
        }
    }
}
