using ResourceManager;
using UnityEngine;
using UnityEngine.UI;

namespace Addressable.Example
{
    public class Example : MonoBehaviour
    {
        [SerializeField] private Image image;

        void Start()
        {
            image.LoadSprite(SpriteCategory.UI, null);
        }
    }
}
