import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DataResult } from '@proxy/data-result';
import { WardDto } from '@proxy/dtos/address';
import { levelWardOptions } from '@proxy/enums';
import { DistrictService, ProvinceService, WardService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { TITLE_NOTI, TYPE_NOTI } from 'src/app/helper/enum-const';

@Component({
  selector: 'app-ward',
  standalone: false,
  templateUrl: './ward.component.html',
  styleUrl: './ward.component.scss',
  providers: [ListService],
})
export class WardComponent implements OnInit {
  ward = { items: [], totalCount: 0 } as PagedResultDto<WardDto>;
  isModalOpen = false;
  levelWards = levelWardOptions;
  form: FormGroup;
  listProvince: { code: string; name: string }[] = [];
  listDistrict: { code: string; name: string }[] = [];
  selectedWard = {} as WardDto;
  wardSearchName!: string;
  selectedProvinceCode!: string;
  selectedDistrictCode!: string;
  isModalUploadFileOpen: boolean = false;
  selectedFile: File | null = null;

  constructor(
    public readonly list: ListService,
    private wardService: WardService,
    private districtService: DistrictService,
    private provinceService: ProvinceService,
    private fb: FormBuilder,
    private notification: NzNotificationService,
    private confirmation: ConfirmationService
  ) {}
  ngOnInit(): void {
    this.loadData();
    this.loadDataProvince();
  }
  loadData() {
    const provinceStreamCreator = query =>
      this.wardService.getList({
        ...query,
        filterName: this.wardSearchName,
        provinceCode: this.selectedProvinceCode,
        districtCode: this.selectedDistrictCode,
      });

    this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
      this.ward = response;
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
  onChange(newValue) {
    this.loadDataDistrict(newValue);
    this.loadData();
  }
  createWard() {
    this.selectedWard = {} as WardDto;
    this.buildForm();
    this.isModalOpen = true;
  }
  editWard(id: number) {
    this.wardService.get(id).subscribe(p => {
      this.selectedWard = p;
      this.buildForm();
      this.isModalOpen = true;
    });
  }
  save() {
    if (this.form.invalid) {
      return;
    }
    const req = this.selectedWard.id
      ? this.wardService.update(this.selectedWard.id, this.form.value)
      : this.wardService.create(this.form.value);

    req.subscribe({
      next: () => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.notification.create(TYPE_NOTI.SUCCESS, TITLE_NOTI, 'Thao tác thành công');
      },
      error: error => {
        this.notification.create(TYPE_NOTI.ERROR, TITLE_NOTI, error.Messege);
      },
    });
  }
  delete(id: number) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.wardService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedWard.name || '', Validators.required],
      provinceCode: [this.selectedWard.provinceCode || null, Validators.required],
      districtCode: [this.selectedWard.districtCode || null, Validators.required],
      levelWard: [this.selectedWard.levelWard || null, Validators.required],
    });
  }

  onFileChange(files: FileList) {
    if (files) {
      this.selectedFile = files.item(0);
    } else {
      this.notification.create(TYPE_NOTI.ERROR, TITLE_NOTI, 'Hãy valid file của bạn');
    }
  }
  uploadFile() {
    if (!this.selectedFile) {
      return;
    }
    const formData: FormData = new FormData();
    formData.append('formFile', this.selectedFile, this.selectedFile.name);
    this.wardService.importExcelWardByFormFile(formData).subscribe({
      next: (response: DataResult<WardDto>) => {
        this.isModalUploadFileOpen = false;
        this.selectedFile = null;
        if (response.isOk && response.errorData.length == 0) {
          this.notification.create(TYPE_NOTI.SUCCESS, TITLE_NOTI, response.msg);
          this.list.get();
          return;
        }
        if (response.errorData) {
          if (response.isOk) {
            this.notification.create(TYPE_NOTI.WARNING, TITLE_NOTI, response.msg);
            this.list.get();
          } else {
            this.notification.create(TYPE_NOTI.ERROR, TITLE_NOTI, response.msg);
          }
        }
      },
    });
  }
}
