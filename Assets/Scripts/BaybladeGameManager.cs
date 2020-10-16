using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class BaybladeGameManager : MonoBehaviourPunCallbacks {
  [Header("UI")]
  public GameObject UI_InformPanelGameObject;
  public TextMeshProUGUI UI_InformText;
  public GameObject searchForGamesButton;
  void Start() {
    UI_InformPanelGameObject.SetActive(true);
    UI_InformText.text = "Search for games to battle!";
  }

  void Update() {

  }

  #region UI Callback Methods
  public void JoinRandomRoom() {
    UI_InformText.text = "Searching for room...";
    PhotonNetwork.JoinRandomRoom();
  }

  #endregion
  
  #region PHOTON Callback Methods

  /**
   * Called when user failed to join random room
   */
  public override void OnJoinRandomFailed(short returnCode, string message) {
    Debug.Log(message);
    CreateAndJoinRoom();
  }

  public override void OnJoinedRoom() {
    base.OnJoinedRoom();
    UI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
    if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
      UI_InformText.text = UI_InformText.text + "\nWaiting for player to join...";
    } else {
      StartCoroutine(DeactivateAfterSeconds(UI_InformPanelGameObject, 2.0f));
    }
    Debug.Log(PhotonNetwork.NickName + " joined room: " + PhotonNetwork.CurrentRoom.Name);
  }

  public override void OnPlayerEnteredRoom(Player newPlayer) {
    base.OnPlayerEnteredRoom(newPlayer);
    UI_InformText.text = newPlayer.NickName + " has joined.";
    Debug.Log(newPlayer.NickName + " joined room: " + PhotonNetwork.CurrentRoom.Name + ", player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    StartCoroutine(DeactivateAfterSeconds(UI_InformPanelGameObject, 2.0f));
  }

  #endregion

  #region PRIVATE Methods
  void CreateAndJoinRoom() {
    string randomRoomName = "Room" + Random.Range(0, 10000);
    RoomOptions roomOptions = new RoomOptions();
    roomOptions.IsOpen = true;
    roomOptions.MaxPlayers = 2;
    PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
  }

  IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds) {
    searchForGamesButton.SetActive(false);
    yield return new WaitForSeconds(_seconds);
    _gameObject.SetActive(false);
  }

  #endregion
}
