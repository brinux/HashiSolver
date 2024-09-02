using System.IO;
using Xunit.Abstractions;

public class XunitConsoleOutput : TextWriter
{
	private readonly ITestOutputHelper _output;

	public XunitConsoleOutput(ITestOutputHelper output)
	{
		_output = output;
	}

	public override void WriteLine(string message)
	{
		_output.WriteLine(message);
	}

	public override void WriteLine(string format, params object[] args)
	{
		_output.WriteLine(format, args);
	}

	public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
}
