using UnityEngine;

[CreateAssetMenu(fileName = "Attack_", menuName = "Battle/Attack")]
public class Attack : ScriptableObject
{
    public string attackName;
    public int useQuantMax;
}