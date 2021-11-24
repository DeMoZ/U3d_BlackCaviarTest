using System;

public interface ISaver
{
    void Save(string data, Action success);
}