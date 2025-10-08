# Chess-with-AI

## Description:
A chess game built in C# with .NET Framework (Windows Forms App). Supports Player vs. Player (PvP) and Player vs. AI (PvAI) game modes, with adjustable AI difficulty levels.

### Features:
* AI opponent powered by the Minimax algorithm with recursive game-tree evaluation.
* Difficulty levels determined by tree depth.
* Material-based scoring system evaluates moves; in tie situations, uses number of squares captured.
* Pruning implemented to reduce computation and improve performance.

### Tech Stack:
* C# (.NET Framework, Windows Forms)

### Getting Started:
1. Run from source:
* Open ChessGame/Chess.sln in Visual Studio.
* Build and run the solution.
2. Run executable directly:
* Go to ChessGame/Chess/obj/Debug/Chess.exe and launch the game.

### Optional:
Explore the AI logic in the folder: ChessGame/Chess/AIMove to see the Minimax implementation and modular OOP design.

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
