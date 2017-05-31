using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformDissapear : MonoBehaviour {

    // Use this for initialization
    public Rigidbody2D platform;
	// Update is called once per frame
    void Start()
    {
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(Wait());
            platform.isKinematic = false;
        }
        Debug.Log("Heeejo!");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
    }
}
