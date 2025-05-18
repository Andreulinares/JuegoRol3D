 using UnityEngine;
 using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class PlayerController : MonoBehaviour
    {
        [Header("Player")]
        // Referencia al prefab de la esfera
    public GameObject spherePrefab;
    public SpawnerPersonaje spawner;
    private EnemigoMelee enemigoMelee;
    public BossAI bossAI;
    public float sphereDistance = 2f;
    private GameObject currentSphere;
    public UIManager uiManager;
    private bool leftPressed = false;
    private bool rightPressed = false;
    private bool upPressed = false;
    private bool downPressed = false;
    
    // Daño que la esfera causará a los enemigos
    public int sphereDamage = 10;
    public int vidaMaxPlayer = 100;
    public int vidaActualPlayer = 100;
    public int manaMaxPlayer = 100;
    public int manaActualPlayer = 100;
    public bool isStunned = false;
    private float stunTimer = 0f;
    private bool _isDead = false;
    public bool isInvincible = false;
    public float invincibleTimer = 0f;
    public GameObject deathScreenUI; // <- Asigna este desde el Inspector
    public enum AttackType { Fire, Water, Electricity, Earth, None }
    public AttackType currentAttackType = AttackType.None;
    public enum Elemento { Fire, Water, Electricity, Earth, None }
    public Elemento playerActivoElemento = Elemento.None;
    public Elemento playerAtaqueElemento = Elemento.None;
    


    // Tiempo de vida de la esfera
    public float sphereLifetime = 2f;
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        private int _animIDAttack;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            vidaMaxPlayer = 100;
            vidaActualPlayer=vidaMaxPlayer;
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            ActualizarBarraDeVida();
            ActualizarBarraMana();

            enemigoMelee = GetComponent<EnemigoMelee>();
        }

        private void Update()
        {
            ActualizarBarraDeVida();
            if (isStunned)
            {
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0f)
                {
                    isStunned = false;
                }
            }

        // Controlar la invencibilidad
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
            }
        }

        // Si está aturdido, cancelar controles
        if (isStunned) return;
            _hasAnimator = TryGetComponent(out _animator);
            
            if(LeftPadClick())
            {
                if(currentAttackType==AttackType.Electricity)
                {
                    AtaqueActivo(AttackType.None);
                }
                else
                {
                    AtaqueActivo(AttackType.Electricity);
                } 
            }
            if(RightPadClick())
            {
                if(currentAttackType==AttackType.Earth)
                {
                    AtaqueActivo(AttackType.None);
                }
                else
                {
                AtaqueActivo(AttackType.Earth);
                }
            }
            if(UpPadClick())
            {
                if(currentAttackType==AttackType.Fire)
                {
                    AtaqueActivo(AttackType.None);
                }
                else
                {
                AtaqueActivo(AttackType.Fire);
                }
            }
            if(DownPadClick())
            {
                if(currentAttackType==AttackType.Water)
                {
                    AtaqueActivo(AttackType.None);
                }
                else
                {
                AtaqueActivo(AttackType.Water);
                }
            }
            if (MouseLeftClick())
            {
                //CreateStaticSphere();
                _animator.SetTrigger(_animIDAttack);
                GetComponentInChildren<PlayerAttack>().ActivarColliderGolpe();
                
            }
            // Actualizar la posición de la esfera para que siga al personaje
            if (currentSphere != null)
            {
                FollowPlayer();
            }

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        /*public void ActivarGolpe()
        {
            Collider[] enemigos = Physics.OverlapSphere(transform.position + transform.forward * 1f, 1f);

                if (enemigos.Length == 0)
                {
                    Debug.Log("El golpe no impactó a ningún enemigo!");
                    return;
                }
                foreach (Collider enemigo in enemigos)
                {
                    if (enemigo.CompareTag("enemy")) 
                    {
                        enemigo.GetComponent<EnemigoMelee>().TakeDamage(sphereDamage);
                        Debug.Log("Golpe impactó al enemigo!");
                    }
                }
        }*/

/*        private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("enemy")) // Verifica que el objeto golpeado es un enemigo
    {
        other.GetComponent<EnemigoMelee>().TakeDamage(sphereDamage);
        Debug.Log("Golpe impactó al enemigo!");
    }
}*/

        private void LateUpdate()
        {
            CameraRotation();
        }
        private bool MouseLeftClick()
        {
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton1);
        }
        private bool LeftPadClick()
{
    float axis = Input.GetAxis("Horizontal2");
    if (axis < -0.5f)
    {
        if (!leftPressed)
        {
            leftPressed = true;
            return true;
        }
    }
    else
    {
        leftPressed = false;
    }
    return false;
}

private bool RightPadClick()
{
    float axis = Input.GetAxis("Horizontal2");
    if (axis > 0.5f)
    {
        if (!rightPressed)
        {
            rightPressed = true;
            return true;
        }
    }
    else
    {
        rightPressed = false;
    }
    return false;
}

private bool UpPadClick()
{
    float axis = Input.GetAxis("Vertical2");
    if (axis < -0.5f) // hacia arriba suele ser negativo
    {
        if (!upPressed)
        {
            upPressed = true;
            return true;
        }
    }
    else
    {
        upPressed = false;
    }
    return false;
}

private bool DownPadClick()
{
    float axis = Input.GetAxis("Vertical2");
    if (axis > 0.5f)
    {
        if (!downPressed)
        {
            downPressed = true;
            return true;
        }
    }
    else
    {
        downPressed = false;
    }
    return false;
}
        private void CreateStaticSphere()
        {
            if (currentSphere == null)
            {
                if(currentAttackType==AttackType.None)
                {
                    PerformAttack(AttackType.None);
                }
                else if(currentAttackType!=AttackType.None && manaActualPlayer>0)
                {
                    PerformAttack(currentAttackType);
                    manaActualPlayer=manaActualPlayer-25;
                    if(manaActualPlayer<=0)
                    {
                        manaActualPlayer=0;
                    }
                    ActualizarBarraMana();
                    
                }
                else
                {   
                    PerformAttack(AttackType.None);
                }
                // Calculamos la posición delante del personaje
                Vector3 spawnPosition = transform.position + transform.forward * sphereDistance + transform.up * 1;

                // Instanciamos la esfera en la posición calculada
                currentSphere = Instantiate(spherePrefab, spawnPosition, Quaternion.identity);

                // Asegurarnos de que el collider sea trigger y no tenga física
                Collider collider = currentSphere.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.isTrigger = true; // Activar el modo trigger
                }
            }
        }

        private void ActualizarBarraMana()
        {
                // Calcular el porcentaje de mana
            float porcentaje = (float)manaActualPlayer / manaMaxPlayer;
                
                // Llamar a la función del UIManager para actualizar la barra
            UIManager.Interface.ActualizarMana(porcentaje);
        }
        private void FollowPlayer()
        {
            // Calculamos la nueva posición de la esfera, frente al jugador
            Vector3 newPosition = transform.position + transform.forward * sphereDistance + transform.up * 1;

            // Movemos la esfera a esa posición
            currentSphere.transform.position = newPosition;

            // La esfera siempre debe rotar con el personaje
            currentSphere.transform.rotation = transform.rotation;
        }


        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDAttack = Animator.StringToHash("isAttack");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    
    private void PerformAttack(AttackType attack)
    {
        currentAttackType = attack;

        switch (currentAttackType)
        {
            case AttackType.Fire:
                playerAtaqueElemento=Elemento.Fire;
                Debug.Log("Jugador realiza un ataque de Fuego!");
                break;
            case AttackType.Water:
                playerAtaqueElemento=Elemento.Water;
                Debug.Log("Jugador realiza un ataque de Agua!");
                break;
            case AttackType.Electricity:
                playerAtaqueElemento=Elemento.Electricity;
                Debug.Log("Jugador realiza un ataque de Electricidad!");
                break;
            case AttackType.Earth:
                playerAtaqueElemento=Elemento.Earth;
                Debug.Log("Jugador realiza un ataque de Tierra!");
                break;
            case AttackType.None:
                playerAtaqueElemento=Elemento.None;
                Debug.Log("Jugador realiza un ataque normal!");
                break;
        }
    }
    private void AtaqueActivo(AttackType attack)
    {
        currentAttackType = attack;

        switch (currentAttackType)
        {
            case AttackType.Fire:
                playerActivoElemento=Elemento.Fire;
                Debug.Log("Jugador obtiene el poder de Fuego!");
                uiManager.mostrarFuego();
                break;
            case AttackType.Water:
                playerActivoElemento=Elemento.Water;
                Debug.Log("Jugador obtiene el poder de Agua!");
                uiManager.mostrarAgua();
                break;
            case AttackType.Electricity:
                playerActivoElemento=Elemento.Electricity;
                Debug.Log("Jugador obtiene el poder de Electricidad!");
                uiManager.mostrarElectricidad();
                break;
            case AttackType.Earth:
                playerActivoElemento=Elemento.Earth;
                Debug.Log("Jugador obtiene el poder de Tierra!");
                uiManager.mostrarTierra();
                break;
            case AttackType.None:
                playerActivoElemento=Elemento.None;
                Debug.Log("Jugador obtiene el ataque normal!");
                uiManager.mostrarNinguno();
                break;
        }
    }
    public void TakeDamage(int damage)
{
    if(isInvincible)
    {
        Debug.Log("Jugador invencible. No recibe daño.");
        return;
    }
    /*if(enemigoMelee.damageBufo==true)
    {
        damage=damage+10;
    }*/
    /*if(bossAI.attackBuff==true)
    {
        damage=damage+10;
    }*/
    vidaActualPlayer -= damage;
    
    ActualizarBarraDeVida();

    if (vidaActualPlayer <= 0 && !_isDead)
    {
        Muerto();
    }
    else
    {
        //Animacion stun
        Stun(2);

    }
}

private void ActualizarBarraDeVida()
{
        // Calcular el porcentaje de vida
    float porcentaje = (float)vidaActualPlayer / vidaMaxPlayer;
        
        // Llamar a la función del UIManager para actualizar la barra
    UIManager.Interface.ActualizarVida(porcentaje);
}
    public void Muerto()
{
    _isDead = true;

    // Desactivar controles
    _input.move = Vector2.zero;
    _input.jump = false;
    _input.sprint = false;
    _playerInput.enabled = false;

    // Activar animación de muerte
    if (_hasAnimator)
    {
        _animator.SetTrigger("Death"); 
    }

    // Mostrar UI de muerte
    if (deathScreenUI != null)
    {
        deathScreenUI.SetActive(true);
    }
    
}
public void RestartGame()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
public void Stun(float stunDuration, float extraInvincibility = 2f)
{
    isStunned = true;
    isInvincible = true;

    stunTimer = stunDuration;
    invincibleTimer = stunDuration + extraInvincibility;
}
public void ReiniciarPersonaje()
{
    transform.position = spawner.puntoSpawn.position;
    vidaActualPlayer=vidaMaxPlayer;
    manaActualPlayer=manaMaxPlayer;
    bossAI.PVActual=bossAI.PVMax;
    bossAI.isApproaching = false;
    bossAI.isChasing = false;
    bossAI.Patrol();
    bossAI.transform.position=bossAI.puntoSpawnBoss.position;
}

}


}