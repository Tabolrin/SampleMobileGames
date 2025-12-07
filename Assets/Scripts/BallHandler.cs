using UnityEngine;
using UnityEngine.InputSystem;


public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float spawnDelay = 0.3f;
    [SerializeField] private float launchDelay = 0.3f;

    private BallBehaviour currentBall;
    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDragging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }


    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null) 
            return;
        
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if(isDragging)
                LaunchBall();
            isDragging = false;
            return;
        }

        isDragging = true;

        currentBallRigidbody.bodyType = RigidbodyType2D.Kinematic;
        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPos);
        currentBallRigidbody.position = worldPosition;
        currentBallRigidbody.linearVelocity = Vector2.zero;
    }

    private void LaunchBall()
    {
        currentBallRigidbody.bodyType = RigidbodyType2D.Dynamic;
        currentBallRigidbody = null;
        
        Invoke(nameof(DetachBall), launchDelay);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallRigidbody = null;
        currentBall.OnLaunch();
        
        Invoke(nameof(SpawnNewBall), spawnDelay);
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        currentBall = ballInstance.GetComponent<BallBehaviour>();
        
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
        
        currentBallSpringJoint.connectedBody = pivot;
    }
}
