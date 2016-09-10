namespace JECSU.Serialization
{
    using System;
    using System.Collections.Generic;
    using JECSU.Components;

    public class Template
    {

        public Entity entity;
        public List<IComponent> components = new List<IComponent>();
    }
}