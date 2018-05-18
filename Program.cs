using System;
using System.Text.RegularExpressions;
using System.Threading;
using Cal.Components;
using ComponentLib;

namespace Cal {
    class Program {
        static void Main (string[] args) {

            CalUnit cal = new CalUnit ("aggregate.calUnit", "calUnit");
            Add add = new Add ("chips.add", "add");
            Sub sub = new Sub ("chips.sub", "sub");
            Mul mul = new Mul ("chips.mul", "mul");
            Div div = new Div ("chips.div", "div");

            Console.WriteLine ("working...");

            string line;
            while ((line = Console.ReadLine ()).Trim () != "q") {
                try {
                    var match = Regex.Match (line, @"(?<a>\d+(\.\d+)?)(?<op>[+\-*/])(?<b>\d+(\.\d+)?)");
                    double a = Convert.ToDouble (match.Groups["a"].Value);
                    string op = match.Groups["op"].Value;
                    double b = Convert.ToDouble (match.Groups["b"].Value);

                    cal.calculate (op, a, b);
                } catch (Exception) {
                    Console.WriteLine ("invalid input.");
                }
            }
            div.stopWork ();
            mul.stopWork ();
            sub.stopWork ();
            add.stopWork ();
            cal.stopWork ();
        }
    }
}