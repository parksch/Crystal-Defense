namespace ClientEnum
{
	public enum Arrow
	{
		Stop,
		Left,
		Right,
		Up,
		Down
	}

	public enum State
	{
		Attack,
		AttackSpeed,
        AttackRange,
    }

	public enum Mode
	{
		Draw,
		Game,
		Pause
	}

	public enum Draw
	{
		One = 1,
		Two,
		Three,
		Four,
		Max,
	}

	public enum DrawHierarchy
    {
		High,
		Pair,
		DoublePair,
        Triple,
        Straight,
		Four,
    }

	public enum Trait
	{
        Swordsman
    }

	public enum UIType
	{
		Wave,
		Life,
		Coin
	}

	public enum StartType
	{
		Caster,
		Target
	}

	public enum DestoryType
	{
        Arrive,
        TimeOut
	}

	public enum AttackType
	{

	}
}