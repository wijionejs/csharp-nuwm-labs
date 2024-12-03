// See https://aka.ms/new-console-template for more information

namespace UnitTesting;


public class LogAnalyzer
{
  public bool IsValidLogFileName(string fileName)
  {
    if (!fileName.ToLower().EndsWith(".slf"))
    {
      return false;
    }
    return true;
  }
}