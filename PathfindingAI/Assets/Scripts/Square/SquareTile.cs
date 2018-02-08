using UnityEngine;

namespace Assets.Scripts.Square
{
    public class SquareTile : MonoBehaviour
    {

        private SpriteRenderer image;
        private int _kind;

        private void Awake()
        {
            _kind = 1;
            image = GetComponent<SpriteRenderer>();
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
                    image.color = Color.black;
                    break;
                case 1:
                    image.color = Color.white;
                    break;
                case 2:
                    image.color = Color.green;
                    break;
                case 3:
                    image.color = Color.red;
                    break;
                case 4: //in the frontier
                    image.color = Color.gray;
                    break;
                case 5: //in the explored
                    image.color = Color.blue;
                    break;
                case 6: //path
                    image.color = Color.yellow;
                    break;
                default:
                    image.color = Color.clear;
                    break;
            }
        }
    }
}
