namespace JECSU.Components
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public class GameView : BaseComponent, IComponent
    {
        [TemplateIgnore]
        public GameViewBehavior view;

        /// <summary>
        /// Will the position be constantly updated?
        /// </summary>
        public bool dynamic = false;

    }
}