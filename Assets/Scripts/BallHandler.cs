using UnityEngine;
using UnityEngine.InputSystem;


public class BallHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D currentBallRigidbody;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            currentBallRigidbody.bodyType = RigidbodyType2D.Dynamic;
            return;
        }

        currentBallRigidbody.bodyType = RigidbodyType2D.Kinematic;
        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPos);
        currentBallRigidbody.position = worldPosition;
    }
}
