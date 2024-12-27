import { ListService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HospitalDto } from '@proxy/dtos/hospital';
import { DistrictService, ProvinceService, WardService } from '@proxy/services';
import { TenantHospitalService } from '@proxy/tenant-app-service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { TITLE_NOTI, TYPE_NOTI } from 'src/app/helper/enum-const';

@Component({
  selector: 'app-create-update-hospital',
  standalone: false,
  templateUrl: './create-update-hospital.component.html',
  styleUrl: './create-update-hospital.component.scss',
  providers: [ListService],
})
export class CreateUpdateHospitalComponent implements OnInit {
  isHiddenInfoUser: boolean = true;
  hospital!: HospitalDto;
  form: FormGroup;
  listProvince: { code: string; name: string }[] = [];
  listDistrict: { code: string; name: string }[] = [];
  listWard: { code: string; name: string }[] = [];
  private idHospital!: string;

  constructor(
    private fb: FormBuilder,
    public readonly list: ListService,
    private router: Router,
    private districtService: DistrictService,
    private provinceService: ProvinceService,
    private wardService: WardService,
    private activatedRoute: ActivatedRoute,
    private tenantHospitalService: TenantHospitalService,
    private notification: NzNotificationService
  ) {
    this.buildForm();
  }
  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.idHospital = params['id'];
      if (this.idHospital) {
        this.getHospital();
        this.isHiddenInfoUser = false;
      }
    });
    this.loadDataProvince();
  }
  getHospital() {
    this.tenantHospitalService.getHospital(Number(this.idHospital)).subscribe(h => {
      this.hospital = h;
      console.log(this.hospital);
      this.form.patchValue({
        createHospital: {
          hospitalName: this.hospital.hospitalName,
          provinceCode: this.hospital.provinceCode,
          districtCode: this.hospital.districtCode,
          wardCode: this.hospital.wardCode,
          hospitalDetailAddress: this.hospital.hospitalDetailAddress,
          hospitalDescription: this.hospital.hospitalDescription,
          hotline: this.hospital.hotline,
        },
      });
    });
  }
  buildForm(): void {
    this.form = this.fb.group({
      userHospital: this.fb.group({
        userName: [''],
        emailAddress: [''],
        password: [''],
      }),

      createHospital: this.fb.group({
        hospitalName: ['', [Validators.required]],
        provinceCode: [null],
        districtCode: [null],
        wardCode: [null],
        hospitalDetailAddress: [''],
        hospitalDescription: [''],
        hotline: ['', [Validators.required]],
      }),
    });
  }
  validateUserHospital(): { isValid: boolean; errorMessage: string | null } {
    const { userName, emailAddress, password } = this.form.get('userHospital')?.value;
    if (userName === null || userName === undefined || userName.trim() === '') {
      return { isValid: false, errorMessage: 'Tên người dùng không được để trống.' };
    }
    if (emailAddress === null || emailAddress === undefined || emailAddress.trim() === '') {
      return { isValid: false, errorMessage: 'Địa chỉ email không được để trống.' };
    }
    if (password === null || password === undefined || password.trim() === '') {
      return { isValid: false, errorMessage: 'Mật khẩu không được để trống.' };
    }
    const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{6,}$/;
    if (!passwordRegex.test(password)) {
      return {
        isValid: false,
        errorMessage: 'Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ, số và ký tự đặc biệt.',
      };
    }
    return { isValid: true, errorMessage: null };
  }

  onCancel(): void {
    this.form.reset();
  }
  onSubmit(): void {
    debugger;
    console.log(this.form.value);
    if (!this.form.valid) {
      return;
    }
    let req;
    if (this.idHospital) {
      req = this.tenantHospitalService.updateInfoHospitalByIdAndInput(
        this.hospital.id,
        this.form.get('createHospital')?.value
      );
    } else {
      const check = this.validateUserHospital();
      if (check.isValid) {
        req = this.tenantHospitalService.createHospital(this.form.value);
      } else {
        this.notification.create(TYPE_NOTI.ERROR, TITLE_NOTI, check.errorMessage);
        return;
      }
    }
    req.subscribe((response: HospitalDto) => {
      this.router.navigate(['list']);
      // this.form.patchValue({
      //   createHospital: {
      //     hospitalName: response.hospitalName,
      //     provinceCode: response.provinceCode,
      //     districtCode: response.districtCode,
      //     wardCode: response.wardCode,
      //     hospitalDetailAddress: response.hospitalDetailAddress,
      //     hospitalDescription: response.hospitalDescription,
      //     hotline: response.hotline,
      //   },
      // });
      // this.isHiddenInfoUser = false;
    });
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
