using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField]
    private float weaponLength;
    [SerializeField]
    private LayerMask hitLayerMask;

    private bool _canDealDamage = false;
    private bool _doOnce = false;
    private List<GameObject> _damageHits = new List<GameObject>();

    public void StartDealDamage()
    {
        _canDealDamage = true;
        _doOnce = true;
        _damageHits.Clear();
    }

    public void EndDealDamage()
    {
        _canDealDamage = false;
    }

    void Update()
    {
        if (!_canDealDamage)
            return;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, hitLayerMask))
        {
            if (!_damageHits.Contains(hit.transform.gameObject))
            {
                Actor actor = null;
                if (!hit.transform.gameObject.TryGetComponent(out actor))
                    return;

                if (actor is NPC && !((NPC)actor).IsAlive)
                    return;

                Weapon weapon = null;
                weapon = GetComponentInParent<Weapon>();
                actor.TakeDamage(weapon.Damage, weapon.GetComponentInParent<Actor>().transform.gameObject);

                if (_doOnce)
                {
                    weapon.RemoveDurability(1);
                    _doOnce = false;
                }

                _damageHits.Add(hit.transform.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
