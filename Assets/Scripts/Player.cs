using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float moveSpeed;
    [SerializeField] private GameInput gameInput;

    private int playerHealth = 100;
    private float healthChangeDelay = 3f;
    private float healtChangeTimer = 0f;


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

        healtChangeTimer -= Time.deltaTime;

        if (Fireplace.Instance.GetIsPlayerTriggered())
        {
            if (healtChangeTimer < 0)
            {
                healtChangeTimer = healthChangeDelay;
                playerHealth += 10;
            }
        } else
        {
            if (healtChangeTimer < 0)
            {
                healtChangeTimer = healthChangeDelay;
                playerHealth -= 10;
            }
        }
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

        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVector, moveDistance);

        if (!canMove)
        {
            //Cannot move towards directionVector

            //Atempt only x movement
            Vector3 directionVectorX = new Vector3(directionVector.x, 0, 0).normalized;
            canMove = (directionVector.x < -.5f || directionVector.x > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorX, moveDistance);

            if (canMove)
            {
                //Can move only on the X
                directionVector = directionVectorX;
            } else
            {
                //Cannot move only on the X

                //Attempt only Z movement
                Vector3 directionVectorZ = new Vector3(0, 0, directionVector.z).normalized;
                canMove = (directionVector.z < -.5f || directionVector.z > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorZ, moveDistance);

                if (canMove)
                {
                    //Can move only on the Z
                    directionVector = directionVectorZ;
                } else
                {
                    //Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += directionVector * moveDistance;
        }

        //transform.position += directionVector * moveDistance;

        if (directionVector != Vector3.zero)
        {
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, directionVector, Time.deltaTime * rotateSpeed);
        }
    }
}
