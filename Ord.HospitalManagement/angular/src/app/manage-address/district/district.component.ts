import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DistrictDto } from '@proxy/dtos/address';
import { levelDistrictOptions } from '@proxy/enums';
import { DistrictService, ProvinceService } from '@proxy/services';

@Component({
  selector: 'app-district',
  standalone: false,
  templateUrl: './district.component.html',
  styleUrl: './district.component.scss',
  providers: [ListService],
})
export class DistrictComponent implements OnInit {
  district = { items: [], totalCount: 0 } as PagedResultDto<DistrictDto>;
  isModalOpen = false;
  levelDistricts = levelDistrictOptions;
  form: FormGroup;
  listProvince: { code: string; name: string }[] = [];
  selectedDistrict = {} as DistrictDto;
  districtSearchName!: string;
  selectedProvinceCode!: string;
  selectedFile: File | null = null;
  isModalUploadFileOpen: boolean = false;

  constructor(
    public readonly list: ListService,
    private districtService: DistrictService,
    private provinceService: ProvinceService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {
    this.list.maxResultCount = 1;
  }
  ngOnInit(): void {
    this.loadData();
    this.loadDataProvince();
  }
  loadData() {
    const provinceStreamCreator = query =>
      this.districtService.getList({
        ...query,
        filterName: this.districtSearchName,
        provinceCode: this.selectedProvinceCode,
      });

    this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
      this.district = response;
      console.log(this.district);
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
      console.log(this.listProvince);
    });
  }

  createDistrict() {
    this.buildForm();
    this.isModalOpen = true;
  }
  editDistrict(id: number) {
    this.districtService.get(id).subscribe(p => {
      this.selectedDistrict = p;
      this.buildForm();
      this.isModalOpen = true;
    });
  }
  save() {
    if (this.form.invalid) {
      return;
    }
    const req = this.selectedDistrict.id
      ? this.districtService.update(this.selectedDistrict.id, this.form.value)
      : this.districtService.create(this.form.value);

    req.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
  delete(id: number) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.districtService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedDistrict.name || '', Validators.required],
      provinceCode: [this.selectedDistrict.provinceCode || null, Validators.required],
      levelDistrict: [this.selectedDistrict.levelDistrict || null, Validators.required],
    });
  }

  onFileChange(files: FileList) {
    if (files) {
      this.selectedFile = files.item(0);
    } else {
      alert('Please select a valid Excel file!');
    }
  }
  uploadFile() {
    if (!this.selectedFile) {
      return;
    }
    const formData: FormData = new FormData();
    formData.append('formFile', this.selectedFile, this.selectedFile.name);
    this.districtService.importExcelByFormFile(formData).subscribe(response => {
      console.log(response);
      this.isModalUploadFileOpen = false;
      this.list.get();
    });
  }
}
