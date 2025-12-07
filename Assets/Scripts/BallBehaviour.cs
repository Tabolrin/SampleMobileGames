using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float postLaunchDestructionDelay = 2f;
    
    public void OnLaunch()
    {
        Invoke(nameof(DestroyBall), postLaunchDestructionDelay);
    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }
    
}
