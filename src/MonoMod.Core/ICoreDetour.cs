﻿using MonoMod.Utils;
using System;
using System.Reflection;

namespace MonoMod.Core
{
    /// <summary>
    /// A single method-to-method managed detour.
    /// </summary>
    [CLSCompliant(true)]
    public interface ICoreDetour : ICoreDetourBase
    {
        /// <summary>
        /// The source method.
        /// </summary>
        MethodBase Source { get; }
        /// <summary>
        /// The target method.
        /// </summary>
        MethodBase Target { get; }
    }

    /// <summary>
    /// An <see cref="ICoreDetour"/> that additionally provides <see cref="SourceMethodClone"/>.
    /// </summary>
    /// <remarks>
    /// An <see cref="ICoreDetour"/> may implement this interface without actually providing a source clone.
    /// </remarks>
    /// <seealso cref="CreateDetourRequest.CreateSourceCloneIfNotILClone"/>
    public interface ICoreDetourWithClone : ICoreDetour
    {
        /// <summary>
        /// A clone of <see cref="ICoreDetour.Source"/>, which behaves as-if it had not been detoured.
        /// </summary>
        /// <remarks>
        /// This method will not be available unless <see cref="CreateDetourRequest.CreateSourceCloneIfNotILClone"/> was
        /// set when the detour was created. If the <see cref="IDetourFactory"/> does not support that option,
        /// this may not be set anyway.
        /// </remarks>
        /// <seealso cref="CreateDetourRequest.CreateSourceCloneIfNotILClone"/>
        MethodInfo? SourceMethodClone { get; }


#pragma warning disable CA1200 // Avoid using cref tags with a prefix
        /// <summary>
        /// A <see cref="DynamicMethodDefinition"/> that contains the IL for <see cref="SourceMethodClone"/>, if it has any.
        /// </summary>
        /// <remarks>
        /// <para>This may be <see langword="null"/> even if <see cref="SourceMethodClone"/> is not. This represents a method
        /// which has no (meaningful) IL body. Clients should be sure to handle this case. For instance, RuntimeDetour treats
        /// this case as meaning that <see cref="T:MonoMod.RuntimeDetour.ILHook"/>s cannot be applied to the method.</para>
        /// </remarks>
        DynamicMethodDefinition? SourceMethodCloneIL { get; }
#pragma warning restore CA1200 // Avoid using cref tags with a prefix
    }
}
