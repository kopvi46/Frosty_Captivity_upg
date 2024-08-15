using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float moveSpeed;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private int playerHealth = 100;
    public int PlayerHealth 
    {  
        get 
        { 
            return playerHealth; 
        } 
        set 
        {  
            if (value < 0)
                {
                    playerHealth = 0;
                } else
            {
                playerHealth = value;
            }
        } 
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 directionVector = cameraForward * inputVector.y + cameraRight * inputVector.x;

        float moveDistance = moveSpeed * Time.deltaTime;

        transform.position += directionVector * moveDistance;

        if (directionVector != Vector3.zero)
        {
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, directionVector, Time.deltaTime * rotateSpeed);
        }
    }
}
