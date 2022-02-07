using System;
using System.Collections.Generic;

namespace Zoo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            bool isWork = true;
            int aviaryNumber = 1;
            string femaleGender = "female";
            string maleGender = "male";

            List<Animal> lions = new List<Animal>
            {
                new PantheraLeo(femaleGender),
                new PantheraLeo(femaleGender),
                new PantheraLeo(maleGender)
            };

            List<Animal> vipers = new List<Animal>
            {
                new VipuraBerus(maleGender),
                new VipuraBerus(maleGender),
                new VipuraBerus(femaleGender),
                new VipuraBerus(femaleGender)
            };

            List<Animal> boars = new List<Animal>
            {
                new SusScrofa(maleGender),
                new SusScrofa(maleGender)
            };

            List<Animal> coders = new List<Animal>
            {
                new ProgrammatorOrdinarius(),
                new ProgrammatorOrdinarius()
            };

            List<Aviary> aviaries = new List<Aviary>
            {
                new Aviary(aviaryNumber++, lions),
                new Aviary(aviaryNumber++, vipers),
                new Aviary(aviaryNumber++, boars),
                new Aviary(aviaryNumber++, coders)
            };

            Zoo zoo = new Zoo(aviaries);

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine("Для выхода нажмите ESC");
                Console.WriteLine("или любую другую клавишу, чтобы выбрать вольер для просмотра.\n");
                zoo.ShowAviaries();

                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        isWork = false;
                        break;
                    default:
                        zoo.ApproachToAviary();
                        break;
                }
            }
        }
    }

    class Zoo
    {
        private List<Aviary> _aviaries;

        public Zoo(List<Aviary> aviaries)
        {
            _aviaries = aviaries;
        }

        public void ShowAviaries()
        {
            foreach (Aviary aviary in _aviaries)
            {
                Console.WriteLine($"Вольер №{aviary.Number}");
            }
        }

        public void ApproachToAviary()
        {
            Console.Write("\nВыберите номер вольера, чтобы подойти к нему: ");

            string userInput = Console.ReadLine();
            bool success = int.TryParse(userInput, out int inputNumber);
            bool isFind = TryFindAviary(inputNumber, out Aviary aviary);

            if (success && isFind)
            {
                DrawBorder();
                aviary.PrintInfo();
            }
            else
            {
                DrawBorder();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nВольера с таким номером в зоопарке нет.");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ReadKey();
        }

        private bool TryFindAviary(int inputNumber, out Aviary selectedAviary)
        {
            bool isFind = false;
            selectedAviary = null;

            foreach (Aviary aviary in _aviaries)
            {
                if (aviary.Number == inputNumber)
                {
                    isFind = true;
                    selectedAviary = aviary;
                }
            }

            return isFind;
        }
        
        private void DrawBorder()
        {
            char symbol = '*';
            int borderLength = 30;

            for (int i = 0; i < borderLength; i++)
            {
                Console.Write(symbol);
            }
        }
    }

    class Aviary
    {
        private List<Animal> _animals;
        private int _quantity;
        private int _maleCount;
        private int _femaleCount;
        private int _number;
        private string _species;

        public Aviary(int number, List<Animal> animals)
        {
            _number = number;
            _animals = animals;
            _quantity = 0;
            _maleCount = 0;
            _femaleCount = 0;

            if(_animals.Count > 0)
            {
                _species = _animals[0].Species;
            }
            else
            {
                _species = null;
            }
        }

        public int Number => _number;

        public void PrintInfo()
        {
            if (_animals.Count > 0)
            {
                RemoveAnimalDifferentSpecies();
                CalculateAnimals();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nВ этом вольере живёт {_species}.\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Здесь обитают {_quantity} особей");
                PrintGenderInfo();
                Console.WriteLine($"Если Вам повезёт, то сможете услышать,\nкак {_species.ToLower()} издаёт звук: {_animals[0].Sound}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("\nВ этом вольере пока никто не живёт");
            }
        }

        private void RemoveAnimalDifferentSpecies()
        {
            bool isHave = true;

            while (isHave)
            {
                Animal lastAnimal = _animals[_animals.Count - 1];
                Animal differentAnimal = null;

                foreach (Animal animal in _animals)
                {
                    if (animal.Species != _species)
                    {
                        differentAnimal = animal;
                        break;
                    }
                    else if (lastAnimal.Species == _species)
                    {
                        isHave = false;
                    }
                }

                _animals.Remove(differentAnimal);
            }
        }

        private void CalculateAnimals()
        {
            string femaleMarker = "female";
            string maleMarker = "male";

            foreach (Animal animal in _animals)
            {
                _quantity++;

                if (animal.Gender == femaleMarker)
                {
                    _femaleCount++;
                }
                else if (animal.Gender == maleMarker)
                {
                    _maleCount++;
                }
            }
        }

        private void PrintGenderInfo()
        {
            if (_femaleCount == 0 && _maleCount > 0)
            {
                Console.WriteLine("Все из них самцы.");
            }
            else if (_femaleCount > 0 && _maleCount == 0)
            {
                Console.WriteLine("Все из них самки.");
            }
            else if (_femaleCount > 0 && _maleCount > 0)
            {
                Console.WriteLine($"{_maleCount} из них самцы, а {_femaleCount} самки.");
            }
            else
            {
                Console.WriteLine("Ученные-зоологи всё еще не научились определять пол этих удивительных созданий.");
            }
        }

    }

    abstract class Animal
    {
        private string _sound;
        private string _species;
        private string _gender;

        public Animal(string gender)
        {
            _gender = gender;
        }

        public string Gender => _gender;

        public string Sound
        {
            get
            {
                return _sound;
            }
            protected set
            {
                _sound = value;
            }
        }

        public string Species
        {
            get
            {
                return _species;
            }
            protected set
            {
                _species = value;
            }
        }
    }

    class PantheraLeo : Animal
    {
        public PantheraLeo(string gender) : base(gender)
        {
            Species = "Лев";
            Sound = "\"Арррррр\"";
        }
    }

    class VipuraBerus : Animal
    {
        public VipuraBerus(string gender) : base(gender)
        {
            Species = "Гадюка";
            Sound = "\"Шшшшшш\"";
        }
    }

    class SusScrofa : Animal
    {
        public SusScrofa(string gender) : base(gender)
        {
            Species = "Кабан";
            Sound = "\"Уиииии\"";
        }
    }

    class ProgrammatorOrdinarius : Animal
    {
        public ProgrammatorOrdinarius() : base(null)
        {
            Species = "Кодер";
            Sound = "\"3 миллиарда устройств работают на Джаве, вообще-то\"";
        }
    }
}
