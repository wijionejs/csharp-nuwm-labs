namespace UnitTesting;

using NUnit.Framework;

[TestFixture]
public class LogAnalyzerTests
{
  LogAnalyzer _logAnalyzer;

  [SetUp]
  public void SetUp()
  {
    _logAnalyzer = new LogAnalyzer();
  }

  [Test]
  public void IsValidLogFileName_RandomString_ReturnsFalse()
  {
    var fileName = "Not a file name at all";

    bool isValid = _logAnalyzer.IsValidLogFileName(fileName);

    Assert.That(isValid, Is.False);
  }

  [Test]
  public void IsValidLogFileName_InvalidExtension_ReturnsFalse()
  {
    var fileName = "FileWithInvalidExtension.invalid";

    bool isValid = _logAnalyzer.IsValidLogFileName(fileName);

    Assert.That(isValid, Is.False);
  }

  [Test]
  public void IsValidLogFileName_ValidExtensionLowerCase_ReturnsTrue()
  {
    var fileName = "FileWithValidExtension.slf";

    bool isValid = _logAnalyzer.IsValidLogFileName(fileName);

    Assert.That(isValid, Is.True);
  }

  [Test]
  public void IsValidLogFileName_ValidExtensionMixedCase_ReturnsTrue()
  {
    var fileName = "FileWithValidExtension.sLf";

    bool isValid = _logAnalyzer.IsValidLogFileName(fileName);

    Assert.That(isValid, Is.True);
  }

  [Test]
  public void IsValidLogFileName_ValidExtensionUpperCase_ReturnsTrue()
  {
    var fileName = "FileWithValidExtension.SLF";

    bool isValid = _logAnalyzer.IsValidLogFileName(fileName);

    Assert.That(isValid, Is.True);
  }
}
