using UnityEngine;

namespace Name
{
    public class Player : MonoBehaviour
    {
        public float movementSpeed = 2.5f;
        private Vector2 input;
        public  Rigidbody2D playerRb;

        private void  Update()
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            var targetPos = transform.position;
            targetPos.x += input.x;
        }

        private void Start()
        {
            playerRb.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (Labirinto.DialogueSystem.DialogueManager.Instance.dialogueIsPlaying)
            {
                return;
            }
            playerRb.MovePosition(playerRb.position + input * movementSpeed * Time.fixedDeltaTime);
        }
    }
}
