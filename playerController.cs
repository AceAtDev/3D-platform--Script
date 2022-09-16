using UnityEngine;
using UnityEngine.InputSystem;

namespace ArmadaDev.AceG
{

	internal class playerController : MonoBehaviour
	{
		//  References
		CharacterController characterController;
		Playerinputs playerInputs;

		//  Variables to store player's inputs
		Vector2 currentInputsMovement;
		Vector3 currentMovement;
		bool isMovementPressed;


		float currentHorizontalspeed, currentVerticalSpeed, currentApplicateSpeed; //   X, Y, and Z



		void Awake()
		{
			playerInputs = new Playerinputs();
			characterController = GetComponent<CharacterController>();


			playerInputs.PlayerController.move.started += onMoveInput;
			playerInputs.PlayerController.move.canceled += onMoveInput;
			playerInputs.PlayerController.move.performed += onMoveInput;

			playerInputs.PlayerController.Jump.started += onJumpInput;
			playerInputs.PlayerController.Jump.canceled += onJumpInput;

		}

		//  So we don't have to redefine those values for every new movement input action
		void onMoveInput(InputAction.CallbackContext context)
		{
			currentInputsMovement = context.ReadValue<Vector2>();
			currentMovement.x = currentInputsMovement.x;
			currentMovement.z = currentInputsMovement.y;
			isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
		}

		void onJumpInput(InputAction.CallbackContext context)
		{
			isJumpPressed = context.ReadValueAsButton();
		}


		void Update()
		{
			Gravity();
			Jump();
			Walk();

			
		}

		#region Walking
		[Header("Walking")]
		[SerializeField] float _acceleration = 60f;
		[SerializeField] float _deAcceleration = 30f;
		[SerializeField] float _moveClamp = 13f;
		[SerializeField] float apexBonus = 2f;

		Vector3 RawMovement;

		void Walk()
		{

			if (currentMovement.x != 0) //  X-axis
			{
				//  Set Horizontal speed
				currentHorizontalspeed += currentMovement.x * _acceleration * Time.deltaTime;


				//  Clamp speed
				currentHorizontalspeed = Mathf.Clamp(currentHorizontalspeed, -_moveClamp, _moveClamp);


				// Apply bonus at the apex of a jump
				var apexBonusX = Mathf.Sign(currentMovement.x) * apexBonus * apexPoint;
				currentHorizontalspeed += apexBonusX * Time.deltaTime;
			}
			else // No input, let's slow down the player
			{
				currentHorizontalspeed = Mathf.MoveTowards(currentHorizontalspeed, 0, _deAcceleration * Time.deltaTime);
			}



			if (currentMovement.z != 0) //  Z-axis
			{
				currentApplicateSpeed += currentMovement.z * _acceleration * Time.deltaTime;


				currentApplicateSpeed = Mathf.Clamp(currentApplicateSpeed, -_moveClamp, _moveClamp);


				var apexBonusZ = Mathf.Sign(currentMovement.z) * apexBonus * apexPoint;
				currentHorizontalspeed += apexBonusZ * Time.deltaTime;
			}
			else
			{
				currentApplicateSpeed = Mathf.MoveTowards(currentApplicateSpeed, 0, _deAcceleration * Time.deltaTime);
			}


			RawMovement = new Vector3(currentHorizontalspeed, currentVerticalSpeed, currentApplicateSpeed) * Time.deltaTime; // Used externally

			characterController.Move(RawMovement);
		}

		#endregion


		#region Jump
		[Header("Jump")]
		[SerializeField] float JumpHeight = 10f;
		[SerializeField] float endedJumpEarlyModifier = 3f;
		[SerializeField] float _jumpApexThreshold = 10f;
		//[SerializeField] float coyoteTimeThreshold = 1f; // This should be barly have an effect on the player but you may rise the value to check if it works
		bool isJumpPressed;
		bool endedJumpEarly = false;
		float apexPoint;
		bool isJumping = false;
		bool isGrounded;
		

		//private bool CanUseCoyote => coyoteUsable && !isGrounded && timeLeftGround + coyoteTimeThreshold > Time.time;

		void apex()
		{
			if (!isGrounded)
			{
				// Gets stronger the closer to the top of the jump
				apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(characterController.velocity.y));
				_fallSpeed = Mathf.Lerp(minGravity, maxGravity, apexPoint);
			}
			else
			{
				apexPoint = 0;
			}
		}


		void Jump()
		{
			apex();
			isGrounded = characterController.isGrounded;//	The player won't check on what layer it hit. In case you want to add a layer, than add it here


			if (isGrounded && isJumpPressed)
			{
				currentVerticalSpeed = JumpHeight;
				endedJumpEarly = false;
				isJumping = true;
				
			}
			else
			{
				isJumping = false;
			}

			//	Player left the jump key early
			if (!isGrounded && !isJumpPressed && !isJumping && characterController.velocity.y > 0)
			{
				endedJumpEarly = true;
			}

		}

		#endregion


		#region Gravity
		[Header("Gravity")]
		[SerializeField] float maxGravity = 9.8f;
		[SerializeField] float minGravity = 1.8f;
		[SerializeField] float groundedGravity = 0.5f;
		[SerializeField] float clampfallSpeed = -40f;
		float _fallSpeed;

		

		void Gravity()
		{
			if (isGrounded)
			{
				currentVerticalSpeed = groundedGravity * Time.deltaTime;
			}
			else
			{
				float fallSpeed =
					endedJumpEarly ? _fallSpeed * maxGravity * endedJumpEarlyModifier : _fallSpeed * maxGravity;


				currentVerticalSpeed -= fallSpeed * Time.deltaTime;


				// Limit the fall speed
				if (currentVerticalSpeed < clampfallSpeed)
					currentVerticalSpeed = clampfallSpeed;
			}

		}
		#endregion


		#region OnEnable & OnDisable
		private void OnEnable()
		{
			playerInputs.PlayerController.Enable(); // Enable action map
		}

		private void OnDisable()
		{
			playerInputs.PlayerController.Disable(); // Disable action map
		}
		#endregion
	}

}
