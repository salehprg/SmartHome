import { TranslateService } from '@ngx-translate/core';
import { HomeService } from './_services/home.service';
import { Component, ViewChild } from '@angular/core';
import { NgbModal, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'sh-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [NgbModalConfig, NgbModal],
})
export class HomeComponent {
  @ViewChild('fetchError') fetchError;
  error: any;
  lookups = [
    this.translate.instant('HOME.LOOKUP.LAMPS'),
    this.translate.instant('HOME.LOOKUP.GADGETS'),
    this.translate.instant('HOME.LOOKUP.DOORS_AND_WINDOORS'),
    this.translate.instant('HOME.LOOKUP.SMART_PLUGS'),
  ];
  active: string = this.translate.instant('HOME.LOOKUP.LAMPS');

  constructor(
    public homeService: HomeService,
    private modalService: NgbModal,
    private translate: TranslateService,
    config: NgbModalConfig
  ) {
  }

  openWifiModal(content) {
    this.modalService.dismissAll();
    this.modalService.open(content, { scrollable: true });
  }

  reloadPage() {
    window.location.reload();
  }
}
