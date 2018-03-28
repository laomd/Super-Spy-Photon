using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using DG.Tweening;

public class GameSceneManager : ManagerBase {
	public static GameObject localHero;
	public static GameSceneManager instance;
	public GameObject spyKind;
	public UnityEngine.UI.Image gameImage, disablePanel;
	public GameObject[] towerGroups;
	Dictionary<PunTeams.Team, HashSet<string>> teamOccupiedTowers = new Dictionary<PunTeams.Team, HashSet<string>>();
	public override void Awake ()
	{
		base.Awake ();
		instance = this;
		teamOccupiedTowers [PunTeams.Team.red] = new HashSet<string> ();
		teamOccupiedTowers [PunTeams.Team.blue] = new HashSet<string> ();
		teamOccupiedTowers [PunTeams.Team.none] = null;
	}

	public override void Start()
	{
		base.Start ();
		var joystick = GetComponentInChildren<ETCJoystick> ();
		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue) {
			GameSceneManager.instance.GetComponentInChildren<PolygonCollider2D>().transform.rotation = Quaternion.Euler (0, 0, 180);
			joystick.TurnAndMove = -1;
			if (joystick.followOffset.z > 0) {
				joystick.followOffset.z *= -1;
			}
			joystick.followOffset.z *= -1;
			GameSceneManager.instance.GetComponentInChildren<MiniMapCameraManager> ().flag = -1;
		}
		GeneratePlayer ();
		foreach (var item in GetComponentsInChildren<SkillEffect>()) {
			item.SetSkillImage (LobbyPlayer.spriteToGamePlayer[LobbyPlayer.localLobbyPlayer.heroInfo.sprite.name]);
		}
		ShowMoney.instance.SetCharacter (localHero);
		joystick.axisX.directTransform = joystick.axisY.directTransform = joystick.cameraLookAt = localHero.transform;

		if (PhotonNetwork.isMasterClient) {
			ChooseTowers ();
		}

		disablePanel.gameObject.SetActive (false);
	}

	public override void OnJoinedRoom ()
	{
		base.OnJoinedRoom ();
		disablePanel.gameObject.SetActive (false);
	}

	public override void OnDisconnectedFromPhoton ()
	{
		base.OnDisconnectedFromPhoton ();
		if (maxPlayers == 1) {
			OnGameOver (PunTeams.Team.none);
		} else {
			disablePanel.gameObject.SetActive (true);
			PhotonNetwork.ReconnectAndRejoin ();
		}
	}

	void GeneratePlayer() {
		string playerName = LobbyPlayer.spriteToGamePlayer[PhotonNetwork.player.NickName];
		localHero = PhotonNetwork.Instantiate (playerName, Vector3.zero, Quaternion.identity, 0);
		localHero.PropertyComponent ().myTeam = PhotonNetwork.player.GetTeam ();
	}

	void ChooseTowers() {
		foreach (var item in towerGroups) {
			TowerProperty[] tps = item.GetComponentsInChildren<TowerProperty> ();
			for (int i = 0; i < 2; i++) {
				int j;
				do {
					j = Random.Range (0, tps.Length);
				} while (tps[j].isChoosed);
				tps [j].OnChoosed (true);
			}
		}
	}

	public void OnTowerTeamChanged(string tower, PunTeams.Team oldTeam, PunTeams.Team newTeam) 
	{
		if (teamOccupiedTowers[oldTeam] != null) {
			teamOccupiedTowers [oldTeam].Remove (tower);
		}
		if (teamOccupiedTowers[newTeam] != null) {
			teamOccupiedTowers [newTeam].Add (tower);
			if (teamOccupiedTowers[newTeam].Count >= 4) {
				OnGameOver (newTeam);
			}
		}
	}

	void OnGUI() {
	
	}

	void OnGameOver(PunTeams.Team victory) {
		DayNightController.instance.enabled = false;
		foreach (var item in GameObject.FindObjectsOfType<Property>()) {
			item.enabled = false;
		}
		if (PhotonNetwork.player.GetTeam() == victory || (localHero.PropertyComponent() as HeroProperty).isSpy) {
			gameImage.sprite = Resources.Load<Sprite> ("victory");
		} else {
			gameImage.sprite = Resources.Load<Sprite> ("defeat");
		}
		gameImage.transform.parent.gameObject.SetActive (true);
		DOTween.To (() => gameImage.transform.localScale, (x) => gameImage.transform.localScale = x, new Vector3 (1.2f, 1.2f, 1.2f), 1.5f);
	}

	protected override void OnExit ()
	{
		base.OnExit ();
		PhotonNetwork.Destroy (localHero);
		LoadScene (0);
	}
}
