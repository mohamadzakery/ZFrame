
using System;
using System.Diagnostics;


public class ZFrame
{
    byte[] buffer = null;
    byte[] byteLength = null;
    int LastLen = 0;
    int LastRecivedLen = 0;
    int LenLength = 0;


    public delegate void onPacketRecived(byte[] data);
    public onPacketRecived OnPacketRecived;



    public void Reset()
    {
        LenLength = 0;
        LastLen = 0;
        LastRecivedLen = 0;
    }


    public ZFrame(int length)
    {
        buffer = new byte[length];
        byteLength = new byte[4];

        //OnPacketRecived = recived;
    }


    public void FromFrame(byte[] dataToAdd)
    {
        int offset = 0;
        int avalableLen = dataToAdd.Length;

        if (LenLength < 4)
        {

            if (LenLength + avalableLen >= 4)
            {
                Array.Copy(dataToAdd, 0, byteLength, LenLength, 4 - LenLength);
                //if (BitConverter.IsLittleEndian)
                //    Array.Reverse(byteLength);
                LastLen = BitConverter.ToInt32(byteLength, 0);
                offset = 4 - LenLength;
                LenLength = 4;
                avalableLen = dataToAdd.Length - offset;
            }
            else
            {
                Array.Copy(dataToAdd, 0, byteLength, LenLength, avalableLen);
                //LastLen = BitConverter.ToInt32(byteLength, 0);
                LenLength = avalableLen;
                offset = avalableLen;
                avalableLen = 0;
            }
        }


        if (avalableLen <= 0)
            return;



        if (LastRecivedLen + avalableLen >= LastLen)
        {
            //packet fuul recived

            byte[] dataReciv = new byte[LastLen];

            Array.Copy(buffer, 0, dataReciv, 0, LastRecivedLen);
            Array.Copy(dataToAdd, offset, dataReciv, LastRecivedLen, LastLen - LastRecivedLen);

            OnPacketRecived?.Invoke(dataReciv);
            avalableLen -= (LastLen - LastRecivedLen);
            offset += (LastLen - LastRecivedLen);
            LenLength = 0;
            LastLen = 0;
            LastRecivedLen = 0;


            if (avalableLen > 0)
            {
                byte[] remainData = new byte[avalableLen];
                Array.Copy(dataToAdd, offset, remainData, 0, avalableLen);
                FromFrame(remainData);
            }

        }
        else
        {
            // Buffer.BlockCopy(buffer, LastRecivedLen, dataToAdd, offset, avalableLen); bug=========<><><> fix
            Array.Copy(dataToAdd, offset,buffer, LastRecivedLen, avalableLen);
            LastRecivedLen += avalableLen;
        }
    }


    //private void recived(byte[] data)
    //{
    //    Console.WriteLine("recived:" + System.Text.Encoding.UTF8.GetString(data));
    //}


    public byte[] ToFrame(string message)
    {
        var bt = System.Text.Encoding.UTF8.GetBytes(message);
        return ToFrame(bt);
    }

    public byte[] ToFrame(byte[] data)
    {
        byte[] res = new byte[data.Length + 4];
        byte[] len = BitConverter.GetBytes(data.Length);

        Buffer.BlockCopy(len, 0, res, 0, 4);
        Buffer.BlockCopy(data, 0, res, 4, data.Length);
        return res;
    }

}