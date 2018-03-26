using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ScaleController : MonoBehaviour, IPointerClickHandler{
	public Transform minimap;
	public Image point;
	public Sprite minus;
	public Sprite plus;
	public Sprite remind;
	public Sprite normal;

	public void OnPointerClick (PointerEventData eventData) {
		float scale = 1;
		Sprite button_sp = plus, point_sp = normal;
		if (minimap.localScale.x == 1) {
			scale = 1.5f;
			button_sp = minus;
			point_sp = remind;
		}
		minimap.DOScale(new Vector3 (scale, scale, scale), 0.5f);
		GetComponent<Image> ().sprite = button_sp;
		point.sprite = point_sp;
	}
}
