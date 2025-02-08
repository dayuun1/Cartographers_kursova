using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KR_Cartographers.Services;
using KR_Cartographers.Models;
using System.Windows.Controls;
using System.Data.Common;
using System.Net;
using System.Globalization;
using System.Windows.Data;
using System.Reflection;
using System.Xml.Linq;
using System.Windows;
using System.Diagnostics.Eventing.Reader;
using KR_Cartographers.Views;

namespace KR_Cartographers.ViewModels
{
    class OnePlayerGameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IWindowService _windowService;

        public RelayCommand MoveRightCommand { get; set; }
        public RelayCommand MoveLeftCommand { get; set; }
        public RelayCommand MoveDownCommand { get; set; }
        public RelayCommand MoveUpCommand { get; set; }
        public RelayCommand PlaceBlockCommand { get; set; }
        public RelayCommand RotateCWCommand { get; set; }
        public RelayCommand RotateCCWCommand { get; set; }
        public RelayCommand ChengeBlockOrTerrainCommand { get; set; }
        public RelayCommand ChangeMenuViewCommand { get; set; }
        public RelayCommand ShowSettingsCommand { get; set; }
        public RelayCommand QuitCommand { get; set; }
        public List<List<CellViewModel>> AllCells { get; } = new List<List<CellViewModel>>();
        public List<List<CellViewModel>> AllCellsSecondPlayer { get; } = new List<List<CellViewModel>>();
        public CellViewModel _cell;


        private int rowCount = 11;
        private int columnCount = 11;
        private int CurrentCard = 0;
        private int CurrentBlock = 0;
        private int CurrentTerrain = 0;
        private Seasons currentSeason = Seasons.Весна;
        private byte CurrentDay = 0;
        private byte Days = 8;
        private int PrevCard;
        private bool WasFit = false;
        private bool RuinForDotBlock = false;
        private bool _isPlacingBlockAllowed = true;
        private int CurrentCardSecondPlayer = 0;
        private int CurrentBlockSecondPlayer = 0;
        private int CurrentTerrainSecondPlayer = 0;
        private int PrevCardSecondPlayer;
        private bool WasFitSecondPlayer = false;
        private bool RuinForDotBlockSecondPlayer = false;
        private GameGrid grid;
        private List<List<CellViewModel>> cell;
        private int currentCard;
        private int currentBlock;
        private int prevCard;
        private bool wasFit;
        private int currentTerrain;
        private int currentPlayerTurn = 0;
        private bool isPlaced = false;
        private bool isPlacedSecondPlayer = false;
        public bool[] MenuSettingVisibility { get; set; }
        public bool MenuVisibility { get; set; }
        public OnePlayerGameViewModel()
        {
            _windowService = new WindowService();
            StartGame();

            {
                MoveRightCommand = new RelayCommand(obj =>
                {
                    int player = int.Parse((string)obj);
                    if (player != currentPlayerTurn)
                    {
                        return;
                    }
                    if (int.Parse((string)obj) == 0)
                    {
                        InitializeForFirstPlayer();
                    }
                    else
                    {
                        InitializeForSecondPlayer();
                    }
                    if (_isPlacingBlockAllowed)
                    {
                        RemoveBlock(cell, Game.CardsList[currentCard], currentBlock);
                        Game.MoveRight(grid, Game.CardsList[currentCard].Block[currentBlock]);
                        OnPropertyChanged(nameof(MoveRightCommand));
                        UpdateField(cell, ref CurrentCard, currentBlock, prevCard, wasFit, grid);
                        DrawBlock(cell, Game.CardsList[currentCard], currentBlock, currentTerrain);
                    }
                });

                MoveLeftCommand = new RelayCommand(obj =>
            {
                int player = int.Parse((string)obj);
                if (player != currentPlayerTurn)
                {
                    return;
                }
                if (int.Parse((string)obj) == 0)
                {
                    InitializeForFirstPlayer();
                }
                else
                {
                    InitializeForSecondPlayer();
                }
                if (_isPlacingBlockAllowed)
                {
                    RemoveBlock(cell, Game.CardsList[currentCard], currentBlock);
                    Game.MoveLeft(grid, Game.CardsList[currentCard].Block[currentBlock]);
                    OnPropertyChanged(nameof(MoveLeftCommand));
                    UpdateField(cell, ref CurrentCard, currentBlock, prevCard, wasFit, grid);
                    DrawBlock(cell, Game.CardsList[currentCard], currentBlock, currentTerrain);
                }
            });

                MoveDownCommand = new RelayCommand(obj =>
                {
                    int player = int.Parse((string)obj);
                    if (player != currentPlayerTurn)
                    {
                        return;
                    }
                    if (int.Parse((string)obj) == 0)
                    {
                        InitializeForFirstPlayer();
                    }
                    else
                    {
                        InitializeForSecondPlayer();
                    }
                    if (_isPlacingBlockAllowed)
                    {
                        RemoveBlock(cell, Game.CardsList[currentCard], currentBlock);
                        Game.MoveDown(grid, Game.CardsList[currentCard].Block[currentBlock]);
                        OnPropertyChanged(nameof(MoveDownCommand));
                        UpdateField(cell, ref CurrentCard, currentBlock, prevCard, wasFit, grid);
                        DrawBlock(cell, Game.CardsList[currentCard], currentBlock, currentTerrain);
                    }
                });

                MoveUpCommand = new RelayCommand(obj =>
                {
                    int player = int.Parse((string)obj);
                    if (player != currentPlayerTurn)
                    {
                        return;
                    }
                    if (int.Parse((string)obj) == 0)
                    {
                        InitializeForFirstPlayer();
                    }
                    else
                    {
                        InitializeForSecondPlayer();
                    }
                    if (_isPlacingBlockAllowed)
                    {
                        RemoveBlock(cell, Game.CardsList[currentCard], currentBlock);
                        Game.MoveUp(grid, Game.CardsList[currentCard].Block[currentBlock]);
                        OnPropertyChanged(nameof(MoveUpCommand));
                        UpdateField(cell, ref CurrentCard, currentBlock, prevCard, wasFit, grid);
                        DrawBlock(cell, Game.CardsList[currentCard], currentBlock, currentTerrain);
                    }
                });

                RotateCWCommand = new RelayCommand(obj =>
                {
                    int player = int.Parse((string)obj);
                    if (player != currentPlayerTurn)
                    {
                        return;
                    }
                    if (int.Parse((string)obj) == 0)
                    {
                        InitializeForFirstPlayer();
                    }
                    else
                    {
                        InitializeForSecondPlayer();
                    }
                    if (_isPlacingBlockAllowed)
                    {
                        RemoveBlock(cell, Game.CardsList[currentCard], currentBlock);
                        Game.RotateBlockCW(grid, Game.CardsList[currentCard].Block[currentBlock]);
                        OnPropertyChanged(nameof(RotateCWCommand));
                        UpdateField(cell, ref CurrentCard, currentBlock, prevCard, wasFit, grid);
                        DrawBlock(cell, Game.CardsList[currentCard], currentBlock, currentTerrain);
                    }
                });

                RotateCCWCommand = new RelayCommand(obj =>
                {
                    int player = int.Parse((string)obj);
                    if (player != currentPlayerTurn)
                    {
                        return;
                    }
                    if (int.Parse((string)obj) == 0)
                    {
                        InitializeForFirstPlayer();
                    }
                    else
                    {
                        InitializeForSecondPlayer();
                    }
                    if (_isPlacingBlockAllowed)
                    {
                        RemoveBlock(cell, Game.CardsList[currentCard], currentBlock);
                        Game.RotateBlockCCW(grid, Game.CardsList[currentCard].Block[currentBlock]);
                        OnPropertyChanged(nameof(RotateCCWCommand));
                        UpdateField(cell, ref CurrentCard, currentBlock, prevCard, wasFit, grid);
                        DrawBlock(cell, Game.CardsList[currentCard], currentBlock, currentTerrain);
                    }
                });

                QuitCommand = new RelayCommand(obj =>
                {
                    _windowService.OpenWindow();
                });

                PlaceBlockCommand = new RelayCommand(async obj =>
                {
                    if (int.Parse((string)obj) == 2)
                    {
                        if (_isPlacingBlockAllowed && CanPlaceBlock(Game.CardsList[CurrentCard], CurrentBlock, Game.gameGrid, WasFit, ref RuinForDotBlock, CurrentCard))
                        {
                            _isPlacingBlockAllowed = false;
                            Game.PlaceBlock(grid, Game.CardsList[CurrentCard], CurrentBlock, CurrentTerrain);
                            if (Game.CardsList[CurrentCard].Block.Length > 1 && CurrentBlock == 0)
                            {
                                Game.CoinsByBlock++;
                            }
                            Game.CoinsByMountain = Game.CountCoinsByMountains(grid);
                            Coins = (Game.CoinsByBlock + Game.CoinsByMountain).ToString();
                            UpdatePoints();
                            if (WasFit)
                            {
                                foreach (Block block in Game.CardsList[CurrentCard].Block)
                                {
                                    block.Reset();
                                }
                                CurrentCard = PrevCard;
                                WasFit = false;
                            }
                            CurrentDay += Game.CardsList[CurrentCard].Days;
                            foreach (Block block in Game.CardsList[CurrentCard].Block)
                            {
                                block.Reset();
                            }
                            UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);

                            CurrentBlock = 0;
                            CurrentCard++;
                            CurrentTerrain = 0;

                            if (CurrentDay >= Days)
                            {
                                ChangeSeason();
                                CurrentCard = 0;
                                CurrentBlock = 0;
                                CurrentTerrain = 0;
                                CurrentDay = 0;
                                SeasonDays = CurrentDay.ToString();

                                while (Game.CardsList[CurrentCard].IsRuin || Game.CardsList[CurrentCard].IsMonster)
                                {
                                    while (Game.CardsList[CurrentCard].IsRuin)
                                    {
                                        ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";
                                        CardDescription = Game.CardsList[CurrentCard].Description;
                                        CurrentCard++;
                                        UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                        await Task.Delay(2000);
                                    }
                                    while (Game.CardsList[CurrentCard].IsMonster)
                                    {
                                        ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";
                                        CardDescription = Game.CardsList[CurrentCard].Description;
                                        await Task.Delay(2000);
                                        Game.AutoDemonsSpawn(Game.gameGrid, Game.CardsList[CurrentCard]);
                                        CurrentCard++;
                                        UpdatePoints();
                                        UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                    }
                                }
                                CardDescription = Game.CardsList[CurrentCard].Description;
                                ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";

                                if (CurrentCard > 0 && !Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock], Game.CardsList[CurrentCard - 1].IsRuin))
                                {
                                    if (Game.CardsList[CurrentCard].Block.Length > 1 && Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock + 1], Game.CardsList[CurrentCard - 1].IsRuin))
                                    {
                                        //Можна поставити тільки другий блок
                                    }
                                    else
                                    {

                                        if (Game.CardsList[CurrentCard - 1].IsRuin)
                                        {
                                            RuinForDotBlock = true;
                                        }
                                        PrevCard = CurrentCard;
                                        CurrentCard = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
                                        WasFit = true;
                                    }
                                }
                                else if (CurrentCard == 0 && !Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock], false))
                                {
                                    if (Game.CardsList[CurrentCard].Block.Length > 1 && Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock + 1], false))
                                    {
                                        //Можна поставити тільки другий блок
                                    }
                                    else
                                    {
                                        PrevCard = CurrentCard;
                                        CurrentCard = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
                                        WasFit = true;
                                    }
                                }
                                UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                DrawBlock(AllCells, Game.CardsList[CurrentCard], CurrentBlock, CurrentTerrain);
                            }
                            else
                            {
                                while (Game.CardsList[CurrentCard].IsRuin || Game.CardsList[CurrentCard].IsMonster)
                                {
                                    while (Game.CardsList[CurrentCard].IsRuin)
                                    {
                                        ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";
                                        CardDescription = Game.CardsList[CurrentCard].Description;
                                        CurrentCard++;
                                        UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                        await Task.Delay(2000);
                                    }
                                    while (Game.CardsList[CurrentCard].IsMonster)
                                    {
                                        ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";
                                        CardDescription = Game.CardsList[CurrentCard].Description;
                                        await Task.Delay(2000);
                                        Game.AutoDemonsSpawn(Game.gameGrid, Game.CardsList[CurrentCard]);
                                        CurrentCard++;
                                        UpdatePoints();
                                        UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                    }
                                }
                                CardDescription = Game.CardsList[CurrentCard].Description;
                                ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";

                                if (!Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock], Game.CardsList[CurrentCard - 1].IsRuin))
                                {
                                    if (Game.CardsList[CurrentCard].Block.Length > 1 && Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock + 1], Game.CardsList[CurrentCard - 1].IsRuin))
                                    {
                                        //Можна поставити тільки другий блок
                                    }
                                    else
                                    {
                                        if (Game.CardsList[CurrentCard - 1].IsRuin)
                                        {
                                            RuinForDotBlock = true;
                                        }
                                        PrevCard = CurrentCard;
                                        CurrentCard = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
                                        WasFit = true;
                                    }
                                }

                                UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                DrawBlock(AllCells, Game.CardsList[CurrentCard], CurrentBlock, CurrentTerrain);
                            }

                            _isPlacingBlockAllowed = true;
                            OnPropertyChanged(nameof(ImageUrl));
                        }
                    }
                    int player = int.Parse((string)obj);
                    if (player != currentPlayerTurn)
                    {
                        return;
                    }
                    if (int.Parse((string)obj) == 0 || int.Parse((string)obj) == 1)
                    {
                        if (int.Parse((string)obj) == 0)
                        {

                            if (_isPlacingBlockAllowed && CanPlaceBlock(Game.CardsList[CurrentCard], CurrentBlock, Game.gameGrid, WasFit, ref RuinForDotBlock, CurrentCard))
                            {
                                _isPlacingBlockAllowed = false;
                                Game.PlaceBlock(Game.gameGrid, Game.CardsList[CurrentCard], CurrentBlock, CurrentTerrain);
                                UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                if (Game.CardsList[CurrentCard].Block.Length > 1 && CurrentBlock == 0)
                                {
                                    Game.CoinsByBlock++;
                                }
                                Game.CoinsByMountain = Game.CountCoinsByMountains(grid);
                                Coins = (Game.CoinsByBlock + Game.CoinsByMountain).ToString();
                                UpdatePoints();
                                if (WasFit)
                                {
                                    foreach (Block block in Game.CardsList[CurrentCard].Block)
                                    {
                                        block.Reset();
                                    }
                                    CurrentCard = PrevCard;
                                    WasFit = false;
                                }
                                foreach (Block block in Game.CardsList[CurrentCard].Block)
                                {
                                    block.Reset();
                                }

                                CurrentBlock = 0;
                                CurrentTerrain = 0;
                                isPlaced = true;
                                DrawBlock(AllCellsSecondPlayer, Game.CardsList[CurrentCardSecondPlayer], CurrentBlockSecondPlayer, CurrentTerrainSecondPlayer);
                                SwitchTurn();
                            }
                        }

                        if (int.Parse((string)obj) == 1)
                        {
                            if (_isPlacingBlockAllowed && CanPlaceBlock(Game.CardsList[CurrentCardSecondPlayer], CurrentBlockSecondPlayer, Game.gameGridSecondPlayer, WasFitSecondPlayer, ref RuinForDotBlockSecondPlayer, CurrentCardSecondPlayer))
                            {
                                _isPlacingBlockAllowed = false;
                                Game.PlaceBlock(Game.gameGridSecondPlayer, Game.CardsList[CurrentCardSecondPlayer], CurrentBlockSecondPlayer, CurrentTerrainSecondPlayer);
                                if (Game.CardsList[CurrentCardSecondPlayer].Block.Length > 1 && CurrentBlockSecondPlayer == 0)
                                {
                                    Game.CoinsByBlockSecondPlayer++;
                                }
                                Game.CoinsByMountainSecondPlayer = Game.CountCoinsByMountains(Game.gameGridSecondPlayer);
                                CoinsSecondPlayer = (Game.CoinsByBlockSecondPlayer + Game.CoinsByMountainSecondPlayer).ToString();
                                UpdatePoints();
                                if (WasFitSecondPlayer)
                                {
                                    foreach (Block block in Game.CardsList[CurrentCardSecondPlayer].Block)
                                    {
                                        block.Reset();
                                    }
                                    CurrentCardSecondPlayer = PrevCardSecondPlayer;
                                    WasFitSecondPlayer = false;
                                }
                                foreach (Block block in Game.CardsList[CurrentCardSecondPlayer].Block)
                                {
                                    block.Reset();
                                }
                                UpdateField(AllCellsSecondPlayer, ref CurrentCardSecondPlayer, CurrentBlockSecondPlayer, PrevCardSecondPlayer, WasFitSecondPlayer, Game.gameGridSecondPlayer);
                                CurrentBlockSecondPlayer = 0;
                                CurrentTerrainSecondPlayer = 0;
                                CurrentDay += Game.CardsList[CurrentCard].Days;
                                isPlacedSecondPlayer = true;
                            }
                        }
                        if (CurrentDay >= Days)
                        {
                            ChangeSeason();
                            CurrentCard = 0;
                            CurrentBlock = 0;
                            CurrentTerrain = 0;
                            CurrentDay = 0;
                            CurrentCardSecondPlayer = 0;
                            CurrentBlockSecondPlayer = 0;
                            CurrentTerrainSecondPlayer = 0;
                            SeasonDays = CurrentDay.ToString();

                            while (Game.CardsList[CurrentCard].IsRuin)
                            {
                                ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";
                                CardDescription = Game.CardsList[CurrentCard].Description;
                                CurrentCard++;
                                CurrentCardSecondPlayer++;
                                UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                UpdateField(AllCellsSecondPlayer, ref CurrentCardSecondPlayer, CurrentBlockSecondPlayer, PrevCardSecondPlayer, WasFitSecondPlayer, Game.gameGridSecondPlayer);
                                await Task.Delay(2000);
                            }
                            CardDescription = Game.CardsList[CurrentCard].Description;
                            ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";
                            CheckBlock();
                            UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                            UpdateField(AllCellsSecondPlayer, ref CurrentCardSecondPlayer, CurrentBlockSecondPlayer, PrevCardSecondPlayer, WasFitSecondPlayer, Game.gameGridSecondPlayer);
                            _isPlacingBlockAllowed = true;
                        }
                        if (isPlaced && isPlacedSecondPlayer)
                        {
                            SwitchTurn();
                            CurrentCard++;
                            CurrentCardSecondPlayer++;
                            isPlaced = false;
                            isPlacedSecondPlayer = false;
                            while (Game.CardsList[CurrentCard].IsRuin)
                            {
                                ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";
                                CardDescription = Game.CardsList[CurrentCard].Description;
                                CurrentCard++;
                                CurrentCardSecondPlayer++;
                                UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                UpdateField(AllCellsSecondPlayer, ref CurrentCardSecondPlayer, CurrentBlockSecondPlayer, PrevCardSecondPlayer, WasFitSecondPlayer, Game.gameGridSecondPlayer);
                                await Task.Delay(2000);
                            }
                            CardDescription = Game.CardsList[CurrentCard].Description;
                            ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";

                            if (!Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock], Game.CardsList[CurrentCard > 0 ? CurrentCard - 1 : 0].IsRuin))
                            {
                                if (Game.CardsList[CurrentCard].Block.Length > 1 && Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock + 1], Game.CardsList[CurrentCard - 1].IsRuin))
                                {
                                    //Можна поставити тільки другий блок
                                }
                                else
                                {
                                    if (Game.CardsList[CurrentCard - 1].IsRuin)
                                    {
                                        RuinForDotBlock = true;
                                    }
                                    PrevCard = CurrentCard;
                                    CurrentCard = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
                                    WasFit = true;
                                }
                            }

                            if (!Game.CanFitBlock(Game.gameGridSecondPlayer, Game.CardsList[CurrentCardSecondPlayer].Block[CurrentBlockSecondPlayer], Game.CardsList[CurrentCardSecondPlayer > 0 ? CurrentCardSecondPlayer - 1 : 0].IsRuin))
                            {
                                if (Game.CardsList[CurrentCardSecondPlayer].Block.Length > 1 && Game.CanFitBlock(Game.gameGridSecondPlayer, Game.CardsList[CurrentCardSecondPlayer].Block[CurrentBlockSecondPlayer + 1], Game.CardsList[CurrentCardSecondPlayer - 1].IsRuin))
                                {
                                    //Можна поставити тільки другий блок
                                }
                                else
                                {
                                    if (Game.CardsList[CurrentCardSecondPlayer - 1].IsRuin)
                                    {
                                        RuinForDotBlockSecondPlayer = true;
                                    }
                                    PrevCardSecondPlayer = CurrentCardSecondPlayer;
                                    CurrentCardSecondPlayer = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
                                    WasFitSecondPlayer = true;
                                }
                            }
                            UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                            UpdateField(AllCellsSecondPlayer, ref CurrentCardSecondPlayer, CurrentBlockSecondPlayer, PrevCardSecondPlayer, WasFitSecondPlayer, Game.gameGridSecondPlayer);
                            DrawBlock(AllCells, Game.CardsList[CurrentCard], CurrentBlock, CurrentTerrain);
                        }
                        _isPlacingBlockAllowed = true;
                    }


                });

                ChengeBlockOrTerrainCommand = new RelayCommand(obj =>
                    {
                        if (int.Parse((string)obj) == 0)
                        {
                            if (_isPlacingBlockAllowed)
                            {
                                if (Game.CardsList[CurrentCard].IsBlockOriental)
                                {
                                    RemoveBlock(AllCells, Game.CardsList[CurrentCard], CurrentBlock);
                                    Game.CardsList[CurrentCard].Block[CurrentBlock].Reset();
                                    CurrentBlock++;
                                    if (CurrentBlock > Game.CardsList[CurrentCard].Block.Length - 1)
                                    {
                                        CurrentBlock = 0;
                                    }
                                    UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                    DrawBlock(AllCells, Game.CardsList[CurrentCard], CurrentBlock, CurrentTerrain);
                                }
                                else
                                {
                                    CurrentTerrain++;
                                    if (CurrentTerrain > Game.CardsList[CurrentCard].TerrainType.Length - 1)
                                    {
                                        CurrentTerrain = 0;
                                    }
                                    UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                                    DrawBlock(AllCells, Game.CardsList[CurrentCard], CurrentBlock, CurrentTerrain);
                                }
                            }
                        }
                        else
                        {
                            if (_isPlacingBlockAllowed)
                            {
                                if (Game.CardsList[CurrentCardSecondPlayer].IsBlockOriental)
                                {
                                    RemoveBlock(AllCellsSecondPlayer, Game.CardsList[CurrentCardSecondPlayer], CurrentBlockSecondPlayer);
                                    Game.CardsList[CurrentCardSecondPlayer].Block[CurrentBlockSecondPlayer].Reset();
                                    CurrentBlockSecondPlayer++;
                                    if (CurrentBlockSecondPlayer > Game.CardsList[CurrentCardSecondPlayer].Block.Length - 1)
                                    {
                                        CurrentBlockSecondPlayer = 0;
                                    }
                                    UpdateField(AllCellsSecondPlayer, ref CurrentCardSecondPlayer, CurrentBlockSecondPlayer, PrevCardSecondPlayer, WasFitSecondPlayer, Game.gameGridSecondPlayer);
                                    DrawBlock(AllCellsSecondPlayer, Game.CardsList[CurrentCard], CurrentBlockSecondPlayer, CurrentTerrainSecondPlayer);
                                }
                                else
                                {
                                    CurrentTerrainSecondPlayer++;
                                    if (CurrentTerrainSecondPlayer > Game.CardsList[CurrentCard].TerrainType.Length - 1)
                                    {
                                        CurrentTerrainSecondPlayer = 0;
                                    }
                                    UpdateField(AllCellsSecondPlayer, ref CurrentCardSecondPlayer, CurrentBlockSecondPlayer, PrevCardSecondPlayer, WasFitSecondPlayer, Game.gameGridSecondPlayer);
                                    DrawBlock(AllCellsSecondPlayer, Game.CardsList[CurrentCard], CurrentBlockSecondPlayer, CurrentTerrainSecondPlayer);
                                }
                            }
                        }

                    });

                ChangeMenuViewCommand = new RelayCommand(obj =>
                {
                    if (MenuVisibility && MenuSettingVisibility[0])
                    {
                        MenuVisibility = false;
                    }
                    else
                    {
                        MenuVisibility = true;
                    }
                    MenuSettingVisibility[0] = true;
                    MenuSettingVisibility[1] = false;
                    OnPropertyChanged(nameof(MenuSettingVisibility));
                    OnPropertyChanged(nameof(MenuVisibility));
                });

                ShowSettingsCommand = new RelayCommand(obj =>
                {
                    MenuSettingVisibility[0] = false;
                    MenuSettingVisibility[1] = true;
                    OnPropertyChanged(nameof(MenuSettingVisibility));
                });

                QuitCommand = new RelayCommand(obj =>
                {
                    _windowService.OpenWindow();
                });

            }
        }
        private void SwitchTurn()
        {
            currentPlayerTurn = (currentPlayerTurn == 0) ? 1 : 0;
        }
        public async Task StartGame()
        {
            MenuVisibility = false;
            MenuSettingVisibility = [true, false];
            Game.InitializeGame();
            for (int row = 0; row < rowCount; row++)
            {
                var rowList = new List<CellViewModel>();
                for (int column = 0; column < columnCount; column++)
                {
                    var cell = new CellViewModel(row, column);
                    if (Game.gameGrid[row, column] == 6)
                    {
                        cell.IsMountain = true;
                    }
                    if (Game.gameGrid[row, column] == 7)
                    {
                        cell.IsRuins = true;
                    }
                    else
                    {
                        cell.IsRuins = false;
                    }
                    rowList.Add(cell);
                }
                AllCells.Add(rowList);

            }
            for (int row = 0; row < rowCount; row++)
            {
                var rowList = new List<CellViewModel>();
                for (int column = 0; column < columnCount; column++)
                {
                    var cell = new CellViewModel(row, column);
                    if (Game.gameGridSecondPlayer[row, column] == 6)
                    {
                        cell.IsMountain = true;
                    }
                    if (Game.gameGridSecondPlayer[row, column] == 7)
                    {
                        cell.IsRuins = true;
                    }
                    else
                    {
                        cell.IsRuins = false;
                    }
                    rowList.Add(cell);
                }
                AllCellsSecondPlayer.Add(rowList);
            }
            UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
            UpdateField(AllCellsSecondPlayer, ref CurrentCardSecondPlayer, CurrentBlockSecondPlayer, PrevCardSecondPlayer, WasFitSecondPlayer, Game.gameGridSecondPlayer);
            ImageConditionA = @"../Images/Conditions/" + Game.CurrentConditionsList[0].Name + ".jpg";
            ImageConditionB = @"../Images/Conditions/" + Game.CurrentConditionsList[1].Name + ".jpg";
            ImageConditionC = @"../Images/Conditions/" + Game.CurrentConditionsList[2].Name + ".jpg";
            ImageConditionD = @"../Images/Conditions/" + Game.CurrentConditionsList[3].Name + ".jpg";
            ConditionAPoints = "0";
            ConditionBPoints = "0";
            ConditionCPoints = "0";
            ConditionDPoints = "0";
            MinusMonsters = "0";
            Coins = "0";
            Points = "0";
            ConditionAPointsSecondPlayer = "0";
            ConditionBPointsSecondPlayer = "0";
            ConditionCPointsSecondPlayer = "0";
            ConditionDPointsSecondPlayer = "0";
            MinusMonstersSecondPlayer = "0";
            CoinsSecondPlayer = "0";
            PointsSecondPlayer = "0";
            SeasonDays = "0";
            ConditionAText = Game.CurrentConditionsList[0].Description;
            ConditionBText = Game.CurrentConditionsList[1].Description;
            ConditionCText = Game.CurrentConditionsList[2].Description;
            ConditionDText = Game.CurrentConditionsList[3].Description;
            CardDescription = Game.CardsList[CurrentCard].Description;

            _isPlacingBlockAllowed = false;
            while (Game.CardsList[CurrentCard].IsMonster || Game.CardsList[CurrentCard].IsRuin)
            {
                while (Game.CardsList[CurrentCard].IsMonster)
                {
                    Game.ReshuffleCards(Game.CardsList);
                    CurrentCard = 0;
                }
                while (Game.CardsList[CurrentCard].IsRuin)
                {
                    ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";
                    CardDescription = Game.CardsList[CurrentCard].Description;
                    await Task.Delay(2000);
                    CurrentCard++;
                    CurrentCardSecondPlayer++;
                    UpdateField(AllCells, ref CurrentCard, CurrentBlock, PrevCard, WasFit, Game.gameGrid);
                    UpdateField(AllCellsSecondPlayer, ref CurrentCard, CurrentBlockSecondPlayer, PrevCardSecondPlayer, WasFitSecondPlayer, Game.gameGridSecondPlayer);
                }
            }
            _isPlacingBlockAllowed = true;
            ImageUrl = @"../Images/Cards/" + Game.CardsList[CurrentCard].Name + ".jpg";
            CardDescription = Game.CardsList[CurrentCard].Description;
            DrawBlock(AllCells, Game.CardsList[CurrentCard], CurrentBlock, CurrentTerrain);
        }

        public void InitializeForFirstPlayer()
        {
            grid = Game.gameGrid;
            cell = AllCells;
            currentCard = CurrentCard;
            currentBlock = CurrentBlock;
            prevCard = PrevCard;
            wasFit = WasFit;
            currentTerrain = CurrentTerrain;
        }
        public void InitializeForSecondPlayer()
        {
            grid = Game.gameGridSecondPlayer;
            cell = AllCellsSecondPlayer;
            currentCard = CurrentCardSecondPlayer;
            currentBlock = CurrentBlockSecondPlayer;
            prevCard = PrevCard;
            wasFit = WasFitSecondPlayer;
            currentTerrain = CurrentTerrainSecondPlayer;
        }
        public void CheckBlock()
        {
            if (CurrentCard > 0 && !Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock], Game.CardsList[CurrentCard - 1].IsRuin))
            {
                if (Game.CardsList[CurrentCard].Block.Length > 1 && Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock + 1], Game.CardsList[CurrentCard - 1].IsRuin))
                {
                    //Можна поставити тільки другий блок
                }
                else
                {

                    if (Game.CardsList[CurrentCard - 1].IsRuin)
                    {
                        RuinForDotBlock = true;
                    }
                    PrevCard = CurrentCard;
                    CurrentCard = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
                    WasFit = true;
                }
            }
            else if (CurrentCard == 0 && !Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock], false))
            {
                if (Game.CardsList[CurrentCard].Block.Length > 1 && Game.CanFitBlock(Game.gameGrid, Game.CardsList[CurrentCard].Block[CurrentBlock + 1], false))
                {
                    //Можна поставити тільки другий блок
                }
                else
                {
                    PrevCard = CurrentCard;
                    CurrentCard = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
                    WasFit = true;
                }
            }
            if (CurrentCardSecondPlayer > 0 && !Game.CanFitBlock(Game.gameGridSecondPlayer, Game.CardsList[CurrentCardSecondPlayer].Block[CurrentBlockSecondPlayer], Game.CardsList[CurrentCardSecondPlayer - 1].IsRuin))
            {
                if (Game.CardsList[CurrentCardSecondPlayer].Block.Length > 1 && Game.CanFitBlock(Game.gameGridSecondPlayer, Game.CardsList[CurrentCardSecondPlayer].Block[CurrentBlockSecondPlayer + 1], Game.CardsList[CurrentCardSecondPlayer - 1].IsRuin))
                {
                    //Можна поставити тільки другий блок
                }
                else
                {

                    if (Game.CardsList[CurrentCard - 1].IsRuin)
                    {
                        RuinForDotBlockSecondPlayer = true;
                    }
                    PrevCardSecondPlayer = CurrentCardSecondPlayer;
                    CurrentCardSecondPlayer = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
                    WasFitSecondPlayer = true;
                }
            }
            else if (CurrentCardSecondPlayer == 0 && !Game.CanFitBlock(Game.gameGridSecondPlayer, Game.CardsList[CurrentCardSecondPlayer].Block[CurrentBlockSecondPlayer], false))
            {
                if (Game.CardsList[CurrentCardSecondPlayer].Block.Length > 1 && Game.CanFitBlock(Game.gameGridSecondPlayer, Game.CardsList[CurrentCardSecondPlayer].Block[CurrentBlockSecondPlayer + 1], false))
                {
                    //Можна поставити тільки другий блок
                }
                else
                {
                    PrevCardSecondPlayer = CurrentCardSecondPlayer;
                    CurrentCardSecondPlayer = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
                    WasFitSecondPlayer = true;
                }
            }
        }
        public void DrawBlock(List<List<CellViewModel>> cells, Card card, int currentBlock, int currentTerrain)
        {
            foreach (Position position in card.Block[currentBlock].TilePositions())
            {
                cells[position.Row][position.Column].TerrainType = card.TerrainType[currentTerrain];
            }
        }

        public void UpdatePoints()
        {
            ConditionAPoints = Game.CountConditions(0, Game.gameGrid).ToString();
            ConditionBPoints = Game.CountConditions(1, Game.gameGrid).ToString();
            ConditionCPoints = Game.CountConditions(2, Game.gameGrid).ToString();
            ConditionDPoints = Game.CountConditions(3, Game.gameGrid).ToString();
            MinusMonsters = Game.CountMonsters(Game.gameGrid).ToString();
            ConditionAPointsSecondPlayer = Game.CountConditions(0, Game.gameGridSecondPlayer).ToString();
            ConditionBPointsSecondPlayer = Game.CountConditions(1, Game.gameGridSecondPlayer).ToString();
            ConditionCPointsSecondPlayer = Game.CountConditions(2, Game.gameGridSecondPlayer).ToString();
            ConditionDPointsSecondPlayer = Game.CountConditions(3, Game.gameGridSecondPlayer).ToString();
            MinusMonstersSecondPlayer = Game.CountMonsters(Game.gameGridSecondPlayer).ToString();
        }
        public void UpdateField(List<List<CellViewModel>> cells, ref int currentCard, int currentBlock, int prevCard, bool wasFit, GameGrid gameGrid)
        {
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    cells[row][column].TerrainType = (TerrainType)gameGrid[row, column];
                    cells[row][column].IsTransparent = false;
                }
            }

            if (!Game.CardsList[currentCard].IsRuin)
            {
                foreach (Position position in Game.CardsList[currentCard].Block[currentBlock].TilePositions())
                {
                    int row = position.Row;
                    int column = position.Column;
                    if (!gameGrid.IsEmpty(row, column))
                    {
                        cells[row][column].IsTransparent = true;
                    }
                    else
                    {
                        cells[row][column].IsTransparent = false;
                    }
                }
            }
            if (wasFit)
            {
                currentCard = prevCard;
            }
            if (currentCard >= 1 && Game.CardsList[currentCard] != null && Game.CardsList[currentCard - 1].IsRuin)
            {
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        if (gameGrid[row, column] == 7)
                        {
                            cells[row][column].IsRuinsHighlighted = true;
                        }
                    }
                }
            }
            else
            {
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        if (gameGrid[row, column] == 7)
                        {
                            cells[row][column].IsRuinsHighlighted = false;
                        }
                    }
                }
            }
            if (wasFit)
            {
                currentCard = Game.CardsList.FindIndex(card => card.Name == "Rift Lands");
            }
            SeasonDays = CurrentDay.ToString();
        }

        public void RemoveBlock(List<List<CellViewModel>> cell, Card card, int currentBlock)
        {
            foreach (Position position in card.Block[currentBlock].TilePositions())
            {
                CellViewModel oneCell = cell[position.Row][position.Column];
                oneCell.TerrainType = TerrainType.None;
            }
        }

        public bool CanPlaceBlock(Card card, int currentBlock, GameGrid gameGrid, bool wasFit, ref bool ruinForDot, int currentCard)
        {
            if (currentCard > 0 && Game.CardsList[currentCard - 1].IsRuin && Game.IsRuinInGrid(gameGrid) && !wasFit ||
                (wasFit && ruinForDot && Game.IsRuinInGrid(gameGrid)))
            {
                return CheckTilePositions(card.Block[currentBlock], gameGrid, ref ruinForDot, checkForRuin: true);
            }
            else if (wasFit && ruinForDot && !Game.IsRuinInGrid(gameGrid))
            {
                return CheckTilePositions(card.Block[currentBlock], gameGrid, ref ruinForDot, checkForEmpty: true);
            }

            return Game.CanPlaceBlock(gameGrid, card.Block[currentBlock]);
        }

        private bool CheckTilePositions(Block block, GameGrid gameGrid, ref bool ruinForDot, bool checkForRuin = false, bool checkForEmpty = false)
        {
            bool isBlockRuin = false;

            foreach (Position position in block.TilePositions())
            {
                int row = position.Row;
                int column = position.Column;

                if (!gameGrid.IsEmptyOrRuin(row, column))
                {
                    return false;
                }

                if (checkForRuin && gameGrid.IsRuin(row, column))
                {
                    ruinForDot = false;
                    isBlockRuin = true;
                }

                if (checkForEmpty && gameGrid.IsEmpty(row, column))
                {
                    ruinForDot = false;
                    return true;
                }
            }

            return checkForRuin ? isBlockRuin : false;
        }

        public void ChangeSeason()
        {
            switch (currentSeason)
            {
                case Seasons.Весна:
                    Game.CardsList.Add(Game.MonstersList[1]);
                    Points = Game.CountPoints(Game.CountConditions(0, Game.gameGrid), Game.CountConditions(1, Game.gameGrid), (Game.CoinsByBlock + Game.CoinsByMountain), Game.CountMonsters(Game.gameGrid)).ToString();
                    PointsSecondPlayer = Game.CountPointsSecondPlayer(Game.CountConditions(0, Game.gameGridSecondPlayer), Game.CountConditions(1, Game.gameGridSecondPlayer), (Game.CoinsByBlockSecondPlayer + Game.CoinsByMountainSecondPlayer), Game.CountMonsters(Game.gameGridSecondPlayer)).ToString();
                    CurrentSeason = Seasons.Літо;
                    break;
                case Seasons.Літо:
                    Game.CardsList.Add(Game.MonstersList[2]);
                    Points = Game.CountPoints(Game.CountConditions(1, Game.gameGrid), Game.CountConditions(2, Game.gameGrid), (Game.CoinsByBlock + Game.CoinsByMountain), Game.CountMonsters(Game.gameGrid)).ToString();
                    PointsSecondPlayer = Game.CountPointsSecondPlayer(Game.CountConditions(1, Game.gameGridSecondPlayer), Game.CountConditions(2, Game.gameGridSecondPlayer), (Game.CoinsByBlockSecondPlayer + Game.CoinsByMountainSecondPlayer), Game.CountMonsters(Game.gameGridSecondPlayer)).ToString();
                    CurrentSeason = Seasons.Осінь;
                    Days--;
                    break;
                case Seasons.Осінь:
                    Game.CardsList.Add(Game.MonstersList[3]);
                    Points = Game.CountPoints(Game.CountConditions(2, Game.gameGrid), Game.CountConditions(3, Game.gameGrid), (Game.CoinsByBlock + Game.CoinsByMountain), Game.CountMonsters(Game.gameGrid)).ToString();
                    PointsSecondPlayer = Game.CountPointsSecondPlayer(Game.CountConditions(2, Game.gameGridSecondPlayer), Game.CountConditions(3, Game.gameGridSecondPlayer), (Game.CoinsByBlockSecondPlayer + Game.CoinsByMountainSecondPlayer), Game.CountMonsters(Game.gameGridSecondPlayer)).ToString();
                    CurrentSeason = Seasons.Зима;
                    Days--;
                    break;
                case Seasons.Зима:
                    Points = Game.CountPoints(Game.CountConditions(3, Game.gameGrid), Game.CountConditions(0, Game.gameGrid), (Game.CoinsByBlock + Game.CoinsByMountain), Game.CountMonsters(Game.gameGrid)).ToString();
                    PointsSecondPlayer = Game.CountPointsSecondPlayer(Game.CountConditions(3, Game.gameGridSecondPlayer), Game.CountConditions(0, Game.gameGridSecondPlayer), (Game.CoinsByBlockSecondPlayer + Game.CoinsByMountainSecondPlayer), Game.CountMonsters(Game.gameGridSecondPlayer)).ToString();
                    MessageBox.Show("Перший гравець:" + Game.Points.ToString());
                    var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                    if (currentWindow is TwoPlayersGameWindow)
                    {
                        MessageBox.Show("Другий гравець:" + Game.PointsSecondPlayer.ToString());
                    }
                    RestartGame();
                    break;
            }
            Game.ReshuffleCards(Game.CardsList);
        }

        public void RestartGame()
        {
            CurrentCard = 0;
            CurrentBlock = 0;
            CurrentTerrain = 0;
            CurrentSeason = Seasons.Весна;
            CurrentDay = 0;
            Days = 8;
            WasFit = false;
            RuinForDotBlock = false;
            _isPlacingBlockAllowed = true;
            CurrentCardSecondPlayer = 0;
            CurrentBlockSecondPlayer = 0;
            CurrentTerrainSecondPlayer = 0;
            WasFitSecondPlayer = false;
            RuinForDotBlockSecondPlayer = false;
            StartGame();
        }

        private string _imageUrl;

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }

        private string _imageConditionA;
        public string ImageConditionA
        {
            get { return _imageConditionA; }
            set
            {
                if (_imageConditionA != value)
                {
                    _imageConditionA = value;
                    OnPropertyChanged(nameof(ImageConditionA));
                }
            }
        }

        private string _imageConditionB;
        public string ImageConditionB
        {
            get { return _imageConditionB; }
            set
            {
                if (_imageConditionB != value)
                {
                    _imageConditionB = value;
                    OnPropertyChanged(nameof(ImageConditionB));
                }
            }
        }

        private string _imageConditionC;
        public string ImageConditionC
        {
            get { return _imageConditionC; }
            set
            {
                if (_imageConditionC != value)
                {
                    _imageConditionC = value;
                    OnPropertyChanged(nameof(ImageConditionC));
                }
            }
        }

        private string _imageConditionD;
        public string ImageConditionD
        {
            get { return _imageConditionD; }
            set
            {
                if (_imageConditionD != value)
                {
                    _imageConditionD = value;
                    OnPropertyChanged(nameof(ImageConditionD));
                }
            }
        }

        private string _conditionAPoints;
        public string ConditionAPoints
        {
            get { return _conditionAPoints; }
            set
            {
                if (_conditionAPoints != value)
                {
                    _conditionAPoints = value;
                    OnPropertyChanged(nameof(ConditionAPoints));
                }
            }
        }

        private string _conditionBPoints;
        public string ConditionBPoints
        {
            get { return _conditionBPoints; }
            set
            {
                if (_conditionBPoints != value)
                {
                    _conditionBPoints = value;
                    OnPropertyChanged(nameof(ConditionBPoints));
                }
            }
        }

        private string _conditionCPoints;
        public string ConditionCPoints
        {
            get { return _conditionCPoints; }
            set
            {
                if (_conditionCPoints != value)
                {
                    _conditionCPoints = value;
                    OnPropertyChanged(nameof(ConditionCPoints));
                }
            }
        }

        private string _conditionDPoints;
        public string ConditionDPoints
        {
            get { return _conditionDPoints; }
            set
            {
                if (_conditionDPoints != value)
                {
                    _conditionDPoints = value;
                    OnPropertyChanged(nameof(ConditionDPoints));
                }
            }
        }

        private string _minusMonsters;
        public string MinusMonsters
        {
            get { return _minusMonsters; }
            set
            {
                if (_minusMonsters != value)
                {
                    _minusMonsters = value;
                    OnPropertyChanged(nameof(MinusMonsters));
                }
            }
        }

        private string _coins;
        public string Coins
        {
            get { return _coins; }
            set
            {
                if (_coins != value)
                {
                    _coins = value;
                    OnPropertyChanged(nameof(Coins));
                }
            }
        }

        private string _points;
        public string Points
        {
            get { return _points; }
            set
            {
                if (_points != value)
                {
                    _points = value;
                    OnPropertyChanged(nameof(Points));
                }
            }
        }

        private string _conditionAPointsSecondPlayer;
        public string ConditionAPointsSecondPlayer
        {
            get { return _conditionAPointsSecondPlayer; }
            set
            {
                if (_conditionAPointsSecondPlayer != value)
                {
                    _conditionAPointsSecondPlayer = value;
                    OnPropertyChanged(nameof(ConditionAPointsSecondPlayer));
                }
            }
        }

        private string _conditionBPointsSecondPlayer;
        public string ConditionBPointsSecondPlayer
        {
            get { return _conditionBPointsSecondPlayer; }
            set
            {
                if (_conditionBPointsSecondPlayer != value)
                {
                    _conditionBPointsSecondPlayer = value;
                    OnPropertyChanged(nameof(ConditionBPointsSecondPlayer));
                }
            }
        }

        private string _conditionCPointsSecondPlayer;
        public string ConditionCPointsSecondPlayer
        {
            get { return _conditionCPointsSecondPlayer; }
            set
            {
                if (_conditionCPointsSecondPlayer != value)
                {
                    _conditionCPointsSecondPlayer = value;
                    OnPropertyChanged(nameof(ConditionCPointsSecondPlayer));
                }
            }
        }

        private string _conditionDPointsSecondPlayer;
        public string ConditionDPointsSecondPlayer
        {
            get { return _conditionDPointsSecondPlayer; }
            set
            {
                if (_conditionDPointsSecondPlayer != value)
                {
                    _conditionDPointsSecondPlayer = value;
                    OnPropertyChanged(nameof(ConditionDPointsSecondPlayer));
                }
            }
        }

        private string _minusMonstersSecondPlayer;
        public string MinusMonstersSecondPlayer
        {
            get { return _minusMonstersSecondPlayer; }
            set
            {
                if (_minusMonstersSecondPlayer != value)
                {
                    _minusMonstersSecondPlayer = value;
                    OnPropertyChanged(nameof(MinusMonstersSecondPlayer));
                }
            }
        }

        private string _coinsSecondPlayer;
        public string CoinsSecondPlayer
        {
            get { return _coinsSecondPlayer; }
            set
            {
                if (_coinsSecondPlayer != value)
                {
                    _coinsSecondPlayer = value;
                    OnPropertyChanged(nameof(CoinsSecondPlayer));
                }
            }
        }

        private string _pointsSecondPlayer;
        public string PointsSecondPlayer
        {
            get { return _pointsSecondPlayer; }
            set
            {
                if (_pointsSecondPlayer != value)
                {
                    _pointsSecondPlayer = value;
                    OnPropertyChanged(nameof(PointsSecondPlayer));
                }
            }
        }

        private string _seasonDays;
        public string SeasonDays
        {
            get { return _seasonDays; }
            set
            {
                if (_seasonDays != value)
                {
                    _seasonDays = value;
                    OnPropertyChanged(nameof(SeasonDays));
                }
            }
        }
        public Seasons CurrentSeason
        {
            get { return currentSeason; }
            set
            {
                if (currentSeason != value)
                {
                    currentSeason = value;
                    OnPropertyChanged(nameof(CurrentSeason));
                }
            }
        }

        private string _conditionAText;
        public string ConditionAText
        {
            get { return _conditionAText; }
            set
            {
                if (_conditionAText != value)
                {
                    _conditionAText = value;
                    OnPropertyChanged(nameof(ConditionAText));
                }
            }
        }

        private string _conditionBText;
        public string ConditionBText
        {
            get { return _conditionBText; }
            set
            {
                if (_conditionBText != value)
                {
                    _conditionBText = value;
                    OnPropertyChanged(nameof(ConditionBText));
                }
            }
        }

        private string _conditionCText;
        public string ConditionCText
        {
            get { return _conditionCText; }
            set
            {
                if (_conditionCText != value)
                {
                    _conditionCText = value;
                    OnPropertyChanged(nameof(ConditionCText));
                }
            }
        }

        private string _conditionDText;
        public string ConditionDText
        {
            get { return _conditionDText; }
            set
            {
                if (_conditionDText != value)
                {
                    _conditionDText = value;
                    OnPropertyChanged(nameof(ConditionDText));
                }
            }
        }

        private string _cardDescription;
        public string CardDescription
        {
            get { return _cardDescription; }
            set
            {
                if (_cardDescription != value)
                {
                    _cardDescription = value;
                    OnPropertyChanged(nameof(CardDescription));
                }
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
