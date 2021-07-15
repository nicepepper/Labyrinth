using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollide : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _damageToSelf = 10;

    private void HitObject(GameObject theObject)
    {
        var theirDamage = theObject.GetComponentInParent<DamageTaking>();
        if (theirDamage)
        {
            theirDamage.TakeDamage(_damage);
        }

        var ourDamage = this.GetComponent<DamageTaking>();
        if (ourDamage)
        {
            ourDamage.TakeDamage(_damageToSelf);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter" + collider.gameObject.name);
        HitObject(collider.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        HitObject(collision.gameObject);
    }
}
