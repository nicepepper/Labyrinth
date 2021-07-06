using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
   private Animator _animator;
   private bool isOpened;
   
   private void Start()
   {
      _animator = transform.GetComponent<Animator>();
   }

   public void Action()
   {
      if (isOpened)
      {
         Close();
      }
      else
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
   }

   public void Close () 
   {
      isOpened = false;
      _animator.SetBool("isOpened", isOpened);
   }
}
