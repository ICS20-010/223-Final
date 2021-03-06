using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private CharacterController cc;
  [SerializeField] private Animator anim;
  [SerializeField] private GameObject model;
  [SerializeField] private Camera cam;
  [SerializeField] private Transform footPos;
  [SerializeField] private PlayerData pData;

  public int jumpStaminaCost = 5;
  public int SprintStaminaCost = 1;

  private float speed = 8.0f; // speed of player's XZ movement 
  private float sprintSpeed = 14.0f;
  private float rotateToFaceMovementSpeed = 5.0f;
  // private float rotateToFaceAwayFromCameraSpeed = 5.0f;
  private float gravity = -9.81f;         // downward pull of gravity 
  private float yVelocity = 0.0f;                 // current Y velocity
  private float yVelocityWhenGrounded = -20.0f;   // Y velocity when grounded

  private float jumpHeight = 2.0f;    // jump height in units
  private float jumpTime = 0.5f;      // jump air time in seconds
  private float initialJumpVelocity;  // the upward velocity at start of jump
  private int maxJumps = 2;           // # of jumps the player can do (double jump)
  private int availableJumps;         // how many jumps are available
  private bool canWallJump = false;
  private Vector3 wallJumpNormal;

  void Start()
  {
    // calculate time to the top of the jump (need this for gravity calculation)
    float timeToApex = jumpTime / 2.0f;
    // calculate gravity
    gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
    // calculate velocity using (gravity)
    initialJumpVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
    // initialize jumps available
    availableJumps = maxJumps;

    // Hide cursor and stuff
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  // Update is called once per frame
  void Update()
  {
    // Determine XZ movement
    // =====================
    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    // convert from local to world coordinates.
    movement = transform.TransformDirection(movement);
    // ensure diagonal movement doesn't exceed horiz/vert movement speed
    movement = Vector3.ClampMagnitude(movement, 1.0f);
    // rotate model
    if (movement.magnitude > 0)
    {
      RotateModelToFaceMovement(movement);
      RotatePlayerToFaceAwayFromCamera();
    }
    // Update animation
    anim.SetFloat("Velocity", movement.magnitude);

    // determine XZ movement speed
    if (Input.GetKey("left shift") && movement.magnitude > 0 && pData.playerAttributes.stamina > 1)
    {

      anim.SetBool("isSprinting", true);
      movement *= sprintSpeed;
      Messenger<int>.Broadcast(GameEvents.STAMINA_CONSUMED, SprintStaminaCost);
    }
    else
    {
      anim.SetBool("isSprinting", false);
      movement *= speed;
    }
    // Determine Y movement (based on gravity [g])
    // ===========================================
    // Formula for instantaneous velocity [v] of a falling object after elapsed time [t]:
    // v = gt (from https://en.wikipedia.org/wiki/Equations_for_a_falling_body)
    yVelocity += gravity * Time.deltaTime;

    // if we are on the ground and we were falling
    if (cc.isGrounded && yVelocity < 0.0)
    {
      anim.SetBool("isGrounded", true);
      canWallJump = false;
      wallJumpNormal = Vector3.zero;
      // set downward velocity to something constant.
      yVelocity = yVelocityWhenGrounded;
      availableJumps = maxJumps;
    }

    // if jump is pressed and a jump is available, then jump!
    if (Input.GetButtonDown("Jump") && availableJumps > 0 && !canWallJump && pData.playerAttributes.stamina >= jumpStaminaCost)
    {
      Messenger<int>.Broadcast(GameEvents.STAMINA_CONSUMED, jumpStaminaCost);
      anim.SetBool("isGrounded", false);
      anim.SetTrigger("Jump");
      yVelocity = initialJumpVelocity;
      availableJumps--;
    }
    else if (Input.GetButtonDown("Jump") && !cc.isGrounded && canWallJump && pData.playerAttributes.stamina >= jumpStaminaCost)
    {
      yVelocity = initialJumpVelocity;
      // rotate model
      model.transform.rotation = Quaternion.LookRotation(new Vector3(wallJumpNormal.x, 0f, wallJumpNormal.z));
      Messenger<int>.Broadcast(GameEvents.STAMINA_CONSUMED, jumpStaminaCost);
      anim.SetBool("isGrounded", false);
      anim.SetTrigger("Jump");
      canWallJump = false;
    }

    if (!cc.isGrounded && wallJumpNormal != Vector3.zero)
    {
      movement.x *= wallJumpNormal.x;
      movement.z *= wallJumpNormal.z;

      model.transform.rotation = Quaternion.LookRotation(new Vector3(wallJumpNormal.x, 0f, wallJumpNormal.z));
    }
    // Make our yVelocity part of the movement vector.
    movement.y = yVelocity;

    // Move the player!  (via the CharacterController)
    // ===============================================
    cc.Move(movement * Time.deltaTime);

    // broadcast to update the timer
    Messenger.Broadcast(GameEvents.TIMER_UPDATE);

  }

  private void OnControllerColliderHit(ControllerColliderHit hit)
  {

    if (!cc.isGrounded && hit.transform.tag == "Wall")
    {
      wallJumpNormal = hit.normal;
      canWallJump = true;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    switch (other.tag)
    {
      case "Exit":
        {
          exitTrigger exit = other.GetComponent<exitTrigger>();
          if (exit.exitLocked && pData.hasKey())
          {
            Messenger.Broadcast(GameEvents.KEY_USED);
            Messenger.Broadcast(GameEvents.EXIT_OPENED);
          }
          if (!exit.exitLocked)
          {
            Messenger<int>.Broadcast(GameEvents.EXIT_ENTERED, exit.exitId);
          }
          break;
        }
      case "Projectile":
        {
          projectileShot pShot = other.GetComponent<projectileShot>();
          Messenger<int>.Broadcast(GameEvents.HEALTH_CONSUMED, pShot.damage);
          Destroy(pShot.gameObject);
          break;
        }
    }
  }

  private void RotateModelToFaceMovement(Vector3 moveDirection)
  {
    Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));

    model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, rotateToFaceMovementSpeed * Time.deltaTime);
  }

  private void RotatePlayerToFaceAwayFromCamera()
  {
    Quaternion camRotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
    transform.rotation = camRotation;
    // transform.rotation = Quaternion.Slerp(transform.rotation, camRotation, rotateToFaceAwayFromCameraSpeed * Time.deltaTime);
  }
}
