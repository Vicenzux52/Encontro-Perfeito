using System.Collections.Generic;
using UnityEngine;

public class CarSpawnerController : MonoBehaviour
{
    public GameObject[] cars;
    public float timer, speed, time;
    public List<GameObject> carInstantiateL;
    public Transform carDestroyer;
    public int i;

    void Start()
    {
        carInstantiateL = new List<GameObject>();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        Spawner();
        Movement();
        Clear();
    }

    public void Spawner()
    {
        if (timer >= time) 
        {
            timer = 0;

            int randomI = Random.Range(0, cars.Length);
            GameObject randomCar = cars[randomI];

            GameObject carInstantiate = Instantiate(randomCar, this.transform.position, this.transform.rotation);
            carInstantiateL.Add(carInstantiate);
        }
    }

    public void Movement() 
    {
        for (int i = 0; i < carInstantiateL.Count; i++)
        {
            if (carInstantiateL[i] != null) 
            {
                carInstantiateL[i].transform.position = Vector3.MoveTowards(carInstantiateL[i].transform.position, carDestroyer.position, speed * Time.deltaTime);
            }
        }
    }

    public void Clear() 
    {
        for (int i = carInstantiateL.Count - 1; i >= 0; i--)
        {
            if (carInstantiateL[i] == null)
            {
                carInstantiateL.RemoveAt(i);
            }
            else if (Vector3.Distance(carInstantiateL[i].transform.position, carDestroyer.position) < 0.1f)
            {
                Destroy(carInstantiateL[i]);
            }
        }
    }
}
