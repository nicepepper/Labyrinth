using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
   public bool IsOpened { get; private set; }

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
   }

   public void Close () 
   {
      IsOpened = false;
   }
}
