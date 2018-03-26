using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class Introduce : MonoBehaviour {
    private GameObject show;
    private Image picture;
    private Text show_introduce, text, index;
    
    public int symbol=0; 
    public string introduce;
	private PropsProperty show_property,thisproperty;
    public bool ischangeteam ;
    private void Start()
    {
        ischangeteam = false;
		show = GameObject.Find ("Canvas/StorePanel/Weapons Panel/Introduce/b_a_s/Buy");
        show.SetActive(false);
		text = show.GetComponentInChildren<Text>();
        picture = GameObject.Find("Canvas/StorePanel/Weapons Panel/Introduce/Image").GetComponent<Image>();
		show_introduce= GameObject.Find("Canvas/StorePanel/Weapons Panel/Introduce/Text").GetComponent<Text>();
		show_property = show.GetComponent<PropsProperty>();
		thisproperty = transform.GetComponent<PropsProperty>();
    }
    public void Click()
    {
        show.GetComponent<Buy>().symbol = symbol;
        show_introduce.transform.Find("Text").gameObject.SetActive(true);
        show_property.money = thisproperty.money;
        show_property.attack = thisproperty.attack;
        show_property.cd = thisproperty.cd;
        show_property.move = thisproperty.move;
        show_property.scale = thisproperty.scale;
        show_introduce.text = introduce;
		show_introduce.transform.Find("Text").GetComponent<Text>().text = "$"+transform.GetComponent<PropsProperty>().money.ToString();
        picture.sprite=transform.Find("Image").GetComponent<Image>().sprite;


        if (transform.parent.name == "package item")
        {
			picture.GetComponent<IndexRecord>().index = transform.GetComponent<IndexRecord>().index;
            text.text = "Sell";
            if(symbol == 6)
                show.SetActive(false);
                
            else if (transform.Find("Image").GetComponent<Image>().sprite == show.GetComponent<Buy>().nullpicture)
            {
                show_introduce.transform.Find("Text").gameObject.SetActive(false);
                show.SetActive(false);
            }
                
            else
                show.SetActive(true);

        }
        else
        {
            text.text = "Buy";
            if (show.GetComponent<Buy>().count == 4||symbol==6&& ischangeteam == true)
                show.SetActive(false);
            else
                show.SetActive(true);
            
        }
    }
}
