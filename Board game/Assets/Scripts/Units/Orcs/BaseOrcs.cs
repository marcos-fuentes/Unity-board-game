namespace Units.Orcs {
    public class BaseOrcs : BaseUnit
    {
        private void Awake() {
            unitName = GetType().Name;
            faction = Faction.Orcs;
        }
    }
}