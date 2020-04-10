using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Change strings of characters to strings of binary code and vice-versa.
///
/// Keep it clean and simple!
/// Created by Maxime Phaneuf
/// April 2019
/// </summary>
public static class BinaryParser
{
    /// <summary>
    /// Takes a string of characters and converts it to binary.
    /// </summary>
    public static string StringToBinary(string data)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in data.ToCharArray())
            sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
        return sb.ToString();
    }

    /// <summary>
    /// Takes a string of binary and converts it to a string of characters
    /// </summary>
    public static string BinaryToString(string data)
    {
        List<Byte> byteList = new List<Byte>();
        for (int i = 0; i < data.Length; i += 8)
            byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
        return Encoding.ASCII.GetString(byteList.ToArray());
    }
}
