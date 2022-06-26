using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TicTacToe : Form
    {
        bool isXTurn = false;
        static int size = 3;
        short movement = 0;
        int[,] array = new int[size, size];

        public TicTacToe()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            movement++;
            Button bt = sender as Button;
            bt.Enabled = false;
            bt.BackColor = Color.Gold;
            TableLayoutPanelCellPosition pos = tblPanel.GetPositionFromControl(bt);
            if (isXTurn)
            {
                bt.Text = "X";
                array[pos.Row, pos.Column] = 1;
            }
            else
            {
                bt.Text = "O";
                array[pos.Row, pos.Column] = 2;
            }

            CheckGameState(pos);
        }

        private void CheckGameState(TableLayoutPanelCellPosition pos)
        {
            if (GameFinished(pos, 1))
            {
                MessageBox.Show("Game is finished. " + "X" + " is the winner!");
                Close();
            }
            else if (GameFinished(pos, 2))
            {
                MessageBox.Show("Game is finished. " +  "O" + " is the winner!");
                Close();
            }
            else if (movement == 9)
            {
                MessageBox.Show("Draw!");
                Close();
            }

            isXTurn = !isXTurn;
        }

        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void InfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tic Tac toe");
        }

        private bool GameFinished(TableLayoutPanelCellPosition pos, int type)
        {
            bool column = true;
            bool row = true;
            bool diagonals = true;
            bool bDiagonals = true;

            //check cols
            for (int i = 0; i < size; i++)
            {
                if (array[pos.Row, i] != type) {
                    column = false;
                    break;
                }                
            }

            //check rows
            for (int i = 0; i < size; i++)
            {
                if (array[i, pos.Column] != type) {
                    row = false;
                    break;
                }
            }

            //check diag
            if (pos.Column == pos.Row)
            {
                //we're on a diagonal
                for (int i = 0; i < size; i++)
                {
                    if (array[i, i] != type)
                    {
                        diagonals = false;
                        break;
                    }
                }
            }
            else
            {
                diagonals = false;
            }

            //check anti diag
            if (pos.Column + pos.Row == size - 1)
            {
                for (int i = 0; i < size; i++)
                {
                    if (array[i, (size - 1) - i] != type)
                    {
                        bDiagonals = false;
                        break;
                    }
                }
            }
            else
            {
                bDiagonals = false;
            }

            return diagonals || column || row || bDiagonals;
        }
    }
}
