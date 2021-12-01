using UnityEngine;

namespace Tiles
{
    /**
 * Script class for the Tiles that are walkable
 */
    public class GrassBaseTile : BaseTile, IWalkable
    {
        [SerializeField] private Color _baseColor, _offsetColor;
        public override void Init(int horizontalX, int horizontalY) {
            //To set one of each tiles with one color to make it more visual
            //TOOD: Change this for tiles with sprites
            var isOffset = (horizontalX + horizontalY) % 2 == 1;
            _renderer.color = isOffset ? _offsetColor : _baseColor;
        }

        public bool IsWalkable()
        {
            return _tileUnit == null;
        }


        public void MoveUnit(BaseUnit baseUnit)
        {
            throw new System.NotImplementedException();
        }
    }
}