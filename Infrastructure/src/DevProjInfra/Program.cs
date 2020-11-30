using Amazon.CDK;

namespace DevProjInfra
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new DevProjInfraStack(app, "DevProjInfraStack");

            app.Synth();
        }
    }
}
