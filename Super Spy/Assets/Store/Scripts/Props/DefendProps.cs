using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
public class DefendProps : Props
{
    public GameObject[] arr;
    // Use this for initialization
    private string[] text ;
    private string[] introduce = new string[3];
    private double []buff;
    private int[] money;
    public GameObject nomal_panel, spy_panel;
    private void Start()
    {
        text =new string[3]{ "cursorHand_beige","cursorHand_grey","cursorHand_blue" };
        money = new int[3] { 24, 56, 62 };
        buff = new double[3] { 0.2, 0.5, 1 };
        introduce[0] = "布手套\n\t平A冷却 -0.2\n\n  ";
        introduce[1] = "铁手套\n\t平A冷却 -0.5\n\n  ";
        introduce[2] = "金刚石手套\n\t平A冷却 -1\n\n  ";
    }
    public void show_weapons()
    {
        nomal_panel.SetActive(true);
        spy_panel.SetActive(false);
        GameObject.Find("Canvas/StorePanel/Weapons Panel/weapons").GetComponent<Image>().color = transform.GetComponent<Image>().color;
        for (int i = 0; i < 3; i++)
            arr[i].SetActive(true);
        for (int i = 3; i < arr.Length; i++)
            arr[i].SetActive(false);
        
        
        for (int i = 0; i < 3; i++)
        {
            arr[i].GetComponent<PropsProperty>().scale = 1;
            arr[i].GetComponent<PropsProperty>().cd = buff[i];
            arr[i].GetComponent<PropsProperty>().move = arr[i].GetComponent<PropsProperty>().attack = 0;
            arr[i].transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(text[i]);
            arr[i].GetComponent<Introduce>().introduce = introduce[i];
            arr[i].GetComponent<PropsProperty>().money = money[i];
            arr[i].GetComponent<Introduce>().symbol = 0;
        }
    }
}



