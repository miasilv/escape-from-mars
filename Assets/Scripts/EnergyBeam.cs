using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBeam : MonoBehaviour
{
    private float speed = 35.0f;
    private float secondsAlive = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Shoot");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    IEnumerator Shoot() {
        yield return new WaitForSeconds(secondsAlive);
        Destroy(gameObject);
    }
}
