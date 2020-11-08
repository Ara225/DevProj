using Amazon.CDK;

namespace ProjectManagerInfra
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new ProjectManagerInfraStack(app, "ProjectManagerInfraStack");

            app.Synth();
        }
    }
}
