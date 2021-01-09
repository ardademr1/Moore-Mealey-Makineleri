using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsmanArdaDemir
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Moore MooreMakine;
        Mealey MealeyMakine;
        string StateString;
        string outputString;
        string current_path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = null;
            try
            {
                
                lines = System.IO.File.ReadAllLines(current_path + "\\INPUT.txt");
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("INPUT.txt dosyası bulunamadı!");
            }
            
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label7.Text = $"{lines[0]}";
            label8.Text = $"{lines[1]}";
            label9.Text = $"{lines[2]}";
            // Display the file contents by using a foreach loop.
            label3.Visible = true;
            string[] states = lines[0].Replace(" ", "").Split('{', '}')[1].Split(',');
            string[] alphabet = lines[1].Replace(" ", "").Split('{', '}')[1].Split(',');
            string[] output = lines[2].Replace(" ", "").Split('{', '}')[1].Split(',');

            if (MooreRadio.Checked)
            {
                MooreMakine = new Moore(alphabet, output, states);
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();
                dataGridView1.Columns.Add("Old State", "Old State");
                dataGridView1.Columns[0].ReadOnly = true;
                foreach (var alfabe in MooreMakine.alphabet)
                {
                    dataGridView1.Columns.Add(
                        new DataGridViewTextBoxColumn()
                        {
                            HeaderText = alfabe,
                            Name = alfabe
                        });
                }
                dataGridView1.Columns.Add(
                    new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Output",
                        Name = "Output"
                    });

                for (int i = 0; i < MooreMakine.state.Length; i++)
                {
                    var state = MooreMakine.state[i];
                    dataGridView1.Rows.Add(new string[] { state });
                }
            }
            else if (MealeyRadio.Checked)
            {
                MealeyMakine = new Mealey(alphabet, output, states);
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();
                dataGridView1.Columns.Add("Old State", "Old State");
                dataGridView1.Columns[0].ReadOnly = true;
                foreach (var item in MealeyMakine.alphabet)
                {
                    dataGridView1.Columns.Add(
                        new DataGridViewTextBoxColumn()
                        {
                            HeaderText = item + " , New State",
                            Name = item + " , New State"
                        });
                    dataGridView1.Columns.Add(
                        new DataGridViewTextBoxColumn()
                        {
                            HeaderText = item + " , Output",
                            Name = item + " , Output"
                        });
                }
                for (int i = 0; i < MealeyMakine.state.Length; i++)
                {
                    var state = MealeyMakine.state[i];
                    dataGridView1.Rows.Add(new string[] { state });
                }
            }
        }
        
        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MooreRadio.Checked)
            {
                List<State> StateList = new List<State>();
                StateList = new List<State>();
                for(int i = 0; i < MooreMakine.state.Length; i++) {
                    var eachState = MooreMakine.state[i];
                    var Input = "";
                    StateList.Add(new State(eachState,Input));
                }
                for(int a=0;a< dataGridView1.RowCount; a++) // satır ı dolaşmak
                {
                    DataGridViewRow row=dataGridView1.Rows[a];
                    for (int b = 0; b < row.Cells.Count; b++){ // satırdaki her bir hücreyi dolaşmak
                        DataGridViewCell cell = row.Cells[b];
                        if (cell.Value != null)
                        {
                            var newState = StateList.Find(x => x.name == row.Cells[0].Value.ToString());
                            newState.Transitions = new List<Transition>();
                            newState.Outputs = row.Cells[row.Cells.Count - 1].Value.ToString();
                            for (int i = 0; i < MooreMakine.alphabet.Length; i++)
                            {
                                var upTransitionState = StateList.Find(x => x.name == row.Cells[i + 1].Value.ToString());
                                var Trnstion = new Transition()
                                {
                                    Input = MooreMakine.alphabet[i],
                                    State = upTransitionState
                                };
                                newState.Transitions.Add(Trnstion);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Tabloda veya aranacak kelime girdisinde hata var!");
                        }

                    }
                }
                

                var currentState = StateList[0];
                string stateString = currentState.name + " ";
                richTextBox1.Text = "";
                string outputString = currentState.Outputs + " ";
                for (int i = 0; i < textBox1.Text.Length; i++)
                {
                    var letter = textBox1.Text[i];
                    var eslesenGecis = currentState.Transitions.Find(transition => transition.Input == letter.ToString());
                    outputString += currentState.Outputs + " ";
                    stateString += currentState.name + " ";
                    currentState = eslesenGecis.State;
                }
                string InputString = "";
                for (int i = 0; i < textBox1.Text.Length; i++)
                {
                    var harfler = textBox1.Text[i];
                    InputString += $"\t{harfler}";
                }
                    
                string outStates = stateString.Replace(" ", "\t");
                string output = outputString.Replace(" ", "\t");
                richTextBox1.Text = $"InputString:\t{InputString}\n" + $"State:\t\t{outStates}\n" + $"Output:\t{ output}";
                string[] lines = { $"InputString:{InputString}", $"State:{outStates}", $"Output:{output}" };
                System.IO.File.WriteAllLines(current_path + "\\OUTPUT.txt", lines);
                label6.Visible = true;
            }
            else if (MealeyRadio.Checked)
            {
                List<State> StateList = new List<State>();
                for (int i = 0; i < MealeyMakine.state.Length; i++)
                {
                    var eachState = MealeyMakine.state[i];
                    var Input = "";
                    StateList.Add(new State(eachState,Input));
                }

                for (int a = 0; a < dataGridView1.RowCount; a++)
                {
                    DataGridViewRow row = dataGridView1.Rows[a];
                    for (int b = 0; b < row.Cells.Count; b++)
                    { // satırdaki her bir hücreyi dolaşmak
                        DataGridViewCell cell = row.Cells[b];
                        if (cell.Value != null)
                        {
                            var durum = StateList.Find(x => x.name == row.Cells[0].Value.ToString());
                            durum.Transitions = new List<Transition>();
                            for (int y = 1; y < row.Cells.Count; y += 2)
                            {
                                var takeCell = row.Cells[y];
                                var upTransitionState = StateList.Find(x => x.name == takeCell.Value.ToString());
                                var output = row.Cells[y + 1].Value.ToString();
                                Transition trnston = new Transition()
                                {
                                    Input = MealeyMakine.alphabet[y / 2].ToString(),
                                    Output = output,
                                    State = upTransitionState
                                };
                                durum.Transitions.Add(trnston);
                            }
                        }
                        else { MessageBox.Show("Tabloda veya aranacak kelime girdisinde hata var!"); }

                    }
                }
                richTextBox1.Text = "";

                var currentState = StateList[0];
                StateString = "";
                outputString = "";
                for(int i = 0; i < textBox1.Text.Length; i++)
                {
                    var letters = textBox1.Text[i];
                    var findTransition = currentState.Transitions.Find(transition => transition.Input == letters.ToString());
                    currentState = findTransition.State;
                    outputString += findTransition.Output + " ";
                    StateString += currentState.name + " ";
                    string InputString = string.Empty;
                    string OutStates = StateString.Replace(" ", "\t");
                    string Output = outputString.Replace(" ", "\t");
                    for (int j = 0; j < textBox1.Text.Length; j++)
                    {
                        var letter = textBox1.Text[j];
                        InputString += $"{letter}\t";
                    }
                    richTextBox1.Text = $"InputString:\t{InputString}\n" + $"New State:\t{OutStates}\n" + $"Output:\t{ Output}";
                    string[] lines = { $"InputString:{InputString}", $"State:{OutStates}", $"Output:{Output}" };
                    System.IO.File.WriteAllLines(current_path + "\\OUTPUT.txt", lines);
                    label6.Visible = true;
                }

            }
        }

        private void MealeyRadio_Click(object sender, EventArgs e)
        {
            label3.Visible = label7.Visible = label8.Visible = label9.Visible = label6.Visible = false;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            textBox1.Text = "Aranacak Kelimeyi Girin...";
            richTextBox1.Text = string.Empty;

        }

        private void MooreRadio_Click(object sender, EventArgs e)
        {
            label3.Visible = label7.Visible = label8.Visible = label9.Visible = label6.Visible = false;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            textBox1.Text = "Aranacak Kelimeyi Girin...";
            richTextBox1.Text = string.Empty;
        }
    }
}
