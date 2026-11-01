using System;
using System.Generic.Collection;

// What every Subscriber (Observer) must be able to do
//step1
public interface ISubscriber
{
    void ReceiveNotification(string channelName, string videoTitle);
}

//step2
// What every Creator (Subject) must be able to do
public interface IYouTubeChannel
{
    void Subscribe(ISubscriber subscriber);
    void Unsubscribe(ISubscriber subscriber);
    void NotifySubscribers();
}

//step 3
public class YouTubeChannel : IYouTubeChannel
{
    private readonly List<ISubscriber> _subscribers = new List<ISubscriber>();
    private readonly string _channelName;
    private string _latestVideoTitle = "";

    public YouTubeChannel(string name)
    {
        _channelName = name;
    }

    // This method is called whenever the creator uploads a new video
    public void UploadVideo(string title)
    {
        _latestVideoTitle = title;
        Console.WriteLine($"\n[{_channelName}] Uploaded new video: '{title}'");
        
        // Instantly notify everyone on the subscription list
        NotifySubscribers();
    }

    public void Subscribe(ISubscriber subscriber)
    {
        _subscribers.Add(subscriber);
    }

    public void Unsubscribe(ISubscriber subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    public void NotifySubscribers()
    {
        foreach (var subscriber in _subscribers)
        {
            // Push the update to each individual subscriber
            subscriber.ReceiveNotification(_channelName, _latestVideoTitle);
        }
    }
}

//step4 
public class YouTubeUser : ISubscriber
{
    private readonly string _userName;

    public YouTubeUser(string name)
    {
        _userName = name;
    }

    // This method is triggered automatically when a channel uploads a video
    public void ReceiveNotification(string channelName, string videoTitle)
    {
        Console.WriteLine($"   -> Ping! Notification for {_userName}: '{channelName}' just posted '{videoTitle}'");
    }
}

class Program
{
    static void Main()
    {
        // 1. Create a YouTube Channel
        YouTubeChannel techChannel = new YouTubeChannel("TechGuyReviews");

        // 2. Create some users
        YouTubeUser userAlice = new YouTubeUser("Alice");
        YouTubeUser userBob = new YouTubeUser("Bob");

        // 3. They click the "Subscribe" button
        techChannel.Subscribe(userAlice);
        techChannel.Subscribe(userBob);

        // 4. The channel uploads a video -> Both users get an instant notification!
        techChannel.UploadVideo("iPhone 18 Pro Review");

        // 5. Bob decides to unsubscribe from the channel
        techChannel.Unsubscribe(userBob);

        // 6. The channel uploads another video -> Only Alice gets the notification!
        techChannel.UploadVideo("Top 5 Laptops of 2026");
    }
}