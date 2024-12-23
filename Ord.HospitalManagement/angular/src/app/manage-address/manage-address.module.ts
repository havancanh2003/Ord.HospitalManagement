import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageAddressRoutingModule } from './manage-address-routing.module';
import { ProvinceComponent } from './province/province.component';
import { SharedModule } from '../shared/shared.module';
import { DistrictComponent } from './district/district.component';
import { WardComponent } from './ward/ward.component';

@NgModule({
  declarations: [ProvinceComponent, DistrictComponent, WardComponent],
  imports: [CommonModule, ManageAddressRoutingModule, SharedModule],
})
export class ManageAddressModule {}
