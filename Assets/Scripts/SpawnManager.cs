using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviourPunCallbacks {
  public GameObject[] playerPrefabs;
  public Transform[] spawnPositions;

  void Start() {

  }

  void Update() {

  }

  #region PHOTON Callbacks
  public override void OnJoinedRoom() {
    Debug.Log("Hello");
    if (PhotonNetwork.IsConnectedAndReady) {
      object playerSelectionNumber;
      if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARBaybladeGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber)) {
        Debug.Log("Spawning: " + (int)playerSelectionNumber);
        int randomSpawnPoint = Random.Range(0, spawnPositions.Length - 1);
        Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].position;
        PhotonNetwork.Instantiate(playerPrefabs[(int)playerSelectionNumber].name, instantiatePosition, Quaternion.identity);
      }
    }
  }

  #endregion
}
