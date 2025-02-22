# Chess-with-AI

The chess game is built in C# with the .NET framework (Windows Forms App) which includes the game modes of player versus player (PvP) and player versus AI (PvAI) at varying levels of difficulty.

The AI uses the Minimax Algorithm which is a recursive algorithm that looks at all the possible moves that can be made which allows the AI to choose the next move that minimizes the lost, this is achieved with the idea of backtracking through the tree where each node will compare it's child nodes in order to find the child node that provides the best outcome (this is determined by a material system where each piece is worth a certain amount of points), this process will continue until it works it's way back up to the root node of the tree which will represent the next mode that will be made (which happens to be the optimal move).

Furthermore, the difficulty of the AI is determined by how deep the tree goes and incase of scenarios where the child nodes that are been compared to are equal in material worth, it will use another metric called the amount of squares captured to determine the child that provides the best outcome. The algorithm also involves the prunning of certain nodes that are not worth looking into to reduce the processing time.

To access the code, go to the path: ChessGame (folder) > Chess.sln.
To access the executable file and play the chess game directly, go to the path: ChessGame (folder) > Chess(folder) > obj (folder) > Debug (folder) > Chess (.exe)

# Reference

## Chess Images
The chess pieces and chess board images are used in this project came from another source.

### Original Source
* Source URL: https://en.wikipedia.org/wiki/Rules_of_chess
* Author: Wikipedia

### Other Source
* Source URL: https://github.com/melvic-ybanez/my_chessmate
* Author: Melvic Ybanez

## Chess Icon
* Description: The chess icon is used in this project came from another source.
* Source URL: https://www.iconarchive.com/show/papirus-apps-icons-by-papirus-team/chess-icon.html
* Author: Papirus Dev Team
* Image Project Path: ChessGame/Chess/Resources/Papirus-Team-Papirus-Apps-Chess.ico
