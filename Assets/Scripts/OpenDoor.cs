using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class OpenDoor : MonoBehaviour {

    [SerializeField] private float _smooth = 2.0f;
    [SerializeField] private float _openingAngle = 90.0f;

    private Vector3 _defaultRot;
    private Vector3 _openRot;
    private bool _open;
    private bool _enter;
    
    void Start () 
    {
		_defaultRot = transform.eulerAngles;
        _openRot = new Vector3 (_defaultRot.x, _defaultRot.y + _openingAngle, _defaultRot.z);
    }
    
    void Update () 
    {
        if (_open) 
        {
            transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, _openRot, Time.deltaTime * _smooth);
        } 
        else 
        {
            transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, _defaultRot, Time.deltaTime * _smooth);
        }
        
        if (Input.GetKeyDown (KeyCode.F) && _enter) {
            _open = !_open;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player") 
        {
            _enter = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player") 
        {
            _enter = false;
        }
    }
}