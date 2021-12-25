import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { RGBService } from '../components/rgb/_services/rgb.service';
import { HomeService } from '../_services/home.service';

@Component({
  selector: 'sh-lookup',
  templateUrl: './lookup.component.html',
  styleUrls: ['./lookup.component.scss']
})
export class LookupComponent implements OnInit {
  // lookups = [
  //   this.translate.instant('HOME.LOOKUP.GADGETS'),
  //   this.translate.instant('HOME.LOOKUP.DOORS_AND_WINDOORS'),
  //   this.translate.instant('HOME.LOOKUP.SMART_PLUGS')];
  // active: string = this.translate.instant('HOME.LOOKUP.GADGETS');
  @Input() active: string = 'gadgets';
  windoors: any;
  rgbIsOn: boolean;

  smartplugs: any;

  constructor(
    public homeService: HomeService,
    private router: Router,
    public translate: TranslateService,
    private rgbService: RGBService) {

  }

  ngOnInit() {
    this.rgbService._switch_checked$.subscribe(sw => this.rgbIsOn = sw)

  }

  getSp(id){
    const room = this.homeService._home$.value.rooms.find(room => room.roomId === id)
    return room.roomDevices.filter(m => m.deviceType === 4)
  }

  isEmpty(modules: any) {
    return modules.length === 0
  }

  isEmptyOfDevices(modules) {
    return !modules || (modules.leDs.length === 0 && modules.curtains.length === 0)
  }

  goToDetails(id) {
    this.router.navigateByUrl(`/smartplug/details/${id}`)
  }
}
