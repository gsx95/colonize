using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour
{

    private const float SPEED = 10.0f;

    void Update()
    {
        Vector3 p = GetTranslation();
        p *= Time.deltaTime;
        transform.Translate(p);
    }

    private Vector3 GetTranslation()
    {
        Vector3 p_Velocity = new Vector3();

        // Forwards
        if (Input.GetKey(KeyCode.W))
            p_Velocity += new Vector3(-1, 0, 1);

        // Backwards
        if (Input.GetKey(KeyCode.S))
            p_Velocity += new Vector3(1, 0, -1);

        // Left
        if (Input.GetKey(KeyCode.A))
            p_Velocity += new Vector3(-1, 0, -1);

        // Right
        if (Input.GetKey(KeyCode.D))
            p_Velocity += new Vector3(1, 0, 1);

        // Up
        if (Input.GetKey(KeyCode.Space))
            p_Velocity += new Vector3(1.4f, 2f, -1.4f);

        // Down
        if (Input.GetKey(KeyCode.LeftShift))
            p_Velocity += new Vector3(-1.4f, -2f, 1.4f);

        return p_Velocity * SPEED;
    }
}