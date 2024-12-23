using UnityEngine;

public class LimitMovement : MonoBehaviour
{
    [SerializeField] private float maxX = 3f; 
    private float initialX; 

    private void Start()
    {
        initialX = transform.position.x; 
    }

    private void Update()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, initialX, maxX);

        transform.position = position;
    }
}