using Units;

namespace Tiles
{
    public interface IWalkable
    {
        public bool Walkable();

        public bool Spawnable();
        public void MoveUnit(BaseUnit baseUnit);
    }
}