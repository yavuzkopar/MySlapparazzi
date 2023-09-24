using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterMover : NetworkBehaviour
{

    [SerializeField] float speed;
    float rotY;
    Rigidbody _rigidbody;
    bool camJump;
    [SerializeField] Animator animator;
    private void Awake()
    {
        _rigidbody= GetComponent<Rigidbody>();
        camJump= true;
    }
    public override void Spawned()
    {
        
    }
    public override void FixedUpdateNetwork()
    {
        
        if (GetInput(out NetworkInputData networkInputData))
        {
            if(networkInputData.movementInput.magnitude >0.1f)
                transform.rotation = Quaternion.LookRotation(new Vector3(networkInputData.movementInput.x, 0, networkInputData.movementInput.y), Vector3.up);
            Vector2 moveInput = Vector2.ClampMagnitude(networkInputData.movementInput, 1);
           Vector3 move = new Vector3(moveInput.x,0,moveInput.y);   
            transform.position += move * Runner.DeltaTime * speed;
            if(networkInputData.isJumpPressed)
            {
                Jump();
            }
            animator.SetFloat("Vertical", move.magnitude);
            if (networkInputData.isInteracted )
            {
               animator.SetTrigger("take");
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        camJump = true;
    }
    void Jump()
    {
        if (!camJump) return;
        animator.SetTrigger("jump");
        _rigidbody.velocity = Vector3.up * 5;
        camJump = false;
    }
    
}
