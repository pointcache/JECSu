# JECSu
Just another ECS for Unity

This is prototype ECS i made as exercise and attempt to grasp better Entity systems concepts.
It uses mixed concepts from various places (like Entitas framework) but i made several decisions.

* no code generation required for it to function.
* has to acknowledge Unity specific nature
* Has to be braindead simple to use
* Has to be performant as far as generic nature allows

What currently exists and how it works:

concepts:

* Entity - an object that holds some data to identify it and query the system for its components
* Component - a data block/object that (ideally) should not hold any logic (apart from some initialization and setters/getters)
      it is used not only to store data but to identify and make object behavior just by its existance, so that Systems can operate on it.
* System - object performing actual work on components/entities, here go all your manager and everything 
    * iInitializeSystem - will get initialized on creation
    * iMatcherSystem<T,T1,T2,T3> - will track all entities for certain component composition and trigger OnMatchAdded events 
    * iExecuteSystem - will call Execute() method on tick.
* Pool - class that stores components of same type, serves as a cache/tracker whats added/removed etc, allows entities to query for its components
    All these interfaces can be added on same class or just the ones that are necessary.
* Matcher - he has to locate components that match criterias. Used by systems under the hood.

Concrete classes:

* EntityManager - main dependency, exists in scene and receives unity callbacks (void FixedUpdate(); will execute all iExecute systems) 
performs some internal work and in general you dont touch it.
* Entity - this class implements entity, not much to say, you can AddComponent<T> to it
* Component - relatively big file, has interfaces and other things for components to work
* Pool - contains Pool class as well as Pool<T> which i call "concrete" pool of certain type, allows you to get all components of type T
just by using Pool<T>.components
* Matcher - this guys responsibility is to search for components that match the criteria. Like var agressivemobs = Matcher.Is<Mob>().Is<Agressive>();
has static and dynamic methods.

Additional things
* Helpers - i cant explain what this file does, its uncomprehendable.
* Save - uses FullSerializer ( https://github.com/jacobdufault/fullserializer ) to save all entities with Serializeable component.
* Interpolation - easy way to handle interpolation of transform after the Position component was changed.

See Example scene for detailed example of game setup (pretty bare bones right now, but so is the project).
