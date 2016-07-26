using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static ChessPro.GUI.Properties.Resources;
using ChessPro.GUI.EngineOptionControls;
using ChessPro.Resources;
using System.Linq;
using System.Drawing.Drawing2D;

namespace ChessPro.GUI
{
    public partial class MainWindow : Form, IMainView
    {
        private List<ControlPiecePair> pieceControls;
        private ControlPiecePair selectedPiecePair;
        private IMainViewPresenter presenter;
        private int lastOptionBottomLocation;
        private bool reverseBoard;
        private Color blackFieldColor;
        private Color whiteFieldColor;

        private struct BoardIndexes
        {
            public char CharIndex;
            public int NumberIndex;

            public BoardIndexes(char charIndex, int numberIndex)
            {
                this.CharIndex = charIndex;
                this.NumberIndex = numberIndex;
            }
        }

        public MainWindow(IMainViewPresenter presenter)
        {
            InitializeComponent();

            whiteFieldColor = Color.White;
            blackFieldColor = Color.DimGray;
            reverseBoard = false;

            this.searchTimeTextBox.Text = "500";
            this.nodesTextBox.Text = "50";
            this.depthTextBox.Text = "20";


            this.presenter = presenter;
            this.blackFieldColorPanelPreview.BackColor = blackFieldColor;
            this.whiteFieldColorPanelPreview.BackColor = whiteFieldColor;
            this.whiteFieldColorPanelPreview.BorderStyle = 
            this.blackFieldColorPanelPreview.BorderStyle = BorderStyle.FixedSingle;

            this.boardPictureBox.Image = CreateBoardImage(boardPictureBox.Width);
            this.pieceControls = new List<ControlPiecePair>();
            this.boardPictureBox.Click += TryMovePiece;
            SetDoubleBuffering(boardPictureBox);
        }

        private void TryMovePiece(object sender, EventArgs e)
        {
            if (selectedPiecePair != null)
            {
                BoardIndexes indexes = ConvertPointToBoardIndexes(boardPictureBox.PointToClient(Cursor.Position));
                bool userCanMove = presenter.UserTryMove(selectedPiecePair.Piece, indexes.CharIndex, indexes.NumberIndex);

                if (!userCanMove)
                {
                    //reset to source field - move is incorrect
                    Point startLocation = ConvertIndexesToPoint(
                   selectedPiecePair.Piece.NumberIndex,
                   selectedPiecePair.Piece.CharIndex);

                   selectedPiecePair.Control.Location = startLocation;
                }

                selectedPiecePair = null;
            }
        }

        private Image CreateBoardImage(int size)
        {
            Image map = new Bitmap(size, size);

            using (Graphics painter = Graphics.FromImage(map))
            {
                //creating black/white square fiels

                DrawFieldsSquaresOnBoardImage(painter, size);

                //creating coordinates on fields

                DrawFieldsIndexesOnBoardImage(painter, size);
            }

            return map;

        }

        private void DrawFieldsSquaresOnBoardImage(Graphics imageGraphic,int imageSize)
        {
            int squareSize = imageSize / 8;
            Color firstColor = reverseBoard ? whiteFieldColor : blackFieldColor;
            Color secondColor = reverseBoard ? blackFieldColor : whiteFieldColor;

            Rectangle field = new Rectangle(0, 0, squareSize, squareSize);

            LinearGradientBrush firstFieldBrush =  CreateFieldLinearBrush(firstColor,field);
            LinearGradientBrush secondFieldBrush = CreateFieldLinearBrush(secondColor,field);
            LinearGradientBrush brush = firstFieldBrush;
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    field.X = squareSize * j;
                    field.Y = squareSize * i;
                   
                    if (i % 2 == 0)
                    {
                        brush = j % 2 == 0 ? 
                            CreateFieldLinearBrush(secondColor, field) : 
                            CreateFieldLinearBrush(firstColor, field);
                    }
                    else
                    {
                        brush = j % 2 == 0 ? 
                            CreateFieldLinearBrush(firstColor, field) : 
                            CreateFieldLinearBrush(secondColor, field);
                    }
                    
                    imageGraphic.FillRectangle(brush, field);
                }
            }
        }

        private LinearGradientBrush CreateFieldLinearBrush(Color color,Rectangle rectangle)
        {
            int r = color.R - 40 > 0 ? color.R - 40 : 0;
            int g = color.G - 40 > 0 ? color.G - 40 : 0;
            int b = color.B - 40 > 0 ? color.B - 40 : 0;

            LinearGradientBrush brush = new LinearGradientBrush(
                new Point(rectangle.X,rectangle.Y),
                new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height),
                Color.FromArgb(255,r,g,b),
                color);

            return brush;
        }

        private void DrawFieldsIndexesOnBoardImage(Graphics imageGraphic, int imageSize)
        {
            int squareSize = imageSize/8;

            Font coordinatesFont = new Font("Verdana", 10.5F,FontStyle.Bold);
            Brush fontBrush = new SolidBrush(Color.Red);

            Brush whiteFieldFontBrush = new SolidBrush(Color.FromArgb(255, blackFieldColor));
            Brush blackFieldFontBrush = new SolidBrush(Color.FromArgb(255, whiteFieldColor));

            PointF coordinatePosition = new PointF(0, 0);
            int numberIndex = 0;
            char charIndex = 'a';

            for (int y = 0; y < 8; y++)
            {
                coordinatePosition.Y = squareSize * y;
                numberIndex = reverseBoard ? y + 1 : 8-y;

                if (y % 2 == 0)
                {
                    fontBrush = reverseBoard ? blackFieldFontBrush : whiteFieldFontBrush;
                }
                else
                {
                    fontBrush = reverseBoard ? whiteFieldFontBrush : blackFieldFontBrush;
                }

                imageGraphic.DrawString(numberIndex.ToString(), coordinatesFont, fontBrush, coordinatePosition);
            }
            coordinatePosition.Y = (8 * squareSize) - coordinatesFont.Size * 2;
            for (int x = 0; x < 8; x++)
            {
                coordinatePosition.X = squareSize * x + 3;
                charIndex = reverseBoard ? (char)('h' - x - 32) : (char)('a' + x - 32);

                if (x % 2 == 0)
                {
                    fontBrush = reverseBoard ? whiteFieldFontBrush : blackFieldFontBrush;
                }
                else
                {
                    fontBrush = reverseBoard ? blackFieldFontBrush : whiteFieldFontBrush;
                }

                
                imageGraphic.DrawString(charIndex.ToString(), coordinatesFont, fontBrush, coordinatePosition);
            }
        }

        private void SelectPieceControl(ControlPiecePair controlPiecePair, object sender, EventArgs e)
        {
            //user want to do normal move - not take any oponent piece, just move piece to empty filed.
            if (selectedPiecePair == null)
            {
                selectedPiecePair = controlPiecePair;
                //controlPiecePair.Control.BackColor = Color.FromArgb(40, Color.Green);
            }
            //some piece is alredy selected - user select second one -  probably he want to take some piece from board.
            else
            {
                TryMovePiece(sender, e);
            }
        }

        private Image GetPieceImage(PieceComponent piece)
        {
            if (piece.PieceColor == PieceComponent.Color.Black)
            {
                switch (piece.PieceType)
                {
                    case PieceComponent.Type.Rook:
                        return black_rook;
                    case PieceComponent.Type.Knight:
                        return black_knight;
                    case PieceComponent.Type.Bishop:
                        return black_bishop;
                    case PieceComponent.Type.Queen:
                        return black_queen;
                    case PieceComponent.Type.King:
                        return black_king;
                    case PieceComponent.Type.Pawn:
                        return black_pawn;
                    default:
                        throw new Exception("cannot find resorces");
                        
                }
            }
            else
            {
                switch (piece.PieceType)
                {
                    case PieceComponent.Type.Rook:
                        return white_rook;
                    case PieceComponent.Type.Knight:
                        return white_knight;
                    case PieceComponent.Type.Bishop:
                        return white_bishop;
                    case PieceComponent.Type.Queen:
                        return white_queen;
                    case PieceComponent.Type.King:
                        return white_king;
                    case PieceComponent.Type.Pawn:
                        return white_pawn;
                    default:
                        throw new Exception("cannot find resource");
                }
            }
        }

        private int GetActualSquareSize()
        {
            return boardPictureBox.Width / 8;
        }

        private Point ConvertIndexesToPoint(int numberIndex, char charIndex)
        {
            int squareSize = GetActualSquareSize();
            int x = charIndex - 'a';
            int y = 8 - numberIndex;

            y = reverseBoard ? 7 - y : y;
            x = reverseBoard ? 7 - x : x;

            Point location = new Point(x * squareSize, y * squareSize);
            return location;
        }

        private BoardIndexes ConvertPointToBoardIndexes(Point positionOnBoardPictureBox)
        {
            int squareSize = GetActualSquareSize();
            int numberIndex = 8 - (positionOnBoardPictureBox.Y / squareSize);
            char charIndex = (char)((positionOnBoardPictureBox.X / squareSize) + 'a');

            numberIndex = reverseBoard ? 9 - numberIndex : numberIndex;
            charIndex = reverseBoard ? (char)(('h' - charIndex) + 'a') : charIndex;

            BoardIndexes indexesValues = new BoardIndexes(charIndex,numberIndex);

            return indexesValues;
        }

        private ControlPiecePair FindPieceControlPair(PieceComponent piece)
        {
            foreach (var pair in pieceControls)
            {
                if (pair.Piece.Equals(piece))
                    return pair;
            }
            return null;
        }

        private void ChangeControlPiecePairPosition(ControlPiecePair selectedPiecePair, char newCharIndex, int newNumberIndex)
        {
            Point newLocation = ConvertIndexesToPoint(newNumberIndex, newCharIndex);
            selectedPiecePair.Control.Location = newLocation;
            selectedPiecePair.Piece.SetLocation(newCharIndex, newNumberIndex);
        }

        private PictureBox CreatePiecePictureBox(PieceComponent piece)
        {
            PictureBox control = new PictureBox();
            Image img = GetPieceImage(piece);
            int squareSize = GetActualSquareSize();
            control.Image = GetPieceImage(piece);
            control.Width = squareSize;
            control.Height = squareSize;
            control.BackColor = Color.Transparent;
            control.Location = ConvertIndexesToPoint(piece.NumberIndex, piece.CharIndex);

            return control;
                 
        }

        private void PieceControlDropped(ControlPiecePair controlPiecePair, Point dropLocationOnParent)
        {
            selectedPiecePair = controlPiecePair;
            BoardIndexes dropLocation = ConvertPointToBoardIndexes(dropLocationOnParent);
            TryMovePiece(null, null);
        }

        private void SetDoubleBuffering(Control control)
        {
            System.Reflection.PropertyInfo aProp =
         typeof(System.Windows.Forms.Control).GetProperty(
               "DoubleBuffered",
               System.Reflection.BindingFlags.NonPublic |
               System.Reflection.BindingFlags.Instance);

            aProp.SetValue(control, true, null);
            System.Reflection.MethodInfo aProp2 = typeof(System.Windows.Forms.Control).GetMethod(
              "SetStyle",
              System.Reflection.BindingFlags.NonPublic |
              System.Reflection.BindingFlags.Instance);
            aProp2.Invoke(control, new object[] { ControlStyles.OptimizedDoubleBuffer, true });
        }

        #region IMainView

        public void AddPiece(PieceComponent piece)
        {
            PictureBox control = CreatePiecePictureBox(piece);
            ControlPiecePair pair = new ControlPiecePair(control, piece);
            DragableControlPiecePair dragablePair = new DragableControlPiecePair(pair, boardPictureBox);

            pair.SelectedPieceControl += SelectPieceControl;
            dragablePair.ControlPiecePairDropped += PieceControlDropped;

            pieceControls.Add(pair);
            boardPictureBox.Controls.Add(control);
            SetDoubleBuffering(control);
        }

        public void RemoveAllPieces()
        {
            ControlPiecePair[] existingControls = pieceControls.ToArray();
            foreach (var item in existingControls)
            {
                boardPictureBox.Controls.Remove(item.Control);
                pieceControls.Remove(item);
            }
        }

        public void RemovePiece(char charIndex, int numberIndex)
        {
            foreach (ControlPiecePair controlPair in pieceControls)
            {
                if (controlPair.Piece.CharIndex == charIndex &&
                    controlPair.Piece.NumberIndex == numberIndex)
                {
                    boardPictureBox.Controls.Remove(controlPair.Control);
                    pieceControls.Remove(controlPair);
                    break;
                }
            }
        }

        public void MovePieceComponent(PieceComponent piece, char newCharIndex,int newNumberIndex)
        {
            if (selectedPiecePair != null && selectedPiecePair.Piece.Equals(piece))
            {
                //here user wants to move - its simple repeat from presenter to do user move     
                ChangeControlPiecePairPosition(selectedPiecePair, newCharIndex, newNumberIndex);
            }
            else
            {
                //searching pair 
                ControlPiecePair pair = FindPieceControlPair(piece);
                if (pair != null)
                {
                    ChangeControlPiecePairPosition(pair, newCharIndex, newNumberIndex);
                }
                else throw new Exception("Cannot change position becouse PieceComponent is not exists!");
            }
        }

        public void MovePieceComponentAsync(PieceComponent piece, char newCharIndex, int newNumberIndex)
        {
            this.Invoke((MethodInvoker)delegate 
            { this.MovePieceComponent(piece,newCharIndex,newNumberIndex); });
        }

        public void RemovePieceAsync(char charIndex, int numberIndex)
        {
            this.Invoke((MethodInvoker)delegate { this.RemovePiece(charIndex, numberIndex); });
        }

        public void AddPieceAsync(PieceComponent piece)
        {
            this.Invoke((MethodInvoker)delegate () { this.AddPiece(piece); });
        }

        public void ShowGameInformation(string information)
        {
            this.infoLabel.Text = information;
        }

        public void ShowAvailableEngines(string[] enginesNames)
        {
            this.selectEngineComboBox.Items.AddRange(enginesNames);
        }

        public void SetEngineOptions(UCIOption[] options)
        {
            TabPage optionsPage = this.gamePropertiesTabControl.TabPages["engineOptionsTabPage"];
            optionsPage.Controls.Clear();
            lastOptionBottomLocation = 0;

            UCIOption[] sortedOptions = SortOptionsCollection(options);
 
            foreach (var option in sortedOptions)
            {
                AddEngineOption(option);
            }
        }

        public void SetEngineLoadStatus(string status)
        {
            this.engineStatusLabel.Text = string.Format("Status: {0}", status);
        }

        public void SetEngineID(string[] idLines)
        {
            this.engineIdRichTextBox.Text = String.Join("\n", idLines);
        }

        public PieceComponent.Color GetPlayerColor()
        {
            if (blackRadioButton.Checked)
                return PieceComponent.Color.Black;
            else return PieceComponent.Color.White;
        }

        public SearchProperties GetSearchProperties()
        {
            return new SearchProperties()
            {
                Nodes = int.Parse(nodesTextBox.Text),
                Depth = int.Parse(depthTextBox.Text),
                SearchTime = int.Parse(searchTimeTextBox.Text),
                SearchInfinity = searchInfinityCheckBox.Checked
            };
        }

        public void ShowInformationBox(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowErrorMessage(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }

        #endregion

        private UCIOption[] SortOptionsCollection(UCIOption[] options)
        {
            var buttonOptions = from o in options where o is UCIButtonOption select o;
            var textBoxOptions = from o in options where o is UCIStringOption select o;
            var spinOptions = from o in options where o is UCISpinOption select o;
            var checkOptions = from o in options where o is UCICheckOption select o;
            var comboOptions = from o in options where o is UCIComboOption select o;

            List<UCIOption> sorted = new List<UCIOption>();
            sorted.AddRange(spinOptions);
            sorted.AddRange(comboOptions);
            sorted.AddRange(textBoxOptions);
            sorted.AddRange(checkOptions);
            sorted.AddRange(buttonOptions);

            return sorted.ToArray();
        }

        private void AddEngineOption(UCIOption option)
        {
            UserControl engineOptionControl;

            switch (option.OptionType)
            {
                case UCIOption.Type.Check:
                    engineOptionControl = new EngineCheckOption(option as UCICheckOption);
                    break;
                case UCIOption.Type.Spin:
                    engineOptionControl = new EngineSpinOption(option as UCISpinOption);
                    break;
                case UCIOption.Type.Combo:
                    engineOptionControl = new EngineComboOption(option as UCIComboOption);
                    break;
                case UCIOption.Type.Button:
                    engineOptionControl = new EngineButtonOption(option as UCIButtonOption);
                    break;
                case UCIOption.Type.String:
                    engineOptionControl = new EngineStringOption(option as UCIStringOption);
                    break;
                default:
                    throw new Exception("Unknow OptionType value");
            }
            TabPage optionsPage = this.gamePropertiesTabControl.TabPages["engineOptionsTabPage"];

            engineOptionControl.Location = new Point(0, lastOptionBottomLocation);
            (engineOptionControl as IEngineOptionControl).SetWidth(optionsPage.Width - 20);

            //10 is margin from top of last added control
            lastOptionBottomLocation += engineOptionControl.Height;
            optionsPage.Controls.Add(engineOptionControl);


            (engineOptionControl as IEngineOptionControl).OptionChangedEvent += (o, v) => presenter.UpdateEngineOption(o, v);
        }

        private void NewGameButtonClick(object sender, EventArgs e)
        {
            presenter.NewGame();
        }

        private void LoadEngine(object sender, EventArgs e)
        {
            
            int selectedIndex = selectEngineComboBox.SelectedIndex;
            if (selectedIndex < 0)
            {
                ShowInformationBox("Please select engine!");
                return;
            }
            string engineName = selectEngineComboBox.Items[selectedIndex].ToString();

            presenter.LoadEngine(engineName);
        }

        private void ShowEngineOutput(object sender, EventArgs e)
        {
            presenter.ShowEngineCommunicationView();
        }

        private void ReverseBoardCheckBoxChanged(object sender, EventArgs e)
        {
            reverseBoard = reverseBoardCheckBox.Checked;
            RefreshChessBoard();
        }

        private void RefreshChessBoard()
        {
            this.boardPictureBox.Image = CreateBoardImage(boardPictureBox.Width);
            //simple move pieces on same position - ConvertIndexToPoint do work with reversing
            foreach (ControlPiecePair pair in pieceControls)
            {
                ChangeControlPiecePairPosition(pair, pair.Piece.CharIndex, pair.Piece.NumberIndex);
            }
        }

        private void BlackFieldColorPanelClicked(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            DialogResult result= colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                blackFieldColor = colorDialog.Color;
                blackFieldColorPanelPreview.BackColor = blackFieldColor;
            }
            RefreshChessBoard();
        }

        private void WhiteFieldColorPanelClicked(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                whiteFieldColor = colorDialog.Color;
                whiteFieldColorPanelPreview.BackColor = whiteFieldColor;
            }
            RefreshChessBoard();
        }

        private void GetHint(object sender, EventArgs e)
        {
            presenter.GetHint();
        }

        private void SaveSearchProperties(object sender, EventArgs e)
        {
            presenter.SetSearchProperties(BuildSearchProperties());
        }

        private SearchProperties BuildSearchProperties()
        {
            SearchProperties properties = new SearchProperties()
            {
                Depth = int.Parse(depthTextBox.Text),
                Nodes = int.Parse(depthTextBox.Text),
                SearchInfinity = searchInfinityCheckBox.Checked,
                SearchTime = int.Parse(searchTimeTextBox.Text)
            };
            return properties;
        }

        private void SearchPropertiesTextBoxTextChanged(object sender, EventArgs e)
        {
            TextBox senderTextBox = sender as TextBox;
            int parsedValue;
            bool parsingResult = int.TryParse(senderTextBox.Text, out parsedValue);
            if (!parsingResult)
            {
                senderTextBox.Text = "1";
            }
        }

        private void MoveNowButtonClick(object sender, EventArgs e)
        {
            presenter.MoveNow();
        }
    }
}
