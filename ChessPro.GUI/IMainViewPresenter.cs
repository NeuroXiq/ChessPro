using ChessPro.Resources;

namespace ChessPro.GUI
{
    public interface IMainViewPresenter
    {
        void NewGame();
        bool UserTryMove(PieceComponent piece, char newCharIndex, int newNumberIndex);
        void LoadEngine(string engineName);
        void GetHint();
        void ShowEngineCommunicationView();
        void UpdateEngineOption(UCIOption option, string newValue);
        void SetSearchProperties(SearchProperties properties);
        void MoveNow();
    }
}
