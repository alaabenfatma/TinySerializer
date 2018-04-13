using System;
using System.Collections.Generic;
using System.IO;

namespace Test
{
    internal class Program
    {
        private static void OnePet()
        {
            var petData = File.ReadAllText("OnePet.txt");
            var pet = TinySerializer.TinySerializer.DeSerialize(petData, new Pet());
            Console.WriteLine(
                $"This pet is a {pet.Type} and it is called {pet.Name}.\nAge:{pet.Age}\nWeight:{pet.Weight}KG");
        }

        private static void ManyPets()
        {
            var petsData =
                TinySerializer.TinySerializer.ExtractData(File.ReadAllText("ManyPets.txt"), "<?", "?>", true);
            var pets = new List<Pet>();
            Console.WriteLine($"This list contains {petsData.Count} pets.");
            foreach (var pet in petsData)
                pets.Add(TinySerializer.TinySerializer.DeSerialize(pet, new Pet()));
            foreach (var pet in pets)
                Console.WriteLine($"\nThis pet is called {pet.Name}.\nAge:{pet.Age}\nWeight:{pet.Weight}KG");
        }
        private static void Serialize()
        {
            var pet = new Pet();
            Console.Write("Type of the pet:"); var type = Console.ReadLine();
            Console.Write("Name of the pet:"); var name = Console.ReadLine();
            Console.Write("Age of the pet:"); var age = Console.ReadLine();
            Console.Write("Weight of the pet:"); var weight = Convert.ToDouble(Console.ReadLine());
            pet.Age = age;
            pet.Name = name;
            pet.Weight = weight;
            pet.Type = type;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Generated code:\n{TinySerializer.TinySerializer.Serialize(pet)}");
        }
        private static void Main()
        {
            Console.Title = "TinySerializer for .NET";
            Console.WriteLine("1)Serialize\n2)Deserialize");
            var choice = Console.ReadLine();
            if (choice == "1")
                Serialize();
            else if (choice == "2")
            {
                Console.WriteLine("1)Show one pet\n2)Show many pets");
                choice = Console.ReadLine();
                if (choice == "1")
                    OnePet();
                else if (choice == "2")
                    ManyPets();

            }

            Console.ReadKey();
        }

        private class Pet
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Age { get; set; }
            public double Weight { get; set; }
        }
    }
}