import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { WardDto } from '@proxy/dtos/address';
import { levelWardOptions } from '@proxy/enums';
import { DistrictService, ProvinceService, WardService } from '@proxy/services';

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

  constructor(
    public readonly list: ListService,
    private wardService: WardService,
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
      this.wardService.getList({
        ...query,
        filterName: this.wardSearchName,
        provinceCode: this.selectedProvinceCode,
        districtCode: this.selectedDistrictCode,
      });

    this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
      this.ward = response;
      console.log(this.ward);
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
        this.listProvince = [...this.listProvince, ...data];
      }
      console.log(this.listProvince);
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
        this.listDistrict = [...this.listDistrict, ...data];
      }
      console.log(this.listDistrict);
    });
  }
  onChange(newValue) {
    console.log(1);

    console.log(newValue);
  }

  createWard() {
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

    req.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
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
}
