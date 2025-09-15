using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController THIS;
    GameObject player;
    string[] collectibles;
    int collectIndice;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (GameObject.FindGameObjectsWithTag("Player").Length > 0) collectibles = new string[GameObject.FindGameObjectsWithTag("Player").Length];
        collectIndice = 0;
    }

    private void Awake()
    {
        if (THIS == null)
        {
            THIS = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        FrontalMovement();
        if (player.GetComponent<Player>().screenShake) Camera.main.GetComponent<CameraEffects>().ScreenShake(player.GetComponent<Player>().screenShakeForce);
        else Camera.main.GetComponent<CameraEffects>().ResetPosition();
    }

    void FrontalMovement()
    {
        if (!player.GetComponent<Player>().isDelayed)
        {
            transform.position += new Vector3(0, 0, player.GetComponent<Player>().frontSpeed * Time.deltaTime * player.GetComponent<Player>().others);
        }
        else
        {
            transform.position += new Vector3(0, 0, player.GetComponent<Player>().frontSpeed * Time.deltaTime * player.GetComponent<Player>().delaySpeed * player.GetComponent<Player>().others);
        }
    }

    public void Collect(string item)
    {
        collectibles[collectIndice] = item;
        collectIndice++;
        for (int i = 0; i < collectibles.Length; i++)
        {
            Debug.Log("Objetos coletados: " + collectibles[i]);

        }
    }
}
