using UnityEngine;

public class InteractionPlayer : MonoBehaviour
{
    private IInteractable _currentInteractionObject;
    private GameObject _interactionGameObject;
    
    public IInteractable CurrentInteractionObject
    {
        get => _currentInteractionObject;
        
        private set 
        { 
            _currentInteractionObject = value;

            if (_currentInteractionObject != null)
            {
                ShowInteractionButton();
            }
            else
            {
                HideInteractionButton();
            }
        }
    }

    private void ShowInteractionButton () 
    {

    }

    private void HideInteractionButton () 
    {

    }

    private void InteractionButtonClick () 
    {
        if (CurrentInteractionObject != null) 
        {
            CurrentInteractionObject.Action();
            CurrentInteractionObject = null;
        }
    }

    private void OnTriggerEnter (Collider collider) 
    {
        IInteractable Interaction = collider.gameObject.GetComponent<IInteractable>();
        
        if (Interaction != null) 
        {
            _interactionGameObject = collider.gameObject;
            CurrentInteractionObject = Interaction;
        }
    }

    private void OnTriggerExit (Collider collider) 
    {
        if (collider.gameObject == _interactionGameObject)
        {
            CurrentInteractionObject = null;
        }
    }
}
