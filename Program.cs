using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Fizzhz
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Thread ct = new Thread(new ThreadStart(CT));
            ct.IsBackground = true;
            ct.Start();

            using (var game = new Game1())
                game.Run();

        }

        public static List<Action> PrepearingActions = new List<Action>();
        public static List<Action> Executing = new List<Action>();

        public static Random rnd = new Random();
        public static Color[] Colors = new Color[] {Color.Green ,Color.Red, Color.Blue, Color.Orange, Color.Yellow,  Color.Indigo, Color.Purple };

        static void CT()
        {
            while (true)
            {
                try
                {
                    string a = Console.ReadLine();
                    if (a == null) break;

                    string command = a.Split(' ')[0];
                    if (command == "add")
                    {
                        string target = a.Split(' ')[1];
                        string targetField = a.Split(' ')[2];
                        string type = a.Split(' ')[3];

                        Slider Target = null;

                        Console.WriteLine((int)target[0] - (int)'a');

                        if (targetField == "a") Target = Game1.WeightSistems[(int)target[0] - (int)'a'].a;
                        if (targetField == "k") Target = Game1.WeightSistems[(int)target[0] - (int)'a'].k;
                        if (targetField == "m") Target = Game1.WeightSistems[(int)target[0] - (int)'a'].m;

                        PrepearingActions.Add(
                            delegate
                            {
                                if (type == "+")
                                    Target.CurrentValue += 1f;
                                else
                                    Target.CurrentValue -= 1f;
                                Target.UpdatedFormula = true;
                            }
                        );


                        String.Format("Added simulation {0} on {1} on field {2}", type, target, targetField);
                    }

                    if (command == "addw")
                    {
                        Game1.WeightSistems.Add(new WeightSistem(Colors[Game1.WeightSistems.Count], new Vector2(Game1.WeightSistems.Count * 300, 0)));
                    }

                    if (command == "execute")
                        for (int i = 0; i < PrepearingActions.Count; i++)
                            if (PrepearingActions[i] != null)
                                Executing.Add(PrepearingActions[i]);

                    if (command == "clear")
                    {
                        Executing.Clear();
                        PrepearingActions.Clear();
                    }

                    if(command == "rm"){
                        Game1.WeightSistems = new List<WeightSistem>();
                        Game1.WeightSistems.Add(new WeightSistem(Colors[Game1.WeightSistems.Count], new Vector2(Game1.WeightSistems.Count * 300, 0)));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

    }
}
