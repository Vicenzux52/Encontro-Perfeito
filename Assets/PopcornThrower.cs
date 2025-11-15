
using UnityEngine;
using UnityEngine.Video;

public class PopcornThrower : MonoBehaviour
{
    public VideoPlayer movie;
    public double movieTimeTrigger = 12;
    double movieTime;
    bool popcornThrowed = false;

    public Transform popcornSacks;
    //float popcornSacksInitialY;
    public float upAcceleration = 5;
    public float throwUpDownTime = 2;
    public float throwDownTime = 4;
    public float popcornSpawnTime = 0.75f;
    public float popcornSpawnTimer = 0;
    bool throwUp = false;
    bool throwDown = false;

    Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = transform.Find("Player");
        //popcornSacksInitialY = popcornSacks.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        movieTime = movie.time;
        if (movieTime >= movieTimeTrigger)
        {
            if (!popcornThrowed)
            {
                ThrowUp();
                popcornThrowed = true;
            }
        }
        else if (movieTime < 3)
        {
            popcornThrowed = false;
        }
        if (throwUp)
        {
            popcornSacks.position += Vector3.up * upAcceleration * Time.deltaTime;
            if (popcornSacks.position.y > 20)
            {
                popcornSacks.position -= Vector3.up * popcornSacks.position.y;
                throwUp = false;
            }
        }
        if (throwDown)
        {
            if (popcornSpawnTimer > popcornSpawnTime)
            {
                //instaciar as pipocas do mal
                popcornSpawnTimer = -Time.deltaTime;
            }
            popcornSpawnTimer += Time.deltaTime;
        }
    }

    void ThrowUp()
    {
        throwUp = true;
        Invoke("ThrowDown", throwUpDownTime);
    }

    void ThrowDown()
    {
        throwDown = true;
        Invoke("ThrowDownEnd", throwDownTime);
    }

    void ThrowDownEnd()
    {
        throwDown = false;
    }
}
