using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour {

    public float xMin = 5.5f;
    public float xMax = 5.5f;
    public float moveSpeed = 5f;
    private bool isMovingRigth = true;

    private float StartPositionX;
    private Rigidbody2D rigidBody;


    // Use this for initialization
    void Awake()
    {
        StartPositionX = this.transform.position.x;
        this.transform.position = new Vector2(Random.Range(StartPositionX - xMin, StartPositionX + xMax), this.transform.position.y);
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMovingRigth)
        {
            if (this.transform.position.x < StartPositionX + xMax)
                MoveRigth();
            else
            {
                isMovingRigth = false;
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
}
