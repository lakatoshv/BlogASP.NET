using System;
using Blog.Core;

namespace Blog.Data.Core
{
    /// <summary>
    /// Entity.
    /// </summary>
    public abstract class Entity : IEntityBase<int>
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Override ==.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>bool.</returns>
        public static bool operator ==(Entity x, Entity y) => Equals(x, y);

        /// <summary>
        /// Override !=.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>bool.</returns>
        public static bool operator !=(Entity x, Entity y) => !(x == y);

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">obj.</param>
        /// <returns>bool.</returns>
        public override bool Equals(object obj) => this.Equals(obj as Entity);

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="other">other.</param>
        /// <returns>bool.</returns>
        public virtual bool Equals(Entity other)
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

        /// <inheritdoc cref="IEntityBase{T}"/>
        public override int GetHashCode() =>
            Equals(this.Id, default(int)) ? base.GetHashCode() : this.Id.GetHashCode();

        /// <summary>
        /// Is transient.
        /// </summary>
        /// <param name="obj">obj.</param>
        /// <returns>bool.</returns>
        private static bool IsTransient(Entity obj) =>
            obj != null && Equals(obj.Id, default(int));

        /// <summary>
        /// Get unproxied type.
        /// </summary>
        /// <returns>Type.</returns>
        private Type GetUnproxiedType() => this.GetType();
    }
}
