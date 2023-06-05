using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private GameObject player;
    private Animator anim;

    private float speedMin = 2.0f;
    private float speedMax = 5.0f;
    private float speed;
    
    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        anim = gameObject.GetComponentInChildren<Animator>();

        speed = Random.Range(speedMin, speedMax);
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(player.GetComponent<Transform>());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

}
