using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondtrollerLevel1 : MonoBehaviour {
    public float moveSpeed = 1f;
    public Rigidbody2D rigidBody;
    public LayerMask groundLayer=-1;
    public Animator anim;
    public int maxKeyNum = 1;
    private float distToGround;

    private bool isWalking = false;
    private bool isFacingRight;
    private bool isJumping;

    public float jumpForce = 12f;

    private Vector2 startPosition;
    private float killOffset = 2f;

    private int score = 0;
    private int keyNumber = 0;
    private int lives = 3;

    public Transform[] groundPoints;
    public float groundRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;

    public AudioClip coinSound;
    private AudioSource source;

    // Use this for initialization    
    void Start()
    {
        isFacingRight = true;
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        source.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        isGrounded = IsGrounded();
    }
    void Update()
    {
        isWalking = false;
        isJumping = false;
        if (GameManager.instance.currentGameState == GameManager.GameState.GS_GAME)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (!isFacingRight)
                    Flip();
                MoveRight();
                isWalking = true;
            }
            else
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.W))
            {
                if (isFacingRight)
                    Flip();
                isWalking = true;
                MoveLeft();
            }
            else
            {
                if (rigidBody.velocity.x > 0.01f)
                    rigidBody.velocity = new Vector2(0.95f * rigidBody.velocity.x, rigidBody.velocity.y);
                else
                {
                    if (rigidBody.velocity.x < 0.01f)
                        rigidBody.velocity = new Vector2(0.95f * rigidBody.velocity.x, rigidBody.velocity.y);
                    else
                        rigidBody.velocity = new Vector2(0f, 0f);
                }

            }
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
            {
                isWalking = false;
                isJumping = true;
                Jump();
            }
            if (isGrounded && isJumping)
            {
                isGrounded = false;
                isJumping = true;
            }
        }
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("IsJumping", isJumping);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score += 1;
            GameManager.instance.AddCoins(1);
            other.gameObject.SetActive(false);
            source.PlayOneShot(coinSound, AudioListener.volume);
        }
        else if (other.CompareTag("Meta"))
        {
            if (GameManager.instance.keysCompleted)
            {
                GameManager.instance.LevelCompleted();
            }

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
        else if(other.CompareTag("Fall"))
        {
            GameManager.instance.GameOver();
        }

       /* if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }*/
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
        if (rigidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }  
                }
            }
        }
        return false;
      
        /*if (Physics2D.Raycast(this.transform.position, Vector2.down, 2f, groundLayer.value))
            return true;
        return false;*/
    }

    void Jump()
    {
        if(IsGrounded())
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (rigidBody.velocity.x < moveSpeed)
        {
            rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.right * 0.1f, ForceMode2D.Impulse);
        }
    }

    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (rigidBody.velocity.x > -moveSpeed)
        {
            rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.left * 0.1f, ForceMode2D.Impulse);
        }
    }

}
