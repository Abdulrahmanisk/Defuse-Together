using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimittheDoor : MonoBehaviour
{
    [SerializeField] private float minX = -3f; 
    private float initialX; 

    private void Start()
    {
        initialX = transform.position.x; 
    }

    private void Update()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, minX, initialX);

        transform.position = position;
    }
}
    

