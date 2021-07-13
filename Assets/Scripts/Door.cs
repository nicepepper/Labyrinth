using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
   [SerializeField] private int _idDoor;

   private Animator _animator;
   private bool _isOpened;
   private Transform _transform;
   
   public event Action<int> OnOpen = delegate {}; 

   private void Start()
   {
      _animator = transform.GetComponent<Animator>();
      _transform = transform.GetComponent<Transform>();
   }

   // private void Update()
   // {
   //    Debug.Log(_transform.position + "," + _idDoor);
   // }

   public void Action()
   {
      if (!_isOpened)
      {
         Open();
      }
   }

   public bool IsActed()
   {
      return _isOpened;
   }

   public void Open () 
   {
      _isOpened = true;
      _animator.SetBool("isOpened", _isOpened);
      OnOpen?.Invoke(_idDoor);
   }

   public void Close () 
   {
      _isOpened = false;
      _animator.SetBool("isOpened", _isOpened);
   }

   public int IdDoor => _idDoor;
}
