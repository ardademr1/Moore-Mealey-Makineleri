using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsmanArdaDemir
{
    
    class Mealey
    {
        public string[] alphabet { get; set; }
        public string[] output { get; set; }
        public string[] state { get; set; }
        public Mealey(string[] alphabet, string[] output, string[] state) //Constructer
        {
            this.alphabet = alphabet;
            this.output = output;
            this.state = state;
        }
        

    }
}
