using UnityEngine;
/// Clase base abstracta para un ESTADO de IA (patrulla, persecuciÃ³n, etc.).
public abstract class State : ScriptableObject
{
    public Action[] action;    // Condiciones
    public State[] nextState;  // Estados destino emparejados
    public abstract State Run(GameObject owner);
}