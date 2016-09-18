using UnityEngine;
using System;
using System.Collections.Generic;
using JECSU;
using JECSU.Components;

public class Test : MonoBehaviour {

	void Start()
    {
        Systems.New<TestGameViewSystem>();

        var palm = Entity.FromID("palm01").AddComponent<GameView>();

    }
}
