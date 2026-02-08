using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidbody2D;
    private void Awake()
    {
        landerRigidbody2D=GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            float force = 700f;
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            float turnSpeed=+100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            float turnSpeed = -100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
    }
}
