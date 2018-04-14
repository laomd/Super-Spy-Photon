using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DayNightController : Photon.NetworkBehaviour, IEventSystemHandler {
	public static DayNightController instance;
	public static int DayTime = 120;
	public static int NightTime = 30;
	float curTime;
	public GameObject SpyButton;

	Slider timeSlider;
	bool isNight;
	[System.Serializable]
	public class DayNightEvent : UnityEvent<bool>
	{
		
	}

	[SerializeField]
	DayNightEvent m_OnDayNightChange = new DayNightEvent();
	public DayNightEvent onDayNightChanged {
		get {
			return m_OnDayNightChange;
		}
		set {
			m_OnDayNightChange = value;
		}
	}

	public override void Awake ()
	{
		base.Awake ();
		instance = this;
		timeSlider = GetComponent<Slider>();
	}

	public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			stream.SendNext (timeSlider.maxValue);
			stream.SendNext (timeSlider.value);
		} else {
			timeSlider.maxValue = (float)stream.ReceiveNext ();
			timeSlider.value = (float)stream.ReceiveNext ();
		}
	}

	// Use this for initialization
	void Start () {
		GetComponent<RectTransform> ().localPosition = new Vector3 (0, 0, 0);
		timeSlider.onValueChanged.AddListener (delegate(float arg0) {
			if (arg0 == timeSlider.minValue) {
				timeSlider.maxValue = NightTime;
				isNight = true;
				m_OnDayNightChange.Invoke(isNight);
			} else if (arg0 == timeSlider.maxValue) {
				curTime = timeSlider.maxValue = DayTime;
				isNight = false;
				m_OnDayNightChange.Invoke(isNight);
			}
		});

		onDayNightChanged.AddListener (delegate(bool arg0) {
			SpyButton.SetActive (arg0 && GameSceneManager.localHero.GetComponent<HeroProperty>().isSpy);
			if (arg0) {
				Camera.main.gameObject.AddComponent<FogOfWarEffect>();
			}
			else {
				FogOfWarEffect fow = Camera.main.GetComponent<FogOfWarEffect>();
				if (fow) {
					Destroy(fow);
				}
			}
		});

		if (isLocalPlayer) {
			StartCoroutine (Timer ());
		}
	}
		
	IEnumerator Timer() {
		timeSlider.value = timeSlider.maxValue;
		while (true) {
			yield return null;
			if (enabled) {
				int tmp;
				if (isNight) {
					curTime += Time.deltaTime;
					tmp = (int)Mathf.Floor (curTime);
				} else {
					curTime -= Time.deltaTime;
					tmp = (int)Mathf.Ceil (curTime);
				}
				if (tmp != timeSlider.value) {
					timeSlider.value = tmp;
				}
			}
		}
	}
}
