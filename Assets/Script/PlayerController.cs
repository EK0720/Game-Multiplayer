using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun{
  private PlayerController standingOnPlayer;
  [HideInInspector]
  public int id;
  public float runSpeed = 0.6f; // Running speed.
  public float jumpForce = 2.6f; // Jump height.

  public Sprite jumpSprite; // Sprite that shows up when the character is not on the ground. [OPTIONAL]

  private Rigidbody2D body; // Variable for the RigidBody2D component.
  private SpriteRenderer sr; // Variable for the SpriteRenderer component.
  private Animator anima; // Variable for the Animator component. [OPTIONAL]

  private bool isGrounded; // Variable that will check if character is on the ground.
  public GameObject groundCheckPoint; // The object through which the isGrounded check is performed.
  public float groundCheckRadius; // isGrounded check radius.
  public LayerMask groundLayer; // Layer wich the character can jump on.

  private bool jumpPressed = false; // Variable that will check is "Space" key is pressed.
  private bool APressed = false; // Variable that will check is "A" key is pressed.
  private bool DPressed = false; // Variable that will check is "D" key is pressed.
  public Player photonPlayer;
  public static PlayerController me;
  void Awake() {
    body = GetComponent<Rigidbody2D>(); // Setting the RigidBody2D component.
    sr = GetComponent<SpriteRenderer>(); // Setting the SpriteRenderer component.
    anima = GetComponent<Animator>(); // Setting the Animator component. [OPTIONAL]
  }

  // Update() is called every frame.
  [PunRPC]
  public void Initialize (Player player) {
    id = player.ActorNumber;
    photonPlayer = player;
    // GameManager.instance.players [id - 1] = this;
  }
  private void Update() {
    if (!photonView.IsMine) 
        return;
    if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true; // Checking on "Space" key pressed.
    if (Input.GetKey(KeyCode.A)) APressed = true; // Checking on "A" key pressed.
    if (Input.GetKey(KeyCode.D)) DPressed = true; // Checking on "D" key pressed.
  }

  // Update using for physics calculations.
  [PunRPC]
  public void FixedUpdate() {
      isGrounded = Physics2D.OverlapCircle(groundCheckPoint.transform.position, groundCheckRadius, groundLayer); // Checking if character is on the ground.
      // Left/Right movement.
      if (APressed) {
          body.velocity = new Vector2(-runSpeed, body.velocity.y); // Move left physics.
          transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z); // Rotating the character object to the left.
          APressed = false; // Returning initial value.
          anima.SetBool("movement", true);
      }
      else if (DPressed) {
          body.velocity = new Vector2(runSpeed, body.velocity.y); // Move right physics.
          transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z); // Rotating the character object to the right.
          DPressed = false; // Returning initial value.
          anima.SetBool("movement", true);
      }
      else{
        body.velocity = new Vector2(0, body.velocity.y);
        anima.SetBool("movement", false);

      } 
      // Jumps.
      if (jumpPressed && (isGrounded || standingOnPlayer != null)) {
        if (isGrounded) {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        } else {
            body.AddForce((Vector2.up + Vector2.right * Mathf.Sign(standingOnPlayer.transform.localScale.x)) * jumpForce, ForceMode2D.Impulse);
            standingOnPlayer = null;
        }
        jumpPressed = false;
      }

      // Setting jump sprite. [OPTIONAL]
    //   if (!isGrounded) {
    //     anima.enabled = false; // Turning off animation.
    //     sr.sprite = jumpSprite; // Setting the sprite.
    //   }
    //   else anima.enabled = true; // Turning on animation.
  }
  void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.CompareTag("Player")) {
        standingOnPlayer = other.gameObject.GetComponent<PlayerController>();
    }
}
}