using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour {
  public Transform playerSwitcherTransform;
  public Button next_button;
  public Button prev_button;
  public int playerSelectionNumber;
  public GameObject[] spinnerTopModels;

  [Header("UI")]
  public TextMeshProUGUI playerModelType_Text;
  public GameObject toBattleScene;
  public GameObject selectScene;

  #region UNITY Methods
  void Start() {
    playerSelectionNumber = 0;
  }

  void Update() {

  }
  #endregion

  #region UI Callback Methods
  public void NextPlayer() {
    next_button.enabled = false;
    prev_button.enabled = false;
    playerSelectionNumber = (playerSelectionNumber + 1) % 4;
    StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));
    setPlayerType();
  }

  public void PreviousPlayer() {
    next_button.enabled = false;
    prev_button.enabled = false;
    playerSelectionNumber = (playerSelectionNumber - 1 + 4) % 4;
    StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));
    setPlayerType();
  }

  public void setPlayerType() {
    if (playerSelectionNumber == 0 || playerSelectionNumber == 1) {
      // This means the player model is of type attack
      playerModelType_Text.text = "Attack";
    } else {
      playerModelType_Text.text = "Defend";
    }
    Debug.Log(playerModelType_Text);
  }

  public void OnSelectButtonClicked() {
    ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable {
      {MultiplayerARBaybladeGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber }
    };
    PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    selectScene.SetActive(false);
    toBattleScene.SetActive(true);
  }

  public void onBackButtonClicked() {
    SceneLoader.Instance.LoadScene("Scene_Lobby");
  }

  public void onBattleButtonClicked() {
    SceneLoader.Instance.LoadScene("Scene_Gameplay");
  }

  public void onReSelectButtonClicked() {
    toBattleScene.SetActive(false);
    selectScene.SetActive(true);
  }

  #endregion

  #region Private Methods
  IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f) {
    Quaternion originalRotation = transformToRotate.rotation;
    Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);
    float elapsedTime = 0.0f;
    while (elapsedTime < duration) {
      transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    next_button.enabled = true;
    prev_button.enabled = true;
    transformToRotate.rotation = finalRotation;
  }

  #endregion
}
