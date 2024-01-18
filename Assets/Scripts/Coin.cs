using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public AudioSource coinPickupSound;
    public Collider coll;

    private void Start()
    {
        
    }
    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.CoinPickup();
            coinPickupSound.Play();
            gameObject.SetActive(false);
        }
    }

}
