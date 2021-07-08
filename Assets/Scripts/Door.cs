using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
   [SerializeField] private int _idDoor;

   private Animator _animator;
   private bool isOpened;
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
      if (!isOpened)
      {
         Open();
      }
   }

   public bool IsActed()
   {
      return isOpened;
   }

   public void Open () 
   {
      isOpened = true;
      _animator.SetBool("isOpened", isOpened);
      OnOpen?.Invoke(_idDoor);
   }

   public void Close () 
   {
      isOpened = false;
      _animator.SetBool("isOpened", isOpened);
   }
}
