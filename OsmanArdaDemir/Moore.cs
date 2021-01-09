using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsmanArdaDemir
{
    
    public class Transition
    {
        public string Input { get; set; }
        public string Output { get; set; }  
        public State State { get; set; }
    }
    public class State
    {
        public List<Transition> Transitions { get; set; }
        public string name { get; set; }
        public string Outputs { get; set; }
        public string Input { get; set; }
        public State(string name,string Input)
        {
            this.name = name;
            this.Input = Input;
        }
    }
    class Moore
    {
        public string[] alphabet { get; set; }
        public string[] output { get; set; }
        public string[] state { get; set; }
        
        public Moore(string[] alphabet, string[] output, string[] state) //constructor
        {
            this.alphabet = alphabet;
            this.output = output;
            this.state = state;
        }
    }
}
