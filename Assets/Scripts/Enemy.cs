using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private GameObject player;
    private Animator anim;
    private GameManager gameManager;
    [SerializeField] ParticleSystem explosionParticles;

    private float speedMin = 2.0f;
    private float speedMax = 5.0f;
    public float speed;
    private bool moving = true;
    
    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        gameManager = GameManager.Instance;
        anim = gameObject.GetComponentInChildren<Animator>();
        moving = true;
        speed = Random.Range(speedMin, speedMax);
    }

    // Update is called once per frame
    void Update() {
        if (gameManager.playing && moving) {
            transform.LookAt(player.GetComponent<Transform>());
            anim.SetBool("Walk Forward", true);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        } else {
            anim.SetBool("Walk Forward", false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Enemy collided with " + other.name);
        if (other.gameObject.CompareTag("Player")) {
            StartCoroutine(Stab(other.gameObject));
        } else if (other.gameObject.CompareTag("EnergyBeam")) {
            Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
            Destroy(other.gameObject);
            StartCoroutine(Die());
        }
    }

    IEnumerator Stab(GameObject player) {
        anim.SetBool("Walk Forward", false);
        anim.SetTrigger("Smash Attack");
        moving = false;
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Cast Spell");
    }

    IEnumerator Die() {
        anim.SetTrigger("Die");
        moving = false;
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1.16f);
        Destroy(gameObject);
    }

}
