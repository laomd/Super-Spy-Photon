using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
	public class LobbyPlayer : Photon.NetworkBehaviour
    {
		public static LobbyPlayer localLobbyPlayer;
        public GameObject localIcone;
        public GameObject remoteIcone;
		public Image heroInfo;

		[HideInInspector]
		public bool isReady;
		public static Dictionary<string, string> spriteToGamePlayer;

		public override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (gameObject);
		}

		public void Start ()
		{
			if (spriteToGamePlayer == null) {
				spriteToGamePlayer = new Dictionary<string, string> ();
				spriteToGamePlayer["Blade_Girl"] = "Blade_Girl";
				spriteToGamePlayer["Blade_Warrior"] = "Blade_Warrior";
			}
			ClientReady (false, null);
			if (isLocalPlayer) {
				remoteIcone.SetActive (false);
				localIcone.SetActive (true);
			}
		}

		public override void OnPhotonInstantiate (PhotonMessageInfo info)
		{
			base.OnPhotonInstantiate (info);
			PunTeams.Team myteam = (PunTeams.Team)(photonView.instantiationDataField [0]);
			if (myteam == PhotonNetwork.player.GetTeam()) {
				transform.SetParent (RoomManager.instance.friendList, false);
			} else {
				transform.SetParent (RoomManager.instance.enemyList, false);
			}
		}

		public void OnChooseHero(Sprite hero) {
			heroInfo.sprite = hero;
			heroInfo.color = Color.white;
			if (isLocalPlayer) {
				Image detailedInfo = RoomManager.instance.heroDetailedInfo;
				detailedInfo.sprite = hero;
				detailedInfo.color = Color.white;
			}
		}

		void MakeSymmetric(GameObject obj, int x, int anchoredX) {
			RectTransform trans = obj.GetComponent<RectTransform> ();
			trans.anchoredPosition = new Vector2 (anchoredX, 0);
			trans.pivot = trans.anchorMax = trans.anchorMin = new Vector2 (x, 0.5f);
		}
		void MakeSymmetric() {
			MakeSymmetric (remoteIcone, 1, -15);
			MakeSymmetric (localIcone, 1, -15);
			MakeSymmetric (heroInfo.gameObject, 0, 30);
		}

        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
  
        public void OnReady()
        {
			PhotonNetwork.player.NickName = heroInfo.sprite.name;
			photonView.RPC("ClientReady", PhotonTargets.All, true, heroInfo.sprite.name);
        }

		[PunRPC]
		public void ClientReady(bool ready, string spName) {
			isReady = ready;
			if (ready) {
				OnChooseHero (Resources.Load<Sprite>(spName));
				int cnt = 0;
				foreach (var lobbyPlayer in GameObject.FindObjectsOfType<LobbyPlayer>()) {
					if (lobbyPlayer.isReady) {
						cnt++;
					} else {
						return;
					}
				}
				ManagerBase.LoadScene (2);
			}
		}
    }
}
