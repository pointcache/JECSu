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
			if (!@switch.ContainsKey(t))return null;
			@switch[t].Invoke();
			switch (lookup)
			{
			 case 0:
				 return new JECSU.Components.ColorComponent();
			 case 1:
				 return new JECSU.Components.Interpolator();
			 case 2:
				 return new JECSU.Components.Position();
			 case 3:
				 return new JECSU.Components.Serializeable();
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
		{ typeof(JECSU.Components.ColorComponent), () => lookup = 0 }
		,{ typeof(JECSU.Components.Interpolator), () => lookup = 1 }
		,{ typeof(JECSU.Components.Position), () => lookup = 2 }
		,{ typeof(JECSU.Components.Serializeable), () => lookup = 3 }
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
