using System;
using Xproof.Abstractions.TestProofForTestMethods;
using Xproof.ProbeCore.ProbeOutcome;

namespace Xproof.ProbeCore
{
    /// <summary>
    /// Helper class for working with proof results in Xproof core units.
    /// </summary>
    public static partial class ProofHelper
    {
        /// <summary>
        /// Creates an action that unwraps a proof result and executes the provided action with the unwrapped value.
        /// </summary>
        public static Action Action<T1>(IProbeOutcome<T1> arg1, Action<T1> act)
        {
            if (arg1 is not IProbeOutcomeInternal<T1> internal1)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg1)}\" does not support unwrapping: {arg1?.GetType().FullName}");

            return () => act(internal1.Unwrap());
        }

        /// <summary>
        /// Creates an action that unwraps a proof result and executes the provided action with the unwrapped value.
        /// </summary>
        public static Action Action<T1, T2>(IProbeOutcome<T1> arg1, IProbeOutcome<T2> arg2, Action<T1, T2> act)
        {
            if (arg1 is not IProbeOutcomeInternal<T1> internal1)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg1)}\" does not support unwrapping: {arg1?.GetType().FullName}");
            if (arg2 is not IProbeOutcomeInternal<T2> internal2)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg2)}\" does not support unwrapping: {arg2?.GetType().FullName}");

            return () => act(internal1.Unwrap(), internal2.Unwrap());
        }

        /// <summary>
        /// Creates an action that unwraps a proof result and executes the provided action with the unwrapped value.
        /// </summary>
        public static Action Action<T1, T2, T3>(
            IProbeOutcome<T1> arg1, IProbeOutcome<T2> arg2, IProbeOutcome<T3> arg3,
            Action<T1, T2, T3> act)
        {
            if (arg1 is not IProbeOutcomeInternal<T1> internal1)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg1)}\" does not support unwrapping: {arg1?.GetType().FullName}");
            if (arg2 is not IProbeOutcomeInternal<T2> internal2)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg2)}\" does not support unwrapping: {arg2?.GetType().FullName}");
            if (arg3 is not IProbeOutcomeInternal<T3> internal3)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg3)}\" does not support unwrapping: {arg3?.GetType().FullName}");

            return () => act(internal1.Unwrap(), internal2.Unwrap(), internal3.Unwrap());
        }

        /// <summary>
        /// Creates an action that unwraps a proof result and executes the provided action with the unwrapped value.
        /// </summary>
        public static Action Action<T1, T2, T3, T4>(
            IProbeOutcome<T1> arg1, IProbeOutcome<T2> arg2, IProbeOutcome<T3> arg3, IProbeOutcome<T4> arg4,
            Action<T1, T2, T3, T4> act)
        {
            if (arg1 is not IProbeOutcomeInternal<T1> internal1)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg1)}\" does not support unwrapping: {arg1?.GetType().FullName}");
            if (arg2 is not IProbeOutcomeInternal<T2> internal2)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg2)}\" does not support unwrapping: {arg2?.GetType().FullName}");
            if (arg3 is not IProbeOutcomeInternal<T3> internal3)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg3)}\" does not support unwrapping: {arg3?.GetType().FullName}");
            if (arg4 is not IProbeOutcomeInternal<T4> internal4)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg4)}\" does not support unwrapping: {arg4?.GetType().FullName}");

            return () => act(internal1.Unwrap(), internal2.Unwrap(), internal3.Unwrap(), internal4.Unwrap());
        }

        /// <summary>
        /// Creates an action that unwraps a proof result and executes the provided action with the unwrapped value.
        /// </summary>
        public static Action Action<T1, T2, T3, T4, T5>(
            IProbeOutcome<T1> arg1, IProbeOutcome<T2> arg2, IProbeOutcome<T3> arg3, IProbeOutcome<T4> arg4, IProbeOutcome<T5> arg5,
            Action<T1, T2, T3, T4, T5> act)
        {
            if (arg1 is not IProbeOutcomeInternal<T1> internal1)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg1)}\" does not support unwrapping: {arg1?.GetType().FullName}");
            if (arg2 is not IProbeOutcomeInternal<T2> internal2)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg2)}\" does not support unwrapping: {arg2?.GetType().FullName}");
            if (arg3 is not IProbeOutcomeInternal<T3> internal3)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg3)}\" does not support unwrapping: {arg3?.GetType().FullName}");
            if (arg4 is not IProbeOutcomeInternal<T4> internal4)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg4)}\" does not support unwrapping: {arg4?.GetType().FullName}");
            if (arg5 is not IProbeOutcomeInternal<T5> internal5)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg5)}\" does not support unwrapping: {arg5?.GetType().FullName}");

            return () => act(internal1.Unwrap(), internal2.Unwrap(), internal3.Unwrap(), internal4.Unwrap(), internal5.Unwrap());
        }

        /// <summary>
        /// Creates an action that unwraps a proof result and executes the provided action with the unwrapped value.
        /// </summary>
        public static Action Action<T1, T2, T3, T4, T5, T6>(
            IProbeOutcome<T1> arg1, IProbeOutcome<T2> arg2, IProbeOutcome<T3> arg3, IProbeOutcome<T4> arg4, IProbeOutcome<T5> arg5, IProbeOutcome<T6> arg6,
            Action<T1, T2, T3, T4, T5, T6> act)
        {
            if (arg1 is not IProbeOutcomeInternal<T1> internal1)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg1)}\" does not support unwrapping: {arg1?.GetType().FullName}");
            if (arg2 is not IProbeOutcomeInternal<T2> internal2)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg2)}\" does not support unwrapping: {arg2?.GetType().FullName}");
            if (arg3 is not IProbeOutcomeInternal<T3> internal3)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg3)}\" does not support unwrapping: {arg3?.GetType().FullName}");
            if (arg4 is not IProbeOutcomeInternal<T4> internal4)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg4)}\" does not support unwrapping: {arg4?.GetType().FullName}");
            if (arg5 is not IProbeOutcomeInternal<T5> internal5)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg5)}\" does not support unwrapping: {arg5?.GetType().FullName}");
            if (arg6 is not IProbeOutcomeInternal<T6> internal6)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg6)}\" does not support unwrapping: {arg6?.GetType().FullName}");

            return () => act(internal1.Unwrap(), internal2.Unwrap(), internal3.Unwrap(), internal4.Unwrap(), internal5.Unwrap(), internal6.Unwrap());
        }

        /// <summary>
        /// Creates an action that unwraps a proof result and executes the provided action with the unwrapped value.
        /// </summary>
        public static Action Action<T1, T2, T3, T4, T5, T6, T7>(
            IProbeOutcome<T1> arg1, IProbeOutcome<T2> arg2, IProbeOutcome<T3> arg3, IProbeOutcome<T4> arg4, IProbeOutcome<T5> arg5, IProbeOutcome<T6> arg6, IProbeOutcome<T7> arg7,
            Action<T1, T2, T3, T4, T5, T6, T7> act)
        {
            if (arg1 is not IProbeOutcomeInternal<T1> internal1)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg1)}\" does not support unwrapping: {arg1?.GetType().FullName}");
            if (arg2 is not IProbeOutcomeInternal<T2> internal2)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg2)}\" does not support unwrapping: {arg2?.GetType().FullName}");
            if (arg3 is not IProbeOutcomeInternal<T3> internal3)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg3)}\" does not support unwrapping: {arg3?.GetType().FullName}");
            if (arg4 is not IProbeOutcomeInternal<T4> internal4)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg4)}\" does not support unwrapping: {arg4?.GetType().FullName}");
            if (arg5 is not IProbeOutcomeInternal<T5> internal5)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg5)}\" does not support unwrapping: {arg5?.GetType().FullName}");
            if (arg6 is not IProbeOutcomeInternal<T6> internal6)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg6)}\" does not support unwrapping: {arg6?.GetType().FullName}");
            if (arg7 is not IProbeOutcomeInternal<T7> internal7)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg7)}\" does not support unwrapping: {arg7?.GetType().FullName}");

            return () => act(
                internal1.Unwrap(), internal2.Unwrap(), internal3.Unwrap(),
                internal4.Unwrap(), internal5.Unwrap(), internal6.Unwrap(),
                internal7.Unwrap());
        }

        /// <summary>
        /// Creates an action that unwraps a proof result and executes the provided action with the unwrapped value.
        /// </summary>
        public static Action Action<T1, T2, T3, T4, T5, T6, T7, T8>(
            IProbeOutcome<T1> arg1, IProbeOutcome<T2> arg2, IProbeOutcome<T3> arg3, IProbeOutcome<T4> arg4,
            IProbeOutcome<T5> arg5, IProbeOutcome<T6> arg6, IProbeOutcome<T7> arg7, IProbeOutcome<T8> arg8,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> act)
        {
            if (arg1 is not IProbeOutcomeInternal<T1> internal1)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg1)}\" does not support unwrapping: {arg1?.GetType().FullName}");
            if (arg2 is not IProbeOutcomeInternal<T2> internal2)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg2)}\" does not support unwrapping: {arg2?.GetType().FullName}");
            if (arg3 is not IProbeOutcomeInternal<T3> internal3)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg3)}\" does not support unwrapping: {arg3?.GetType().FullName}");
            if (arg4 is not IProbeOutcomeInternal<T4> internal4)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg4)}\" does not support unwrapping: {arg4?.GetType().FullName}");
            if (arg5 is not IProbeOutcomeInternal<T5> internal5)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg5)}\" does not support unwrapping: {arg5?.GetType().FullName}");
            if (arg6 is not IProbeOutcomeInternal<T6> internal6)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg6)}\" does not support unwrapping: {arg6?.GetType().FullName}");
            if (arg7 is not IProbeOutcomeInternal<T7> internal7)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg7)}\" does not support unwrapping: {arg7?.GetType().FullName}");
            if (arg8 is not IProbeOutcomeInternal<T8> internal8)
                throw new InvalidOperationException($"The given proof result \"{nameof(arg8)}\" does not support unwrapping: {arg8?.GetType().FullName}");

            return () => act(
                internal1.Unwrap(), internal2.Unwrap(), internal3.Unwrap(), internal4.Unwrap(),
                internal5.Unwrap(), internal6.Unwrap(), internal7.Unwrap(), internal8.Unwrap());
        }
    }
}