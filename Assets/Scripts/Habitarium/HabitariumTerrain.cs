using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitariumTerrain : MySingletonComponent<HabitariumTerrain>
{
    HashSet<ARWorldObject> lockedARWorldObjects = new HashSet<ARWorldObject>();

    public bool placeMode;
    public bool lockMode;

    public static bool CheckARWorldObjectLocked(ARWorldObject obj)
    {
        return instance.lockedARWorldObjects.Contains(obj);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            lockMode = !lockMode;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            placeMode = !placeMode;
        }
    }

    public static void LockARWorldObject(ARWorldObject obj)
    {
        if (CheckARWorldObjectLocked(obj))
        {
            return;
        }

        obj.transform.SetParent(instance.transform, true);

        instance.lockedARWorldObjects.Add(obj);
    }

    public static void UnlockARWorldObject(ARWorldObject obj)
    {
        if (!CheckARWorldObjectLocked(obj))
        {
            return;
        }

        obj.transform.SetParent(obj.MyImageTargetBehavious.transform, true);

        instance.lockedARWorldObjects.Remove(obj);
    }

}
