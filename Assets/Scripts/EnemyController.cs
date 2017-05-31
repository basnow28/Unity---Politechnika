using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float xMin = 5.5f;
    public float xMax = 5.5f;
    public float moveSpeed = 2f;
    public Animator anim;

    private float StartPositionX;
    private bool isMovingRigth = true;
    private bool isFacingRight = true;
    private Rigidbody2D rigidBody;
    private float killOffset = 0.3f;


	// Use this for initialization
	void Awake() {
        StartPositionX = this.transform.position.x;
        this.transform.position = new Vector2(Random.Range(StartPositionX - xMin, StartPositionX + xMax), this.transform.position.y);
        rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(isMovingRigth)
        {
            if (this.transform.position.x < StartPositionX + xMax)
                MoveRigth();
            else
            {
                isMovingRigth = false;
                Flip();
                MoveLeft();
            }
        }
        else
        {
            if (this.transform.position.x > StartPositionX - xMin)
                MoveLeft();
            else
            {
                isMovingRigth = true;
                Flip();
                MoveRigth();
            }
        }
		
	}

    void MoveRigth()
    {
        if (rigidBody.velocity.x < moveSpeed)
        {
            rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.right * 0.6f, ForceMode2D.Impulse);
        }
    }

    void MoveLeft()
    {
        if (rigidBody.velocity.x > -moveSpeed)
        {
            rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.left * 0.6f, ForceMode2D.Impulse);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.7f);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.gameObject.transform.position.y > this.gameObject.transform.position.y + killOffset)
            {
                Debug.Log("Enemy dead");
                anim.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
            else
            {
                Debug.Log("Player killed");
            }
        }
    }
}
