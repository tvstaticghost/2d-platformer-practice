using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using JetBrains.Annotations;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

//Red - 7, Blue - 8, Green - 9

public class PlayerScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 10f;
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    private LayerMask groundLayers;
    public int maxJumps = 2;
    int jumpsRemaining;

    private string currentPlayerTag;

    float horizontalMovement;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(191, 56, 53, 255);
        currentPlayerTag = gameObject.tag;
        groundLayers = LayerMask.GetMask("Ground");
        UpdateBlockColliders();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        isGrounded();
    }

    // I Need to add a brief cooldown or come up with a solution to stop rapid color changing when pressing the key once.
    public void ChangePlayerColor()
    {
        switch (gameObject.tag)
        {
            case "Red":
                gameObject.tag = "Blue";
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(90, 110, 225, 255);
                break;
            case "Blue":
                gameObject.tag = "Green";
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(107, 190, 48, 255);
                break;
            default:
                gameObject.tag = "Red";
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(191, 56, 53, 255);
                break;
        }

        Debug.Log("Player Tag: " + gameObject.tag);
        UpdateBlockColliders();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            return;
        }

        if (collision.gameObject.CompareTag(gameObject.tag))
        {
            Debug.Log("Match!");
        }
        else
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            Debug.Log("No Match!");
        }
    }

    private void UpdateBlockColliders()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Red").Concat(GameObject.FindGameObjectsWithTag("Blue")).Concat(GameObject.FindGameObjectsWithTag("Green")).ToArray();

        foreach (GameObject block in blocks)
        {
            if (block.TryGetComponent<Collider2D>(out var col))
            {
                col.enabled = block.CompareTag(gameObject.tag); 
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
                jumpsRemaining--;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
            }
        }
    }

    private void isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayers))
        {
            jumpsRemaining = maxJumps;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
