using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.Traits
{
    /// <summary>
    /// Holds a shallow-cloned copy of the provided creation arguments.
    /// </summary>
    /// <typeparam name="TSource">Type of the creation arguments.</typeparam>
    public class ShallowCloned<TSource>
        where TSource : class, IShallowClonable<TSource>
    {
        /// <summary>
        /// The shallow-cloned creation arguments.
        /// </summary>
        public TSource Cloned { get; }

        /// <summary>
        /// Creates a new instance and stores a shallow clone of the provided arguments.
        /// </summary>
        /// <param name="args">The creation arguments to clone and store.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="args"/> is null.</exception>
        public ShallowCloned(TSource args)
        {
            Cloned = args?.ShallowClone() ?? throw new ArgumentNullException(nameof(args));
            OnShallowCloned(Cloned);
        }

        /// <summary>
        /// Called after the arguments have been shallow-cloned. Can be overridden in derived classes.
        /// </summary>
        protected virtual void OnShallowCloned(TSource cloned)
        {
        }
    }
}