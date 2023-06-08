using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType {Energy, Tool};

public class Powerup : MonoBehaviour {
    
    [SerializeField] PowerupType type;
    [SerializeField] ParticleSystem explosionParticles;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start() {
        gameManager = GameManager.Instance; 
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Powerup collided with " + other.name);
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Player has collided with a " + type);
            Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
            Destroy(gameObject);
            gameManager.Powerup(type);
        }
    }
}
