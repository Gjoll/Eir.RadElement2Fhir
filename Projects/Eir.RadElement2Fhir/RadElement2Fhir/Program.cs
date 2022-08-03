using RadElement2Fhir;

try
{
    RadElementToFhir processor = new RadElementToFhir();
    String[] clArgs = Environment.GetCommandLineArgs();
    Int32 clArgPtr = 1;
    String Arg()
    {
        if (clArgPtr > clArgs.Length)
            throw new Exception("Invalid command line args");
        return clArgs[clArgPtr++];
    }

    while (clArgPtr < clArgs.Length)
    {
        String arg = Arg();
        switch (arg.Trim().ToUpper())
        {
            case "-O":
                processor.OutputPath = Arg();
                break;
            case "-ID":
                processor.RadElementId = Arg();
                break;
            default:
                throw new Exception($"Unknown command line arg '{arg}'");
        }
    }
    processor.Execute();
    return 0;
}
catch(Exception err)
{
    Console.WriteLine(err.Message);
    return -1;
}