using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameController THIS;
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
}
