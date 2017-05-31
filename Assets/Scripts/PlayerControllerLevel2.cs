using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevel2 : MonoBehaviour {

    public float moveSpeed = 1f;
    public Rigidbody2D rigidBody;
    public LayerMask groundLayer = -1;
    public Animator anim;
    private float distToGround;

    private bool isWalking = false;
    private bool isFacingRight;
    private bool isJumping;

    public float jumpForce = 12f;

    private Vector2 startPosition;
    private float killOffset = 1f;

    private int score = 0;
    private int keyNumber = 0;
    private int lives = 3;

    // Use this for initialization    
    void Start()
    {
        isFacingRight = true;
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }
    void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.GS_GAME)
        {
            isWalking = true;
            if (rigidBody.velocity.x < moveSpeed)
            {
                rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            }

            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
            {
                Jump();
            }
            anim.SetBool("IsGrounded", IsGrounded());
            anim.SetBool("IsWalking", isWalking);
        }
        else
        {
            isWalking = false;
            if (rigidBody.velocity.x > 0)
                rigidBody.velocity = new Vector2(Mathf.Max(0f, rigidBody.velocity.x - moveSpeed), rigidBody.velocity.y);
            else if (rigidBody.velocity.x < 0)
                rigidBody.velocity = new Vector2(Mathf.Min(0f, rigidBody.velocity.x - moveSpeed), rigidBody.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score += 1;
            GameManager.instance.AddCoins(1);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Meta"))
        {
            GameManager.instance.LevelCompleted();
        }
        else if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.transform.position.y + killOffset < this.transform.position.y)
            {
                score += 10;
                GameManager.instance.AddCoins(10);
                GameManager.instance.AddEnemies(1);
                print("Enemy killed!  Score : " + score);
            }
            else
            {
                GameManager.instance.DelHearts(1);
                lives -= 1;
                if (lives <= 0)
                    print("Game over");
                this.transform.position = startPosition;
            }
        }
        else if (other.CompareTag("Key"))
        {
            GameManager.instance.AddKeys(1);
            keyNumber += 1;
            print("Keys : " + keyNumber);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Heart"))
        {
            GameManager.instance.AddHearts(1);
            lives += 1;
            print("Lives : " + keyNumber);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Fall"))
        {
            GameManager.instance.GameOver();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    bool IsGrounded()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 2f, groundLayer.value))
            return true;
        return false;
    }

    void Jump()
    {
        if (IsGrounded())
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

}
