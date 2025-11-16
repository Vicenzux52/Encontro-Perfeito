using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameController THIS;
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
}
