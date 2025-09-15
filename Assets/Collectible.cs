
using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string collectibleName;
    void Start()
    {
        if (collectibleName == null) Debug.LogError("Coletavel sem nome");
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.THIS.Collect(collectibleName);
            Destroy(gameObject);
        }
    }
}
