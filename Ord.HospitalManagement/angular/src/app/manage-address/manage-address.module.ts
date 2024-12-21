import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageAddressRoutingModule } from './manage-address-routing.module';
import { ProvinceComponent } from './province/province.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [ProvinceComponent],
  imports: [CommonModule, ManageAddressRoutingModule, SharedModule],
})
export class ManageAddressModule {}
