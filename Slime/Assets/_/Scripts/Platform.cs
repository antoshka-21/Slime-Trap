using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Moving moving = new Moving();
    [SerializeField] private Collisions collisions = new Collisions();
    [SerializeField] private Sounds sounds = new Sounds();



    [System.Serializable]
    private struct Moving
    {
        public float movementBorder;
    }



    [System.Serializable]
    private struct Collisions
    {
        public ObjectContainer effectsContainer;
        public Vector2 effectOffset;
    }



    [System.Serializable]
    private struct Sounds
    {
       public AudioClip collisionClip;
    }



    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;



    private float direction;
    private float speed;



    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }



    private void Update()
    {
        Move();
    }



    public void SetSpeed(float value)
    {
        direction = Random.value > 0.5f ? 1f : -1f;
        speed = value;
    }



    private void Move()
    {
        if (_transform.position.x < -moving.movementBorder)
        {
            _transform.position = new Vector2(-moving.movementBorder, _transform.position.y);
            TurnAround(1f);
        }

        else if (_transform.position.x > moving.movementBorder)
        {
            _transform.position = new Vector2(moving.movementBorder, _transform.position.y);
            TurnAround(-1f);
        }

        _transform.Translate(_transform.right * speed * direction * Time.deltaTime);
    }



    private void TurnAround(float newDirection)
    {
        Transform collEffect = collisions.effectsContainer.GetObject().transform;

        collEffect.transform.localScale = new Vector2(direction, 1f);
        collEffect.position = _transform.position + new Vector3(collisions.effectOffset.x * direction,
                                                                collisions.effectOffset.y, 0f);

        direction = newDirection;

        _audioSource.PlayOneShotWithRandomPitch(sounds.collisionClip, 0.8f, 1.2f);
    }
}
