using System;
using System.IO;
using System.Collections.Generic;

namespace AdventofCode
{
    class Program
    {
        static Dictionary<int, Func<int, List<int>, List<int>>> opcodes = new Dictionary<int, Func<int, List<int>, List<int>>>();

        static void Main(string[] args)
        {
            List<int> modules = GetModules();
            List<int> additional = GetAdditionalFuel(modules);

            Console.WriteLine(String.Format("Day 1 p. 1: {0}", GetFuelRequired(modules)));
            Console.WriteLine(String.Format("Day 1 p. 2: {0}", GetAdditionalFuelTotal(additional)));

            List<int> opcode = GetOpcode();
            opcodes.Add(1, Opcode1);
            opcodes.Add(2, Opcode2);

            Console.WriteLine(String.Format("Day 2 p. 1: {0}", RunOpcode(opcode)[0]));

            List<int> newopcode = GetOpcode();
            Console.WriteLine(String.Format("Day 2 p. 2: {0}", FindOpcode(newopcode)));
        }

        static List<int> GetModules()
        {
            string filename = "inputs.txt";
            int fuel = 0;
            List<int> modules = new List<int>();

            string[] contents = File.ReadAllLines(filename);
            foreach (string line in contents)
            {
                var mass = Convert.ToInt32(line);
                var div = 3.0;

                int fuelRequired = Convert.ToInt32(Math.Floor(mass / div)) - 2;
                fuel = fuel + fuelRequired;

                modules.Add(fuelRequired);
            }

            return modules;
        }

        static int GetFuelRequired(List<int> modules)
        {
            int fuel = 0;

            foreach (int module in modules)
            {
                fuel += module;
            }

            return fuel;
        }

        static List<int> GetAdditionalFuel(List<int> modules)
        {
            List<int> fuel = new List<int>();

            foreach (int moduleFuel in modules)
            {
                int additional = moduleFuel;

                int add = additional;
                while (add > 0)
                {
                    var div = 3.0;
                    add = Convert.ToInt32(Math.Floor(add / div)) - 2;

                    if (add > 0)
                    {
                        additional += add;
                    }
                }

                fuel.Add(additional);
            }

            return fuel;
        }

        static int GetAdditionalFuelTotal(List<int> additional)
        {
            int fuel = 0;

            foreach (int additionalFuel in additional)
            {
                fuel += additionalFuel;
            }

            return fuel;
        }

        static List<int> Opcode1(int pos, List<int> opcode)
        {
            int val1 = opcode[opcode[pos + 1]];
            int val2 = opcode[opcode[pos + 2]];
            int newpos = opcode[pos + 3];

            int newval = val1 + val2;
            opcode[newpos] = newval;

            return opcode;
        }

        static List<int> Opcode2(int pos, List<int> opcode)
        {
            int val1 = opcode[opcode[pos + 1]];
            int val2 = opcode[opcode[pos + 2]];
            int newpos = opcode[pos + 3];

            int newval = val1 * val2;
            opcode[newpos] = newval;

            return opcode;
        }

        static List<int> GetOpcode()
        {
            string filename = "opcode.txt";
            string contents = File.ReadAllText(filename);

            List<int> rtn = new List<int>();
            string[] arr = contents.Split(","[0]);

            foreach (string val in arr)
            {
                try
                {
                    rtn.Add(Convert.ToInt32(val));
                }
                catch
                {
                    Console.WriteLine("Found invalid opcode: " + val);
                }
            }

            return rtn;
        }

        static List<int> RunOpcode(List<int> opcode)
        {
            opcode[1] = 12;
            opcode[2] = 2;

            for (int i = 0; i < opcode.Count - 1; i += 4)
            {
                int op = opcode[i];

                if (opcodes.ContainsKey(op))
                {
                    opcode = opcodes[op](i, opcode);
                }
                else if (op == 99)
                {
                    break;
                }
            }

            return opcode;
        }

        static List<int> DeepCopyOpcode(List<int> opcode)
        {
            List<int> newOpcode = new List<int>();

            foreach (int i in opcode)
            {
                newOpcode.Add(i);
            }

            return newOpcode;
        }

        static int FindOpcode(List<int> opcode)
        {
            for (int noun = 0; noun < 99; noun++)
            {
                for (int verb = 0; verb < 99; verb++)
                {
                    List<int> useOpcode = DeepCopyOpcode(opcode);
                    useOpcode[1] = noun;
                    useOpcode[2] = verb;

                    for (int i = 0; i < useOpcode.Count - 1; i += 4)
                    {
                        int op = useOpcode[i];

                        if (opcodes.ContainsKey(op))
                        {
                            try
                            {
                                useOpcode = opcodes[op](i, useOpcode);
                            }
                            catch
                            {
                                break;
                            }
                        }
                        else if (op == 99)
                        {
                            break;
                        }
                    }

                    if (useOpcode[0] == 19690720)
                    {
                        Console.WriteLine(noun);
                        Console.WriteLine(verb);
                        return 100 * noun + verb;
                    }
                }
            }

            return 0;
        }
    }
}
