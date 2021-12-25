import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Location, LocationStrategy } from '@angular/common';
import { HomeService } from '../../../modules/home/_services/home.service';

@Component({
  selector: 'sh-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss'],
})
export class MapComponent implements OnInit {
  selectedRoom = null;
  viewBox = '-20 -20 618.88 407.99';
  isIE = !!(
    navigator.userAgent.match(/Trident/) ||
    navigator.userAgent.match(/MSIE/) ||
    navigator.userAgent.match(/Edge/)
  );
  isFirefox = navigator.userAgent.toLowerCase().indexOf('firefox') >= 0;

  constructor(
    private location: Location,
    private locationStrategy: LocationStrategy,
    public homeService: HomeService,
    private _ref: ChangeDetectorRef
  ) { }

  ngOnInit() {
    if (this.homeService._selectedRoom$.value)
      this.selectedRoom = this.homeService._selectedRoom$.value.roomId;
  }

  selectRoom(room) {
    this.selectedRoom = room.roomId;
    this.homeService._selectedRoom$.next(room);
  }

  getUrlPath(id: string) {
    const baseHref = this.locationStrategy.getBaseHref().replace(/\/$/, '');
    const path = this.location.path().replace(/\/$/, '');

    return `url(${baseHref}${path}${id})`;
  }
}
