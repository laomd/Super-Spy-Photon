using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby 
{
    public class LobbyInfoPanel : MonoBehaviour
    {
		static LobbyInfoPanel _instance;
		public static LobbyInfoPanel instance {
			get {
				return _instance;
			}
		}

        public Text infoText, buttonText;
		public Button cancelButton, cancelMatchButton;
		Button curButton;

		void Start() {
			_instance = this;
			cancelButton.onClick.AddListener(SimpleClbk);
			cancelMatchButton.onClick.AddListener(CancelMatchClbk);
			SimpleClbk ();
		}

		void ChangeTo(Button other) {
			if (other != curButton) {
				if (curButton != null) {
					curButton.gameObject.SetActive(false);
				}
				curButton = other;
				curButton.gameObject.SetActive (true);
			}
		}

		public void BecomeMessageBox() {
			gameObject.SetActive (true);
			ChangeTo (cancelButton);
		}

		public void BecomeMatchBox() {
			gameObject.SetActive (true);
			ChangeTo (cancelMatchButton);
		}
	
		void SimpleClbk() {
			gameObject.SetActive (false);
		}

		void CancelMatchClbk() {
			SimpleClbk ();
			PhotonNetwork.LeaveRoom ();
		}
	
		public void Display(string info)
        {
            infoText.text = info;
			transform.SetAsLastSibling ();
        }
    }
}