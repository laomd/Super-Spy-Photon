using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Prototype.NetworkLobby
{
	public class LobbyManager : ManagerBase
    {
        static public LobbyManager instance;
        [Space]
        [Header("UI Reference")]
		public RectTransform topPanel;
        public RectTransform mainMenuPanel;
        protected RectTransform currentPanel;

		public override void Start()
        {
			base.Start ();
			instance = this;
            currentPanel = mainMenuPanel;
            GetComponent<Canvas>().enabled = true;
			if (PhotonNetwork.inRoom) {
				PhotonNetwork.LeaveRoom ();
			}
			PhotonNetwork.autoCleanUpPlayerObjects = false;
			ConnectToMaster ();
        }

		protected override void OnExit ()
		{
			base.OnExit ();
			Application.Quit ();
		}

		public override void OnJoinedRoom ()
		{
			base.OnJoinedRoom ();
			LobbyInfoPanel.instance.BecomeMatchBox ();
			StartCoroutine (CheckToStartCoroutine());
		}

		public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
		{
			base.OnPhotonRandomJoinFailed (codeAndMsg);
			PhotonNetwork.CreateRoom (Random.Range (0, 10000).ToString(), 
				new RoomOptions () { MaxPlayers = (byte)maxPlayers, PlayerTtl = 12*60*1000}, null);
		}

		public override void OnDisconnectedFromPhoton ()
		{
			base.OnDisconnectedFromPhoton ();
			ConnectToMaster ();
		}

		public override void OnLeftRoom ()
		{
			base.OnLeftRoom ();
			LobbyInfoPanel.instance.gameObject.SetActive (false);
		}

        public void ChangeTo(RectTransform newPanel)
        {
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null)
            {
                newPanel.gameObject.SetActive(true);
            }
			topPanel.gameObject.SetActive (newPanel != null);

            currentPanel = newPanel;
        }

		IEnumerator CheckToStartCoroutine() {
			int m = PhotonNetwork.room.MaxPlayers;
			while (PhotonNetwork.inRoom) {
				int p = PhotonNetwork.room.PlayerCount;
				if (p < m) {
					yield return null;
					LobbyInfoPanel.instance.Display (p + "/" + m);
				} else {
					break;
				}
			}
			if (PhotonNetwork.inRoom) {
				if (PhotonNetwork.isMasterClient) {
					var players = PhotonNetwork.playerList;
					/*for (int i = 0; i < players.Length; i++) {
						if (i % 2 == 0) {
							players [i].SetTeam (PunTeams.Team.red);
						} else {
							players [i].SetTeam (PunTeams.Team.blue);
						}
						players [i].SetScore (0);
					}*/
					int idx = Random.Range(0, players.Length);
					players [idx].SetScore (1);
					players [(idx + 1) % players.Length].SetScore (1);
				}
				/*while (PhotonNetwork.player.GetTeam() == PunTeams.Team.none) {
					yield return null;
				}*/
				LoadScene (1);
			}
		}
    }
}
