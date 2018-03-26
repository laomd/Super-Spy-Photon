using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowMoney : MonoBehaviour {
	public static ShowMoney instance;
    private Text moneystr;
	private HeroProperty p;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
        moneystr = transform.GetComponent<Text>();
    }
    public void SetCharacter(GameObject pp)
    {
		p = pp.GetComponent<HeroProperty>();
    }
	
	// Update is called once per frame
	void Update () {
        if(p!=null)
            moneystr.text = "$"+p.money.ToString();
	}
}
