using DataPersistence;
using Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerControl
{
    public class Player : MonoBehaviour, IDataPersistence
    {
        public float movementSpeed = 2.5f;
        private Vector2 input;
        public Rigidbody2D playerRb;
        public Animator animator;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}
		private void Start()
        {
            // ACHIEVEMENT: Create First Game
            AchievementsManager.Instance.CompleteAchievement("1Manlalaro");
            playerRb = GetComponent<Rigidbody2D>();  // Proper assignment
            
        }

        // Load the player position from saved game data
        public void LoadData(GameData data) 
        {
            this.transform.position = data.playerPosition;
        }

        // Save the player's position to the game data.
        public void SaveData(GameData data) 
        {
            data.playerPosition = this.transform.position;
            Debug.Log("Player position is saved" + SceneManager.GetActiveScene().name);
        }

        private void Update()
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            var targetPos = transform.position;
            targetPos.x += input.x;
            targetPos.y += input.y;

			animator.SetFloat("Horizontal", input.x);
			animator.SetFloat("Vertical", input.y);
            animator.SetFloat("Speed", input.sqrMagnitude);
            
			
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
