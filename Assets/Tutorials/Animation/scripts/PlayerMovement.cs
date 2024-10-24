using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private Animator animator = null;
    //[SerializeField] private NetworkAnimator networkAnimator = null;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5f;
    
    [ClientCallback]
    private void Update()
    {
        if (!isOwned)
        {
            return;
        }
        var movemnet = new Vector3();
        if(Keyboard.current.wKey.isPressed)
        {
            Debug.Log("w pressed");
            movemnet.z += 1;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            movemnet.z -= 1;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            movemnet.x += 1;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            movemnet.x -= 1;
        }
        
            controller.Move(movemnet * movementSpeed * Time.deltaTime);
        if (controller.velocity.magnitude > 0.2f)
        {
            transform.rotation = Quaternion.LookRotation(movemnet);
        }
        animator.SetBool("IsWalking", controller.velocity.magnitude > 0.2f);
        //networkAnimator.SetTrigger(30);
    }
}
