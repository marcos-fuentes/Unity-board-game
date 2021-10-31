using UnityEngine;

namespace Tiles
{
    /**
 * Script class for the Tiles that are walkable
 */
    public class GrassBaseTile : BaseTile
    {
        [SerializeField] private Color _baseColor, _offsetColor;
        public override void Init(int x, int y) {
            //To set one of each tiles with one color to make it more visual
            //TOOD: Change this for tiles with sprites
            var isOffset = (x + y) % 2 == 1;
            _renderer.color = isOffset ? _offsetColor : _baseColor;
        }
    }
}
