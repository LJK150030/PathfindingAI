using UnityEngine;

namespace Assets.Scripts.Hexagons
{
    public class HexagonTile : MonoBehaviour {
        private SpriteRenderer _image;
        private int _kind;

        private void Awake()
        {
            _kind = 1;
            _image = GetComponent<SpriteRenderer>();
        }

        public void SetKind(int i)
        {
            _kind = i;
            UpdateImage();
        }

        public int GetKind()
        {
            return _kind;
        }

        private void UpdateImage()
        {
            switch (_kind)
            {
                case 0:
                    _image.color = Color.black;
                    break;
                case 1:
                    _image.color = Color.white;
                    break;
                case 2:
                    _image.color = Color.green;
                    break;
                case 3:
                    _image.color = Color.red;
                    break;
                case 4: //in the frontier
                    _image.color = Color.gray;
                    break;
                case 5: //in the explored
                    _image.color = Color.blue;
                    break;
                case 6: //path
                    _image.color = Color.yellow;
                    break;
                default:
                    _image.color = Color.clear;
                    break;
            }
        }
    }
}
