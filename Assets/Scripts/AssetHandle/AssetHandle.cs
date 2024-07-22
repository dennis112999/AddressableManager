using System;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

using UObject = UnityEngine.Object;

namespace ResourceManager
{
    internal static class AssetHandle
    {
        public static async void LoadSprite(this SpriteRenderer spriteRenderer, string address)
        {
            if (!spriteRenderer || string.IsNullOrEmpty(address))
            {
                return;
            }

            try
            {
                await InternalLoadAsync<Sprite, SpriteRenderer, AssetHandleSprite>(spriteRenderer, address, (r, a) => r.sprite = a);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static async void LoadSprite(this Image image, string address)
        {
            if (!image || string.IsNullOrEmpty(address))
            {
                return;
            }

            try
            {
                await InternalLoadAsync<Sprite, Image, AssetHandleSprite>(image, address, (i, a) => i.sprite = a);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static async void LoadTexture(this RawImage image, string address)
        {
            if (!image || string.IsNullOrEmpty(address))
            {
                return;
            }

            try
            {
                await InternalLoadAsync<Texture2D, RawImage, AssetHandleTexture2D>(image, address, (i, a) => i.texture = a);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private static async UniTask InternalLoadAsync<TAsset, TTarget, THandle>(TTarget target, string address, Action<TTarget, TAsset> callback)
            where TAsset : UObject
            where TTarget : Component
            where THandle : AssetHandle<TAsset>
        {
            THandle handle = target.GetComponent<THandle>();
            if (!handle)
            {
                handle = target.gameObject.AddComponent<THandle>();
            }

            TAsset asset = await handle.LoadAsync(address);

            if (asset)
            {
                callback.Invoke(target, asset);
            }
        }
    }

    internal abstract class AssetHandle<T> : MonoBehaviour
        where T : UObject
    {
        private AsyncOperationHandle<T> _handle;
        private AsyncOperationHandle<T> _loadingHandle;

        public async UniTask<T> LoadAsync(string address)
        {
            AsyncOperationHandle<T> oldHandle = _handle;
            AsyncOperationHandle<T> newHandle = Addressables.LoadAssetAsync<T>(address);

            _loadingHandle = newHandle;

            T asset = await newHandle;

            if (!_loadingHandle.Equals(newHandle)) // 对象已销毁或被异步期间的LoadAsync调用覆盖
            {
                Addressables.Release(newHandle);
                return null;
            }

            if (oldHandle.IsValid())
            {
                Addressables.Release(oldHandle);
            }

            _handle = newHandle;

            if (asset)
            {
                OnAssetLoaded(asset);
            }

            return asset;
        }

        protected virtual void OnAssetLoaded(T asset) { }

        protected virtual void OnDestroy()
        {
            _loadingHandle = default;

            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
                _handle = default;
            }
        }
    }
}
