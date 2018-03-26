using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;

public class HeroCeil : MonoBehaviour {
	public void OnClicked() {
		RoomManager.instance.readyButton.interactable = true;
		LobbyPlayer.localLobbyPlayer.OnChooseHero (GetComponent<UnityEngine.UI.Image> ().sprite);
	}
}
