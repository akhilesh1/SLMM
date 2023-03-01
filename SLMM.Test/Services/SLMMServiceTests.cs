using Microsoft.Extensions.Configuration;
using Moq;
using SLMM.Models;
using SLMM.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SLMM.Test.Services
{
    public class SLMMServiceTests
    {
        private MockRepository _mockRepository;

        public Mock<Position> _mockPosition;
        public Mock<IConfiguration> _mockConfig;

        public SLMMServiceTests()
        {
            this._mockRepository = new MockRepository(MockBehavior.Strict);
            this._mockPosition = this._mockRepository.Create<Position>();
            this._mockConfig = this._mockRepository.Create<IConfiguration>();
        }

        private SLMMService CreateService()
        {
            var inMemorySettings = new Dictionary<string, string> {
                    {"Length", "3"},
                    {"Width", "5"},
                     {"TurnTimeMs", "2000"},
                    {"ForwardTimeMs", "5000"},
                    //...populate as needed for the test
                };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new SLMMService(_mockPosition.Object, configuration);
        }

        [Fact]
        public async Task GetPosition_Default_Test()
        {
            var service = this.CreateService();
            var result = await service.GetPosition();
            Assert.NotNull(result);
            Assert.Equal(0, result.X);
            Assert.Equal(0, result.Y);
            Assert.Equal(OrientationEnum.North, result.Orientation);
            this._mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Reset_Test()
        {
            var service = this.CreateService();
            int length = 3;
            int width = 2;

            await service.MoveForward();
            await service.Turn(false);
            await service.MoveForward();
            //Intial position set to 1,1
            Assert.Equal(1, _mockPosition.Object.X);
            Assert.Equal(1, _mockPosition.Object.Y);
            Assert.NotEqual(OrientationEnum.North, _mockPosition.Object.Orientation);

            await service.Reset(length, width);
            //After reset position is 0,0 Facing North
            Assert.Equal(0, _mockPosition.Object.X);
            Assert.Equal(0, _mockPosition.Object.Y);
            Assert.Equal(OrientationEnum.North, _mockPosition.Object.Orientation);
            this._mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Turn_Test()
        {

            var service = this.CreateService();
            await service.Turn(true);// Direction West and 0,0
            Assert.Equal(OrientationEnum.West, _mockPosition.Object.Orientation);

            await service.Turn(false);// Direction North and 0,0
            Assert.Equal(OrientationEnum.North, _mockPosition.Object.Orientation);

            await service.Turn(false);// Direction East and 0,0
            Assert.Equal(OrientationEnum.East, _mockPosition.Object.Orientation);

            Assert.Equal(0, _mockPosition.Object.X);
            Assert.Equal(0, _mockPosition.Object.Y);

        }

        [Fact]
        public async Task MoveForward_Boundry_Test()
        {

            var service = this.CreateService();
            await service.Turn(true);// Direction West and 0,0
            var exception = await Assert.ThrowsAsync<Exception>(() => service.MoveForward());
            Assert.Equal("Reached at start of boundry Length", exception.Message);

            await service.Turn(true);// Direction South and 0,0
            exception = await Assert.ThrowsAsync<Exception>(() => service.MoveForward());
            Assert.Equal("Reached at start of boundry Width", exception.Message);
            await service.Reset(3, 2);
            await service.MoveForward();

            //At 1,0
            exception = await Assert.ThrowsAsync<Exception>(() => service.MoveForward());
            Assert.Equal($"Reached at End of boundry Width {2}", exception.Message);

        }

        [Fact]
        public async Task MoveForward_Test()
        {
            var service = this.CreateService();
            await service.MoveForward();
            await service.MoveForward();
            //After reset position is 0,0 Facing North
            Assert.Equal(0, _mockPosition.Object.X);
            Assert.Equal(2, _mockPosition.Object.Y);
            Assert.Equal(OrientationEnum.North, _mockPosition.Object.Orientation);

        }
    }


}
