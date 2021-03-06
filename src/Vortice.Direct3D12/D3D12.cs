﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpGen.Runtime;
using Vortice.Direct3D;

namespace Vortice.Direct3D12
{
    public static class D3D12
    {
        /// <summary>
        /// Test to create <see cref="ID3D12Device"/> without actually creating it.
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="minFeatureLevel"></param>
        /// <returns></returns>
        public static Result D3D12CreateDevice(IUnknown adapter, FeatureLevel minFeatureLevel)
        {
            return D3D12Internal.CreateDevice(adapter, minFeatureLevel, typeof(ID3D12Device).GUID, out var nativePtr);
        }

        public static Result D3D12CreateDevice(IUnknown adapter, FeatureLevel minFeatureLevel, out ID3D12Device device)
        {
            var result = D3D12Internal.CreateDevice(adapter, minFeatureLevel, typeof(ID3D12Device).GUID, out var nativePtr);
            if (result.Failure)
            {
                device = null;
                return result;
            }

            device = new ID3D12Device(nativePtr);
            return result;
        }

        public static Result D3D12GetDebugInterface<T>(out T debugInterface) where T : ComObject
        {
            var result = D3D12Internal.GetDebugInterface(typeof(T).GUID, out var nativePtr);

            if (result.Failure)
            {
                debugInterface = null;
                return result;
            }

            debugInterface = CppObject.FromPointer<T>(nativePtr);
            return result;
        }
    }
}
