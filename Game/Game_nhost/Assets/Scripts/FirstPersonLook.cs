using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private float maxPitch = 80f;

    private float pitch;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue() * sensitivity * 0.02f;

        // поворот тела (yaw)
        transform.Rotate(0f, delta.x, 0f);

        // поворот камеры (pitch)
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);
        cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);

        // выйти из lock (удобно в редакторе)
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
