using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField]
    private float weaponLenght;
    [SerializeField]
    private LayerMask hitLayerMask;

    private bool _canDealDamage = false;
    private List<GameObject> _damageHits = new List<GameObject>();

    public void StartDealDamage()
    {
        _canDealDamage = true;
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

        if(Physics.Raycast(transform.position, -transform.up, out hit, weaponLenght, hitLayerMask))
        {
            if (!_damageHits.Contains(hit.transform.gameObject))
            {
                Actor actor = (Actor)hit.transform.gameObject.GetComponent(typeof(Actor));
                
                if (actor == null)
                    return;

                if (actor.GetComponentInChildren(typeof(NPC)))
                    if (!actor.GetComponentInChildren<NPC>().IsAlive)
                        return;

                actor.TakeDamage(GetComponentInParent<Weapon>().Damage, 
                    GetComponentInParent<Weapon>().GetComponentInParent<Actor>().transform.gameObject);

                _damageHits.Add(hit.transform.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLenght);
    }
}
