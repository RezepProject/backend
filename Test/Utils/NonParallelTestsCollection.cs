using Emgu.CV.Ocl;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Test.Utils;

[CollectionDefinition("Non-Parallel Tests", DisableParallelization = true)]
public class NonParallelTestsCollection : ICollectionFixture<WebApplicationFactory<Program>>
{
    // This class has no code, and is never created. Its purpose is 
    // simply to be the place for the CollectionDefinition 
    // and hold the TestSetup instance.
}