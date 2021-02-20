using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private Jumping jumping    = new Jumping();
    [SerializeField] private Flight flight      = new Flight();
    [SerializeField] private Landing landing    = new Landing();
    [SerializeField] private Slipping slipping  = new Slipping();
    [SerializeField] private Sounds sounds      = new Sounds();



    [System.Serializable]
    private struct Jumping
    {
        public Transform defaultParent;
        public float jumpForce;
    }



    [System.Serializable]
    private struct Flight
    {
        public float flightStateTransitionThreshold;
        public TrailRenderer trail;
    }



    [System.Serializable]
    private struct Landing
    {
        [Header("Check points")]
        public Transform leftCheckPoint;
        public Transform middleCheckPoint;
        public Transform rightCheckPoint;

        [Header("Other")]
        public LayerMask groundLayer;
        public float perfectLandingDistance;
        public ObjectContainer perfectEffectsContainer;
    }



    [System.Serializable]
    private struct Slipping
    {
        public Vector2 slipOffset;
        public float timeOfSlip;
        public Vector2 offsetIfSlippingStopped;
    }



    [System.Serializable]
    private struct Losing
    {
    }



    [System.Serializable]
    private struct Sounds
    {
        public AudioClip[] getReady;
        public AudioClip[] jump;
        public AudioClip[] landing;
        public AudioClip perfectLanding;
        public AudioClip losing;
    }



    private enum BodyState
    {
        AtRest,
        ReadyToJump,
        InFlight,
        SlipsOff
    }



    public event System.Action PreparedToJump;
    public event System.Action Jumped;
    public event System.Action Landed;
    public event System.Action StartedSlippingOff;
    public event System.Action PerfectLanding;
    public event System.Action NotPerfectLanding;
    public event System.Action Lost;



    private Transform _transform;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private AudioSource _audioSource;



    private BodyState bodyState = BodyState.AtRest;
    private bool isFirstLanding = true;
    private Coroutine slipOffRoutine;



    private void Awake()
    {
        _transform      = transform;
        _rigidbody      = GetComponent<Rigidbody2D>();
        _boxCollider    = GetComponentInChildren<BoxCollider2D>();
        _animator       = GetComponentInChildren<Animator>();
        _audioSource    = GetComponentInChildren<AudioSource>();
    }



    private void OnEnable()
    {
        inputHandler.PushDown += GetReadyToJump;
        inputHandler.PushUp += ToJump;
    }



    private void OnDisable()
    {
        inputHandler.PushDown -= GetReadyToJump;
        inputHandler.PushUp -= ToJump;
    }



    private void Start()
    {
        _rigidbody.velocity = new Vector2(0f, -10f);
    }



    private void Update()
    {
        CheckVerticalSpeed();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Platform":
                ToLand(collision.transform);
                break;



            case "Thorns":
                ToLose();
                break;



            case "Border":

                if (bodyState == BodyState.SlipsOff)
                {
                    ToLose();
                }

                else
                {
                    StartHorizontalShift();
                }

                break;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Border":
                StopHorizontalShift();
                break;
        }
    }



    private void GetReadyToJump()
    {
        if (bodyState == BodyState.AtRest)
        {
            bodyState = BodyState.ReadyToJump;

            _animator.SetTrigger("ReadyToJump");
            _audioSource.PlayOneShotWithRandomPitchFromArray(sounds.getReady, 0.9f, 1.1f);

            PreparedToJump?.Invoke();
        }
    }



    private void ToJump()
    {
        if (bodyState == BodyState.ReadyToJump)
        {
            bodyState = BodyState.InFlight;

            _transform.parent = jumping.defaultParent;
            _transform.Rotate(0f, 0f, 180f);
            _rigidbody.velocity = -_transform.up * jumping.jumpForce;
            _animator.SetBool("OnGround", false);
            _audioSource.PlayOneShotWithRandomPitchFromArray(sounds.jump, 0.9f, 1.1f);

            StartEmittingTrail();

            Jumped?.Invoke();
        }
    }



    private void ToLand(Transform platformTransform)
    {
        if (ItIsUnder() && bodyState == BodyState.InFlight)
        {
            StopEmittingTrail();

            _transform.parent = platformTransform;
            _transform.localPosition = PixelPerfectTool.MakePixelPerfect(_transform.localPosition, 16f);
            _rigidbody.velocity = Vector2.zero;
            _animator.SetBool("OnGround", true);
            _audioSource.PlayOneShotWithRandomPitchFromArray(sounds.landing, 0.9f, 1.1f);


            if (ItIsStablePosition())
            {
                bodyState = BodyState.AtRest;

                _animator.SetTrigger("Landing");

                if (Mathf.Abs(_transform.position.x - platformTransform.position.x) <= landing.perfectLandingDistance && !isFirstLanding)
                {
                    _transform.localPosition = new Vector2(0f, _transform.localPosition.y);
                    _audioSource.PlayOneShotWithRandomPitch(sounds.perfectLanding, 0.9f, 1.1f);

                    var perfectEffect = landing.perfectEffectsContainer.GetObject().transform;

                    perfectEffect.parent = platformTransform;
                    perfectEffect.localPosition = Vector2.zero;
                    perfectEffect.localScale = Mathf.Round(_transform.rotation.eulerAngles.z) == 180f ? new Vector2(1f, -1f)
                                                                                                      : new Vector2(1f, 1f);

                    PerfectLanding?.Invoke();
                }

                else
                {
                    NotPerfectLanding?.Invoke();
                }
            }

            else
            {
                bodyState = BodyState.SlipsOff;

                float direction = Mathf.Sign(_transform.position.x - platformTransform.position.x);

                StartSlipOff(direction);

                _animator.SetTrigger("SlipOff");
            }

            if (!isFirstLanding)
            {
                Landed?.Invoke();
            }

            isFirstLanding = false;
        }
    }



    private void StartHorizontalShift()
    {
        _audioSource.PlayOneShot(sounds.landing[0]);
    }



    private void StopHorizontalShift()
    {
        _transform.localPosition = PixelPerfectTool.MakePixelPerfect(_transform.localPosition, 16f);
    }



    private void StartSlipOff(float direction)
    {
        if (slipOffRoutine == null)
        {
            slipOffRoutine = StartCoroutine(SlipOff(direction));
        }
    }



    private void StopSlipOff()
    {
        if (slipOffRoutine != null)
        {
            StopCoroutine(slipOffRoutine);
        }
    }


    private IEnumerator SlipOff(float direction)
    {
        bodyState = BodyState.SlipsOff;

        _transform.localScale = new Vector2(direction * _transform.right.x, 1f);
        _rigidbody.velocity = new Vector2(0f, 0f);

        StartedSlippingOff?.Invoke();


        yield return new WaitForSeconds(slipping.timeOfSlip);


        bodyState = BodyState.InFlight;

        _animator.SetBool("OnGround", false);
        _animator.SetBool("Flight", true);

        _transform.localScale = new Vector2(1f, 1f);
        _transform.localPosition += new Vector3(_transform.right.x * slipping.slipOffset.x * direction, slipping.slipOffset.y, 0f);
        _transform.parent = jumping.defaultParent;
        _rigidbody.velocity = -_transform.up * jumping.jumpForce / 4f;

        slipOffRoutine = null;
    }



    private void ToLose()
    {
        _rigidbody.simulated = false;
        _animator.SetBool("Losing", true);
        _audioSource.PlayOneShot(sounds.losing);

        StopSlipOff();
        StopEmittingTrail();

        Lost?.Invoke();
    }



    private bool ItIsUnder()
    {
        return Physics2D.Raycast(landing.leftCheckPoint.position,  -_transform.up, 1f, landing.groundLayer) ||
               Physics2D.Raycast(landing.rightCheckPoint.position, -_transform.up, 1f, landing.groundLayer);
    }



    private bool ItIsStablePosition()
    {
        return Physics2D.Raycast(landing.middleCheckPoint.position, -_transform.up, 1f, landing.groundLayer);
    }



    private void CheckVerticalSpeed()
    {
        float vertSpeedAbs = Mathf.Abs(_rigidbody.velocity.y);

        if (vertSpeedAbs > flight.flightStateTransitionThreshold)
        {
            bodyState = BodyState.InFlight;
        }

        _animator.SetBool("Flight", vertSpeedAbs > flight.flightStateTransitionThreshold);
    }



    private void StartEmittingTrail()
    {
        flight.trail.transform.parent = _transform;
        flight.trail.transform.localPosition = new Vector2(0f, -0.25f);
        flight.trail.Clear();
        flight.trail.emitting = true;
    }



    private void StopEmittingTrail()
    {
        flight.trail.transform.parent = jumping.defaultParent;
        flight.trail.emitting = false;
    }
}
