using System;
using System.Collections.Generic;
using System.Linq;

public enum MessengerMode {
    DONT_REQUIRE_LISTENER,
    REQUIRE_LISTENER,
}

static class MessengerInternal {
    public static readonly Dictionary<GameEventTypes, Delegate> EventTable = new Dictionary<GameEventTypes, Delegate>();
    public static readonly MessengerMode DEFAULT_MODE = MessengerMode.REQUIRE_LISTENER;

    public static void AddListener(GameEventTypes eventType, Delegate callback) {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = Delegate.Combine(EventTable[eventType], callback);
    }

    public static void RemoveListener(GameEventTypes eventType, Delegate handler) {
        OnListenerRemoving(eventType, handler);
        EventTable[eventType] = Delegate.Remove(EventTable[eventType], handler);
        OnListenerRemoved(eventType);
    }

    public static T[] GetInvocationList<T>(GameEventTypes eventType) {
        Delegate d;
        
        if (!EventTable.TryGetValue(eventType, out d)) 
            return null;
        
        try {
            return d.GetInvocationList().Cast<T>().ToArray();
        }
        catch {
            throw CreateBroadcastSignatureException(eventType);
        }
    }

    public static void OnListenerAdding(GameEventTypes eventType, Delegate listenerBeingAdded) {
        if (!EventTable.ContainsKey(eventType))
            EventTable.Add(eventType, null);

        Delegate d = EventTable[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
            throw new ListenerException($"Attempting to add listener with inconsistent signature for event type {eventType}. Current listeners have type {d.GetType().Name} and listener being added has type {listenerBeingAdded.GetType().Name}");
    }

    public static void OnListenerRemoving(GameEventTypes eventType, Delegate listenerBeingRemoved) {
        if (EventTable.ContainsKey(eventType)) {
            Delegate d = EventTable[eventType];

            if (d == null) 
                throw new ListenerException($"Attempting to remove listener with for event type {eventType} but current listener is null.");

            if (d.GetType() != listenerBeingRemoved.GetType())
                throw new ListenerException($"Attempting to remove listener with inconsistent signature for event type {eventType}. Current listeners have type {d.GetType().Name} and listener being removed has type {listenerBeingRemoved.GetType().Name}");
        }
        else
            throw new ListenerException($"Attempting to remove listener for type {eventType} but Messenger doesn't know about this event type.");
    }

    public static void OnListenerRemoved(GameEventTypes eventType) {
        if (EventTable[eventType] == null)
            EventTable.Remove(eventType);
    }

    public static void OnBroadcasting(GameEventTypes eventType, MessengerMode mode) {
        if (mode == MessengerMode.REQUIRE_LISTENER && !EventTable.ContainsKey(eventType))
            throw new BroadcastException($"Broadcasting message {eventType} but no listener found.");
    }

    public static BroadcastException CreateBroadcastSignatureException(GameEventTypes eventType) {
        return new BroadcastException($"Broadcasting message {eventType.ToString()} but listeners have a different signature than the broadcaster.");
    }

    public class BroadcastException : Exception {
        public BroadcastException(string msg) : base(msg) {}
    }

    public class ListenerException : Exception {
        public ListenerException(string msg) : base(msg) {}
    }
}

public static class Messenger {
    public static void AddListener(GameEventTypes myGameEventType, Action handler) {
        MessengerInternal.AddListener(myGameEventType, handler);
    }

    public static void AddListener<TReturn>(GameEventTypes eventType, Func<TReturn> handler) {
        MessengerInternal.AddListener(eventType, handler);
    }

    public static void RemoveListener(GameEventTypes eventType, Action handler) {
        MessengerInternal.RemoveListener(eventType, handler);
    }

    public static void RemoveListener<TReturn>(GameEventTypes eventType, Func<TReturn> handler) {
        MessengerInternal.RemoveListener(eventType, handler);
    }

    public static void Broadcast(GameEventTypes eventType) {
        Broadcast(eventType, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast<TReturn>(GameEventTypes eventType, Action<TReturn> returnCall) {
        Broadcast(eventType, returnCall, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(GameEventTypes eventType, MessengerMode mode) {
        MessengerInternal.OnBroadcasting(eventType, mode);
        Action[] invocationList = MessengerInternal.GetInvocationList<Action>(eventType);

        foreach (Action callback in invocationList)
            callback.Invoke();
    }

    public static void Broadcast<TReturn>(GameEventTypes eventType, Action<TReturn> returnCall, MessengerMode mode) {
        MessengerInternal.OnBroadcasting(eventType, mode);
        Func<TReturn>[] invocationList = MessengerInternal.GetInvocationList<Func<TReturn>>(eventType);

        foreach (TReturn result in invocationList.Select(del => del.Invoke()))
            returnCall.Invoke(result);
    }
}

public static class Messenger<T> {
    public static void AddListener(GameEventTypes myGameEventType, Action<T> handler) {
        MessengerInternal.AddListener(myGameEventType, handler);
    }

    public static void AddListener<TReturn>(GameEventTypes eventType, Func<T, TReturn> handler) {
        MessengerInternal.AddListener(eventType, handler);
    }

    public static void RemoveListener(GameEventTypes eventType, Action<T> handler) {
        MessengerInternal.RemoveListener(eventType, handler);
    }

    public static void RemoveListener<TReturn>(GameEventTypes eventType, Func<T, TReturn> handler) {
        MessengerInternal.RemoveListener(eventType, handler);
    }

    public static void Broadcast(GameEventTypes eventType, T arg1) {
        Broadcast(eventType, arg1, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast<TReturn>(GameEventTypes eventType, T arg1, Action<TReturn> returnCall) {
        Broadcast(eventType, arg1, returnCall, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(GameEventTypes eventType, T arg1, MessengerMode mode) {
        MessengerInternal.OnBroadcasting(eventType, mode);
        Action<T>[] invocationList = MessengerInternal.GetInvocationList<Action<T>>(eventType);

        foreach (Action<T> callback in invocationList)
            callback.Invoke(arg1);
    }

    public static void Broadcast<TReturn>(GameEventTypes eventType, T arg1, Action<TReturn> returnCall, MessengerMode mode) {
        MessengerInternal.OnBroadcasting(eventType, mode);
        Func<T, TReturn>[] invocationList = MessengerInternal.GetInvocationList<Func<T, TReturn>>(eventType);

        foreach (TReturn result in invocationList.Select(del => del.Invoke(arg1)))
            returnCall.Invoke(result);
    }
}

public static class Messenger<T, T2> {
    public static void AddListener(GameEventTypes eventType, Action<T, T2> handler) {
        MessengerInternal.AddListener(eventType, handler);
    }

    public static void AddListener<TReturn>(GameEventTypes eventType, Func<T, T2, TReturn> handler) {
        MessengerInternal.AddListener(eventType, handler);
    }

    public static void RemoveListener(GameEventTypes eventType, Action<T, T2> handler) {
        MessengerInternal.RemoveListener(eventType, handler);
    }

    public static void RemoveListener<TReturn>(GameEventTypes eventType, Func<T, T2, TReturn> handler) {
        MessengerInternal.RemoveListener(eventType, handler);
    }

    public static void Broadcast(GameEventTypes eventType, T arg1, T2 arg2) {
        Broadcast(eventType, arg1, arg2, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast<TReturn>(GameEventTypes eventType, T arg1, T2 arg2, Action<TReturn> returnCall) {
        Broadcast(eventType, arg1, arg2, returnCall, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(GameEventTypes eventType, T arg1, T2 arg2, MessengerMode mode) {
        MessengerInternal.OnBroadcasting(eventType, mode);
        Action<T, T2>[] invocationList = MessengerInternal.GetInvocationList<Action<T, T2>>(eventType);

        foreach (Action<T, T2> callback in invocationList)
            callback.Invoke(arg1, arg2);
    }

    public static void Broadcast<TReturn>(GameEventTypes eventType, T arg1, T2 arg2, Action<TReturn> returnCall,
                                          MessengerMode mode) {
        MessengerInternal.OnBroadcasting(eventType, mode);
        Func<T, T2, TReturn>[] invocationList = MessengerInternal.GetInvocationList<Func<T, T2, TReturn>>(eventType);

        foreach (TReturn result in invocationList.Select(del => del.Invoke(arg1, arg2)))
            returnCall.Invoke(result);
    }
}


public static class Messenger<T, T2, T3> {
    public static void AddListener(GameEventTypes eventType, Action<T, T2, T3> handler) {
        MessengerInternal.AddListener(eventType, handler);
    }

    public static void AddListener<TReturn>(GameEventTypes eventType, Func<T, T2, T3, TReturn> handler) {
        MessengerInternal.AddListener(eventType, handler);
    }

    public static void RemoveListener(GameEventTypes eventType, Action<T, T2, T3> handler) {
        MessengerInternal.RemoveListener(eventType, handler);
    }

    public static void RemoveListener<TReturn>(GameEventTypes eventType, Func<T, T2, T3, TReturn> handler) {
        MessengerInternal.RemoveListener(eventType, handler);
    }

    public static void Broadcast(GameEventTypes eventType, T arg1, T2 arg2, T3 arg3) {
        Broadcast(eventType, arg1, arg2, arg3, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast<TReturn>(GameEventTypes eventType, T arg1, T2 arg2, T3 arg3, Action<TReturn> returnCall) {
        Broadcast(eventType, arg1, arg2, arg3, returnCall, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(GameEventTypes eventType, T arg1, T2 arg2, T3 arg3, MessengerMode mode) {
        MessengerInternal.OnBroadcasting(eventType, mode);
        Action<T, T2, T3>[] invocationList = MessengerInternal.GetInvocationList<Action<T, T2, T3>>(eventType);

        foreach (Action<T, T2, T3> callback in invocationList)
            callback.Invoke(arg1, arg2, arg3);
    }

    public static void Broadcast<TReturn>(GameEventTypes eventType, T arg1, T2 arg2, T3 arg3, Action<TReturn> returnCall,
                                          MessengerMode mode) {
        MessengerInternal.OnBroadcasting(eventType, mode);
        Func<T, T2, T3, TReturn>[] invocationList = MessengerInternal.GetInvocationList<Func<T, T2, T3, TReturn>>(eventType);

        foreach (TReturn result in invocationList.Select(del => del.Invoke(arg1, arg2, arg3)))
            returnCall.Invoke(result);
    }
}