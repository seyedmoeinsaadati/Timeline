using UnityEngine;
using System;

public class TimeObjectManager : MonoBehaviour, ITimeAction
{
    public TimeObjectManagerType type;
    public float recordTime = 5f;
    public bool isRewinding = false;
    [Tooltip("if true, associastions are Time Object Manaager's child and assign automatically")]
    public bool childAssociations;

    [SerializeField]
    TimeObject[] timeObjs;

    [SerializeField] protected TimeState timeState = TimeState.Forward;
    [SerializeField] protected StoreType storeType = StoreType.NoMemory;

    [SerializeField]
    TimeObjectManager[] assossiations;
    [SerializeField]
    TimeObjectManager parent;

    #region Actions

    public event Action onA, onB, onC;

    public void OnStartRewind()
    {
    }

    public void OnStopRewind()
    {
    }

    public void OnFinishTape()
    {
    }

    public void OnStartTape()
    {
    }

    public void OnPause()
    {
    }

    public void OnChangeTimeState()
    {
    }

    #endregion

    #region UnityFunctions


    void Awake()
    {
        LoadTimeObjects();
        InitTimeObjects();
        //LoadAssociations();

        SetParentToAssociations();
    }

    void OnEnable()
    {
        LoadTimeObjects();
        SetParentToAssociations();
    }

    #endregion

    #region TimeOjbectActions

    public void StartRewind()
    {
        isRewinding = true;

        switch (type)
        {
            case TimeObjectManagerType.Alone:
                StartRewindYourself();
                break;
            case TimeObjectManagerType.Group:
                if (parent != null)
                {
                    foreach (var item in parent.getAssociations())
                    {
                        item.StartRewindYourself();
                        item.StartRewindAssossiatinos();
                    }
                }
                else
                {
                    StartRewindYourself();
                    StartRewindAssossiatinos();
                }
                break;
            case TimeObjectManagerType.Hierarchical:
                StartRewindYourself();
                StartRewindAssossiatinos();
                break;
            case TimeObjectManagerType.OnlyAssociations:
                StartRewindAssossiatinos();
                break;
            default:
                break;
        }

    }

    public void StartRewindYourself()
    {
        foreach (var item in timeObjs)
        {
            item.StartRewind();
        }
    }

    public void StartRewindAssossiatinos()
    {
        foreach (var _objcet in assossiations)
        {
            _objcet.StartRewind();
        }
    }

    public void StopRewind()
    {
        isRewinding = false;

        switch (type)
        {
            case TimeObjectManagerType.Alone:
                StopRewindYourself();
                break;
            case TimeObjectManagerType.Group:
                if (parent != null)
                {
                    foreach (var item in parent.getAssociations())
                    {
                        item.StopRewindYourself();
                        item.StopRewindAssociations();
                    }
                }
                else
                {
                    StopRewindYourself();
                    StopRewindAssociations();
                }
                break;
            case TimeObjectManagerType.Hierarchical:
                StopRewindYourself();
                StopRewindAssociations();
                break;
            case TimeObjectManagerType.OnlyAssociations:
                StopRewindAssociations();
                break;
            default:
                break;
        }
    }

    public void StopRewindYourself()
    {
        foreach (var item in timeObjs)
        {
            item.StopRewind();
        }
    }

    void StopRewindAssociations()
    {
        foreach (var _objcet in assossiations)
        {
            _objcet.StopRewind();
        }
    }

    public void Pause()
    {
        switch (type)
        {
            case TimeObjectManagerType.Alone:
                PauseYourself();
                break;
            case TimeObjectManagerType.Group:
                if (parent != null)
                {
                    foreach (var item in parent.getAssociations())
                    {
                        item.PauseYourself();
                        item.PauseAssociations();
                    }
                }
                else
                {
                    PauseYourself();
                    PauseAssociations();
                }
                break;
            case TimeObjectManagerType.Hierarchical:
                PauseYourself();
                PauseAssociations();
                break;
            case TimeObjectManagerType.OnlyAssociations:
                PauseAssociations();
                break;
            default:
                break;
        }
    }

    public void PauseYourself()
    {
        foreach (var item in timeObjs)
        {
            item.Pause();
        }
    }

    void PauseAssociations()
    {
        foreach (var _objcet in assossiations)
        {
            _objcet.Pause();
        }
    }

    public void ChangeTimeState(TimeState newTimeState)
    {
        switch (type)
        {
            case TimeObjectManagerType.Alone:
                ChangeTimeStateYourself(newTimeState);
                break;
            case TimeObjectManagerType.Group:
                if (parent != null)
                {
                    foreach (var item in parent.getAssociations())
                    {
                        item.ChangeTimeStateYourself(newTimeState);
                        item.ChangeTimeStateAssossiations(newTimeState);
                    }
                }
                else
                {
                    ChangeTimeStateYourself(newTimeState);
                    ChangeTimeStateAssossiations(newTimeState);
                }
                break;
            case TimeObjectManagerType.Hierarchical:
                ChangeTimeStateYourself(newTimeState);
                ChangeTimeStateAssossiations(newTimeState);
                break;
            case TimeObjectManagerType.OnlyAssociations:
                ChangeTimeStateAssossiations(newTimeState);
                break;
            default:
                break;
        }

    }

    void ChangeTimeStateYourself(TimeState newTimeState)
    {
        foreach (var item in timeObjs)
        {
            item.ChangeTimeState(newTimeState);
        }
    }

    void ChangeTimeStateAssossiations(TimeState newTimeState)
    {
        foreach (var _objcet in assossiations)
        {
            _objcet.ChangeTimeState(newTimeState);
        }
    }

    #endregion


    #region Functions

    public TimeObjectManager[] getAssociations()
    {
        return assossiations;
    }

    void SetParentToAssociations()
    {
        foreach (var item in assossiations)
        {
            item.SetParent(this);
        }
    }

    public void ChangeTimeObjectManagerType(TimeObjectManagerType newType)
    {
        type = newType;
    }

    void SetParent(TimeObjectManager _parent)
    {
        parent = _parent;
    }


    void InitTimeObjects()
    {
        foreach (var item in timeObjs)
        {
            item.recordTime = recordTime;
            item.storeType = storeType;
        }
    }

    void LoadAssociations()
    {
        assossiations = transform.GetComponentsInChildren<TimeObjectManager>();
    }

    //	void Organize ()
    //	{
    //		switch (type) {
    //		case TimeObjectManagerType.Alone:
    //			// don't need parent and assossiations
    //			break;
    //		case TimeObjectManagerType.Group:
    //			SetParentToAssociations ();
    //			break;
    //		case TimeObjectManagerType.Hierarchical:
    //			SetParentToAssociations ();
    //			break;
    //		case TimeObjectManagerType.OnlyAssociations:
    //			// assossisations don't need to know their parent
    //			break;
    //		default:
    //			break;
    //		}
    //	}

    void LoadTimeObjects()
    {
        timeObjs = GetComponents<TimeObject>();
    }



    #endregion

    #region EditorFunctions

    void OnDrawGizmos()
    {
        switch (type)
        {
            case TimeObjectManagerType.Alone:
                Gizmos.DrawIcon(transform.position, "tobj_icon.png", true);
                break;
            case TimeObjectManagerType.Group:
                Gizmos.DrawIcon(transform.position, "tobj_icon.png", true);
                break;
            case TimeObjectManagerType.Hierarchical:
                Gizmos.DrawIcon(transform.position, "tobj_icon.png", true);
                break;
            case TimeObjectManagerType.OnlyAssociations:
                Gizmos.DrawIcon(transform.position, "tobj_icon.png", true);
                break;
            default:
                break;
        }
    }

    #endregion

}


// -----------------DO-Action alghorithm--------------------
//	switch (type) {
//	case TimeObjectManagerType.Alone:
//		ActionYourself();
//		break;
//	case TimeObjectManagerType.Group:
//		if (parent != null) {
//			foreach (var item in parent.getAssociations()) {
//				item.ActionYourself();
//				item.ActionAssociations ();
//			}
//		} else {
//			ActionYourself();
//			ActionAssociations ();
//		}
//		break;
//	case TimeObjectManagerType.Hierarchical:
//		ActionYourself ();
//		ActionAssociations ();
//		break;
//	case TimeObjectManagerType.OnlyAssociations:
//		ActionAssociations ();
//		break;
//	default:
//		break;
//	}