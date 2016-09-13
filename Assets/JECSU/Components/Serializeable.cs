namespace JECSU.Components
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using FullSerializer;

    /// <summary>
    /// Add this to entities you want to be saved by Save()
    /// </summary>
    public class Serializeable : BaseComponent, IComponent
    {
    }

}