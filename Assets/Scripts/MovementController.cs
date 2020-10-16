using UnityEngine;

public class MovementController : MonoBehaviour {
  public Joystick joystick;
  public float speed = 4;
  public float maxVelocityChange = 4f;
  public float tiltAmount = 5f;

  private Vector3 velocityVector = Vector3.zero;
  private Rigidbody rb;
  void Start() {
    rb = GetComponent<Rigidbody>();
  }

  void Update() {
    float _xMovementInput = joystick.Horizontal;
    float _zMovementInput = joystick.Vertical;

    Vector3 _movementHorizontal = transform.right * _xMovementInput;
    Vector3 _movementVertical = transform.forward * _zMovementInput;
    Vector3 _movementVelocityVector = (_movementHorizontal + _movementVertical).normalized * speed;

    move(_movementVelocityVector);
    transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, -1 * joystick.Horizontal * speed * tiltAmount);
  }

  void move(Vector3 movementVelocityVector) {
    velocityVector = movementVelocityVector;
  }

  private void FixedUpdate() {
    if (velocityVector != Vector3.zero) {
      Vector3 velocity = rb.velocity;
      Vector3 velocityChange = velocityVector - velocity;

      velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
      velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
      velocityChange.y = 0f;

      rb.AddForce(velocityChange, ForceMode.Acceleration);
    }
  }
}
