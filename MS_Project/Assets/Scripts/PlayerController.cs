using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody thisRigidbody;

    UnityEngine.Vector2 moveInput;
    public float moveSpeed = 1;
    public float jumpForce = 3;

    public LayerMask terrainLayer;
    public Transform groundCheck;
    public bool isGrounded;

    // 
    private Transform sprite;
    private SpriteRenderer spriteRenderer;
    private Animator spriteAnim;
    private Animator playerFlip;

    // 
    private Transform mainCamera;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
    Direction CurrentDirection()
    {
        if (moveInput.x > 0 && moveInput.y > 0)
        {
            return Direction.UpRight;
        }
        else if (moveInput.x > 0 && moveInput.y < 0)
        {
            return Direction.DownRight;
        }
        else if (moveInput.x < 0 && moveInput.y > 0)
        {
            return Direction.UpLeft;
        }
        else if (moveInput.x < 0 && moveInput.y < 0)
        {
            return Direction.DownLeft;
        }
        else if (moveInput.x > 0)
        {
            return Direction.Right;
        }
        else if (moveInput.x < 0)
        {
            return Direction.Left;
        }
        else if (moveInput.y > 0)
        {
            return Direction.Up;
        }
        else if (moveInput.y < 0)
        {
            return Direction.Down;
        }
        else
        {
            return Direction.Down;
        }
    }

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").gameObject.transform;

        terrainLayer = LayerMask.GetMask("Terrain");

        thisRigidbody = GetComponent<Rigidbody>();

        playerFlip = GetComponent<Animator>();

        sprite = gameObject.transform.GetChild(0);
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        
        spriteAnim = sprite.GetComponent<Animator>();

        groundCheck = gameObject.transform.GetChild(1).gameObject.transform;
        if (Debug.isDebugBuild)
            Debug.Log(gameObject.transform.GetChild(1).gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();
        if (Debug.isDebugBuild)
            Debug.Log("Movement: " + moveInput);

        
        spriteAnim.SetFloat("MoveSpeed", thisRigidbody.velocity.magnitude);

        if (thisRigidbody.velocity.magnitude > 0)
        {
            spriteRenderer.flipX = false;
            if (CurrentDirection() == Direction.Up)
            {
                spriteAnim.Play("WalkUp");
            }
            else if (CurrentDirection() == Direction.Down)
            {
                spriteAnim.Play("WalkDown");
            }
            else if (CurrentDirection() == Direction.Left)
            {
                spriteRenderer.flipX = true;
                spriteAnim.Play("WalkRight");
            }
            else if (CurrentDirection() == Direction.Right)
            {
                spriteAnim.Play("WalkRight");
            }
            else if (CurrentDirection() == Direction.UpLeft)
            {
                spriteRenderer.flipX = true;
                spriteAnim.Play("WalkUpRight");
            }
            else if (CurrentDirection() == Direction.UpRight)
            {
                spriteAnim.Play("WalkUpRight");
            }
            else if (CurrentDirection() == Direction.DownLeft)
            {
                spriteRenderer.flipX = true;
                spriteAnim.Play("WalkDownRight");
            }
            else if (CurrentDirection() == Direction.DownRight)
            {
                spriteAnim.Play("WalkDownRight");
            }
        }

        transform.forward = mainCamera.forward;
    }

    private void FixedUpdate()
    {
        thisRigidbody.velocity = new UnityEngine.Vector3(moveInput.x * moveSpeed, thisRigidbody.velocity.y, moveInput.y * moveSpeed);

        RaycastHit raycastHit;
        if (Physics.Raycast(groundCheck.position, UnityEngine.Vector3.down, out raycastHit, 0.1f, terrainLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        Debug.DrawRay(groundCheck.position, UnityEngine.Vector2.down, Color.red);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //canJump = true;
            //thisRigidbody.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);
            thisRigidbody.velocity += new UnityEngine.Vector3(0f, jumpForce, 0f);
        }
    }


}
