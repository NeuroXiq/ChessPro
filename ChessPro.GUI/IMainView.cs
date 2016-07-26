using ChessPro.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.GUI
{
    public interface IMainView
    {
        void AddPiece(PieceComponent piece);
        void RemovePiece(char charIndex, int numberIndex);
        void RemoveAllPieces();
        void MovePieceComponent(PieceComponent piece, char newCharIndex, int newNumberIndex);

        void MovePieceComponentAsync(PieceComponent piece, char newCharIndex, int newNumberIndex);
        void RemovePieceAsync(char charIndex, int numberIndex);
        void AddPieceAsync(PieceComponent piece);

        void ShowGameInformation(string information);
        void ShowAvailableEngines(string[] enginesNames);
        void ShowInformationBox(string message);
        void SetEngineOptions(UCIOption[] option);
        void SetEngineLoadStatus(string status);
        void SetEngineID(string[] idLines);
        void ShowErrorMessage(string title, string message);


        PieceComponent.Color GetPlayerColor();
        SearchProperties GetSearchProperties();
    }
}
