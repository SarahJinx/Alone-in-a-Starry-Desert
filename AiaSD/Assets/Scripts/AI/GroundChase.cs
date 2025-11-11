using UnityEngine;
using UnityEngine.AI;

// Este estado controla la persecucion del enemigo al jugador
// Mientras vea al jugador lo seguira, si lo pierde volvera a patrullar
[CreateAssetMenu(fileName = "GroundChase", menuName = "ScriptableObjects/States/GroundChase")]
public class GroundChase : State
{
    // Cada cuanto tiempo se recalcula el destino hacia el jugador
    public float repathInterval = 0.1f;

    // Tiempo que espera tras perder de vista al jugador antes de volver a patrullar
    public float loseSightDelay = 2.0f;

    // Contadores internos para medir el tiempo
    float repathTimer;
    float lostTimer;
    bool sawThisFrame;

    // Metodo que se ejecuta cada frame mientras este estado este activo
    public override State Run(GameObject owner)
    {
        // Obtiene el NavMeshAgent para poder moverse
        var agent = owner.GetComponent<NavMeshAgent>();
        // Si no existe el agente no puede moverse
        if (agent == null) return this;

        // Obtiene el componente PlayerRef con la referencia al jugador
        var pref = owner.GetComponent<PlayerRef>();
        // Si no tiene referencia al jugador no puede perseguir
        if (pref == null || pref.player == null) return this;

        // Reinicia la variable que indica si vio al jugador este frame
        sawThisFrame = false;

        // Comprueba si el enemigo puede ver al jugador usando la accion asignada (CanSeePlayer)
        if (action != null && action.Length > 0 && action[0] != null && action[0].Check(owner))
        {
            // Si lo ve reinicia el contador de perdida
            sawThisFrame = true;
            lostTimer = 0f;
        }
        else
            // Si no lo ve suma tiempo al contador de perdida
            lostTimer += Time.deltaTime;

        // Aumenta el contador para controlar cuando recalcular el destino
        repathTimer += Time.deltaTime;
        // Si paso el intervalo programado actualiza el destino
        if (repathTimer >= repathInterval)
        {
            // El destino sera la posicion actual del jugador
            agent.SetDestination(pref.player.transform.position);
            // Reinicia el contador de recalculo
            repathTimer = 0f;
        }

        // Si no vio al jugador durante mas tiempo del permitido vuelve al estado de patrulla
        if (!sawThisFrame && lostTimer >= loseSightDelay)
            return nextState != null && nextState.Length > 0 ? nextState[0] : this;

        // Si lo sigue viendo mantiene la persecucion
        return this;
    }
}
