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
				 return new JECSU.Components.GameView();
			 case 1:
				 return new JECSU.Components.ColorComponent();
			 case 2:
				 return new JECSU.Components.GameRes();
			 case 3:
				 return new JECSU.Components.PRS();
			 case 4:
				 return new JECSU.Components.Serializeable();
			 default:
				 return null;
			}
		}
		static Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {
		{ typeof(JECSU.Components.GameView), () => lookup = 0 }
		,{ typeof(JECSU.Components.ColorComponent), () => lookup = 1 }
		,{ typeof(JECSU.Components.GameRes), () => lookup = 2 }
		,{ typeof(JECSU.Components.PRS), () => lookup = 3 }
		,{ typeof(JECSU.Components.Serializeable), () => lookup = 4 }
		};
	}
}
//EOF
