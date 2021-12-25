import { LayoutService } from './../layout.service';
import { Component, ViewEncapsulation } from "@angular/core";
import { interval } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { environment } from 'src/environments/environment';
import { HomeService } from 'src/app/modules/home/_services/home.service';

@Component({
  selector: 'sh-header',
  encapsulation : ViewEncapsulation.None,
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  now: Date;
  isRasp : boolean;
  constructor(
    private layoutService: LayoutService,
    private modalService: NgbModal ,
    private homeService : HomeService
    
    ) {
    this.now = new Date();
    interval(60 * 1000).subscribe(() => {
      this.now = new Date();
    });

    this.homeService._isRaspberry$.subscribe(isRasp => this.isRasp = isRasp)
    // this.homeService._isRaspberry$.next(true)
      // this.isRasp = true
  }

  getLogo() {
    const themeType = this.layoutService.getProp('theme.type') || 'light';
    if (themeType === 'light') return './assets/media/logos/logo-text.svg';
    else return './assets/media/logos/logo-text-white.svg';
  }

  open(content) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'});
  }

  openScrollable(content){
    this.modalService.open(content , {scrollable : true})
  }
}
