using DataPersistence;
using Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerControl
{
    public class Player : MonoBehaviour, IDataPersistence
    {
        public Joystick joystickMovement;
        public float movementSpeed = 2.5f;
        private Vector2 input;
        public Rigidbody2D playerRb;
        public Animator animator;
        AudioSource audioSource;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}
		private void Start()
        {
            // ACHIEVEMENT: Create First Game
            AchievementsManager.Instance.CompleteAchievement("1Manlalaro");
            audioSource = GetComponent<AudioSource>();
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
			input.x = joystickMovement.Horizontal * movementSpeed;
            input.y = joystickMovement.Vertical * movementSpeed;

            var targetPos = transform.position;
            targetPos.x += input.x;
            targetPos.y += input.y;
            if(input.x != 0 || input.y != 0)
            {
				animator.SetFloat("X", input.x);
				animator.SetFloat("Y", input.y);
                animator.SetBool("isWalking", true);
            }
            else
            {
				animator.SetBool("isWalking", false);
			}
			
     
		}
        private void FixedUpdate()
        {
            // Stop player movement if a dialogue is playing
            if (Dialogue.DialogueManager.Instance.dialogueIsPlaying)
            {
                playerRb.velocity = Vector2.zero;
                return;
            }
            else if (joystickMovement.Direction.y != 0)
            {
                // Move the player based on input
                //playerRb.MovePosition(playerRb.position + input * movementSpeed * Time.fixedDeltaTime);
                playerRb.velocity = new Vector2(joystickMovement.Direction.x * movementSpeed, joystickMovement.Direction.y * movementSpeed);
			}
            else
            {
                playerRb.velocity = Vector2.zero;
            }
        }
    }
}
