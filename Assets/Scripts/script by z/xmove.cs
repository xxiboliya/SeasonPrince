using UnityEngine;

public class xmove : MonoBehaviour
{
    // The speed of the horizontal movement.
    public float speed = 2f;

    // The total distance the object will move back and forth.
    public float distance = 5f;

    private float startPositionX;

    void Start()
    {
        // Store the starting X position.
        startPositionX = transform.position.x;
    }

    void Update()
    {
        // Calculate the new X position using a sine wave for smooth oscillation.
        // Mathf.Sin returns a value between -1 and 1.
        // We multiply it by distance / 2 to set the amplitude of the movement.
        float movement = Mathf.Sin(Time.time * speed) * (distance / 2);
        
        // Update the object's position.
        transform.position = new Vector3(startPositionX + movement, transform.position.y, transform.position.z);
    }
}
