using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    Vector3 originalPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = transform.localPosition;
    }

    // Update is called once per frame
    public void ScreenShake(float force)
    {
        Vector2 randomCircle = Random.insideUnitCircle * force;
        transform.localPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + originalPos;
    }

    public void ResetPosition()
    {
        transform.localPosition = originalPos;
    }
}
