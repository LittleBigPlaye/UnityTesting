using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandle : MonoBehaviour
{
    [SerializeField] WeaponScriptableObject weaponStats;
    [Tooltip("Used to test via Raycast, if the target is hit by weapon")]
    [SerializeField] Transform weaponOrigin;
    [SerializeField] float weaponLength = 1f;

    private new Collider collider;
    public CombatController CombatController { get; set; }

    private TrailRenderer trailRenderer;
    private ParticleSystem trailParticles;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;

        // trailRenderer = GetComponentInChildren<TrailRenderer>();
        // trailRenderer.enabled = false;

        trailParticles = GetComponentInChildren<ParticleSystem>();
        trailParticles.Stop();
    }

    public void SetTriggerState(bool isEnabled)
    {
        collider.enabled = isEnabled;
        if (isEnabled)
        {
            trailParticles.Play();
        }
        else
        {
            trailParticles.Stop();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //make sure to only hit hitable objects, ignore the own hitbox
        if (other.GetComponent<IHitable>() != null && other != CombatController.Collider && IsInLayerMask(CombatController.HitableMask, other.gameObject))
        {

            if (CheckIfObstacleBlocksAttack(other.transform))
            {
                //TODO: trigger animation at attack source
            }
            else
            {
                Damage damage = new Damage(weaponStats.baseDamage);
                other.GetComponent<IHitable>().OnHit(damage, transform.position);
            }
        }
    }

    //checks if there is an obstacle between target and attack source
    private bool CheckIfObstacleBlocksAttack(Transform target)
    {
        if (Physics.Linecast(CombatController.transform.position, target.transform.position, CombatController.ObstacleMask))
        {
            return true;
        }
        return false;
    }

    private bool IsInLayerMask(LayerMask mask, GameObject gameObject)
    {
        return ((mask.value & (1 << gameObject.layer)) > 0);
    }
}