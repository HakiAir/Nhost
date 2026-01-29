using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4.5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        float x = 0f;
        float z = 0f;

        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.wKey.isPressed) z += 1f;
        if (Keyboard.current.sKey.isPressed) z -= 1f;

        Vector3 input = new Vector3(x, 0f, z);
        if (input.sqrMagnitude > 1f) input.Normalize();

        // Движение относительно направления игрока (куда повернут)
        Vector3 desired = transform.TransformDirection(input) * moveSpeed;

        // Сохраняем вертикальную скорость (гравитация)
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(desired.x, velocity.y, desired.z);
    }
}
