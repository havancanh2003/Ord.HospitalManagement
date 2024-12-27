import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PatientDto } from '@proxy/dtos/hospital';
import { DistrictService, ProvinceService, WardService } from '@proxy/services';
import { MangePatientHospitalService } from '@proxy/services/manege-hospital';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { TITLE_NOTI, TYPE_NOTI } from 'src/app/helper/enum-const';

@Component({
  selector: 'app-manage-patient',
  standalone: false,
  templateUrl: './manage-patient.component.html',
  styleUrl: './manage-patient.component.scss',
  providers: [ListService],
})
export class ManagePatientComponent implements OnInit {
  pageSize = 1;
  pageNumber = 1;
  nameSearch!: string;
  patientSelected = {} as PatientDto;
  listProvince: { code: string; name: string }[] = [];
  listDistrict: { code: string; name: string }[] = [];
  listWard: { code: string; name: string }[] = [];
  isVisible = false;
  form: FormGroup;
  patients = { items: [], totalCount: 0 } as PagedResultDto<PatientDto>;
  constructor(
    public readonly list: ListService,
    private mangePatientHospitalService: MangePatientHospitalService,
    private fb: FormBuilder,
    private notification: NzNotificationService,
    private districtService: DistrictService,
    private provinceService: ProvinceService,
    private wardService: WardService
  ) {
    this.list.maxResultCount = 1;
  }
  ngOnInit(): void {
    this.loadData();
  }
  loadData() {
    const patientStreamCreator = () =>
      this.mangePatientHospitalService.getAllPatientByFilterByPageNumberAndPageSizeAndNameAndCode(
        this.pageNumber,
        this.pageSize,
        this.nameSearch,
        null
      );

    this.list.hookToQuery(patientStreamCreator).subscribe(response => {
      this.patients = response;
      console.log(this.patients);
    });
  }
  buildForm(): void {
    this.form = this.fb.group({
      fullname: [this.patientSelected.fullname || '', [Validators.required]],
      provinceCode: [this.patientSelected.provinceCode || null],
      districtCode: [this.patientSelected.districtCode || null],
      wardCode: [this.patientSelected.wardCode || null],
      detailAddress: [this.patientSelected.detailAddress || ''],
      birthday: [this.patientSelected.birthday || ''],
      medicalHistory: [this.patientSelected.medicalHistory || ''],
    });
    this.loadDataProvince();
    if (this.patientSelected.districtCode) {
      this.loadDataDistrict(this.patientSelected.provinceCode);
    }
    if (this.patientSelected.wardCode) {
      this.loadDataWard(this.patientSelected.districtCode);
    }
  }
  onPageChange(page: number): void {
    this.pageNumber = page;
    this.loadData();
  }
  createUpdate(id?: number) {
    if (id) {
      this.mangePatientHospitalService.getPatientById(id).subscribe(p => {
        this.patientSelected = p;
        this.buildForm();
        this.isVisible = true;
      });
      return;
    }
    this.patientSelected = {} as PatientDto;
    this.buildForm();
    this.isVisible = true;
  }

  handleOk(): void {
    if (this.form.invalid) {
      return;
    }
    const req = this.patientSelected.id
      ? this.mangePatientHospitalService.updatePatient(this.patientSelected.id, this.form.value)
      : this.mangePatientHospitalService.createPatient(this.form.value);

    req.subscribe({
      next: () => {
        this.isVisible = false;
        this.form.reset();
        this.list.get();
        this.notification.create(TYPE_NOTI.SUCCESS, TITLE_NOTI, 'Thao tác thành công');
      },
      error: error => {
        this.notification.create(TYPE_NOTI.ERROR, TITLE_NOTI, error.Messege);
      },
    });
  }

  handleCancel(): void {
    this.isVisible = false;
  }
  loadDataProvince() {
    const provinceStreamCreator = query =>
      this.provinceService.getList({ ...query, maxResultCount: 999 });

    this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
      if (response && response.items) {
        const data = response.items.map(p => ({
          code: p.code,
          name: p.name,
        }));
        this.listProvince = [...data];
      }
    });
  }
  loadDataDistrict(provinceCode: string) {
    const provinceStreamCreator = query =>
      this.districtService.getList({ ...query, provinceCode: provinceCode, maxResultCount: 999 });

    this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
      if (response && response.items) {
        const data = response.items.map(p => ({
          code: p.code,
          name: p.name,
        }));
        this.listDistrict = [...data];
      }
    });
  }
  loadDataWard(districtCode: string) {
    const provinceStreamCreator = query =>
      this.wardService.getList({ ...query, districtCode: districtCode, maxResultCount: 999 });

    this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
      if (response && response.items) {
        const data = response.items.map(p => ({
          code: p.code,
          name: p.name,
        }));
        this.listWard = [...data];
      }
    });
  }
  onProvinceChange(provinceCode: string) {
    this.loadDataDistrict(provinceCode);
  }
  onDistrictChange(districtCode: string) {
    this.loadDataWard(districtCode);
  }
}
