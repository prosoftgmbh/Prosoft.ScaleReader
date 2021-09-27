using System;

namespace Prosoft.ScaleReader.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize a new Instance. For Additional Port Options, check different Constructors
            var scaleReader = new ScaleReader();

            // Start the ScaleReader Instance
            scaleReader.Start();

            // Value will be continuously updated in an asynchronys Task. It implements INotifyPropertyChanged so that it is easy to use in Userinterfaces.
            Console.WriteLine(scaleReader.Value);

            // Stop the ScaleReader Instance (Happens automatically in the deconstructor of the ScaleReader)
            scaleReader.Stop();
        }
    }
}