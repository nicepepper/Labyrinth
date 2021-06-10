using System;
using UnityEngine;

public class InteractionPlayer : MonoBehaviour
{
    private KeyCode STR_KEYBOARD_INTERACTION_BUTTON = KeyCode.E;
    private KeyCode STR_MOUSE_INTERACTION_BUTTON = KeyCode.Mouse0;
    
    private GameObject _interactionGameObject;

    public IInteractable CurrentInteractionObject { get; private set; }

    private void Update()
    {
        Interact();
    }


    private void Interact () 
    {
        if (CurrentInteractionObject != null) 
        {
            if (Input.GetKeyDown(STR_KEYBOARD_INTERACTION_BUTTON) || (Input.GetKeyDown(STR_MOUSE_INTERACTION_BUTTON)))
            {
                CurrentInteractionObject.Action();
            }
        }
    }

    private void OnTriggerEnter (Collider col) 
    {
        IInteractable interaction = col.gameObject.GetComponent<IInteractable>();
        
        if (interaction != null) 
        {
            _interactionGameObject = col.gameObject;
            CurrentInteractionObject = interaction;
        }
        
    }

    private void OnTriggerExit (Collider col) 
    {
        if (col.gameObject == _interactionGameObject)
        {
            CurrentInteractionObject = null;
        }
    }
}
