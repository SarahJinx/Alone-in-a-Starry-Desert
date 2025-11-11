using UnityEngine;

// Esta clase comprueba si el enemigo puede ver al jugador
// Usa distancia, angulo de vision y un raycast para detectar si hay linea de vision
[CreateAssetMenu(fileName = "CanSeePlayer", menuName = "ScriptableObjects/Actions/CanSeePlayer")]
public class CanSeePlayer : Action
{
    // Angulo total de vision del enemigo (por ejemplo 90 grados significa 45 a cada lado del frente)
    public float viewAngle = 90f;

    // Distancia maxima a la que el enemigo puede ver al jugador
    public float viewDistance = 20f;

    // Capas que bloquean la vision (por ejemplo paredes u obstaculos)
    public LayerMask obstacleMask;

    // Metodo que comprueba si el enemigo ve al jugador
    // Devuelve true si el jugador esta dentro del campo de vision y no hay obstaculos entre ellos
    public override bool Check(GameObject owner)
    {
        // Busca el componente PlayerRef que contiene la referencia al jugador
        var pref = owner.GetComponent<PlayerRef>();
        // Si no hay referencia al jugador devuelve false
        if (pref == null || pref.player == null) return false;

        // Usa el transform del enemigo como posicion de los ojos
        Transform eyes = owner.transform;

        // Calcula la direccion desde el enemigo hacia el jugador
        Vector3 toTarget = pref.player.transform.position - eyes.position;

        // Comprueba si el jugador esta dentro de la distancia de vision
        // Usa sqrMagnitude para evitar el costo de la raiz cuadrada
        if (toTarget.sqrMagnitude > viewDistance * viewDistance) return false;

        // Calcula el angulo entre la direccion hacia adelante del enemigo y la direccion hacia el jugador
        float angle = Vector3.Angle(eyes.forward, toTarget.normalized);
        // Si el angulo es mayor que la mitad del campo de vision el jugador esta fuera de la vista
        if (angle > viewAngle * 0.5f) return false;

        // Lanza un raycast desde los ojos hacia el jugador
        // Si el rayo choca con algo se guarda la informacion en hit
        if (Physics.Raycast(eyes.position, toTarget.normalized, out RaycastHit hit, viewDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            // Si el rayo golpea al jugador directamente devuelve true
            if (hit.collider.transform.IsChildOf(pref.player.transform)) return true;

            // Si el objeto golpeado pertenece a la capa de obstaculos devuelve false
            if ((obstacleMask.value & (1 << hit.collider.gameObject.layer)) != 0) return false;

            // Si golpea cualquier otra cosa devuelve false
            return false;
        }

        // Si el raycast no golpea nada devuelve false
        return false;
    }
}
