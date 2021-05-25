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
            Button bt = sender as Button;
            bt.Enabled = false;
            bt.BackColor = Color.Olive;
            TableLayoutPanelCellPosition pos = tblPanel.GetPositionFromControl(bt);
            if (isXTurn) {
                bt.Text = "X";
                array[pos.Row, pos.Column] = 1;
            }
            else {
                bt.Text = "O";
                array[pos.Row, pos.Column] = 2;
            }

            bool gamefinished = GameFinished(pos, isXTurn == true?1:2);
            isXTurn = !isXTurn;
        }

        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void InfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private Boolean GameFinished(TableLayoutPanelCellPosition pos, int type)
        {
            bool column = false;
            bool row = false;
            bool diagonals = false; 

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (array[j,i] != type)
                        column = false;
                    if (array[i, j] != type)
                        row = false;
                    if (i == j && array[i, j] == type)
                        diagonals = false;
                }
            }

            return diagonals || column || row;
            //for (int i = 0; i < size; i++)
            //{
            //    for (int j = 0; j < size; j++)
            //    {
            //        if (array[i, j] != type)
            //            break;
            //    }
            //}
        }
    }
}
