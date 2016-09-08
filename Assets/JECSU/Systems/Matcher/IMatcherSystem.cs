namespace JECSU
{
    using System;

    public interface IMatcherSystem : IEntitySystem
    {
        Matcher matcher { get; set; }
        void OnMatchAdded(Entity ent, Type t);
        void OnMatchRemoved(Entity ent, Type t);
    }

    public interface IMatcherSystem<T> : IMatcherSystem   {}
    public interface IMatcherSystem<T, T1> : IMatcherSystem   {}
    public interface IMatcherSystem<T, T1, T2> : IMatcherSystem   {}
    public interface IMatcherSystem<T, T1, T2, T3> : IMatcherSystem   {}
    public interface IMatcherSystem<T, T1, T2, T3, T4> : IMatcherSystem   {}
}