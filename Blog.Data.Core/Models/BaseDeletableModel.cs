using System;
using Blog.Data.Core.Models.Interfaces;

namespace Blog.Data.Core.Models
{
    /// <summary>
    /// Base deletable model.
    /// </summary>
    /// <typeparam name="TKey">TKey.</typeparam>
    public abstract class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity
    {
        /// <inheritdoc cref="IDeletableEntity"/>
        public bool IsDeleted { get; set; }

        /// <inheritdoc cref="IDeletableEntity"/>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(BaseDeletableModel<TKey> x, BaseDeletableModel<TKey> y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(BaseDeletableModel<TKey> x, BaseDeletableModel<TKey> y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Determines whether the specified <see>
        ///     <cref>System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</cref>
        /// </see>
        /// , is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see>
        ///         <cref>System.Object</cref>
        ///     </see>
        ///     to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see>
        ///         <cref>System.Object</cref>
        ///     </see>
        ///     is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as BaseDeletableModel<TKey>);
        }

        /// <summary>
        /// Equals the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public virtual bool Equals(BaseDeletableModel<TKey> other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (IsTransient(this) || IsTransient(other) || !Equals(this.Id, other.Id))
            {
                return false;
            }

            var otherType = other.GetUnproxiedType();
            var thisType = this.GetUnproxiedType();

            return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Equals(Id, default(int)) ? base.GetHashCode() : this.Id.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is transient.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if the specified object is transient; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTransient(BaseDeletableModel<TKey> obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        /// <summary>
        /// Gets the type of the unproxied.
        /// </summary>
        /// <returns></returns>
        private Type GetUnproxiedType()
        {
            return this.GetType();
        }
    }
}