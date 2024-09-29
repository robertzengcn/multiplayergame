using System.Collections;
using UnityEngine;
using Mirror;
using Lobbytest.Tutorials.Lobby.Inputs;

namespace Assets.Tutorials.Lobby.Scripts.Player
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private CharacterController controller= null;

        private Vector2 previorsInput;
        private Controls controls;
        private Controls Controls
        {
            get
            {
                if(controls != null) { return controls; }
                return controls = new Controls();
            }
        }

        public override void OnStartAuthority()
        {
            enabled = true;
            Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            Controls.Player.Move.canceled += ctx => ResetMovement();
        }
        [ClientCallback]
        private void OnEnable() => Controls.Enable();
        [ClientCallback]
        private void OnDisable() => Controls.Disable();
        [ClientCallback]
        private void Update() => Move();
        [Client]
        private void SetMovement(Vector2 movement) => previorsInput = movement;
        [Client]
        private void ResetMovement() => previorsInput = Vector2.zero;
        [Client]
        private void Move()
        {
            Vector3 right = controller.transform.right;
            Vector3 forward = controller.transform.forward;
            right.y = 0f;
            forward.y = 0f;
            Vector3 movement=right.normalized * previorsInput.x + forward.normalized * previorsInput.y;
            controller.Move(movement * moveSpeed * Time.deltaTime);
        }

    }
}