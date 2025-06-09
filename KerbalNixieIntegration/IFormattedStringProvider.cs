using System;

namespace KerbalNixieIntegration;

public interface IFormattedStringProvider
{
    public void Init();
    public string GetValueString();
    
}