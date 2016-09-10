< chicken
		name = "Chicken"
	[Position]
	[Creature]
		max_hp = 90000
		attack_dmg = 1
		attack = "meelee"
		range = 1
		speed = 1
		run_speed = 3
		
	[Animal]
		type = "Chicken"
	[ColorComponent]
		color = #ffffff
	[WorldView]
		prefab = "animals/chicken";
	[Loot]
		loot_table{
			"chicken", 1, 10
			"feather", 3, 6
		}
>		
< chicken_brown
	parent = chicken
	[ColorComponent]
		color = #ffffff
>
< feather
	name = "Feather"
	[Position]
	[Item]
		mass = 0.01
>		

	
	