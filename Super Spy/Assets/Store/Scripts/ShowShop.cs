using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowShop : MonoBehaviour {
    private GameObject shop;
	// Use this for initialization
	void Start () {
        shop = GameObject.Find("Canvas/StorePanel");
        shop.SetActive(false);
	}
	
    void toshow()
    {
		shop.SetActive (!shop.activeInHierarchy);
    }
}
