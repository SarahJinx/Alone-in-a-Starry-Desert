using UnityEngine;
/// Controlador de estados. Vive como componente en el enemigo.
public class StateMachine : MonoBehaviour
{
    public State startingState;
    State current;
    void Awake(){ current = startingState; }
    void Update(){
        if(current==null) return;
        State next = current.Run(gameObject);
        if(next != current) current = next;
    }
}