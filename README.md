# ZFrame
fast and flixable Framing library for TCP and other network protocols.
This library support all network protoclos for solving  dynamic data framing of any network stream. 


# Examples

## Example: convert Zframe to native type (string)


```c#
    
    ZFrame frame = null;
    
    //can init in any method but i init in constructor of class
    public Constructor() 
    {
        frame = new ZFrame(65536);
        frame.OnPacketRecived += PacketRecived;
    }
    
    //Zframe event
    void PacketRecived(byte[] data)
    {
        var message = Encoding.UTF8.GetString(data);
        Debug.Log("Raw:" + message);
    }
    
    //OnRecive of  C# Socket
    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        byte[] bytes = new byte[size];
        Array.Copy(buffer, (int)offset, bytes, 0, (int)size);
        frame.FromFrame(bytes);

    }
    
```




## Example: convert native type (string) to Zframe 


```c#

    //send method
    public void Send(string message)
    {
        //convert any type to Zframe byte[]
        byte[] ZframePacket=frame.ToFrame(message);
        SendAsync(ZframePacket);
    }


```


