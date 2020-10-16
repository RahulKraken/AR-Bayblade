using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun {
  public TextMeshProUGUI playerNameText;
  void Start() {
    if (photonView.IsMine) {
      // Local player
      transform.GetComponent<MovementController>().enabled = true;
      transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
    } else {
      // Not me -> disable movement controller
      transform.GetComponent<MovementController>().enabled = false;
      transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
    }
    setPlayerName();
  }

  void setPlayerName() {
    if (playerNameText != null) {
      if (photonView.IsMine) {
        playerNameText.text = "YOU";
        playerNameText.color = Color.red;
      } else {
        playerNameText.text = photonView.Owner.NickName;
        playerNameText.color = Color.blue;
      }
    }
  }
}
