namespace RadElement2Fhir.Tests
{
    using RadElement2Fhir;

    public class UnitTest1
    {
        [Fact]
        public async void Run()
        {
            Options options = new Options()
            {
            };

            options.ValueSets.Add(new Options.ValueSet
            {
                Id = "RDE20"
            });

            Processor processor = new Processor(options);
            await processor.Execute();
        }
    }
}