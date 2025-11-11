using UnityEngine;
/// Clase base abstracta para cualquier "condiciÃ³n" que use la IA.
public abstract class Action : ScriptableObject
{
    public abstract bool Check(GameObject owner);
}