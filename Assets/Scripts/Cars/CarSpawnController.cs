using System.Collections.Generic;
using UnityEngine;

public class CarSpawnerController : MonoBehaviour
{
    public GameObject[] cars;
    public float timer, speed, time, timeVariation = 0.25f;
    float randomTimeOffset;
    public List<GameObject> carInstantiateL;
    public Transform carDestroyer;

    CameraHolder turning;

    public int i;

    void Start()
    {
        randomTimeOffset = Random.Range(-time * timeVariation, time * timeVariation); 
        carInstantiateL = new List<GameObject>();
        turning = FindFirstObjectByType<CameraHolder>();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (i == turning.cameraState) 
        {
//            Debug.Log("State " + turning.cameraState);
            Spawner();
            Movement();
            Clear();
        }
    }

    public void Spawner()
    {
        if (timer >= time + randomTimeOffset)
        {
            timer = 0;

            int randomI = Random.Range(0, cars.Length);
            GameObject randomCar = cars[randomI];

            GameObject carInstantiate = Instantiate(randomCar, this.transform.position, this.transform.rotation);
            carInstantiateL.Add(carInstantiate);
            randomTimeOffset = Random.Range(-time * timeVariation, time * timeVariation);
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
