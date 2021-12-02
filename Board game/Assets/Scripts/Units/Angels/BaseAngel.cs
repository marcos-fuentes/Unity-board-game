namespace Units.Angels {
    public class BaseAngel : BaseUnit {
        private void Awake() {
            unitName = GetType().Name;
            faction = Faction.Angels;
        }
    }
}