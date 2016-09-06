/*This is generated file.
This class speeds up component creation in a safe manner.

===============================================================================================*/

using UnityEngine;
using System;
using System.Collections.Generic;
using EntitySystem.Components;
namespace EntitySystem
{
	public static class ComponentFactory
	{
		static int lookup;
		public static iComponent MakeNew<T>() where T : BaseComponent
		{
			Type t = typeof(T);
			if (!@switch.ContainsKey(t))return null;
			@switch[t].Invoke();
			switch (lookup)
			{
			 case 0:
				 return new EntitySystem.Components.Position();
			 case 1:
				 return new EntitySystem.Components.Interpolator();
			 case 2:
				 return new EntitySystem.Components.ColorComponent();
			 case 3:
				 return new EntitySystem.Components.Serializeable();
			 case 4:
				 return new Game.Ball();
			 case 5:
				 return new Game.Rocket();
			 case 6:
				 return new Game.Borders();
			 case 7:
				 return new Game.GameSettings();
			 case 8:
				 return new Game.View();
			 case 9:
				 return new Game.GameResources();
			 case 10:
				 return new Game.SomeMessage();
			 default:
				 return null;
			}
		}
		static Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {
		{ typeof(EntitySystem.Components.Position), () => lookup = 0 }
		,{ typeof(EntitySystem.Components.Interpolator), () => lookup = 1 }
		,{ typeof(EntitySystem.Components.ColorComponent), () => lookup = 2 }
		,{ typeof(EntitySystem.Components.Serializeable), () => lookup = 3 }
		,{ typeof(Game.Ball), () => lookup = 4 }
		,{ typeof(Game.Rocket), () => lookup = 5 }
		,{ typeof(Game.Borders), () => lookup = 6 }
		,{ typeof(Game.GameSettings), () => lookup = 7 }
		,{ typeof(Game.View), () => lookup = 8 }
		,{ typeof(Game.GameResources), () => lookup = 9 }
		,{ typeof(Game.SomeMessage), () => lookup = 10 }
		};
	}
}
//EOF
