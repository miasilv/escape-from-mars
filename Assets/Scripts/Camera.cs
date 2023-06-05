using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    private GameObject[] cameras;    
    private GameObject player;

    private int currentCamIndex = 0;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        cameras = GameObject.FindGameObjectsWithTag("Camera");
        
        currentCamIndex = 0;
        cameras[currentCamIndex].gameObject.SetActive(true);
        for (int i = 1; i < cameras.Length; i++) {
            cameras[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        // rotate focal point with player
        transform.rotation = player.transform.rotation;
        
        // move focal point with player
        transform.position = player.transform.position;

        if (Input.GetKeyDown(KeyCode.C)) {
            SwitchCamera();
        }
    }

    // switches the camera to the next camera in the array
    void SwitchCamera() {
        cameras[currentCamIndex].gameObject.SetActive(false);
        currentCamIndex = (currentCamIndex + 1) % cameras.Length;
        cameras[currentCamIndex].gameObject.SetActive(true);
    }
}
