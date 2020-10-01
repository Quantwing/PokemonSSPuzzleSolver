using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PuzzleSolver
{
    public partial class PokemonSolvers : Form
    {
        private Panel activeSolverPanel;
        private Thread activeSolverThread;
        private List<int[,]> voltorbFlipResult;
        private List<int[,]> voltorbFlipFilteredResult = new List<int[,]>();
        private List<Button> voltorbResultButtons = new List<Button>();
        private Dictionary<string, int[]> voltorbPressedResultLabels = new Dictionary<string, int[]>();
        private int voltorbCurrentActiveResult = 0;

        public PokemonSolvers()
        {
            InitializeComponent();
        }

        private void voltorbflipbutton_Click(object sender, EventArgs e)
        {
            if (activeSolverPanel == voltorbflippanel)
            {
                activeSolverPanel.Visible = false;
                activeSolverPanel = null;
            }
            else
            {
                if (activeSolverPanel != null)
                {
                    activeSolverPanel.Visible = false;
                }

                activeSolverPanel = voltorbflippanel;
                activeSolverPanel.Visible = true;
            }
        }

        private void startsolverbutton_Click(object sender, EventArgs e)
        {
            if (activeSolverThread == null)
            {
                int[] points = new int[0];
                int[] orbs = new int[0];

                try
                {
                    points = new int[] {
                        int.Parse(line1rightinput_points.Text),
                        int.Parse(line2rightinput_points.Text),
                        int.Parse(line3rightinput_points.Text),
                        int.Parse(line4rightinput_points.Text),
                        int.Parse(line5rightinput_points.Text),
                        int.Parse(line1bottominput_points.Text),
                        int.Parse(line2bottominput_points.Text),
                        int.Parse(line3bottominput_points.Text),
                        int.Parse(line4bottominput_points.Text),
                        int.Parse(line5bottominput_points.Text)};
                    orbs = new int[] {
                        int.Parse(line1rightinput_orbs.Text),
                        int.Parse(line2rightinput_orbs.Text),
                        int.Parse(line3rightinput_orbs.Text),
                        int.Parse(line4rightinput_orbs.Text),
                        int.Parse(line5rightinput_orbs.Text),
                        int.Parse(line1bottominput_orbs.Text),
                        int.Parse(line2bottominput_orbs.Text),
                        int.Parse(line3bottominput_orbs.Text),
                        int.Parse(line4bottominput_orbs.Text),
                        int.Parse(line5bottominput_orbs.Text)};
                }
                catch
                {
                    solverinfolabel.Text = "Input parse error";
                    return;
                }

                solverinfolabel.Text = "Calculating...";
                activeSolverThread = new Thread(() => new VoltorbFlip(this, points, orbs).Start());
                activeSolverThread.Start();
            }
            else
            {
                solverinfolabel.Text = "Aborted";
                activeSolverThread.Abort();
                activeSolverThread = null;
            }
        }

        public void Asd()
        {
            solverinfolabel.Text = "vittu";
        }

        public void VoltorbFlipSolverComplete(VoltorbFlip.Results result)
        {
            activeSolverThread = null;
            voltorbFlipResult = result.cells;
            voltorbPressedResultLabels.Clear();
            RefreshVoltorResultDisplay();
        }

        private void RefreshVoltorResultDisplay()
        {
            FilterAndSortVoltorbResult();
            CreateVoltorbResultButtons();
            SetVoltorbFlipResultVisible(0);
            solverinfolabel.Text = voltorbFlipFilteredResult.Count + " results";
        }

        private void CreateVoltorbResultButtons()
        {
            for (int i = 0; i < voltorbResultButtons.Count; i++)
            {
                voltorbResultButtons[i].Dispose();
            }
            voltorbResultButtons.Clear();
            for (int i = 0; i < Math.Min(voltorbFlipFilteredResult.Count, 10); i++)
            {
                Button resultButton = new Button();
                voltorbflippanel.Controls.Add(resultButton);
                voltorbResultButtons.Add(resultButton);
                resultButton.Location = new System.Drawing.Point(3 + i % 5 * 85, 375 + (int)Math.Floor(i / 5d) * 25);
                resultButton.Name = "resultbutton" + i;
                resultButton.Size = new System.Drawing.Size(75, 23);
                resultButton.TabIndex = 10;
                resultButton.Text = "Result " + (i + 1);
                resultButton.UseVisualStyleBackColor = true;
                int index = i;
                resultButton.Click += new System.EventHandler((x, y) => { SetVoltorbFlipResultVisible(index); } );
            }
        }

        private void FilterAndSortVoltorbResult()
        {
            voltorbFlipFilteredResult.Clear();
            int[][] filters = voltorbPressedResultLabels.Values.ToArray();
            bool flag;

            flag = true;
            for (int i = 0; i < voltorbFlipResult.Count; i++)
            {
                for (int p = 0; p < filters.GetLength(0); p++)
                {
                    if (voltorbFlipResult[i][filters[p][0], filters[p][1]] != filters[p][2])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    voltorbFlipFilteredResult.Add(voltorbFlipResult[i]);
                }
            }
        }

        private void SetVoltorbFlipResultVisible(int index)
        {
            voltorbCurrentActiveResult = index;
            if (voltorbCurrentActiveResult >= voltorbFlipFilteredResult.Count)
                return;

            line11resultlabel.Text = voltorbFlipFilteredResult[index][0, 0] != 0 ? voltorbFlipFilteredResult[index][0, 0].ToString() : "X";
            line11resultlabel.Visible = true;
            line12resultlabel.Text = voltorbFlipFilteredResult[index][1, 0] != 0 ? voltorbFlipFilteredResult[index][1, 0].ToString() : "X";
            line12resultlabel.Visible = true;
            line13resultlabel.Text = voltorbFlipFilteredResult[index][2, 0] != 0 ? voltorbFlipFilteredResult[index][2, 0].ToString() : "X";
            line13resultlabel.Visible = true;
            line14resultlabel.Text = voltorbFlipFilteredResult[index][3, 0] != 0 ? voltorbFlipFilteredResult[index][3, 0].ToString() : "X";
            line14resultlabel.Visible = true;
            line15resultlabel.Text = voltorbFlipFilteredResult[index][4, 0] != 0 ? voltorbFlipFilteredResult[index][4, 0].ToString() : "X";
            line15resultlabel.Visible = true;

            line21resultlabel.Text = voltorbFlipFilteredResult[index][0, 1] != 0 ? voltorbFlipFilteredResult[index][0, 1].ToString() : "X";
            line21resultlabel.Visible = true;
            line22resultlabel.Text = voltorbFlipFilteredResult[index][1, 1] != 0 ? voltorbFlipFilteredResult[index][1, 1].ToString() : "X";
            line22resultlabel.Visible = true;
            line23resultlabel.Text = voltorbFlipFilteredResult[index][2, 1] != 0 ? voltorbFlipFilteredResult[index][2, 1].ToString() : "X";
            line23resultlabel.Visible = true;
            line24resultlabel.Text = voltorbFlipFilteredResult[index][3, 1] != 0 ? voltorbFlipFilteredResult[index][3, 1].ToString() : "X";
            line24resultlabel.Visible = true;
            line25resultlabel.Text = voltorbFlipFilteredResult[index][4, 1] != 0 ? voltorbFlipFilteredResult[index][4, 1].ToString() : "X";
            line25resultlabel.Visible = true;

            line31resultlabel.Text = voltorbFlipFilteredResult[index][0, 2] != 0 ? voltorbFlipFilteredResult[index][0, 2].ToString() : "X";
            line31resultlabel.Visible = true;
            line32resultlabel.Text = voltorbFlipFilteredResult[index][1, 2] != 0 ? voltorbFlipFilteredResult[index][1, 2].ToString() : "X";
            line32resultlabel.Visible = true;
            line33resultlabel.Text = voltorbFlipFilteredResult[index][2, 2] != 0 ? voltorbFlipFilteredResult[index][2, 2].ToString() : "X";
            line33resultlabel.Visible = true;
            line34resultlabel.Text = voltorbFlipFilteredResult[index][3, 2] != 0 ? voltorbFlipFilteredResult[index][3, 2].ToString() : "X";
            line34resultlabel.Visible = true;
            line35resultlabel.Text = voltorbFlipFilteredResult[index][4, 2] != 0 ? voltorbFlipFilteredResult[index][4, 2].ToString() : "X";
            line35resultlabel.Visible = true;

            line41resultlabel.Text = voltorbFlipFilteredResult[index][0, 3] != 0 ? voltorbFlipFilteredResult[index][0, 3].ToString() : "X";
            line41resultlabel.Visible = true;
            line42resultlabel.Text = voltorbFlipFilteredResult[index][1, 3] != 0 ? voltorbFlipFilteredResult[index][1, 3].ToString() : "X";
            line42resultlabel.Visible = true;
            line43resultlabel.Text = voltorbFlipFilteredResult[index][2, 3] != 0 ? voltorbFlipFilteredResult[index][2, 3].ToString() : "X";
            line43resultlabel.Visible = true;
            line44resultlabel.Text = voltorbFlipFilteredResult[index][3, 3] != 0 ? voltorbFlipFilteredResult[index][3, 3].ToString() : "X";
            line44resultlabel.Visible = true;
            line45resultlabel.Text = voltorbFlipFilteredResult[index][4, 3] != 0 ? voltorbFlipFilteredResult[index][4, 3].ToString() : "X";
            line45resultlabel.Visible = true;

            line51resultlabel.Text = voltorbFlipFilteredResult[index][0, 4] != 0 ? voltorbFlipFilteredResult[index][0, 4].ToString() : "X";
            line51resultlabel.Visible = true;
            line52resultlabel.Text = voltorbFlipFilteredResult[index][1, 4] != 0 ? voltorbFlipFilteredResult[index][1, 4].ToString() : "X";
            line52resultlabel.Visible = true;
            line53resultlabel.Text = voltorbFlipFilteredResult[index][2, 4] != 0 ? voltorbFlipFilteredResult[index][2, 4].ToString() : "X";
            line53resultlabel.Visible = true;
            line54resultlabel.Text = voltorbFlipFilteredResult[index][3, 4] != 0 ? voltorbFlipFilteredResult[index][3, 4].ToString() : "X";
            line54resultlabel.Visible = true;
            line55resultlabel.Text = voltorbFlipFilteredResult[index][4, 4] != 0 ? voltorbFlipFilteredResult[index][4, 4].ToString() : "X";
            line55resultlabel.Visible = true;
        }

        private void LabelClick(Label label, int[] pos)
        {
            if (voltorbPressedResultLabels.ContainsKey(label.Name))
            {
                voltorbPressedResultLabels.Remove(label.Name);
                label.BackColor = DefaultBackColor;
            }
            else
            {
                voltorbPressedResultLabels.Add(label.Name, new int[] { pos[0], pos[1], voltorbFlipFilteredResult[voltorbCurrentActiveResult][pos[0], pos[1]] });
                label.BackColor = Color.Green;
            }
            RefreshVoltorResultDisplay();
        }

        private void line11resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line11resultlabel, new int[] { 0, 0 });
        }

        private void line12resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line12resultlabel, new int[] { 1, 0 });
        }

        private void line13resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line13resultlabel, new int[] { 2, 0 });
        }

        private void line14resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line14resultlabel, new int[] { 3, 0 });
        }

        private void line15resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line15resultlabel, new int[] { 4, 0 });
        }

        private void line21resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line21resultlabel, new int[] { 0, 1 });
        }

        private void line22resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line22resultlabel, new int[] { 1, 1 });
        }

        private void line23resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line23resultlabel, new int[] { 2, 1 });
        }

        private void line24resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line24resultlabel, new int[] { 3, 1 });
        }

        private void line25resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line25resultlabel, new int[] { 4, 1 });
        }

        private void line31resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line31resultlabel, new int[] { 0, 2 });
        }

        private void line32resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line32resultlabel, new int[] { 1, 2 });
        }

        private void line33resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line33resultlabel, new int[] { 2, 2 });
        }

        private void line34resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line34resultlabel, new int[] { 3, 2 });
        }

        private void line35resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line35resultlabel, new int[] { 4, 2 });
        }

        private void line41resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line41resultlabel, new int[] { 0, 3 });
        }

        private void line42resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line42resultlabel, new int[] { 1, 3 });
        }

        private void line43resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line43resultlabel, new int[] { 2, 3 });
        }

        private void line44resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line44resultlabel, new int[] { 3, 3 });
        }

        private void line45resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line45resultlabel, new int[] { 4, 3 });
        }

        private void line51resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line51resultlabel, new int[] { 0, 4 });
        }

        private void line52resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line52resultlabel, new int[] { 1, 4 });
        }

        private void line53resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line53resultlabel, new int[] { 2, 4 });
        }

        private void line54resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line54resultlabel, new int[] { 3, 4 });
        }

        private void line55resultlabel_Click(object sender, EventArgs e)
        {
            LabelClick(line55resultlabel, new int[] { 4, 4 });
        }
    }
}
