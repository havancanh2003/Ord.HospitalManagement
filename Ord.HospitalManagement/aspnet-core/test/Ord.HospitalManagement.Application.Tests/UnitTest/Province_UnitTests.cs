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
using Autofac.Core;
using Volo.Abp;
using System.Linq.Expressions;

namespace Ord.HospitalManagement.UnitTest
{
    public class Province_UnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRepository<Province, int>> _repositoryMock;
        private readonly Mock<IDapperRepo> _dapperRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IGenerateCode> _generateCodeMock;
        private readonly ProvinceAppService _provinceAppService;

        public Province_UnitTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _repositoryMock = _fixture.Freeze<Mock<IRepository<Province, int>>>();
            _dapperRepoMock = _fixture.Freeze<Mock<IDapperRepo>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _generateCodeMock = _fixture.Freeze<Mock<IGenerateCode>>();

            _provinceAppService = new ProvinceAppService(
                _repositoryMock.Object,
                _dapperRepoMock.Object,
                _mapperMock.Object,
                _generateCodeMock.Object
            );
        }
        [Fact]
        public async Task CreateAsync_Should_Create_Province()
        {
            // Arrange
            var input = _fixture.Create<CreateUpdateProvinceDto>();
            var mappedProvince = _fixture.Create<Province>();
            var generatedCode = "PROV_1234";
            mappedProvince.Code = generatedCode;
            var mappedDto = _fixture.Create<ProvinceDto>();

            _mapperMock.Setup(m => m.Map<Province>(input)).Returns(mappedProvince);
            _mapperMock.Setup(m => m.Map<ProvinceDto>(mappedProvince)).Returns(mappedDto);

            // Act
            var result = await _provinceAppService.CreateAsync(input);

            // Assert
            _repositoryMock.Verify(r => r.InsertAsync(It.Is<Province>(p => p == mappedProvince), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(mappedDto, result);
            Assert.Equal(generatedCode, result.Code);
        }
        [Fact]
        public async Task GetListAsync_Should_Return_Filtered_Provinces()
        {
            // Arrange
            var input = new CustomePagedAndSortedResultRequestProvinceDto
            {
                FilterName = "Tỉnh",
                SkipCount = 0,
                MaxResultCount = 10
            };

            var provinceDtos = _fixture.CreateMany<ProvinceDto>(5).ToList();
            var total = provinceDtos.Count;
            var queryResult = (total: total, lists: provinceDtos);

            _dapperRepoMock
                .Setup(d => d.QueryMultiGetAsync<ProvinceDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _provinceAppService.GetListAsync(input);

            // Assert
            Assert.Equal(total, result.TotalCount);
            Assert.Equal(provinceDtos, result.Items);
        }

        [Fact]
        public async Task GetProvinceByCode_ShouldReturnProvinceDto_WhenProvinceExists()
        {
            // Arrange
            var code = "123";
            var province = new Province { Code = code };
            var provinceDto = new ProvinceDto { Code = code };

            _repositoryMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Province, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(province);

            _mapperMock.Setup(m => m.Map<Province, ProvinceDto>(province))
                       .Returns(provinceDto);

            // Act
            var result = await _provinceAppService.GetProvinceByCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(code, result.Code);
        }
        [Fact]
        public async Task UpdateAsync_Should_ThrowException_When_ProvinceDoesNotExist()
        {
            // Arrange
            var nonExistentId = 99; // ID không tồn tại
            var input = new CreateUpdateProvinceDto
            {
                // Điền các giá trị phù hợp cho DTO
                Name = "Updated Province",
                LevelProvince = LevelProvince.Province,
            };

            _repositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(Province)); // Giả lập không tìm thấy Province

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await _provinceAppService.UpdateAsync(nonExistentId, input));

            Assert.Equal("Xảy ra lỗi", exception.Message);

            // Đảm bảo phương thức UpdateAsync trên Repository không được gọi
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Province>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact]
        public async Task DeleteAsync_Should_ThrowArgumentException_When_IdIsZero()
        {
            // Arrange
            var provinceId = 0;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _provinceAppService.DeleteAsync(provinceId));

            exception.Message.ShouldBe("ID không hợp lệ");
        }
    }
}
