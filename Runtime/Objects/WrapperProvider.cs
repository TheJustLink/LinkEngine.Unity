using System.Collections.Generic;

using UnityEngine;

using LinkEngine.Assets;
using LinkEngine.Components;
using LinkEngine.Unity.Assets;
using LinkEngine.Unity.Components;

namespace LinkEngine.Unity.Objects
{
    static class WrapperProvider
    {
        private static readonly Dictionary<System.Type, System.Type> s_wrapperTypes = new()
        {
            // Assets
            { typeof(ISprite), typeof(SpriteWrapper) },

            // Components
            { typeof(ITransform2D), typeof(TransformWrapper) },
            { typeof(ITransform), typeof(TransformWrapper) },
            { typeof(ISpriteRenderer), typeof(SpriteRendererWrapper) }
        };
        private static readonly Dictionary<System.Type, WrapperEntry> s_wrapperEntries = new();

        public static T CreateWrapper<T>(Object nativeObject) where T : class
        {
            var entry = GetWrapperEntry<T>();

            return entry.CreateWrapper<T>(nativeObject);
        }
        public static WrapperEntry GetWrapperEntry<T>()
        {
            var assetType = typeof(T);

            if (s_wrapperTypes.TryGetValue(assetType, out var wrapperType))
                return GetWrapperEntryBy(wrapperType);

            throw new System.NotImplementedException($"Asset type not supported: {assetType}");
        }
        public static WrapperEntry GetWrapperEntryBy(System.Type wrapperType)
        {
            if (s_wrapperEntries.TryGetValue(wrapperType, out var entry) == false)
                s_wrapperEntries[wrapperType] = entry = new WrapperEntry(wrapperType);

            return entry;
        }
    }
}