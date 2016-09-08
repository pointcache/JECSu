namespace JECSU
{
    using System;

    /// <summary>
    /// Accessor for component
    /// </summary>
    public interface IComponent
    {
        //actual concrete type i.e. Position
        Type type { get; }
        Entity owner { get; set; }
        int ownerid { get; set; }
        string ownername { get; set; }
    }
}