using UnityEngine;
using UnityEngine.AI;

// Este estado controla la patrulla del enemigo entre dos puntos A y B
// Si el enemigo ve al jugador cambiara al estado de persecucion
[CreateAssetMenu(fileName = "GroundPatrol", menuName = "ScriptableObjects/States/GroundPatrol")]
public class GroundPatrol : State
{
    // Distancia minima para considerar que el enemigo ha llegado a un punto
    public float stopDistance = 1.0f;

    // Este metodo se ejecuta cada frame mientras este estado este activo
    public override State Run(GameObject owner)
    {
        // Obtiene el componente NavMeshAgent del enemigo que permite moverse por el NavMesh
        var agent = owner.GetComponent<NavMeshAgent>();
        // Si no tiene agente devuelve este mismo estado y no hace nada
        if (agent == null) return this;

        // Obtiene el componente PatrolPoints que contiene los puntos A y B de patrulla
        var points = owner.GetComponent<PatrolPoints>();
        // Si no existen los puntos no se puede patrullar
        if (points == null || points.pointA == null || points.pointB == null) return this;

        // Obtiene o crea el componente PatrolABData que guarda si el enemigo va hacia A o hacia B
        var mem = owner.GetComponent<PatrolABData>();
        if (mem == null) mem = owner.AddComponent<PatrolABData>();

        // Calcula el destino actual segun si va hacia B o hacia A
        Vector3 target = mem.goingToB ? points.pointB.position : points.pointA.position;

        // Comprueba si el enemigo no tiene camino o ya llego al destino
        if (!agent.hasPath || agent.pathStatus != NavMeshPathStatus.PathComplete || agent.remainingDistance <= stopDistance)
        {
            // Si llego al destino y no esta calculando otro camino cambia de direccion
            if (agent.remainingDistance <= stopDistance && !agent.pathPending)
                mem.goingToB = !mem.goingToB;

            // Actualiza el destino con el nuevo punto
            target = mem.goingToB ? points.pointB.position : points.pointA.position;
            // Indica al agente que vaya al nuevo destino
            agent.SetDestination(target);
        }

        // Comprueba si se cumple alguna accion (por ejemplo ver al jugador)
        if (action != null && action.Length > 0 && action[0] != null && action[0].Check(owner))
            // Si la condicion es verdadera cambia al siguiente estado configurado (GroundChase)
            return nextState != null && nextState.Length > 0 ? nextState[0] : this;

        // Si no se cumplen condiciones sigue en patrulla
        return this;
    }
}
