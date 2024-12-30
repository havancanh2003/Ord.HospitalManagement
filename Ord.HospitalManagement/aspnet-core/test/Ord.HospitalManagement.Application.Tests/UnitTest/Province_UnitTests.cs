using Microsoft.EntityFrameworkCore;
using Moq;
using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.DTOs.Address.ModelFilter;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.Entities;
using Ord.HospitalManagement.Entities.Address;
using Ord.HospitalManagement.Enums;
using Ord.HospitalManagement.IServices.Address;
using Ord.HospitalManagement.IServices.Hospital;
using Ord.HospitalManagement.Services;
using Ord.HospitalManagement.Services.Common;
using Ord.HospitalManagement.Services.ManegeHospital;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Xunit;
using static Ord.HospitalManagement.Permissions.HospitalManagementPermissions;
using NSubstitute;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Volo.Abp.ObjectMapping;
using Ord.HospitalManagement.IServices;
using System.Threading;

namespace Ord.HospitalManagement.UnitTest
{
    public class Province_UnitTests
    {
        private readonly Mock<IRepository<Province, int>> _repositoryMock;
        private readonly Mock<IDapperRepo> _dapperMock;
        private readonly Mock<IGenerateCode> _generateCodeMock;
        private readonly Mock<ICurrentUser> _currentUserMock;
        private readonly ProvinceAppService _provinceAppService;
        private readonly IFixture _fixture;
        //private readonly IProvinceAppService _provinceAppService;
        public Province_UnitTests()
        {
            _repositoryMock = new Mock<IRepository<Province, int>>();
            _dapperMock = new Mock<IDapperRepo>();
            _generateCodeMock = new Mock<IGenerateCode>();
            _currentUserMock = new Mock<ICurrentUser>();
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _provinceAppService = new ProvinceAppService(
                _repositoryMock.Object,
                _dapperMock.Object,
                _generateCodeMock.Object,
                _currentUserMock.Object
            );
        }
        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedProvinceDto()
        {
            // Arrange
            var createDto = _fixture.Create<CreateUpdateProvinceDto>();
            var generatedCode = "PROV_1234";
            var province = new Province { Code = generatedCode, Name = createDto.Name, LevelProvince = createDto.LevelProvince };

            _generateCodeMock.Setup(g => g.AutoGenerateCode(It.IsAny<string>())).Returns(generatedCode);
            _repositoryMock
                    .Setup(r => r.InsertAsync(It.IsAny<Province>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(province);

            // Act
            var result = await _provinceAppService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(generatedCode, result.Code);  // Kiểm tra giá trị mã province
            Assert.Equal(province.Name, result.Name);  // Kiểm tra tên province
            Assert.Equal(province.LevelProvince, result.LevelProvince);  // Kiểm tra level province

            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Province>(), true, default), Times.Once);
        }


        [Fact]
        public async Task Should_Get_All_Issues()
        {
            //Act
            var all = await _provinceAppService.GetListAsync(new DTOs.Address.ModelFilter.CustomePagedAndSortedResultRequestProvinceDto());
            //Assert
            all.TotalCount.ShouldBeGreaterThan(0);
        }

        public async Task GetProvinceByCodeAsync_ShouldThrowArgumentNullException_WhenProvinceDoesNotExist()
        {
            //
            var provinceCode = "PROV_8429";
            _repositoryMock.Setup(r => r.FirstOrDefaultAsync(p => p.Code.Equals(provinceCode),default))
                .ReturnsAsync((Province)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _provinceAppService.GetProvinceByCode(provinceCode));
            exception.Message.ShouldBe("Không tồn tại thông tin địa chỉ trong hệ thống");
        }
        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var provinceId = 1;
            var inputDto = _fixture.Create<CreateUpdateProvinceDto>();

            _repositoryMock
                .Setup(r => r.GetAsync(provinceId,false,default))
                .ThrowsAsync(new Exception("Xảy ra lỗi"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await _provinceAppService.UpdateAsync(provinceId, inputDto));

            exception.Message.ShouldBe("Xảy ra lỗi");
        }

    }
}
