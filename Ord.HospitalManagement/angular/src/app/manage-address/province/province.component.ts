import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProvinceDto } from '@proxy/dtos/address';
import { levelProvinceOptions } from '@proxy/enums';
import { ProvinceService } from '@proxy/services';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { TITLE_NOTI, TYPE_NOTI } from 'src/app/helper/enum-const';
import { DataResult } from '@proxy/data-result';

@Component({
  selector: 'app-province',
  standalone: false,
  templateUrl: './province.component.html',
  styleUrl: './province.component.scss',
  providers: [ListService],
})
export class ProvinceComponent implements OnInit {
  province = { items: [], totalCount: 0 } as PagedResultDto<ProvinceDto>;
  isModalOpen = false;
  isModalUploadFileOpen!: boolean;
  levelProvinces = levelProvinceOptions;
  form: FormGroup;
  selectedProvince = {} as ProvinceDto;
  proviceSearchName!: string;
  selectedFile: File | null = null;

  constructor(
    public readonly list: ListService,
    private provinceService: ProvinceService,
    private fb: FormBuilder,
    private notification: NzNotificationService,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }
  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedProvince.name || '', Validators.required],
      levelProvince: [this.selectedProvince.levelProvince || null, Validators.required],
    });
  }
  loadData() {
    const provinceStreamCreator = query =>
      this.provinceService.getList({ ...query, filterName: this.proviceSearchName });

    this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
      this.province = response;
      console.log(this.province);
    });
  }
  createProvince() {
    this.selectedProvince = {} as ProvinceDto;
    this.buildForm();
    this.isModalOpen = true;
  }
  editProvince(id: number) {
    this.provinceService.get(id).subscribe(p => {
      this.selectedProvince = p;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const req = this.selectedProvince.id
      ? this.provinceService.update(this.selectedProvince.id, this.form.value)
      : this.provinceService.create(this.form.value);

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
        this.provinceService.delete(id).subscribe(() => this.list.get());
      }
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
    this.provinceService.importExcelByFormFile(formData).subscribe({
      next: (response: DataResult<ProvinceDto>) => {
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
