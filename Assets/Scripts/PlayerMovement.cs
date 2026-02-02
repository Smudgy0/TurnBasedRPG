using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] private float speed;

    [SerializeField] private PlayerInput playerInput;

    public static Vector2 sceneStartPos;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        Debug.Log(playerInput.actionEvents[0].actionName);
        SetPlayerPosition();
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

    public void SetPlayerPosition()
    {
        if (sceneStartPos == Vector2.zero) return;
        transform.parent.position = sceneStartPos;
    }
}
