using System;

[Serializable]
public abstract class EventData
{
    public string name;
}

//An event bus implementation without using Unity features, use this for
//the optional challenge:
//Publisher:
//  EventBus<YourEventData>.Publish(new YourEventData(...)) to publish
//
//Subscriber:
//  EventBus<YourEventData>.OnEventPublished += to subscribe
//  EventBus<YourEventData>.OnEventPublished -= to unsubscribe


public class EventBus<T> where T : EventData
{
    public static event Action<T> OnEventPublished;

    public static void Publish(T eventData)
    {
        OnEventPublished?.Invoke(eventData);
    }
}
