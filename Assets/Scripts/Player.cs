using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;

    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float turnSpeed = 10f;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private void Update()
    {
       HandleMovement();
       HandleInteraction();
    } 

    private void HandleInteraction() 
    {
        Vector2 inputVector = gameInput.GetMovmentVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero)
            lastInteractDir = moveDir;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {

            }
        }
           
        else
            Debug.Log("-");
    }
    private void HandleMovement()
    {
        Vector3 inputVector = gameInput.GetMovmentVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;

        bool canMove = CanMove(moveDir);

        bool CanMove(Vector3 moveDirection)
        {
            return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);
        }

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = CanMove(moveDirX);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = CanMove(moveDirZ);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
            transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * turnSpeed);

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * turnSpeed);

    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
