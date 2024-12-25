import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ManageInfoHospital } from '@proxy/dtos';
import { TenantHospitalService } from '@proxy/tenant-app-service';

@Component({
  selector: 'app-manage-hospital',
  standalone: false,
  templateUrl: './manage-hospital.component.html',
  styleUrl: './manage-hospital.component.scss',
  providers: [ListService],
})
export class ManageHospitalComponent implements OnInit {
  pageSize = 1;
  currentPage = 1;
  hospitals = { items: [], totalCount: 0 } as PagedResultDto<ManageInfoHospital>;

  constructor(
    public readonly list: ListService,
    private router: Router,
    private tenantHospitalService: TenantHospitalService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {
    this.list.maxResultCount = 1;
  }
  ngOnInit(): void {
    this.loadData();
  }
  loadData() {
    const manageHospitalStreamCreator = () =>
      this.tenantHospitalService.getInfoHospitalsByPageNumberAndPageSize(
        this.currentPage,
        this.pageSize
      );

    this.list.hookToQuery(manageHospitalStreamCreator).subscribe(response => {
      this.hospitals = response;
      console.log(this.hospitals);
    });
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadData();
  }
  createUpdate(id?: number) {
    if (id) {
      this.router.navigate(['manage-hospital/action-hospital', id]);
      return;
    }
    this.router.navigate(['manage-hospital/action-hospital']);
  }
}
