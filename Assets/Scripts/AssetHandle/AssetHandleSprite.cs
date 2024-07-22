using UnityEngine;
using UnityEngine.UI;

namespace ResourceManager
{
    internal class AssetHandleSprite : AssetHandle<Sprite>
    {
        [SerializeField]
        private AspectRatioFitter _aspectRatioFitter;

        protected override void OnAssetLoaded(Sprite asset)
        {
           if(_aspectRatioFitter)
            {
                Rect rect = asset.rect;
                _aspectRatioFitter.aspectRatio = rect.width / rect.height;
            }
        }
    }
}