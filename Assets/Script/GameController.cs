using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameController THIS;
    GameObject player;
    static public float Timer = 180;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FrontalMovement();
    }

    void FrontalMovement()
    {
        if (!player.GetComponent<Player>().isDelayed)
        {
            transform.position += new Vector3(0, 0, player.GetComponent<Player>().frontSpeed * Time.deltaTime);
        }
        else
        {
            transform.position += new Vector3(0, 0, player.GetComponent<Player>().frontSpeed * Time.deltaTime * player.GetComponent<Player>().delaySpeed);
        }
    }
}
