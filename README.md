![Tested on Unity 5.4.0f3](https://img.shields.io/badge/Tested%20on%20unity-5.4.0f3-blue.svg?style=flat-square)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
![License MIT](https://img.shields.io/badge/license-MIT-green.svg)
# JECSu
Just another ECS for Unity

#What is ECS and what are the use cases:

ECS stands for Entity-Component-System, they are 3 separate concepts, not like this is a "system" that just has entities and components, instead it is a framework that has 
- Entities
- Components
- Systems

These 3 concepts allow for a very special way to decouple data from implementation, in a way mvc and other oop concepts
can't. ECS is one of the most sophisticated architectures in software developed finding it's use in many mmos and big games.
In ECS all the data is represented as components on entities, where entity is empty object that doesnt perform any actions.
So all inheritance problems with class hierarchy are solved by Composition. https://en.wikipedia.org/wiki/Composition_over_inheritance
This causes fundamental shift in thinking process and simplifies writing manageable code.

So when components are data, and entities are compositions of components, systems represent concrete implementation.
Systems may iterate over component types, and perform work on them based on their current state, they may query for
specific component arrangement, and do pretty much the same you would expect from encapsulated work object.

The real benefit comes when you realize that all the data in the game is represented by a flat list of entities and separate lists of components of the same type, that are static and by themselves don't perform any job, they become alive only when the system modifies them. It's a heart of a data driven approach, where the logic has a bird eye view on a sea of data.

There are many resources you could read about ECS and data driven approach:
http://scottbilas.com/files/2002/gdc_san_jose/game_objects_slides.pdf
http://cowboyprogramming.com/2007/01/05/evolve-your-heirachy/

This particular ECS is being designed with several use cases in mind:
* Large scale games, with dynamic loading of assets
* Dynamic non stop simulation outside of scene, scalability of simulation
* Procedural games with heavy emphasis on modding and editing performed outside of Unity editor

This prototype ECS was made as exercise and attempt to better grasp  Entity systems.
It uses mixed concepts from various places (like Entitas framework) but has several decisions:

* no code generation required for it to function.
* has to acknowledge Unity specific nature
* Has to be braindead simple to use
* Has to be performant as far as generic nature allows
* Out of the box has mechanism for templating and accessing data for modding/ runtime editing

What currently exists and how it works:

concepts:

* Entity - an object that holds some data to identify it and query the system for its components
* Component - a data block/object that (ideally) should not hold any logic (apart from some initialization and setters/getters)
      it is used not only to store data but to identify and make object behavior just by its existance, so that Systems can operate on it.
* System - object performing actual work on components/entities, here go all your manager and everything 
    * iInitializeSystem - will get initialized on creation
    * iMatcherSystem<T,T1,T2,T3> - will track all entities for certain component composition and trigger OnMatchAdded events 
    * iExecuteSystem - will call Execute() method on tick.
    * 
    * All these interfaces can be added on same class or just the ones that are necessary.
* Pool - class that stores components of same type, serves as a cache/tracker whats added/removed etc, allows entities to query for its components
    
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
* CodeGenerator/ComponentFactory - generates a class that has static component constructors, its not mandatory to use, but its speeds things up a bit.
See Example scene for detailed example of game setup (pretty bare bones right now, but so is the project).

#License: 
Project is released under MIT license 
Copyright (c) 2016 Alexander Antonov (pointcache) and contributors

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

