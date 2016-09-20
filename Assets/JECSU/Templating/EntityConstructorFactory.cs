/*This is generated file.
This class speeds up component creation in a safe manner.

===============================================================================================*/

namespace JECSU
{
using System;
using System.Collections.Generic;
	public partial class EntityConstructor
	{
		static int lookup;
		static Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {
		{ typeof(JECSU.Components.GameView), () => lookup = 0 }
		,{ typeof(JECSU.Components.ColorComponent), () => lookup = 1 }
		,{ typeof(JECSU.Components.GameRes), () => lookup = 2 }
		,{ typeof(JECSU.Components.PRS), () => lookup = 3 }
		,{ typeof(JECSU.Components.Serializeable), () => lookup = 4 }
		};
		public static void AssignFromTemplate(IComponent component, Dictionary<string, string> values)
		{
		if (@switch.ContainsKey(component.type))
			@switch[component.type].Invoke();
		switch (lookup)
		{
			 case 0:
				JECSU_Components_GameView_assignFromTemplate((JECSU.Components.GameView) component, values);
			 break;
			 case 1:
				JECSU_Components_ColorComponent_assignFromTemplate((JECSU.Components.ColorComponent) component, values);
			 break;
			 case 2:
				JECSU_Components_GameRes_assignFromTemplate((JECSU.Components.GameRes) component, values);
			 break;
			 case 3:
				JECSU_Components_PRS_assignFromTemplate((JECSU.Components.PRS) component, values);
			 break;
			 case 4:
				JECSU_Components_Serializeable_assignFromTemplate((JECSU.Components.Serializeable) component, values);
			 break;
		}
	}
	static void JECSU_Components_GameView_assignFromTemplate(JECSU.Components.GameView comp, Dictionary<string,string> values) {
		foreach (var p in values){
			if(p.Key == "dynamic" ){
				comp.dynamic = convertBool(p.Value);
			}
		}
	 comp.Dirty();
	}

	static void JECSU_Components_ColorComponent_assignFromTemplate(JECSU.Components.ColorComponent comp, Dictionary<string,string> values) {
		foreach (var p in values){
			if(p.Key == "color" ){
				comp.color = convertColor(p.Value);
			}
		}
	 comp.Dirty();
	}

	static void JECSU_Components_GameRes_assignFromTemplate(JECSU.Components.GameRes comp, Dictionary<string,string> values) {
		foreach (var p in values){
			if(p.Key == "prefabID" ){
				comp.prefabID = p.Value;
			}
		}
	 comp.Dirty();
	}

	static void JECSU_Components_PRS_assignFromTemplate(JECSU.Components.PRS comp, Dictionary<string,string> values) {
		foreach (var p in values){
			if(p.Key == "rotation" ){
				comp.rotation = convertV3(p.Value);
			}
			else if(p.Key == "scale" ){
				comp.scale = convertV3(p.Value);
			}
			else if(p.Key == "position" ){
				comp.position = convertV3(p.Value);
			}
		}
	 comp.Dirty();
	}

	static void JECSU_Components_Serializeable_assignFromTemplate(JECSU.Components.Serializeable comp, Dictionary<string,string> values) {
	}

	}
}
