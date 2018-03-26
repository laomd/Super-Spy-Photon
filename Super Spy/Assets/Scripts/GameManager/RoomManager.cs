using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Prototype.NetworkLobby
{
    //List of players in the lobby
	public class RoomManager : ManagerBase
    {
		static RoomManager _instance = null;

        public RectTransform friendList, enemyList;
		public Image heroDetailedInfo;
		public Button readyButton;
		public GameObject lobbyPlayerPrefab;
		public static RoomManager instance {
			get {
				return _instance;
			}
		}

		public override void Start()
		{
			base.Start ();
			_instance = this;
			readyButton.onClick.RemoveAllListeners();
			readyButton.onClick.AddListener(OnReadyClicked);
			LobbyPlayer.localLobbyPlayer = AddPlayer ();
        }

		void OnReadyClicked() {
			readyButton.interactable = false;
			readyButton.GetComponentInChildren<Text> ().text = "已准备";
			foreach (var item in GetComponentsInChildren<HeroCeil>()) {
				item.GetComponent<Button> ().interactable = false;
			}
			LobbyPlayer.localLobbyPlayer.OnReady ();
		}


		public LobbyPlayer AddPlayer()
        {
			object[] data = new object[]{ PhotonNetwork.player.GetTeam() };
			GameObject obj = PhotonNetwork.Instantiate (lobbyPlayerPrefab.name, Vector3.zero, Quaternion.identity, 0, data);
			if (obj) {
				return obj.GetComponent<LobbyPlayer>();
			} else {
				return null;
			}
        }

		public override void OnDisconnectedFromPhoton ()
		{
			base.OnDisconnectedFromPhoton ();
			PhotonNetwork.ReconnectAndRejoin ();
		}

		protected override void OnExit ()
		{
			base.OnExit ();
			PhotonNetwork.Destroy (LobbyPlayer.localLobbyPlayer.gameObject);
			LoadScene (0);
		}
    }
}
