using Units;

namespace Tiles
{
    public interface IWalkable
    {
        public bool IsWalkable();
        public void MoveUnit(BaseUnit baseUnit);
    }
}