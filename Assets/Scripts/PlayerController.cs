using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Animator anim;

    private float speed = 10.0f;
    private float rotationSpeed = 100.0f;
    private float xBound = 25.0f;
    private float zBound = 25.0f;

    // Start is called before the first frame update
    void Start() {
		anim = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        MovePlayer();
    }

    // Move the player
    void MovePlayer() {
        // get the user input and move the player
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime);

        // animate the player if moving
        if (verticalInput != 0 || horizontalInput != 0) {
            anim.SetInteger("AnimationPar", 1);
        } else {
            anim.SetInteger("AnimationPar", 0);
        }

        // constrain the players position
        if (transform.position.z < -zBound) {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        } 
        else if (transform.position.z > zBound) {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }

        if (transform.position.x < -xBound) {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        } 
        else if (transform.position.x > xBound) {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Player collided with " + other.name);
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("Player has collided with an enemy");
        }
    }
}
