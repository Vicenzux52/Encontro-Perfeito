using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public Transform directionalLight;
    public int rotationTime;
    public Material skyboxRotation, skyboxNight;
    private float seconds, multiplier;
    private bool isNight = false;

    void Start()
    {
        multiplier = 86400 / rotationTime;

        if (skyboxRotation != null)
        {
            RenderSettings.skybox = skyboxRotation;
        }
    }

    void Update()
    {
        seconds += Time.deltaTime * multiplier;

        if (seconds >= 86400)
        {
            seconds = 0;
        }

        SkyboxProcessing();
    }

    private void SkyboxProcessing()
    {
        float rotationX = Mathf.Lerp(0, 270, seconds / 86400);
        directionalLight.rotation = Quaternion.Euler(rotationX, 0, 0);

        if (rotationX >= 200 && !isNight)
        {
            RenderSettings.skybox = skyboxNight;
            DynamicGI.UpdateEnvironment();
            isNight = true;
        }

        if (rotationX <= 0 && isNight)
        {
            RenderSettings.skybox = skyboxRotation;
            DynamicGI.UpdateEnvironment();
            isNight = false;
        }
    }
}
