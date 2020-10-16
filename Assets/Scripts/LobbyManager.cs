using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks {

  [Header("Login UI")]
  public InputField playerNameInputField;
  public GameObject UI_LoginGameObject;

  [Header("Lobby UI")]
  public GameObject UI_LobbyGameObject;
  public GameObject UI_3DGameObject;

  [Header("Connection Status UI")]
  public GameObject UI_ConnectionStatusGameObject;
  public Text connectionStatusText;
  public bool showConnectionStatus = false;

  #region UNITY Methods
  void Start() {
    UI_LobbyGameObject.SetActive(false);
    UI_3DGameObject.SetActive(false);
    UI_ConnectionStatusGameObject.SetActive(false);
    UI_LoginGameObject.SetActive(false);

    if (PhotonNetwork.IsConnected) {
      UI_LobbyGameObject.SetActive(true);
      UI_3DGameObject.SetActive(true);
    } else {
      UI_LoginGameObject.SetActive(true);
    }
  }

  void Update() {
    if (showConnectionStatus) connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;
  }

  #endregion

  #region UI Callback Methods
  public void OnEnterGameButtonClicked() {
    // connect
    string playerName = playerNameInputField.text;
    if (!string.IsNullOrEmpty(playerName)) {
      if (!PhotonNetwork.IsConnected) {
        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();

        // change UI
        UI_LobbyGameObject.SetActive(false);
        UI_3DGameObject.SetActive(false);
        UI_LoginGameObject.SetActive(false);

        UI_ConnectionStatusGameObject.SetActive(true);
        showConnectionStatus = true;
      }
    } else {
      Debug.Log("Enter a name plis!!");
    }
  }

  public void OnQuickMatchButtonClicked() {
    SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
  }

  #endregion

  #region PHOTON Callback Methods
  public override void OnConnected() {
    base.OnConnected();
    Debug.Log("Connected to Internet");
  }

  public override void OnConnectedToMaster() {
    base.OnConnectedToMaster();
    Debug.Log(PhotonNetwork.LocalPlayer.NickName + ": connected to photon");

    // change UI
    UI_LoginGameObject.SetActive(false);
    UI_ConnectionStatusGameObject.SetActive(false);

    UI_LobbyGameObject.SetActive(true);
    UI_3DGameObject.SetActive(true);
  }

  #endregion
}
