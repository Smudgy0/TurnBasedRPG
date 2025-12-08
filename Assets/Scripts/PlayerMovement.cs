using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] private float speed;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        moveInput = value.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}
