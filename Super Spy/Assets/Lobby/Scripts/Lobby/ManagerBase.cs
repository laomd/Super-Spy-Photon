using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerBase : Photon.PunBehaviour {
	float last_esc_time = 0;
	public static int maxPlayers;

	public static void LoadScene(int sceneIdx) {
		if (sceneIdx == 1) {
			PhotonNetwork.automaticallySyncScene = true;
			PhotonNetwork.LoadLevel (1);
		} else {
			SceneManager.LoadSceneAsync (sceneIdx);
		}
	}

	public static void ConnectToMaster() {
		PhotonNetwork.ConnectUsingSettings (Version.Current.ToString());
	}

	public virtual void Awake() {

	}
		
	void OnGUI() {
		GUIStyle style = new GUIStyle ();
		style.fontSize = 20;
		string detailed;
		var state = PhotonNetwork.connectionStateDetailed;
		if (state == ClientState.ConnectedToNameServer || state == ClientState.ConnectedToMaster || state == ClientState.ConnectedToGameserver) {
			detailed = "已连接";
			style.normal.textColor = Color.green;
		} else {
			if (state == ClientState.Disconnected) {
				detailed = "已断开";
				style.normal.textColor = Color.red;
			} else {
				if (state == ClientState.Joined) {
					detailed = "匹配成功";
					style.normal.textColor = Color.green;
				} else {
					if (state == ClientState.ConnectingToNameServer || state == ClientState.ConnectingToGameserver || state == ClientState.ConnectingToMasterserver) {
						detailed = "请稍候...";
						style.normal.textColor = Color.yellow;
					} else {
						detailed = state.ToString ();
						style.normal.textColor = Color.blue;
					}
				}
			}
		}
		GUI.Label (new Rect ((Screen.width - detailed.Length) / 2, 75, 200, 50), detailed, style);
	}

	public virtual void Start() {

	}

	protected virtual void OnExit() {

	}
	public virtual void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (last_esc_time == 0 || Time.time - last_esc_time > 1.0f) {
				last_esc_time = Time.time;
			} else {
				OnExit ();
			}
		}
	}
}
