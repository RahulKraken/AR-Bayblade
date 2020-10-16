using UnityEngine;
using Photon.Pun;

public class NetworkSynchronization : MonoBehaviour, IPunObservable {
  Rigidbody rb;
  PhotonView photonView;

  Vector3 networkedPosition;
  Quaternion networkedRotation;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
    photonView = GetComponent<PhotonView>();

    networkedPosition = new Vector3();
    networkedRotation = new Quaternion();
  }

  void Start() {

  }

  void Update() {

  }

  private void FixedUpdate() {
    if (!photonView.IsMine) {
      rb.position = Vector3.MoveTowards(rb.position, networkedPosition, Time.fixedDeltaTime);
      rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, Time.fixedDeltaTime * 100);
    }
  }

  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    if (stream.IsWriting) {
      // My GameObject, I need to write info realted to it
      // Send data to other players
      stream.SendNext(rb.position);
      stream.SendNext(rb.rotation);
    } else {
      // Not my GameObject, read and update
      networkedPosition = (Vector3)stream.ReceiveNext();
      networkedRotation = (Quaternion)stream.ReceiveNext();
    }
  }
}
