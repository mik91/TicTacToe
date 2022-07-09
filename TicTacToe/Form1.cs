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
        static int size = 3;
        short movement = 0;
        int[,] array = new int[size, size];

        struct MoveStruct
        {
            public int row, col;
        };

        public TicTacToe()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            movement++;
            Button bt = sender as Button;
            bt.Enabled = false;
            bt.BackColor = Color.Beige;
            TableLayoutPanelCellPosition pos = tblPanel.GetPositionFromControl(bt);

            bt.Text = "O";
            array[pos.Column, pos.Row] = 2;

            if (movement < size * size)
            {
                // AI turn
                TableLayoutPanelCellPosition pos2 = PlayAI();
                CheckGameState(pos, pos2);
            }
            else
            {
                MessageBox.Show("Draw!");
                tblPanel.Enabled = false;
            }

        }

        private TableLayoutPanelCellPosition PlayAI()
        {
            MoveStruct moveStruct = findBestMove(array);
            movement++;
            array[moveStruct.col, moveStruct.row] = 1;
            Button btnX = tblPanel.GetControlFromPosition(moveStruct.col, moveStruct.row) as Button;
            btnX.Text = "X";
            btnX.Enabled = false;
            btnX.BackColor = Color.AliceBlue;
            TableLayoutPanelCellPosition pos = tblPanel.GetPositionFromControl(btnX);
            return pos;
        }

        private void CheckGameState(TableLayoutPanelCellPosition pos, TableLayoutPanelCellPosition pos2)
        {
            if (GameFinished(pos2, 1))
            {
                MessageBox.Show("Game is finished. " + "X" + " is the winner!");
                tblPanel.Enabled = false;
                //Close();
            }
            else if (GameFinished(pos, 2))
            {
                MessageBox.Show("Game is finished. " +  "O" + " is the winner!");
                tblPanel.Enabled = false;
                //Close();
            }
            if (movement == size * size)
            {
                MessageBox.Show("Draw!");
                tblPanel.Enabled = false;
                //Close();
            }

            //isXTurn = !isXTurn;
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
                if (array[pos.Column, i] != type) {
                    column = false;
                    break;
                }                
            }

            //check rows
            for (int i = 0; i < size; i++)
            {
                if (array[i, pos.Row] != type) {
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

        bool isMovesLeft(int[,] array)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (array[i, j] == 0)
                        return true;
            return false;
        }

        // This is the evaluation function as discussed
        // in the previous article ( http://goo.gl/sJgv68 )
        int evaluate(int[,] array)
        {
            // Checking for Rows for X or O victory.
            for (int row = 0; row < size; row++)
            {
                if (array[row,0] == array[row,1] &&
                    array[row,1] == array[row,2])
                {
                    if (array[row,0] == 1)
                        return +10;
                    else if (array[row,0] == 2)
                        return -10;
                }
            }

            // Checking for Columns for X or O victory.
            for (int col = 0; col < 3; col++)
            {
                if (array[0,col] == array[1,col] &&
                    array[1,col] == array[2,col])
                {
                    if (array[0,col] == 1)
                        return +10;

                    else if (array[0,col] == 2)
                        return -10;
                }
            }

            // Checking for Diagonals for X or O victory.
            if (array[0,0] == array[1,1] && array[1,1] == array[2,2])
            {
                if (array[0,0] == 1)
                    return +10;
                else if (array[0,0] == 2)
                    return -10;
            }

            if (array[0,2] == array[1,1] && array[1,1] == array[2,0])
            {
                if (array[0,2] == 1)
                    return +10;
                else if (array[0,2] == 2)
                    return -10;
            }

            // Else if none of them have won then return 0
            return 0;
        }

        // This is the minimax function. It considers all
        // the possible ways the game can go and returns
        // the value of the array
        int minimax(int[,] array, int depth, bool isMax)
        {
            int score = evaluate(array);

            // If Maximizer has won the game return his/her
            // evaluated score
            if (score == 10)
                return score;

            // If Minimizer has won the game return his/her
            // evaluated score
            if (score == -10)
                return score;

            // If there are no more moves and no winner then
            // it is a tie
            if (isMovesLeft(array) == false)
                return 0;

            // If this maximizer's move
            if (isMax)
            {
                int best = -1000;

                // Traverse all cells
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // Check if cell is empty
                        if (array[j,i] == 0)
                        {
                            // Make the move
                            array[j,i] = 1;

                            // Call minimax recursively and choose
                            // the maximum value
                            best = Math.Max(best,
                                minimax(array, depth + 1, !isMax));

                            // Undo the move
                            array[j,i] = 0;
                        }
                    }
                }
                return best;
            }

            // If this minimizer's move
            else
            {
                int best = 1000;

                // Traverse all cells
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // Check if cell is empty
                        if (array[j,i] == 0)
                        {
                            // Make the move
                            array[j,i] = 2;

                            // Call minimax recursively and choose
                            // the minimum value
                            best = Math.Min(best,
                                   minimax(array, depth + 1, !isMax));

                            // Undo the move
                            array[j,i] = 0;
                        }
                    }
                }
                return best;
            }
        }

        // This will return the best possible move for the player
        MoveStruct findBestMove(int[,] array)
        {
            int bestVal = -1000;
            MoveStruct bestMove;
            bestMove.row = -1;
            bestMove.col = -1;

            // Traverse all cells, evaluate minimax function for
            // all empty cells. And return the cell with optimal
            // value.
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // Check if cell is empty
                    if (array[j,i] == 0)
                    {
                        // Make the move
                        array[j,i] = 1;

                        // compute evaluation function for this
                        // move.
                        int moveVal = minimax(array, 0, false);

                        // Undo the move
                        array[j,i] = 0;

                        // If the value of the current move is
                        // more than the best value, then update
                        // best/
                        if (moveVal > bestVal)
                        {
                            bestMove.row = i;
                            bestMove.col = j;
                            bestVal = moveVal;
                        }
                    }
                }
            }



            return bestMove;
        }

    }
}
