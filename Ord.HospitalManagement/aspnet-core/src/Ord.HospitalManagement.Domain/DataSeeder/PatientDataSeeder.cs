using Ord.HospitalManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Ord.HospitalManagement.DataSeeder
{
    public class PatientDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public PatientDataSeeder()
        {
        }

        public async Task SeedAsync(DataSeedContext context)
        {
           
            // Create sample patients
            //var patients = new[]
            //{
            //new Patient
            //{
            //    TenantId = context.TenantId,
            //    HospitalId = 3,
            //    Code = "PAT001",
            //    Fullname = "John Doe",
            //    ProvinceCode = "01",
            //    DistrictCode = "001",
            //    WardCode = "0001",
            //    DetailAddress = "123 Main St",
            //    Birthday = new DateTime(1980, 1, 1),
            //    MedicalHistory = "No known allergies"
            //},
            //new Patient
            //{
            //    TenantId = context.TenantId,
            //    HospitalId = 3,
            //    Code = "PAT002",
            //    Fullname = "Jane Smith",
            //    ProvinceCode = "01",
            //    DistrictCode = "002",
            //    WardCode = "0002",
            //    DetailAddress = "456 Elm St",
            //    Birthday = new DateTime(1990, 2, 2),
            //    MedicalHistory = "Diabetic"
            //}
        //};

        //    // Insert patients into the database
        //    foreach (var patient in patients)
        //    {
        //        await _patientRepository.InsertAsync(patient);
        //    }
        }
    }
}
