using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
   private Animator _animator;
   public bool IsOpened { get; private set; }
   
   private void Start()
   {
      _animator = transform.GetComponent<Animator>();
   }

   public void Action()
   {
      if (IsOpened)
      {
         Close();
      }
      else
      {
         Open();
      }
   }
   
   public void Open () 
   {
      IsOpened = true;
      _animator.SetBool("isOpened", IsOpened);
   }

   public void Close () 
   {
      IsOpened = false;
      _animator.SetBool("isOpened", IsOpened);
   }
}
