using DataPersistence;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour, IDataPersistence
    {
        public float movementSpeed = 2.5f;
        private Vector2 input;
        public Rigidbody2D playerRb;

        private void Start()
        {
            playerRb = GetComponent<Rigidbody2D>();  // Proper assignment
        }

        // Load the player position from saved game data
        public void LoadData(GameData data) 
        {
            this.transform.position = data.playerPosition;
        }

        // Save the player's position to the game data
        public void SaveData(GameData data) 
        {
            data.playerPosition = this.transform.position;
        }

        private void Update()
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            var targetPos = transform.position;
            targetPos.x += input.x;
            targetPos.y += input.y;
        }

        private void FixedUpdate()
        {
            // Stop player movement if a dialogue is playing
            if (Dialogue.DialogueManager.Instance.dialogueIsPlaying)
            {
                return;
            }

            // Move the player based on input
            playerRb.MovePosition(playerRb.position + input * movementSpeed * Time.fixedDeltaTime);
        }
    }
}
