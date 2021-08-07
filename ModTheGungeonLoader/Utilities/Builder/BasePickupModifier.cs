using System;
using UnityEngine;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Base pickup modifier.
    /// </summary>
    public abstract class BasePickupModifier : MonoBehaviour
    {

        /// <summary>
        /// Pickup object
        /// </summary>
        protected PickupObject Pickup { get; private set; }

        void Awake()
        {
            Pickup = GetComponent<PickupObject>() ?? throw new Exception("PickupObject may not be null and you may only use it on Passive/Active/Gun objects.");
        }
    }
}
