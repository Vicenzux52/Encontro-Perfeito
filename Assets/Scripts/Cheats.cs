using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    public GameObject player;
    public GameObject instructions;
    void Start()
    {
        instructions.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            instructions.SetActive(!instructions.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(0);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.GetComponent<Collider>().enabled ^= player.GetComponent<Collider>().enabled;
        }
    }
}
