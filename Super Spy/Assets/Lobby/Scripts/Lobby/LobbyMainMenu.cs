using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
		public void OnClickRanDomJoin(int maxPlayers) {
			if (!PhotonNetwork.connected) {
				ManagerBase.ConnectToMaster ();
			}
			ManagerBase.maxPlayers = maxPlayers;
			PhotonNetwork.JoinRandomRoom (null, (byte)maxPlayers);
		}
    }
}
