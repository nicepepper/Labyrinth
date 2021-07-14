using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class MaleDummyMovement : MonoBehaviour
{
    [SerializeField] private MovementCharacteristics _characteristics;
    [SerializeField] private Transform _camera;

    private readonly string STR_VERTICAL = "Vertical";
    private readonly string STR_HORIZONTAL = "Horizontal";
    private readonly string STR_RUN = "Run";
    private readonly string STR_JUMP = "Jump";

    private CharacterController _characterController;
    private Animator _animator;
    private float _vertical;
    private float _horizontal;
    private float _run;
    private Vector3 _direction;
    private Quaternion _look;

    private const float _distanceOffsetCamera = 15f;
    private Vector3 TargetRotate => _camera.forward * _distanceOffsetCamera;
    private bool _isIdle => _vertical == 0.0f && _horizontal == 0.0f;
    public event Action<bool> Pause = delegate {}; 
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        Cursor.visible = _characteristics.VisibleCursor;
        
    }

    private void Update()
    {
        Movement();
        Rotate();
        SetPauseGame();
    }
    
    public void SetPauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause?.Invoke(true);
        }
    }

    public void SetCamera(Transform _transform)
    {
        _camera = _transform;
    }

    private void Movement()
    {
        if (_characterController.isGrounded)
        {
            _horizontal = Input.GetAxis(STR_HORIZONTAL);
            _vertical = Input.GetAxis(STR_VERTICAL);
            _run = Input.GetAxis(STR_RUN);

            _direction = transform.TransformDirection(_horizontal, 0, _vertical).normalized;
            
            PlayAnimation();
            Jump();
        }
        
        _direction.y -= _characteristics.Gravity * Time.deltaTime;
        
        float speed = _run * _characteristics.RunSpeed + _characteristics.MovementSpeed;
        Vector3 dir = _direction * speed * Time.deltaTime;

        dir.y = _direction.y;
        _characterController.Move(dir);
    }

    private void Rotate()
    {
        if (_isIdle)
        {
            return;
        }
        
        Vector3 target = TargetRotate;
        target.y = 0;
        _look = Quaternion.LookRotation(target);
        float speed = _characteristics.AngularSpeed * Time.deltaTime;
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _look, speed);
    }

    public void Jump()
    {
        if (Input.GetButtonDown(STR_JUMP))
        {
            _animator.SetTrigger(STR_JUMP);
            _direction.y += _characteristics.JumpForce;
        }
    }

    private void PlayAnimation()
    {
        float vertical = _run * _vertical + _vertical;
        float horizontal = _run * _horizontal + _horizontal;
    
        _animator.SetFloat(STR_VERTICAL, vertical);
        _animator.SetFloat(STR_HORIZONTAL, horizontal);
    }
}
