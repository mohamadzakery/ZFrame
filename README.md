# ZFrame
fast and flixable Framing library for TCP and other network protocols.
This library support all network protoclos for solving  dynamic data framing of any network stream. 


# Examples

## Example: convert Zframe to native type (string)


```c#
ZFrame frame = null;

    public Constructor() 
    {
        frame = new ZFrame(65536);
        frame.OnPacketRecived += PacketRecived;
    }
    
    
     void PacketRecived(byte[] data)
    {
        var message = Encoding.UTF8.GetString(data);
        Debug.Log("Raw:" + message);
    }
    
    
    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        byte[] bytes = new byte[size];
        Array.Copy(buffer, (int)offset, bytes, 0, (int)size);
        frame.FromFrame(bytes);

    }
    
```




## Example: convert native type (string) to Zframe 


```c#

    public void SendAsyncFrame(string message)
    {
        SendAsync(frame.ToFrame(message));
    }


```


