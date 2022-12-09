﻿using MonoMod.Core.Utils;
using MonoMod.Utils;
using System;

namespace MonoMod.Core.Platforms {
    public interface IArchitecture {
        ArchitectureKind Target { get; }
        ArchitectureFeature Features { get; }

        BytePatternCollection KnownMethodThunks { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <para>This must only be accessed if <see cref="Features"/> includes <see cref="ArchitectureFeature.CreateAltEntryPoint"/>.</para>
        /// </remarks>
        IAltEntryFactory AltEntryFactory { get; }

        NativeDetourInfo ComputeDetourInfo(IntPtr from, IntPtr to, int maxSizeHint = -1);
        /// <summary>
        /// Gets the actual bytes making up the specified detour.
        /// </summary>
        /// <param name="info">The <see cref="NativeDetourInfo"/> representing the detour.</param>
        /// <param name="buffer">A buffer which will hold the byte sequence. It must be at least <see cref="NativeDetourInfo.Size"/> bytes in length.</param>
        /// <param name="allocationHandle">A handle to any allocation which must stay alive with the detour.</param>
        /// <returns>The number of bytes written to the buffer.</returns>
        int GetDetourBytes(NativeDetourInfo info, Span<byte> buffer, out IDisposable? allocationHandle);

        NativeDetourInfo ComputeRetargetInfo(NativeDetourInfo detour, IntPtr to, int maxSizeHint = -1);

        int GetRetargetBytes(NativeDetourInfo original, NativeDetourInfo retarget, Span<byte> buffer,
            out IDisposable? allocationHandle, out bool needsRepatch, out bool disposeOldAlloc);

        /// <summary>
        /// Populates a native vtable with proxy stubs to an object with the same vtable shape.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The expected layout for the proxy object is this:
        /// <list type="u">
        ///     <item><c><see cref="IntPtr.Size"/> * 0</c></item><description><c> = pVtbl</c> A pointer to the vtable generated by this method.</description>
        ///     <item><c><see cref="IntPtr.Size"/> * 1</c></item><description><c> = pWrapped</c> A pointer to the wrapped object.</description>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="vtableBase">The base pointer for the vtable to fill. This must be large enough to hold <paramref name="vtableSize"/> entries.</param>
        /// <param name="vtableSize">The number of vtable entries to fill.</param>
        /// <returns>A collection of <see cref="IAllocatedMemory"/> which contain the stubs referenced by the generated vtable.</returns>
        ReadOnlyMemory<IAllocatedMemory> CreateNativeVtableProxyStubs(IntPtr vtableBase, int vtableSize);

        /// <summary>
        /// Creates an architecture-specific special entry stub, that passes an extra argument not in the normal calling convention.
        /// </summary>
        /// <param name="target">The target to call.</param>
        /// <param name="argument">The extra argument for that target.</param>
        /// <returns>An <see cref="IAllocatedMemory"/> containing the generated stub.</returns>
        IAllocatedMemory CreateSpecialEntryStub(IntPtr target, IntPtr argument);
    }

    public interface INativeDetourKind {
        int Size { get; }
    }

    public readonly record struct NativeDetourInfo(IntPtr From, IntPtr To, INativeDetourKind InternalKind, IDisposable? InternalData) {
        public int Size => InternalKind.Size;
    }
}
