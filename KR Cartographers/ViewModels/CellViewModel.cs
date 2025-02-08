using KR_Cartographers.Services;
using System.ComponentModel;
using KR_Cartographers.Models;

namespace KR_Cartographers.ViewModels
{
    internal class CellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IWindowService _windowService;

        public int Row { get; }
        public int Column { get; }

        private TerrainType _terrainType = TerrainType.None;
        private bool _isTransparent;
        public string _ruinImage;
        public TerrainType TerrainType
        {
            get => _terrainType;
            set
            {
                _terrainType = value;
                OnPropertyChanged(nameof(TerrainType));
            }
        }

        public bool IsTransparent
        {
            get { return _isTransparent; }
            set
            {
                if (_isTransparent != value)
                {
                    _isTransparent = value;
                    OnPropertyChanged(nameof(IsTransparent));
                }
            }
        }

        private bool _isRuins;
        public bool IsRuins
        {
            get { return _isRuins; }
            set
            {
                if (_isRuins != value)
                {
                    _isRuins = value;
                    OnPropertyChanged(nameof(IsRuins));
                }
            }
        }

        private bool _isMountain;
        public bool IsMountain
        {
            get { return _isMountain; }
            set
            {
                if( _isMountain != value)
                {
                    _isMountain = value;
                    OnPropertyChanged(nameof(IsMountain));
                }
            }
        }

        private bool _isRuinsHighlighted;
        public bool IsRuinsHighlighted
        {
            get { return _isRuinsHighlighted; }
            set
            {
                _isRuinsHighlighted = value;
                OnPropertyChanged(nameof(IsRuinsHighlighted));
            }
        }

        public CellViewModel(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
