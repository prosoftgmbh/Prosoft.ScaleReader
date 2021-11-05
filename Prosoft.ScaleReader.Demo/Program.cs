using System;

namespace Prosoft.ScaleReader.Demo
{
    class Program
    {
        static ScaleReader ScaleReader { get; set; }

        static void Main(string[] args)
        {
            // Initialize a new Instance. For additional Port Options, check different Constructors. 
            ScaleReader = new ScaleReader();

            // Start the ScaleReader Instance
            ScaleReader.Start();

            // Hook into the PropertyChanged event of our ScaleReader
            ScaleReader.PropertyChanged += ScaleReader_PropertyChanged;

            Console.WriteLine("Press any Key to end the Programm.");
            Console.ReadLine();

            // Unhook our Method from our ScaleReader
            ScaleReader.PropertyChanged -= ScaleReader_PropertyChanged;

            // Stop the ScaleReader Instance (Happens automatically in the deconstructor of the ScaleReader class)
            ScaleReader.Stop();
        }

        private static void ScaleReader_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // The Value Property will be continuously updated in an asynchronys Task. It implements INotifyPropertyChanged, so that it is easy to use in Userinterfaces.
            Console.WriteLine(ScaleReader.Value);
        }
    }
}