using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 travelOffset = new Vector3(0f, 0f, 7f);
    public float speed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distance = travelOffset.magnitude;

        if (distance < 0.001f)
            return;

        float travelled = Mathf.PingPong(Time.time * speed, distance);

        float t = travelled / distance;

        transform.position = Vector3.Lerp(startPosition, startPosition + travelOffset, t);
    }
}
