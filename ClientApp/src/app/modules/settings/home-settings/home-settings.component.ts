import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HomeService } from '../../home/_services/home.service';

@Component({
  selector: 'sh-settings-home',
  templateUrl: './home-settings.component.html',
  styleUrls: ['./home-settings.component.scss'],
})
export class HomeSettingsComponent implements OnInit {
  lookups = [
    this.translate.instant('HOME.LOOKUP.LAMPS'),
    this.translate.instant('HOME.LOOKUP.GADGETS'),
    this.translate.instant('HOME.LOOKUP.DOORS_AND_WINDOORS'),
    this.translate.instant('HOME.LOOKUP.SMART_PLUGS'),
  ];
  active: string = this.translate.instant('HOME.LOOKUP.LAMPS');

  constructor(
    public homeService: HomeService,
    private translate: TranslateService
  ) {}

  ngOnInit() {
    this.homeService.fetchHome();
  }
}
