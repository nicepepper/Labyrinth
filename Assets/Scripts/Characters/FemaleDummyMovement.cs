using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class FemaleDummyMovement : MonoBehaviour
{
   [SerializeField] private MovementCharacteristics _characteristics;
   [SerializeField] private Transform _target;

   private readonly string STR_VERTICAL = "Vertical";
   private CharacterController _characterController;
   private Animator _animator;
  
   private Dictionary<string, float> _state = new Dictionary<string, float>()
   {
       {"Idle", 0.0f},
       {"Walk", 1.0f},
       {"Run", 2.0f}
   };
   
   private Vector3 _direction;
   private Quaternion _look;
   private float _currentState;
   private Vector3 _transformPosition;
   private Vector3 _targetPosition;

   private void Start()
   {
      _characterController = GetComponent<CharacterController>();
      _animator = GetComponent<Animator>();
      
      _currentState = _state["Idle"];
   }

   private void Update()
   {
       Movement();
       Rotate();
       PlayAnimation();
   }

   public void SetTarget(Transform transform)
   {
       _target = transform;
   }

   public bool IsTarget()
   {
       return _target;
   }

   private void Movement()
   {
       if (_target)
       {
           _direction = transform.TransformDirection(0, 0, 1).normalized;
           float speed = _characteristics.RunSpeed;
           Vector3 dir = _direction * speed * Time.deltaTime;

           _targetPosition = _target.position;
           _targetPosition.y = 0.0f;

           _transformPosition = transform.position;
           _transformPosition.y = 0.0f;
       
           if (Vector3.Distance(_targetPosition, _transformPosition) >= speed * Time.deltaTime)
           {
               _characterController.Move(dir);
               _currentState = _state["Run"];
           }
           else
           {
               _currentState = _state["Idle"];
               _target = null;
           }
       }
   }

   private void Rotate()
   {
       if (_target)
       {
           Vector3 target = _target.transform.position - transform.position;
           target.y = 0;
           _look = Quaternion.LookRotation(target);
           var speed = _characteristics.AngularSpeed * Time.deltaTime;
       
           transform.rotation = Quaternion.RotateTowards(transform.rotation, _look, speed);
       }
   }
   
   private void PlayAnimation()
   {
     _animator.SetFloat(STR_VERTICAL, _currentState);
   }
}
