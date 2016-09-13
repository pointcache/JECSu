/*This is generated file.
This class speeds up component creation in a safe manner.

===============================================================================================*/

namespace JECSU
{
using System;
using System.Collections.Generic;
	public static class ComponentFactory
	{
		static int lookup;
		public static IComponent MakeNew<T>() where T : BaseComponent
		{
			Type t = typeof(T);
			return MakeNew(t);
		}
		public static IComponent MakeNew(Type t)
		{
			if (!@switch.ContainsKey(t))return null;
			@switch[t].Invoke();
			switch (lookup)
			{
			 case 0:
				 return new Game.Ball();
			 case 1:
				 return new Game.Rocket();
			 case 2:
				 return new Game.Borders();
			 case 3:
				 return new Game.GameSettings();
			 case 4:
				 return new Game.View();
			 case 5:
				 return new Game.GameResources();
			 case 6:
				 return new Game.SomeMessage();
			 case 7:
				 return new JECSU.Components.ColorComponent();
			 case 8:
				 return new JECSU.Components.Interpolator();
			 case 9:
				 return new JECSU.Components.Position();
			 case 10:
				 return new JECSU.Components.Serializeable();
			 default:
				 return null;
			}
		}
		static Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {
		{ typeof(Game.Ball), () => lookup = 0 }
		,{ typeof(Game.Rocket), () => lookup = 1 }
		,{ typeof(Game.Borders), () => lookup = 2 }
		,{ typeof(Game.GameSettings), () => lookup = 3 }
		,{ typeof(Game.View), () => lookup = 4 }
		,{ typeof(Game.GameResources), () => lookup = 5 }
		,{ typeof(Game.SomeMessage), () => lookup = 6 }
		,{ typeof(JECSU.Components.ColorComponent), () => lookup = 7 }
		,{ typeof(JECSU.Components.Interpolator), () => lookup = 8 }
		,{ typeof(JECSU.Components.Position), () => lookup = 9 }
		,{ typeof(JECSU.Components.Serializeable), () => lookup = 10 }
		};
	}
}
//EOF
