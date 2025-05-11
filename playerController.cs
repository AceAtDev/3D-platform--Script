using UnityEngine;
using UnityEngine.InputSystem;

namespace ArmadaDev.AceG
{
    /// <summary>
    /// Controls player movement, jumping, and gravity interactions
    /// </summary>
    internal class PlayerController : MonoBehaviour
    {
        #region Component References
        private CharacterController _characterController;
        private Playerinputs _playerInputs;
        #endregion

        #region Input Variables
        private Vector2 _currentInputMovement;
        private Vector3 _currentMovement;
        private bool _isMovementPressed;
        private bool _isJumpPressed;
        #endregion

        #region Movement Variables
        private Vector3 _rawMovement;
        private float _currentHorizontalSpeed;  // X-axis
        private float _currentVerticalSpeed;    // Y-axis
        private float _currentApplicateSpeed;   // Z-axis
        #endregion

        #region Walking Properties
        [Header("Walking")]
        [SerializeField] private float _acceleration = 60f;
        [SerializeField] private float _deceleration = 30f;
        [SerializeField] private float _maxMoveSpeed = 13f;
        [SerializeField] private float _apexBonus = 2f;
        #endregion

        #region Jump Properties
        [Header("Jump")]
        [SerializeField] private float _jumpHeight = 10f;
        [SerializeField] private float _endedJumpEarlyModifier = 3f;
        [SerializeField] private float _jumpApexThreshold = 10f;
        //[SerializeField] private float _coyoteTimeThreshold = 1f;
        
        private bool _endedJumpEarly = false;
        private float _apexPoint;
        private bool _isJumping = false;
        private bool _isGrounded;
        #endregion

        #region Gravity Properties
        [Header("Gravity")]
        [SerializeField] private float _maxGravity = 9.8f;
        [SerializeField] private float _minGravity = 1.8f;
        [SerializeField] private float _groundedGravity = 0.5f;
        [SerializeField] private float _maxFallSpeed = -40f;
        private float _fallSpeedMultiplier;
        #endregion

        #region Unity Lifecycle Methods
        private void Awake()
        {
            _playerInputs = new Playerinputs();
            _characterController = GetComponent<CharacterController>();

            // Register input callbacks
            _playerInputs.PlayerController.move.started += OnMoveInput;
            _playerInputs.PlayerController.move.canceled += OnMoveInput;
            _playerInputs.PlayerController.move.performed += OnMoveInput;

            _playerInputs.PlayerController.Jump.started += OnJumpInput;
            _playerInputs.PlayerController.Jump.canceled += OnJumpInput;
        }

        private void Update()
        {
            ApplyGravity();
            HandleJump();
            HandleMovement();
        }

        private void OnEnable()
        {
            _playerInputs.PlayerController.Enable(); // Enable action map
        }

        private void OnDisable()
        {
            _playerInputs.PlayerController.Disable(); // Disable action map
        }
        #endregion

        #region Input Handlers
        /// <summary>
        /// Processes movement input from the player
        /// </summary>
        private void OnMoveInput(InputAction.CallbackContext context)
        {
            _currentInputMovement = context.ReadValue<Vector2>();
            _currentMovement.x = _currentInputMovement.x;
            _currentMovement.z = _currentInputMovement.y;
            _isMovementPressed = _currentMovement.x != 0 || _currentMovement.z != 0;
        }

        /// <summary>
        /// Processes jump input from the player
        /// </summary>
        private void OnJumpInput(InputAction.CallbackContext context)
        {
            _isJumpPressed = context.ReadValueAsButton();
        }
        #endregion

        #region Movement Handling
        /// <summary>
        /// Handles horizontal player movement (X and Z axes)
        /// </summary>
        private void HandleMovement()
        {
            // Handle X-axis movement
            if (_currentMovement.x != 0)
            {
                // Set horizontal speed with acceleration
                _currentHorizontalSpeed += _currentMovement.x * _acceleration * Time.deltaTime;

                // Clamp speed to maximum
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_maxMoveSpeed, _maxMoveSpeed);

                // Apply bonus at the apex of a jump
                float apexBonusX = Mathf.Sign(_currentMovement.x) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonusX * Time.deltaTime;
            }
            else // No input, slow down the player
            {
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deceleration * Time.deltaTime);
            }

            // Handle Z-axis movement
            if (_currentMovement.z != 0)
            {
                // Set applicate (z-axis) speed with acceleration
                _currentApplicateSpeed += _currentMovement.z * _acceleration * Time.deltaTime;

                // Clamp speed to maximum
                _currentApplicateSpeed = Mathf.Clamp(_currentApplicateSpeed, -_maxMoveSpeed, _maxMoveSpeed);

                // Apply bonus at the apex of a jump
                float apexBonusZ = Mathf.Sign(_currentMovement.z) * _apexBonus * _apexPoint;
                _currentApplicateSpeed += apexBonusZ * Time.deltaTime;
            }
            else
            {
                _currentApplicateSpeed = Mathf.MoveTowards(_currentApplicateSpeed, 0, _deceleration * Time.deltaTime);
            }

            // Apply movement
            _rawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed, _currentApplicateSpeed) * Time.deltaTime;
            _characterController.Move(_rawMovement);
        }
        #endregion

        #region Jump Handling
        /// <summary>
        /// Calculates the apex point of the jump for variable jump physics
        /// </summary>
        private void CalculateJumpApex()
        {
            if (!_isGrounded)
            {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_characterController.velocity.y));
                _fallSpeedMultiplier = Mathf.Lerp(_minGravity, _maxGravity, _apexPoint);
            }
            else
            {
                _apexPoint = 0;
            }
        }

        /// <summary>
        /// Handles jump mechanics and detection of early jump release
        /// </summary>
        private void HandleJump()
        {
            CalculateJumpApex();
            _isGrounded = _characterController.isGrounded;

            // Start jump when grounded and jump button pressed
            if (_isGrounded && _isJumpPressed)
            {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _isJumping = true;
            }
            else
            {
                _isJumping = false;
            }

            // Detect if player released jump key early during ascent
            if (!_isGrounded && !_isJumpPressed && !_isJumping && _characterController.velocity.y > 0)
            {
                _endedJumpEarly = true;
            }
        }
        #endregion

        #region Gravity Handling
        /// <summary>
        /// Applies appropriate gravity to the player
        /// </summary>
        private void ApplyGravity()
        {
            if (_isGrounded)
            {
                // Apply a small downward force when grounded to keep the player stuck to the ground
                _currentVerticalSpeed = _groundedGravity * Time.deltaTime;
            }
            else
            {
                // Apply stronger gravity if jump was released early
                float fallSpeed = _endedJumpEarly 
                    ? _fallSpeedMultiplier * _maxGravity * _endedJumpEarlyModifier 
                    : _fallSpeedMultiplier * _maxGravity;

                // Apply gravity
                _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Limit the fall speed
                if (_currentVerticalSpeed < _maxFallSpeed)
                    _currentVerticalSpeed = _maxFallSpeed;
            }
        }
        #endregion
    }
}
