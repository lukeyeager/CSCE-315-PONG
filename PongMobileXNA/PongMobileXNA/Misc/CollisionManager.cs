using System;
using System.Collections.Generic;
using PongClasses;

public static class CollisionManager
{
    //Holds Typename -> List of objects
    private static Dictionary<String, LinkedList<PongObject>> Objects = new Dictionary<string,LinkedList<PongObject>>();

    //Generic delegate for collision
    public delegate void CollisionDelegate(PongObject a, PongObject b);

    //Holds Typename -> Typename -> List of CollisionDelegates
    private static Dictionary<String, Dictionary<String, LinkedList<CollisionDelegate>>> Callbacks
        = new Dictionary<String, Dictionary<String, LinkedList<CollisionDelegate>>>();

    public static void RegisterCallback(String Type1, String Type2, CollisionDelegate Callback)
    {
        if (!Callbacks.ContainsKey(Type1))
        {
            Callbacks.Add(Type1, new Dictionary<string, LinkedList<CollisionDelegate>>());   
        }
        if (!Callbacks[Type1].ContainsKey(Type2))
        {
            Callbacks[Type1].Add(Type2, new LinkedList<CollisionDelegate>());
        }
        Callbacks[Type1][Type2].AddLast(Callback);  
    }

    public static bool UnregisterCallback(String Type1, String Type2, CollisionDelegate Callback)
    {
        return Callbacks[Type1][Type2].Remove(Callback);
    }

    public static void AddObject(String Type, PongObject Obj)
    {
        if (!Objects.ContainsKey(Type))
        {
            Objects.Add(Type, new LinkedList<PongObject>());
        }
        Objects[Type].AddLast(Obj);
    }

    public static bool RemoveObject(String Type, PongObject Obj)
    {
        return Objects[Type].Remove(Obj);
    }

    //TODO: optimize this somehow
    public static void HandleCollisions()
    {

        //Added 04/15 by LCY
        // This check for validity of all objects is needed because when a PowerupBubble
        // is "popped", it happens within the loop below, and then a delegate function tries
        // to remove it from the list, but there's a foreach loop that needs the list of objects
        // not to change. There is probably a better way to fix the problem, but this seems to work
        bool everythingValid = false;

        while (!everythingValid)
        {
            everythingValid = true;
            foreach (KeyValuePair<string, LinkedList<PongObject>> o in Objects)
            {
                if (o.Key == "Ball")
                    foreach (Ball b in o.Value)
                    {
                        if (!b.IsActive)
                        {
                            o.Value.Remove(b);
                            everythingValid = false;
                            break;
                        }
                    }
                if (!everythingValid)
                    break;
                if (o.Key == "Bubble")
                    foreach (PowerupBubble b in o.Value)
                    {
                        if (!b.IsActive)
                        {
                            o.Value.Remove(b);
                            everythingValid = false;
                            break;
                        }
                    }
                if (!everythingValid)
                    break;
            }
        }

        foreach (KeyValuePair<String, Dictionary<String, LinkedList<CollisionDelegate>>> kv1 in Callbacks)
        {
            if (!Objects.ContainsKey(kv1.Key))
            {
                continue;
            }
            foreach (KeyValuePair<String, LinkedList<CollisionDelegate>> kv2 in kv1.Value)
            {
                if (!Objects.ContainsKey(kv2.Key))
                {
                    continue;
                }
                foreach (PongObject o1 in Objects[kv1.Key])
                {
                    foreach (PongObject o2 in Objects[kv2.Key])
                    {
                        if (o1.shape.Contains(o2.shape.GetExtremityInDirection(o1.shape.GetCenter())) ||
                            o2.shape.Contains(o1.shape.GetExtremityInDirection(o2.shape.GetCenter())))
                        {
                            foreach(CollisionDelegate cb in kv2.Value)
                            {
                                cb.Invoke(o1, o2);
                            }
                        }
                    }
                }
            }
        }
    }

    public static void ClearAll()
    {
        Objects.Clear();
        Callbacks.Clear();
    }
}