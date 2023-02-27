using UnityEngine;
using System.Collections;


public enum TimeState
{
	Forward,
	Backward,
	Pause
}

public enum PlayMode
{
	Auto,
	Manual
}

public enum StoreType
{
	WithMemory,
	NoMemory
}


public enum TimeAction
{
	Nothing,
	StartRewind,
	StopRewind,
	Pause,
	Resume,
}

// Alone (don't need to parent and assossiations, actions just do on TimeObjects)
// Group (parent and associations control eachother)
// Hieracchical (don't need to parent, only controls yourself and associations)
// OnlyAssociations (only controls Associations)
public enum TimeObjectManagerType
{
	Alone,
	Group,
	Hierarchical,
	OnlyAssociations
}