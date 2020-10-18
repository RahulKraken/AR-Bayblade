using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Controls
 * - Battle mechanics
 * - Updates player health when they are hit
 */
public class BattleScript : MonoBehaviour {
  public Spinner spinnerScript;
  public Image spinSpeedBar_Image;
  public TextMeshProUGUI spinSpeedRatio_Text;
  public float commonDamageCoefficient = 0.04f;

  [Header("Player Type Damage Coefficients")]
  public float doDamageCoefficientAttacker = 10f; // ADV - do more damage
  public float getDamagedCoefficientAttacker = 1.2f; // DISADV - get more damage

  public float doDamageCoefficientDefender = 0.75f; // DISADV - do less damage
  public float getDamagedCoefficientDefender = 0.2f; // ADV - get less damage

  private float startSpinSpeed;
  private float currentSpinSpeed;

  private bool isAttacker;
  private bool isDefender;

  private void Awake() {
    startSpinSpeed = spinnerScript.spinSpeed;
    currentSpinSpeed = spinnerScript.spinSpeed;

    spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
  }

  private void CheckPlayerType() {
    isAttacker = false;
    isDefender = false;

    if (gameObject.name.Contains("Attacker")) {
      isAttacker = true;
    } else if (gameObject.name.Contains("Defender")) {
      isDefender = true;
      spinnerScript.spinSpeed = 4400;
      startSpinSpeed = spinnerScript.spinSpeed;
      currentSpinSpeed = spinnerScript.spinSpeed;
      spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;
    }
  }

  private void Start() {
    CheckPlayerType();
  }

  private void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.CompareTag("Player")) {
      // compare the speeds of the beyblade
      float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
      float otherSpeed = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
      Debug.Log("My speed: " + mySpeed + ", other speed: " + otherSpeed);
      
      if (mySpeed > otherSpeed) {
        // apply damage to slower player i.e, other player
        float defaultDamageAmount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600f * commonDamageCoefficient;
        if (isAttacker) {
          defaultDamageAmount *= doDamageCoefficientAttacker;
        } else if (isDefender) {
          defaultDamageAmount *= doDamageCoefficientDefender;
        }
        if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine) {
          collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, defaultDamageAmount);
        }
      }
    }
  }

  [PunRPC]
  public void DoDamage(float _damageAmount) {
    if (isAttacker) {
      _damageAmount *= getDamagedCoefficientAttacker;
    } else if (isDefender) {
      _damageAmount *= getDamagedCoefficientDefender;
    }
    spinnerScript.spinSpeed -= _damageAmount;
    currentSpinSpeed = spinnerScript.spinSpeed;

    spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    spinSpeedRatio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;
  }
}
