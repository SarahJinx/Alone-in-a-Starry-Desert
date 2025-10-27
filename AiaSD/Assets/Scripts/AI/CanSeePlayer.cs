using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CanSeePlayer", menuName = "ScriptableObjects/Actions/CanSeePlayer")]
public class CanSeePlayer : Action
{
    public float viewAngle = 60f;
    public float viewDistance = 25f;
    public LayerMask obstacleMask;

    public override bool Check(GameObject owner)
    {
        var pref = owner.GetComponent<PlayerRef>();
        if (pref == null || pref.player == null) return false;

        Transform eyes = owner.transform;
        Vector3 toTarget = pref.player.transform.position - eyes.position;

        if (toTarget.sqrMagnitude > viewDistance * viewDistance) return false;

        float angle = Vector3.Angle(eyes.forward, toTarget.normalized);
        if (angle > viewAngle * 0.5f) return false;

        if (Physics.Raycast(eyes.position, toTarget.normalized, out RaycastHit hit, viewDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.transform.IsChildOf(pref.player.transform)) return true;
            if ((obstacleMask.value & (1 << hit.collider.gameObject.layer)) != 0) return false;
            return false;
        }
        return false;
    }
}
