using Microsoft.Extensions.Configuration;
using SLMM.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLMM.Services
{
    public class SLMMService : IOperate
    {
        private int _length;
        private int _width;
        private object obj = new object();
        private readonly IConfiguration _config;

        private Position _currentPosition { get; set; }
        public SLMMService(Position position, IConfiguration config)
        {
            //Will configure later to get from app config

            _currentPosition = position;
            this._config = config;
            SetArea(_config.GetValue<int>("Length"), _config.GetValue<int>("Width"));
        }
        public Task<Position> GetPosition()
        {
            return Task.FromResult(_currentPosition);
        }



        public Task Reset(int length, int width)
        {
            SetArea(length, width);
            _currentPosition.X = 0;
            _currentPosition.Y = 0;
            _currentPosition.Orientation = OrientationEnum.North;

            return Task.CompletedTask;


        }

        private void SetArea(int length, int width)
        {
            this._length = length;
            this._width = width;
        }
        /// <summary>
        /// Make a Turn Default right
        /// </summary>
        /// <param name="isLeft">Default right , make true for left</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Position> Turn(bool isLeft)
        {
            if (isLeft)
                await TurnLeft();
            else
                await TurnRight();

            return _currentPosition;
            throw new Exception("Please wait ! Operation is progress!");

        }

        private async Task TurnLeft()
        {

            if (_currentPosition.Orientation == OrientationEnum.North)
                _currentPosition.Orientation = OrientationEnum.West;
            else
                _currentPosition.Orientation -= 1;
            await Task.Delay(_config.GetValue<int>("TurnTimeMs"));
        }

        private async Task TurnRight()
        {
            if (_currentPosition.Orientation == OrientationEnum.West)
                _currentPosition.Orientation = OrientationEnum.North;
            else
                _currentPosition.Orientation += 1;
            await Task.Delay(_config.GetValue<int>("TurnTimeMs"));
        }

        public async Task<Position> MoveForward()
        {
                switch (_currentPosition.Orientation)
                {
                    case OrientationEnum.East:
                        if (_currentPosition.X == _length - 1)
                            throw new Exception($"Reached at End of boundry Length {_length}");
                        else
                            _currentPosition.X++;
                        break;
                    case OrientationEnum.North:
                        if (_currentPosition.Y == _width - 1)
                            throw new Exception($"Reached at End of boundry Width {_width}");
                        else
                            _currentPosition.Y++;
                        break;
                    case OrientationEnum.West:
                        if (_currentPosition.X == 0)
                            throw new Exception($"Reached at start of boundry Length");
                        else
                            _currentPosition.X--;
                        break;
                    case OrientationEnum.South:
                        if (_currentPosition.Y == 0)
                            throw new Exception($"Reached at start of boundry Width");
                        else
                            _currentPosition.Y--;
                        break;
                }
                await Task.Delay(_config.GetValue<int>("ForwardTimeMs"));
                return _currentPosition;
           

        }
    }
}
