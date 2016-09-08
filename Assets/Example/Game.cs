using UnityEngine;
using System;
using System.Collections.Generic;
using JECSU;
using JECSU.Components;
using System.Diagnostics;
using FullSerializer;

public class Game : MonoBehaviour
{


    //These are COMPONENTS, but they are exposed to editor just as classes and it works ok, later i will use them in systems
    public Borders borders;
    public GameSettings gamesettings;
    public GameResources gameres;
    //in entity system components not only hold some data, they are indicators themselves, as in they hold boolean value just by existing, 
    //thus allowing us to have empty components that still perform work and allow systems to function, like Serializeable component is empty,
    //but save system uses its existence to determine if the entity must be saved.
    [Serializable]
    public class Ball : BaseComponent, IComponent
    {
        public Vector3 Direction;
        public float ballSize = 1f;
    }
    [Serializable]
    public class Rocket : BaseComponent, IComponent
    {
        public enum Side { left, right }
        public Side side;
    }
    [Serializable]
    public class Borders : BaseComponent, IComponent
    {
        public float X, Y;
    }

    [Serializable]
    public class GameSettings : BaseComponent, IComponent
    {
        public float ballSpeed;
    }
    
    
    public class View : BaseComponent, IComponent
    {
        [fsIgnore]
        public GameObject view;
    }
    [Serializable]
    public class GameResources : BaseComponent, IComponent
    {
        public GameObject ballPrefab, rocketPrefab;
    }

    public class SomeMessage : BaseComponent, IComponent { }

    void Start()
    {
        GameStart();
    }

    void GameStart()
    {

        Entity.New("gameSettings")
            .AddComponent(gamesettings);

        //game resources component added, it stores references and other stuff
        Entity.New("gameres")
            .AddComponent(gameres);

        Entity.New("Pong")
            .AddComponent<Position>() //
            .AddComponent<Ball>()
            .AddComponent<View>()
            .AddComponent<Interpolator>();

        var rocket_left = Entity.New("RocketLeft")
            .AddComponent<Position>()
            .AddComponent<Rocket>();

        rocket_left.GetComponent<Rocket>().side = Rocket.Side.left;

        var rocket_right = Entity.New("RocketRight")
            .AddComponent<Position>()
            .AddComponent<Rocket>();

        rocket_right.GetComponent<Rocket>().side = Rocket.Side.right;

        Entity.New("Borders")
            .AddComponent(borders);

        //Systems.New<TestSystem>();
        Systems.New<BallController>();
        Systems.New<ViewController>();
        Systems.New<PositionUpdater>();

    }

    /// <summary>
    /// For testing purposes, performance tests etc.
    /// </summary>
    public class TestSystem : BaseSystem, IInitializeSystem //Initialize system interface indicates that the systems "Initialize" method will be called, and thats it.
    {
        public void Initialize()
        {
           // Test1();
        }

        //sped up the whole thing currently at 3.6 seconds on core i7 4970
        void Test1()
        {
            Stopwatch st = Stopwatch.StartNew();
            //create million entities
            int count = 1000000;
            for (int i = 0; i < count; i++)
            {
                Entity.New("_").AddComponent<SomeMessage>();
            }
            UnityEngine.Debug.Log(st.ElapsedMilliseconds);
            st.Stop();
        }

        void Test2()
        {
            Stopwatch st = Stopwatch.StartNew();
            //create million entities
            int count = 1000000;
            for (int i = 0; i < count; i++)
            {
                //make free component
                //free component are questionable decision, very questionable
                //the idea is to have a free floating component that acts as a "message" only to it can be captured by observing systems,
                //however i think its better to have a separate satellite messaging system
                //upd: it came to use after thinking that no, it wont really be better, its better to optimize the entity system so that entity can 
                //be lighting fast so that its feasible to spam them as messages and observe them as part of the pool and save them and change them 
                //dynamically even if they dont represent any particular object, but just an event 

                //so the conclusion is that we really need to optimize entity/component creation speed
            }
            UnityEngine.Debug.Log(st.ElapsedMilliseconds);
            st.Stop();
        }
    }

    /// <summary>
    /// Controls the pong ball.
    /// </summary>
    public class BallController : BaseSystem, IExecuteSystem, IInitializeSystem //Execute system will execute each system tick (Time.fixedTimeStep)
    {
        GameSettings settings;
        Borders borders;
        Ball ball;

        Position ballPos;

        public void Initialize()
        {
            borders = Pool<Borders>.GetFirst();
            ball = Pool<Ball>.GetFirst();
            settings = Pool<GameSettings>.GetFirst();
            ball.Direction = convertTo2d(UnityEngine.Random.insideUnitSphere);
            ballPos = ball.owner.GetComponent<Position>();
        }

        //Here we basically just move the ball and bounce it from walls, at the end we mark it as Dirty(), so that observing systems are notified of change
        public void Execute()
        {
            ballPos.position += ball.Direction * settings.ballSpeed * Time.deltaTime;

            Vector2 predictedpos = ballPos.position + (ball.Direction + (ball.Direction.normalized * ball.ballSize / 2));

            if (predictedpos.y > borders.Y || predictedpos.y < 0f)
            {
                ball.Direction.y *= -1f;
            }
            if (predictedpos.x > borders.X || predictedpos.x < 0f)
            {
                ball.Direction.x *= -1f;
            }

            ballPos.Dirty();
        }

        Vector2 convertTo2d(Vector3 vec)
        {
            float mag = vec.magnitude;
            vec = vec.normalized;
            Vector2 v2 = new Vector2(vec.x, vec.z);
            v2 *= mag;
            return v2;
        }
    }

    public class PositionUpdater : BaseSystem, IInitializeSystem
    {
        public void Initialize()
        {
            Pool.AddDirtyHandler<Position>(PositionChanged);
        }

        void PositionChanged(Position pos)
        {
            if (pos.owner.HasComponent<View>())
            {
                if (pos.owner.HasComponent<Interpolator>())
                {
                    pos.owner.GetComponent<Interpolator>().interpolator.position = pos.position;
                }
                else
                {
                    var view = pos.owner.GetComponent<View>();
                    view.view.transform.position = pos.position;
                }
            }
        }
    }

    public class ViewController : BaseSystem, IMatcherSystem<View> //Matcher system will search for matching types and call OnMatch added/removed 
    {
        public void OnMatchAdded(Entity ent, Type t)
        {
            View view = ent.GetComponent<View>();

            if (ent.HasComponent<Ball>())
            {
                view.view = Instantiate(Pool<GameResources>.GetFirst().ballPrefab);
            }

            if (ent.HasComponent<Interpolator>() && view.view != null)
            {
                var interpolator = ent.GetComponent<Interpolator>();
                interpolator.Init(view.view.transform);
            }
        }

        public void OnMatchRemoved(Entity ent, Type t)
        {
            throw new NotImplementedException();
        }
    }
}
