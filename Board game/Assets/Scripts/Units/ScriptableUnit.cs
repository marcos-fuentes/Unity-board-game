using Units;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction Faction;
    public BaseUnit UnitPrefab;
}

public enum Faction
{
    Angels, Orcs
} 

public enum Class
{
    Magician, Assassin, Warrior, OrcRepair
} 